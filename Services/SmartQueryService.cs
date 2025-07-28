using Microsoft.AspNetCore.Identity;
using MotqenIslamicLearningPlatform_API.DTOs.BotDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Services.Chat;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
using System.Text.Json;

namespace MotqenIslamicLearningPlatform_API.Services
{
    public class SmartQueryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ChatBotService _chatBot;
        private readonly UserManager<User> _userManager;

        public SmartQueryService(UnitOfWork unitOfWork, ChatBotService chatBot, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _chatBot = chatBot;
            _userManager = userManager;
        }

        public async Task<SmartQueryResponse> ProcessSmartQuery(SmartQueryRequest request)
        {
            try
            {
                // Get user info and Role-specific ID
                var userInfo = await GetUserRoleInfo(request.Id, request.Role);
                if (userInfo == null)
                {
                    return new SmartQueryResponse
                    {
                        Answer = "User not found or invalid Role.",
                        DataSource = "System Error"
                    };
                }

                // Query database based on question intent and user Role
                var dbResult = await QueryDatabaseByIntent(request.Question, request.Role, userInfo);

                // Format data for AI context
                var formattedData = FormatDataForAI(dbResult);

                // Create AI prompt with context
                var aiPrompt = CreateAIPromptWithContext(request.Question, request.Role, formattedData);

                // Get AI response
                var aiResponse = await _chatBot.GetChatResponse(aiPrompt);

                return new SmartQueryResponse
                {
                    Answer = aiResponse,
                    DataSource = $"Database query for {request.Role}"
                };
            }
            catch (Exception ex)
            {
                return new SmartQueryResponse
                {
                    Answer = $"I encountered an error processing your question: {ex.Message}",
                    DataSource = "Error"
                };
            }
        }

        private async Task<dynamic> GetUserRoleInfo(string userId, string userRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var s = _unitOfWork.StudentRepo.GetStudentByUserId(userId);
            if (user == null) return null;

            return userRole switch
            {
                "Student" => new
                {
                    UserId = userId,
                    StudentId = s.Id,
                    ParentId = s.ParentId ?? 0
                },
                "Teacher" => new
                {
                    UserId = userId,
                    TeacherId = user.Teacher?.Id
                },
                "Parent" => new
                {
                    UserId = userId,
                    ParentId = user.Parent?.Id
                },
                "Admin" => new
                {
                    UserId = userId,
                    IsAdmin = true
                },
                _ => null
            };
        }

        private async Task<object> QueryDatabaseByIntent(string question, string userRole, dynamic userInfo)
        {
            var intent = DetermineQueryIntent(question);

            return userRole switch
            {
                "Student" => await QueryForStudent(intent, question, userInfo.StudentId, userInfo.ParentId),
                "Teacher" => await QueryForTeacher(intent, question, userInfo.TeacherId),
                "Parent" => await QueryForParent(intent, question, userInfo.ParentId),
                "Admin" => await QueryForAdmin(intent, question),
                _ => new { Error = "Invalid user Role" }
            };
        }

        private string DetermineQueryIntent(string question)
        {
            var lowerQuestion = question.ToLower();

            // Define intent patterns with improved Arabic keywords
            var patterns = new Dictionary<string, string[]>
            {
                ["حضور"] = new[] { "حضور", "حاضر", "غائب", "موجود", "غياب", "attend" },
                ["التقدم"] = new[] { "تقدم", "تتبع", "اداء", "درجات", "تقييم", "progress", "evaluation" },
                ["مواعيد"] = new[] { "موعد", "حلقة", "وقت", "متى", "اليوم", "الغد", "schedule", "time" },
                ["حلقة"] = new[] { "حلقة", "حلقات", "مجموعة", "فصل", "halaqa", "class" },
                ["المادة"] = new[] { "المادة", "مواد", "قرآن", "شرعي", "درس", "subject", "quran" },
                ["طلاب"] = new[] { "طالب", "طلاب", "تلميذ", "متعلم", "student" },
                ["معلمون"] = new[] { "معلم", "معلمون", "موجه", "شيخ", "teacher" },
                ["أولياء الأمور"] = new[] { "ولي أمر", "أب", "أم", "ولي", "parent" }
            };

            foreach (var pattern in patterns)
            {
                if (pattern.Value.Any(keyword => lowerQuestion.Contains(keyword)))
                {
                    return pattern.Key;
                }
            }

            return "general";
        }

