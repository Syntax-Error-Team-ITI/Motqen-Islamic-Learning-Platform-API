using Microsoft.EntityFrameworkCore;

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
        public HalaqaStudent HalaqaStudent { get; set; }
        public DbSet<HalaqaTeacher> HalaqaTeacher { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<TeacherAttendance> TeacherAttendances { get; set; }

        public DbSet<ProgressTracking> ProgressTrackings { get; set; }
        public DbSet<QuranProgressTracking> QuranProgressTrackings { get; set; }
        public DbSet<IslamicSubjectsProgressTracking> IslamicSubjectsProgressTrackings { get; set; }

    }
}
