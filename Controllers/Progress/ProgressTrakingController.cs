using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        IWebHostEnvironment env;
        public ProgressTrakingController(UnitOfWork _unit, IMapper _map, IWebHostEnvironment env)
        {
            unit = _unit;
            mapper = _map;
            this.env = env;
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("student/{studentId}")]
        public IActionResult GetAllProgressForStudent(int studentId, bool includeDeleted = false)
        {
            var progress = unit.ProgressTrackingRepo.GetAllProgressForSpecificStudent(studentId, includeDeleted);
            if (progress == null || !progress.Any())
                return NotFound(new { message = "No progress found for this student" });
            return Ok(mapper.Map<List<ProgressListDTO>>(progress));
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("halaqa/{halaqaId}")]
        public IActionResult GetAllProgressForHalaqa(int halaqaId, bool includeDeleted = false)
        {
            var progress = unit.ProgressTrackingRepo.GetAllProgressForSpecificHalaqa(halaqaId, includeDeleted);
            if (progress == null || !progress.Any())
                return NotFound(new { message = "No progress found for this halaqa" });
            return Ok(mapper.Map<List<ProgressListDTO>>(progress));
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("student/{studentId}/halaqa/{halaqaId}")]
        public IActionResult GetProgresForHalaqa(int studentId, int halaqaId)
        {
            var progress = unit.ProgressTrackingRepo.GetProgressByStudentIdAndHalaqaId(studentId, halaqaId);
            if (progress == null)
                return NotFound(new { message = "No Progress for this student at this halaqa" });
            return Ok(mapper.Map<ProgressListDTO>(progress));
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult AddProgress(ProgressFormDTO progressFromReq)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
               
                var progress = mapper.Map<ProgressTracking>(progressFromReq);

                if (progressFromReq.IsQuranTracking)
                {
                    if (!unit.ProgressTrackingRepo.IsQuranProgressValid(progressFromReq))
                        return BadRequest(new { message = "Invalid Quran progress data. All Quran fields must be provided." });

                    progress.QuranProgressTrackingDetail = new QuranProgressTracking
                    {
                        FromAyah = progressFromReq.FromAyah.Value,
                        ToAyah = progressFromReq.ToAyah.Value,
                        FromSurah = progressFromReq.FromSurah.Value,
                        ToSurah = progressFromReq.ToSurah.Value,
                        Type = progressFromReq.Type.Value,
                        NumberOfLines = progressFromReq.NumberOfLines.Value,
                    };
                }
                else
                {
                    if (!unit.ProgressTrackingRepo.IsIslamicProgressValid(progressFromReq))
                    {
                        return BadRequest(new { message = "Invalid Islamic progress data. All subject fields must be provided." });
                    }

                    progress.IslamicSubjectsProgressTrackingDetail = new IslamicSubjectsProgressTracking
                    {
                        FromPage = progressFromReq.FromPage.Value,
                        ToPage = progressFromReq.ToPage.Value,
                        Subject = unit.HalaqaRepo.GetByIdIncludeSubject(progressFromReq.HalaqaId)?.Subject?.Name ?? "",
                        LessonName = progressFromReq.LessonName!
                    };
                }

                unit.ProgressTrackingRepo.Add(progress);
                var saveResult = unit.Save();

                if (saveResult <= 0)
                {
                    return StatusCode(500, "Failed to save progress tracking");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
            return Ok();
        }

        [Authorize (Roles = "Teacher")]
        [HttpDelete("Quran/{progressId}")]
        public IActionResult DeleteProgressAtQuran(int progressId)
        {
            try
            {
                unit.QuranProgressTrackingRepo.HardDelete(progressId);
                unit.Save();
            }catch(Exception e)
            {

                if (env.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while deleting the progress",
                        Error = e.Message
                    });
                }

                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the progress",
                });
            }

            return Ok("Progress Deleted Successfully");
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("IslamicMatrial/{progressId}")]
        public IActionResult DeleteProgressAtIslamicMaterial(int progressId)
        {
            try
            {
                unit.IslamicSubjectsProgressTrackingRepo.HardDelete(progressId);
                unit.Save();
            }
            catch (Exception e)
            {
                if (env.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while deleting the progress",
                        Error = e.Message
                    });
                }
                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the progress",
                });
            }
            return Ok("Progress Deleted Successfully");
        }

        [Authorize(Roles = "Teacher")]

        [HttpPut("Quran/{progressId}")]
        public IActionResult UpdateProgressAtQuran(int progressId, ProgressFormDTO progressFromReq)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var progress = unit.QuranProgressTrackingRepo.GetById(progressId);
                if (progress == null)
                    return NotFound(new { message = "Progress not found" });
                if (!unit.ProgressTrackingRepo.IsQuranProgressValid(progressFromReq))
                    return BadRequest(new { message = "Invalid Quran progress data. All Quran fields must be provided." });
                progress.FromAyah = progressFromReq.FromAyah.Value;
                progress.ToAyah = progressFromReq.ToAyah.Value;
                progress.FromSurah = progressFromReq.FromSurah.Value;
                progress.ToSurah = progressFromReq.ToSurah.Value;
                progress.Type = progressFromReq.Type.Value;
                progress.NumberOfLines = progressFromReq.NumberOfLines.Value;
                unit.QuranProgressTrackingRepo.Edit(progress);
                unit.Save();
            }
            catch (Exception e)
            {
                if (env.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while updating the progress",
                        Error = e.Message
                    });
                }
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the progress",
                });
            }
            return Ok("Progress Updated Successfully");
        }

        [Authorize(Roles = "Teacher")]

        [HttpPut("IslamicMaterial/{progressId}")]
        public IActionResult UpdateProgressAtIslamicMaterial(int progressId, ProgressFormDTO progressFromReq)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var progress = unit.IslamicSubjectsProgressTrackingRepo.GetById(progressId);
                if (progress == null)
                    return NotFound(new { message = "Progress not found" });
                if (!unit.ProgressTrackingRepo.IsIslamicProgressValid(progressFromReq))
                    return BadRequest(new { message = "Invalid Islamic progress data. All subject fields must be provided." });
                progress.FromPage = progressFromReq.FromPage.Value;
                progress.ToPage = progressFromReq.ToPage.Value;
                progress.Subject = unit.HalaqaRepo.GetByIdIncludeSubject(progressFromReq.HalaqaId)?.Subject?.Name ?? "";
                progress.LessonName = progressFromReq.LessonName!;
                unit.IslamicSubjectsProgressTrackingRepo.Edit(progress);
                unit.Save();
            }
            catch (Exception e)
            {
                if (env.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        Message = "An error occurred while updating the progress",
                        Error = e.Message
                    });
                }
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the progress",
                });
            }
            return Ok("Progress Updated Successfully");
        }
    }
}