        private async Task<object> QueryForStudent(string intent, string question, int? studentId, int? parentId)
        {
            if (!studentId.HasValue) return new { Error = "Student ID not found" };

            return intent switch
            {
                "حضور" => new
                {
                    StudentAttendance = _unitOfWork.StudentAttendanceRepo.GetAll()
                        .Where(a => a.StudentId == studentId.Value && !a.Student.IsDeleted)
                        .OrderByDescending(a => a.AttendanceDate)
                        .Take(30)
                        .Select(a => new
                        {
                            a.AttendanceDate,
                            Status = a.Status.ToString(),
                            HalaqaName = a.Halaqa != null ? a.Halaqa.Name : "Unknown",
                            Subject = a.Halaqa != null && a.Halaqa.Subject != null ? a.Halaqa.Subject.Name : "No Subject"
                        }).ToList()
                },
                "التقدم" => new
                {
                    ProgressTracking = _unitOfWork.ProgressTrackingRepo.GetAll()
                        .Where(p => p.StudentId == studentId.Value && !p.IsDeleted && (p.Student == null || !p.Student.IsDeleted))
                        .OrderByDescending(p => p.Date)
                        .Take(20)
                        .Select(p => new
                        {
                            p.Date,
                            p.Status,
                            p.Evaluation,
                            p.Notes,
                            HalaqaName = p.Halaqa != null ? p.Halaqa.Name : "Unknown",
                            Subject = p.Halaqa != null && p.Halaqa.Subject != null ? p.Halaqa.Subject.Name : "No Subject",
                            QuranProgress = p.QuranProgressTrackingDetail != null ? new
                            {
                                FromSurah = p.QuranProgressTrackingDetail.FromSurah,
                                ToSurah = p.QuranProgressTrackingDetail.ToSurah,
                                FromAyah = p.QuranProgressTrackingDetail.FromAyah,
                                ToAyah = p.QuranProgressTrackingDetail.ToAyah,
                                Type = p.QuranProgressTrackingDetail.Type.ToString(),
                                NumberOfLines = p.QuranProgressTrackingDetail.NumberOfLines
                            } : null,
                            IslamicSubjectsProgress = p.IslamicSubjectsProgressTrackingDetail != null ? new
                            {
                                Subject = p.IslamicSubjectsProgressTrackingDetail.Subject,
                                FromPage = p.IslamicSubjectsProgressTrackingDetail.FromPage,
                                ToPage = p.IslamicSubjectsProgressTrackingDetail.ToPage,
                                LessonName = p.IslamicSubjectsProgressTrackingDetail.LessonName
                            } : null
                        }).ToList()
                },
                "حلقة" => new
                {
                    MyHalaqas = _unitOfWork.HalaqaStudentRepo.GetAll()
                        .Where(hs => hs.StudentId == studentId.Value &&
                                   !hs.Halaqa.IsDeleted &&
                                   (hs.Student == null || !hs.Student.IsDeleted))
                        .Select(hs => new
                        {
                            hs.Halaqa.Name,
                            hs.Halaqa.Description,
                            GenderGroup = hs.Halaqa.GenderGroup.ToString(),
                            hs.DateJoined,
                            Subject = hs.Halaqa.Subject != null ? hs.Halaqa.Subject.Name : "No Subject",
                            Teachers = hs.Halaqa.HalaqaTeachers
                                .Where(ht => ht.Teacher != null && ht.Teacher.User != null && !ht.Teacher.User.IsDeleted)
                                .Select(ht => ht.Teacher.User.FirstName + " " + ht.Teacher.User.LastName)
                                .ToList(),
                            StudentCount = hs.Halaqa.HalaqaStudents.Count(hst => hst.Student == null || !hst.Student.IsDeleted)
                        }).ToList()
                },
                "مواعيد" => new
                {
                    ClassSchedules = _unitOfWork.ClassScheduleRepo.GetAll()
                        .Where(cs => !cs.IsDeleted &&
                                   !cs.Halaqa.IsDeleted &&
                                   _unitOfWork.HalaqaStudentRepo.GetAll()
                                       .Any(hs => hs.StudentId == studentId.Value &&
                                                hs.HalaqaId == cs.HalaqaId &&
                                                (hs.Student == null || !hs.Student.IsDeleted)))
                        .Select(cs => new
                        {
                            Day = cs.Day.ToString(),
                            StartTime = cs.StartTime.ToString(@"hh\:mm"),
                            EndTime = cs.EndTime.ToString(@"hh\:mm"),
                            HalaqaName = cs.Halaqa.Name,
                            Subject = cs.Halaqa.Subject != null ? cs.Halaqa.Subject.Name : "No Subject"
                        }).ToList()
                },
                _ => new
                {
                    StudentInfo = _unitOfWork.StudentRepo.GetAll()
                        .Where(s => s.Id == studentId.Value && !s.IsDeleted)
                        .Select(s => new
                        {
                            s.Id,
                            Name = s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
                            s.Age,
                            s.Gender,
                            s.BirthDate,
                            s.Nationality,
                            ParentName = s.Parent != null && s.Parent.User != null ?
                                       s.Parent.User.FirstName + " " + s.Parent.User.LastName : "No Parent"
                        }).FirstOrDefault(),
                    EnrolledSubjects = _unitOfWork.StudentSubjectRepo.GetAll()
                        .Where(ss => ss.StudentId == studentId.Value && ss.Subject != null)
                        .Select(ss => ss.Subject.Name)
                        .ToList()
                }
            };
        }

