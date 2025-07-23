using Microsoft.AspNetCore.Identity;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Repositories;
using MotqenIslamicLearningPlatform_API.Repositories.AuthRepo;

namespace MotqenIslamicLearningPlatform_API.UnitOfWorks
{
    public class UnitOfWork
    {
        private MotqenDbContext db;
        private SubjectRepository subjectRepo;
        private StudentRepository studentRepo;
        private TeacherRepository teacherRepo;
        private ClassScheduleRepository classScheduleRepo;
        private HalaqaRepository halaqaRepo;
        private HalaqaStudentRepository halaqaStudentRepo;
        private HalaqaTeacherRepository halaqaTeacherRepo;
        private IslamicSubjectsProgressTrackingRepository islamicSubjectsProgressTrackingRepo;
        private ParentRepository parentRepo;
        private ProgressTrackingRepository progressTrackingRepo;
        private QuranProgressTrackingRepository quranProgressTrackingRepo;
        private StudentAttendanceRepository studentAttendanceRepo;
        private StudentSubjectRepository studentSubjectRepo;
        private TeacherAttendanceRepository teacherAttendanceRepo;
        private TeacherSubjectRepository teacherSubjectRepo;
        private AuthRepository authRepo;
        private UserRepository userRepo;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;


        public UnitOfWork(
            MotqenDbContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        public UserRepository UserRepo
        {
            get
            {
                if (userRepo == null)
                {
                    userRepo = new UserRepository(db, userManager);
                }
                return this.userRepo;
            }
        }

        public AuthRepository AuthRepo
        {
            get
            {
                if (authRepo == null)
                {
                    authRepo = new AuthRepository(db, userManager, roleManager, configuration);
                }
                return this.authRepo;
            }
        }

        public StudentRepository StudentRepo
        {
            get
            {
                if (studentRepo == null)
                {
                    studentRepo = new StudentRepository(db);
                }
                return this.studentRepo;
            }
        }
        public TeacherRepository TeacherRepo
        {
            get
            {
                if (teacherRepo == null)
                {
                    teacherRepo = new TeacherRepository(db);
                }
                return this.teacherRepo;
            }
        }
        public ClassScheduleRepository ClassScheduleRepo
        {
            get
            {
                if (classScheduleRepo == null)
                {
                    classScheduleRepo = new ClassScheduleRepository(db);
                }
                return this.classScheduleRepo;
            }
        }
        public HalaqaRepository HalaqaRepo
        {
            get
            {
                if (halaqaRepo == null)
                {
                    halaqaRepo = new HalaqaRepository(db);
                }
                return this.halaqaRepo;
            }
        }
        public HalaqaStudentRepository HalaqaStudentRepo
        {
            get
            {
                if (halaqaStudentRepo == null)
                {
                    halaqaStudentRepo = new HalaqaStudentRepository(db);
                }
                return this.halaqaStudentRepo;
            }
        }
        public HalaqaTeacherRepository HalaqaTeacherRepo
        {
            get
            {
                if (halaqaTeacherRepo == null)
                {
                    halaqaTeacherRepo = new HalaqaTeacherRepository(db);
                }
                return this.halaqaTeacherRepo;
            }
        }
        public IslamicSubjectsProgressTrackingRepository IslamicSubjectsProgressTrackingRepo
        {
            get
            {
                if (islamicSubjectsProgressTrackingRepo == null)
                {
                    islamicSubjectsProgressTrackingRepo = new IslamicSubjectsProgressTrackingRepository(db);
                }
                return this.islamicSubjectsProgressTrackingRepo;
            }
        }
        public ParentRepository ParentRepo
        {
            get
            {
                if (parentRepo == null)
                {
                    parentRepo = new ParentRepository(db);
                }
                return this.parentRepo;
            }
        }
        public ProgressTrackingRepository ProgressTrackingRepo
        {
            get
            {
                if (progressTrackingRepo == null)
                {
                    progressTrackingRepo = new ProgressTrackingRepository(db);
                }
                return this.progressTrackingRepo;
            }
        }
        public QuranProgressTrackingRepository QuranProgressTrackingRepo
        {
            get
            {
                if (quranProgressTrackingRepo == null)
                {
                    quranProgressTrackingRepo = new QuranProgressTrackingRepository(db);
                }
                return this.quranProgressTrackingRepo;
            }
        }
        public StudentAttendanceRepository StudentAttendanceRepo
        {
            get
            {
                if (studentAttendanceRepo == null)
                {
                    studentAttendanceRepo = new StudentAttendanceRepository(db);
                }
                return this.studentAttendanceRepo;
            }
        }
        public TeacherAttendanceRepository TeacherAttendanceRepo
        {
            get
            {
                if (teacherAttendanceRepo == null)
                {
                    teacherAttendanceRepo = new TeacherAttendanceRepository(db);
                }
                return this.teacherAttendanceRepo;
            }
        }

        public StudentSubjectRepository StudentSubjectRepo
        {
            get
            {
                if (studentSubjectRepo == null)
                {
                    studentSubjectRepo = new StudentSubjectRepository(db);
                }
                return this.studentSubjectRepo;
            }
        }
        public SubjectRepository SubjectRepo
        {
            get
            {
                if (subjectRepo == null)
                {
                    subjectRepo = new SubjectRepository(db);
                }
                return this.subjectRepo;
            }
        }
        public TeacherSubjectRepository TeacherSubjectRepo
        {
            get
            {
                if (teacherSubjectRepo == null)
                {
                    teacherSubjectRepo = new TeacherSubjectRepository(db);
                }
                return this.teacherSubjectRepo;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
