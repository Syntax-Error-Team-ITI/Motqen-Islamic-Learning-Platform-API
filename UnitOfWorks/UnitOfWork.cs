using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Repositories;

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
        private IslamicSubjectsProgressTrackingRepository  islamicSubjectsProgressTrackingRepo;
        private ParentRepository parentRepo;
        private ProgressTrackingRepository progressTrackingRepo;
        private QuranProgressTrackingRepository quranProgressTrackingRepo;
        private StudentAttendanceRepository studentAttendanceRepo;
        private StudentSubjectRepository studentSubjectRepo;

        private TeacherAttendanceRepository teacherAttendanceRepo;

        private TeacherRepository teacherRepository;
        public UnitOfWork(MotqenDbContext db)
        {
            this.db = db;
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
        public TeacherRepository TeacherRepository
        {
            get
            {
                if (teacherRepository == null)
                {
                    teacherRepository = new TeacherRepository(db);
                }
                return this.teacherRepository;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}