        private async Task<object> QueryForTeacher(string intent, string question, int? teacherId)
        {
            if (!teacherId.HasValue) return new { Error = "Teacher ID not found" };

            return intent switch
            {
                "حضور" => new
                {
                    TeacherAttendance = _unitOfWork.TeacherAttendanceRepo.GetAll()
                        .Where(a => a.TeacherId == teacherId.Value &&
                                  (a.Teacher == null || !a.Teacher.IsDeleted) &&
                                  !a.Halaqa.IsDeleted)
                        .OrderByDescending(a => a.AttendanceDate)
                        .Take(30)
                        .Select(a => new
                        {
                            a.AttendanceDate,
                            Status = a.Status.ToString(),
                            HalaqaName = a.Halaqa.Name,
                            Subject = a.Halaqa.Subject != null ? a.Halaqa.Subject.Name : "No Subject"
                        }).ToList(),
                    StudentsAttendance = _unitOfWork.StudentAttendanceRepo.GetAll()
                        .Where(sa => _unitOfWork.HalaqaTeacherRepo.GetAll()
                            .Any(ht => ht.TeacherId == teacherId.Value &&
                                     ht.HalaqaId == sa.HalaqaId &&
                                     !ht.Halaqa.IsDeleted &&
                                     (ht.Teacher == null || !ht.Teacher.IsDeleted)) &&
                                     (sa.Student == null || !sa.Student.IsDeleted))
                        .OrderByDescending(sa => sa.AttendanceDate)
                        .Take(50)
                        .Select(sa => new
                        {
                            StudentName = sa.Student != null && sa.Student.User != null ?
                                        sa.Student.User.FirstName + " " + sa.Student.User.LastName : "Unknown",
                            sa.AttendanceDate,
                            Status = sa.Status.ToString(),
                            HalaqaName = sa.Halaqa.Name
                        }).ToList()
                },
                "حلقة" => new
                {
                    MyHalaqas = _unitOfWork.HalaqaTeacherRepo.GetAll()
                        .Where(ht => ht.TeacherId == teacherId.Value &&
                                   !ht.Halaqa.IsDeleted &&
                                   (ht.Teacher == null || !ht.Teacher.IsDeleted))
                        .Select(ht => new
                        {
                            ht.Halaqa.Name,
                            ht.Halaqa.Description,
                            GenderGroup = ht.Halaqa.GenderGroup.ToString(),
                            Subject = ht.Halaqa.Subject != null ? ht.Halaqa.Subject.Name : "No Subject",
                            StudentCount = ht.Halaqa.HalaqaStudents.Count(hs => hs.Student == null || !hs.Student.IsDeleted),
                            Schedule = ht.Halaqa.ClassSchedules
                                .Where(cs => !cs.IsDeleted)
                                .Select(cs => new
                                {
                                    Day = cs.Day.ToString(),
                                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                                    EndTime = cs.EndTime.ToString(@"hh\:mm")
                                }).ToList()
                        }).ToList()
                },
                "طلاب" => new
                {
                    MyStudents = _unitOfWork.HalaqaStudentRepo.GetAll()
                        .Where(hs => _unitOfWork.HalaqaTeacherRepo.GetAll()
                            .Any(ht => ht.TeacherId == teacherId.Value &&
                                     ht.HalaqaId == hs.HalaqaId &&
                                     !ht.Halaqa.IsDeleted &&
                                     (ht.Teacher == null || !ht.Teacher.IsDeleted)) &&
                                     (hs.Student == null || !hs.Student.IsDeleted))
                        .Select(hs => new
                        {
                            StudentName = hs.Student != null && hs.Student.User != null ?
                                        hs.Student.User.FirstName + " " + hs.Student.User.LastName : "Unknown",
                            Age = hs.Student != null ? hs.Student.Age : 0,
                            Gender = hs.Student != null ? hs.Student.Gender : "Unknown",
                            HalaqaName = hs.Halaqa.Name,
                            hs.DateJoined,
                            ParentName = hs.Student != null && hs.Student.Parent != null && hs.Student.Parent.User != null ?
                                       hs.Student.Parent.User.FirstName + " " + hs.Student.Parent.User.LastName : "No Parent"
                        }).ToList()
                },
                "التقدم" => new
                {
                    StudentsProgress = _unitOfWork.ProgressTrackingRepo.GetAll()
                        .Where(pt => _unitOfWork.HalaqaTeacherRepo.GetAll()
                            .Any(ht => ht.TeacherId == teacherId.Value &&
                                     ht.HalaqaId == pt.HalaqaId &&
                                     !ht.Halaqa.IsDeleted &&
                                     (ht.Teacher == null || !ht.Teacher.IsDeleted)) &&
                                     !pt.IsDeleted &&
                                     (pt.Student == null || !pt.Student.IsDeleted))
                        .OrderByDescending(pt => pt.Date)
                        .Take(50)
                        .Select(pt => new
                        {
                            StudentName = pt.Student != null && pt.Student.User != null ?
                                        pt.Student.User.FirstName + " " + pt.Student.User.LastName : "Unknown",
                            pt.Date,
                            pt.Status,
                            pt.Evaluation,
                            pt.Notes,
                            HalaqaName = pt.Halaqa != null ? pt.Halaqa.Name : "Unknown",
                            QuranProgress = pt.QuranProgressTrackingDetail != null ? new
                            {
                                FromSurah = pt.QuranProgressTrackingDetail.FromSurah,
                                ToSurah = pt.QuranProgressTrackingDetail.ToSurah,
                                FromAyah = pt.QuranProgressTrackingDetail.FromAyah,
                                ToAyah = pt.QuranProgressTrackingDetail.ToAyah,
                                Type = pt.QuranProgressTrackingDetail.Type.ToString()
                            } : null
                        }).ToList()
                },
                _ => new
                {
                    TeacherInfo = _unitOfWork.TeacherRepo.GetAll()
                        .Where(t => t.Id == teacherId.Value && !t.IsDeleted)
                        .Select(t => new
                        {
                            t.Id,
                            Name = t.User != null ? t.User.FirstName + " " + t.User.LastName : "Unknown",
                            t.Age,
                            t.Gender
                        }).FirstOrDefault(),
                    TeacherSubjects = _unitOfWork.TeacherSubjectRepo.GetAll()
                        .Where(ts => ts.TeacherId == teacherId.Value && ts.Subject != null)
                        .Select(ts => ts.Subject.Name).ToList()
                }
            };
        }

