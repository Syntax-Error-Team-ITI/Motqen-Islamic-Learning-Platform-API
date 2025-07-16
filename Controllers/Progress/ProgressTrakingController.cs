using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.ProgressDTOs;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Progress
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressTrakingController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;
        public ProgressTrakingController(UnitOfWork _unit , IMapper _map)
        {
            unit = _unit;
            mapper = _map;
        }

        [HttpGet("student/{studentId}")]
        public IActionResult GetAllProgressForStudent(int studentId, bool includeDeleted = false)
        {
            var progress = unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId, includeDeleted);
            if (progress == null || !progress.Any())
                return NotFound(new { message = "No progress found for this student" });
            return Ok(mapper.Map<List<ProgressListDTO>>(progress));
        }

        [HttpGet("halaqa/{halaqaId}")]
        public IActionResult GetAllProgressForHalaqa(int halaqaId, bool includeDeleted = false)
        {
            var progress = unit.ProgressTrackingRepo.GetAllProgressForSpecificHalaqa(halaqaId, includeDeleted);
            if (progress == null || !progress.Any())
                return NotFound(new { message = "No progress found for this halaqa" });
            return Ok(mapper.Map<List<ProgressListDTO>>(progress));
        }

        [HttpGet("student/{studentId}/halaqa/{halaqaId}")]
        public IActionResult GetProgresForHalaqa(int studentId, int halaqaId)
        {
            var progress = unit.ProgressTrackingRepo.GetProgressByStudentIdAndHalaqaId(studentId, halaqaId);
            if (progress == null)
                return NotFound(new { message = "No Progress for this student at this halaqa" });
            return Ok(mapper.Map<ProgressListDTO>(progress));
        }

    }
}
