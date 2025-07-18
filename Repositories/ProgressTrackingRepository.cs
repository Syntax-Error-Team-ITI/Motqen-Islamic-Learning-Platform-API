using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs;
using MotqenIslamicLearningPlatform_API.Enums;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class ProgressTrackingRepository : GenericRepository<ProgressTracking>
    {
        public ProgressTrackingRepository(MotqenDbContext db) : base(db)
        {
        }

        public ICollection<ProgressTracking> GetAllProgressForSpecificStudent(int studentId, bool includeDeleted = false)
        {
            var progressTrackings = Db.ProgressTrackings
                .Include(pt => pt.QuranProgressTrackingDetail)
                .Include(pt => pt.IslamicSubjectsProgressTrackingDetail)
                .Include(s => s.Student)
                .ThenInclude(st => st.User)
                .Include(pt => pt.Halaqa)
                .ThenInclude(h => h.Subject)
                .Where(pt => pt.StudentId == studentId)
                .ToList();
            if (!includeDeleted)
            {
                return progressTrackings.Where(pt => !pt.IsDeleted).ToList();
            }
            return progressTrackings;
        }

        public ICollection<ProgressTracking> GetAllProgressForSpecificHalaqa(int halaqaId, bool includeDeleted = false)
        {
            var progressTrackings = Db.ProgressTrackings
                .Include(pt => pt.QuranProgressTrackingDetail)
                .Include(pt => pt.IslamicSubjectsProgressTrackingDetail)
                .Include(s => s.Student)
                .ThenInclude(st => st.User)
                .Include(pt => pt.Halaqa)
                .ThenInclude(h => h.Subject)
                .Where(pt => pt.HalaqaId == halaqaId)
                .ToList();
            if (!includeDeleted)
            {
                return progressTrackings.Where(pt => !pt.IsDeleted).ToList();
            }
            return progressTrackings;
        }

        public ProgressTracking? GetProgressByStudentIdAndHalaqaId(int studentId, int halaqaId)
        {
            return Db.ProgressTrackings
                .Include(pt => pt.QuranProgressTrackingDetail)
                .Include(pt => pt.IslamicSubjectsProgressTrackingDetail)
                .Include(s => s.Student)
                .ThenInclude(st => st.User)
                .Include(pt => pt.Halaqa)
                .ThenInclude(h => h.Subject)
                .FirstOrDefault(pt => pt.StudentId == studentId && pt.HalaqaId == halaqaId);
        }
        public bool IsQuranProgressValid(ProgressFormDTO progress)
        {
            return progress.FromSurah != null
                && progress.ToSurah != null
                && progress.FromAyah != null
                && progress.ToAyah != null
                && progress.NumberOfLines != null
                && (progress.Type == ProgressType.Memorization || progress.Type == ProgressType.Review);
        }
        public bool IsIslamicProgressValid(ProgressFormDTO progress)
        {
            return progress.FromPage != null
                && progress.ToPage != null
                && progress.Subject != null
                && progress.LessonName != null;
        }
    }
}