        private async Task<object> QueryForParent(string intent, string question, int? parentId)
        {
            if (!parentId.HasValue) return new { Error = "Parent ID not found" };

            var myStudents = _unitOfWork.StudentRepo.GetAll()
                .Where(s => s.ParentId == parentId.Value && !s.IsDeleted)
                .ToList();
            var studentIds = myStudents.Select(s => s.Id).ToList();

            return intent switch
            {
                "حضور" => new
                {
                    ChildrenAttendance = _unitOfWork.StudentAttendanceRepo.GetAll()
                        .Where(a => studentIds.Contains(a.StudentId) &&
                                  (a.Student == null || !a.Student.IsDeleted) &&
                                  !a.Halaqa.IsDeleted)
                        .OrderByDescending(a => a.AttendanceDate)
                        .Take(100)
                        .Select(a => new
                        {
                            StudentName = a.Student != null && a.Student.User != null ?
                                        a.Student.User.FirstName + " " + a.Student.User.LastName : "Unknown",
                            a.AttendanceDate,
                            Status = a.Status.ToString(),
                            HalaqaName = a.Halaqa.Name,
                            Subject = a.Halaqa.Subject != null ? a.Halaqa.Subject.Name : "No Subject"
                        }).ToList()
                },
                "التقدم" => new
                {
                    ChildrenProgress = _unitOfWork.ProgressTrackingRepo.GetAll()
                        .Where(p => studentIds.Contains(p.StudentId ?? 0) &&
                                  !p.IsDeleted &&
                                  (p.Student == null || !p.Student.IsDeleted))
                        .OrderByDescending(p => p.Date)
                        .Take(50)
                        .Select(p => new
                        {
                            StudentName = p.Student != null && p.Student.User != null ?
                                        p.Student.User.FirstName + " " + p.Student.User.LastName : "Unknown",
                            p.Date,
                            p.Status,
                            p.Evaluation,
                            p.Notes,
                            HalaqaName = p.Halaqa != null ? p.Halaqa.Name : "Unknown",
                            QuranProgress = p.QuranProgressTrackingDetail != null ? new
                            {
                                FromSurah = p.QuranProgressTrackingDetail.FromSurah,
                                ToSurah = p.QuranProgressTrackingDetail.ToSurah,
                                Type = p.QuranProgressTrackingDetail.Type.ToString()
                            } : null
                        }).ToList()
                },
                "طلاب" => new
                {
                    MyChildren = myStudents.Select(s => new
                    {
                        s.Id,
                        Name = s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
                        s.Age,
                        s.Gender,
                        s.BirthDate,
                        s.Nationality,
                        Halaqas = s.HalaqaStudents
                            .Where(hs => !hs.Halaqa.IsDeleted)
                            .Select(hs => new
                            {
                                Name = hs.Halaqa.Name,
                                Subject = hs.Halaqa.Subject != null ? hs.Halaqa.Subject.Name : "No Subject"
                            }).ToList()
                    }).ToList()
                },
                "مواعيد" => new
                {
                    ChildrenSchedules = _unitOfWork.ClassScheduleRepo.GetAll()
                        .Where(cs => !cs.IsDeleted &&
                                   !cs.Halaqa.IsDeleted &&
                                   _unitOfWork.HalaqaStudentRepo.GetAll()
                                       .Any(hs => studentIds.Contains(hs.StudentId) &&
                                                hs.HalaqaId == cs.HalaqaId &&
                                                (hs.Student == null || !hs.Student.IsDeleted)))
                        .Select(cs => new
                        {
                            Day = cs.Day.ToString(),
                            StartTime = cs.StartTime.ToString(@"hh\:mm"),
                            EndTime = cs.EndTime.ToString(@"hh\:mm"),
                            HalaqaName = cs.Halaqa.Name,
                            Subject = cs.Halaqa.Subject != null ? cs.Halaqa.Subject.Name : "No Subject",
                            Students = cs.Halaqa.HalaqaStudents
                                .Where(hs => studentIds.Contains(hs.StudentId) &&
                                           (hs.Student == null || !hs.Student.IsDeleted))
                                .Select(hs => hs.Student != null && hs.Student.User != null ?
                                            hs.Student.User.FirstName + " " + hs.Student.User.LastName : "Unknown")
                                .ToList()
                        }).ToList()
                },
                _ => new
                {
                    ParentInfo = _unitOfWork.ParentRepo.GetAll()
                        .Where(p => p.Id == parentId.Value && !p.IsDeleted)
                        .Select(p => new
                        {
                            p.Id,
                            Name = p.User != null ? p.User.FirstName + " " + p.User.LastName : "Unknown",
                            p.PhoneNumber,
                            p.Address
                        }).FirstOrDefault(),
                    MyChildren = myStudents.Select(s => new
                    {
                        s.Id,
                        Name = s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
                        s.Age,
                        s.Gender
                    }).ToList()
                }
            };
        }

