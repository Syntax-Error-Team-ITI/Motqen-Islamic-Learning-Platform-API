using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;

namespace MotqenIslamicLearningPlatform_API.Models
{
    public class MotqenDbContext : DbContext
    {
        public MotqenDbContext(DbContextOptions<MotqenDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Halaqa> Halaqas { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<HalaqaStudent> HalaqaStudent { get; set; }
        public DbSet<HalaqaTeacher> HalaqaTeacher { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<TeacherAttendance> TeacherAttendances { get; set; }
        public DbSet<ProgressTracking> ProgressTrackings { get; set; }
        public DbSet<QuranProgressTracking> QuranProgressTrackings { get; set; }
        public DbSet<IslamicSubjectsProgressTracking> IslamicSubjectsProgressTrackings { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index on TeacherAttendance (TeacherId, HalaqaId, AttendanceDate)
            modelBuilder.Entity<TeacherAttendance>()
                .HasIndex(a => new { a.TeacherId, a.HalaqaId, a.AttendanceDate })
                .IsUnique();
        }
    }
}