        private async Task<object> QueryForAdmin(string intent, string question)
        {
            return intent switch
            {
                "طلاب" => new
                {
                    TotalStudents = _unitOfWork.StudentRepo.GetAll().Count(s => !s.IsDeleted),
                    RecentStudents = _unitOfWork.StudentRepo.GetAll()
                        .Where(s => !s.IsDeleted)
                        .OrderByDescending(s => s.Id)
                        .Take(10)
                        .Select(s => new
                        {
                            s.Id,
                            Name = s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
                            s.Age,
                            s.Gender,
                            ParentName = s.Parent != null && s.Parent.User != null ?
                                       s.Parent.User.FirstName + " " + s.Parent.User.LastName : "No Parent"
                        }).ToList()
                },
                "معلمون" => new
                {
                    TotalTeachers = _unitOfWork.TeacherRepo.GetAll().Count(t => !t.IsDeleted),
                    Teachers = _unitOfWork.TeacherRepo.GetAll()
                        .Where(t => !t.IsDeleted)
                        .Take(20)
                        .Select(t => new
                        {
                            t.Id,
                            Name = t.User != null ? t.User.FirstName + " " + t.User.LastName : "Unknown",
                            t.Age,
                            t.Gender,
                            HalaqaCount = t.HalaqaTeachers.Count(ht => !ht.Halaqa.IsDeleted),
                            Subjects = t.TeacherSubjects
                                .Where(ts => ts.Subject != null)
                                .Select(ts => ts.Subject.Name).ToList()
                        }).ToList()
                },
                "حلقة" => new
                {
                    TotalHalaqas = _unitOfWork.HalaqaRepo.GetAll().Count(h => !h.IsDeleted),
                    Halaqas = _unitOfWork.HalaqaRepo.GetAll()
                        .Where(h => !h.IsDeleted)
                        .Take(20)
                        .Select(h => new
                        {
                            h.Id,
                            h.Name,
                            h.Description,
                            GenderGroup = h.GenderGroup.ToString(),
                            Subject = h.Subject != null ? h.Subject.Name : "No Subject",
                            StudentCount = h.HalaqaStudents.Count(hs => hs.Student == null || !hs.Student.IsDeleted),
                            TeacherCount = h.HalaqaTeachers.Count(ht => ht.Teacher == null || !ht.Teacher.IsDeleted),
                            Teachers = h.HalaqaTeachers
                                .Where(ht => ht.Teacher != null && ht.Teacher.User != null && !ht.Teacher.User.IsDeleted)
                                .Select(ht => ht.Teacher.User.FirstName + " " + ht.Teacher.User.LastName)
                                .ToList()
                        }).ToList()
                },
                "حضور" => new
                {
                    TodayAttendance = _unitOfWork.StudentAttendanceRepo.GetAll()
                        .Where(a => a.AttendanceDate.Date == DateTime.Today &&
                                  (a.Student == null || !a.Student.IsDeleted))
                        .GroupBy(a => a.Status)
                        .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                        .ToList(),
                    RecentAttendance = _unitOfWork.StudentAttendanceRepo.GetAll()
                        .Where(a => a.Student == null || !a.Student.IsDeleted)
                        .OrderByDescending(a => a.AttendanceDate)
                        .Take(50)
                        .Select(a => new
                        {
                            StudentName = a.Student != null && a.Student.User != null ?
                                        a.Student.User.FirstName + " " + a.Student.User.LastName : "Unknown",
                            a.AttendanceDate,
                            Status = a.Status.ToString(),
                            HalaqaName = a.Halaqa.Name
                        }).ToList(),
                    TeacherAttendanceToday = _unitOfWork.TeacherAttendanceRepo.GetAll()
                        .Where(a => a.AttendanceDate.Date == DateTime.Today &&
                                  (a.Teacher == null || !a.Teacher.IsDeleted))
                        .GroupBy(a => a.Status)
                        .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                        .ToList()
                },
                _ => new
                {
                    SystemStats = new
                    {
                        TotalStudents = _unitOfWork.StudentRepo.GetAll().Count(s => !s.IsDeleted),
                        TotalTeachers = _unitOfWork.TeacherRepo.GetAll().Count(t => !t.IsDeleted),
                        TotalHalaqas = _unitOfWork.HalaqaRepo.GetAll().Count(h => !h.IsDeleted),
                        TotalParents = _unitOfWork.ParentRepo.GetAll().Count(p => !p.IsDeleted),
                        ActiveHalaqasToday = _unitOfWork.ClassScheduleRepo.GetAll()
                            .Where(cs => !cs.IsDeleted &&
                                       !cs.Halaqa.IsDeleted &&
                                       cs.Day.ToString().ToLower() == DateTime.Today.DayOfWeek.ToString().ToLower())
                            .Count(),
                        TotalSubjects = _unitOfWork.SubjectRepo.GetAll().Count(s => !s.IsDeleted)
                    },
                    RecentProgressEntries = _unitOfWork.ProgressTrackingRepo.GetAll()
                        .Where(pt => !pt.IsDeleted &&
                                   (pt.Student == null || !pt.Student.IsDeleted))
                        .OrderByDescending(pt => pt.Date)
                        .Take(10)
                        .Select(pt => new
                        {
                            StudentName = pt.Student != null && pt.Student.User != null ?
                                        pt.Student.User.FirstName + " " + pt.Student.User.LastName : "Unknown",
                            pt.Date,
                            pt.Status,
                            pt.Evaluation,
                            HalaqaName = pt.Halaqa != null ? pt.Halaqa.Name : "Unknown"
                        }).ToList()
                }
            };
        }

        private string FormatDataForAI(object dbResult)
        {
            if (dbResult == null) return "No data available.";

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                };

                return JsonSerializer.Serialize(dbResult, options);
            }
            catch (Exception ex)
            {
                return $"Unable to format data: {ex.Message}";
            }
        }

        private string CreateAIPromptWithContext(string userQuestion, string userRole, string formattedData)
        {
            return $@"
You are an AI assistant for an Islamic Learning Platform called Motqen. 
The user is a {userRole} asking: ""{userQuestion}""

Here is the relevant data from the database:
{formattedData}

Please provide a helpful, accurate, and concise answer based on this data. 
Guidelines for your response:
- If asking about attendance ( غياب / الغياب/ حضور / الحضور), provide specific dates, status information, and trends
- If asking about progress ( تقدم / تقيم/  التقيم/ التقدم), include evaluation details, notes, and Quran/Islamic subjects progress
- If asking about schedules (  مواعيد الحلقات/ مواعيد), provide clear time, day, and halaqa information
- If asking about(معلمين / طلاب /مواعيد الحلقات/حلقات) students/teachers/halaqas, provide relevant details and relationships
- If asking about subjects (المادة/المواد), include subject names and related halaqas
- Keep your response conversational, helpful, and easy to understand
- Use Islamic terms appropriately (e.g., Halaqa for study circles, Surah for Quran chapters)
- If the data is empty or insufficient, politely explain what information is not available
- Provide actionable insights when possible (e.g., attendance patterns, progress trends)
- Format dates and times in a user-friendly manner
- When showing Quran progress, mention Surah and Ayah ranges clearly
- For Islamic subjects progress, mention page ranges and lesson names

Answer in Arabic if the question is in Arabic, or in English if the question is in English. Be professional and supportive:";
        }
    }
}