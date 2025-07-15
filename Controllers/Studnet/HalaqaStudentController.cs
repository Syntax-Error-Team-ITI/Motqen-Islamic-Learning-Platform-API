using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalaqaStudentController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }
        public HalaqaStudentController(UnitOfWork unit, IMapper mapper)
        {
            Unit = unit;
            Mapper = mapper;
        }
        [HttpPost]
        public IActionResult assignStudentToHalaqa(StudentHalaqaFormDTO studentHalaqaFromReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = Unit.StudentRepo.GetById(studentHalaqaFromReq.StudentId);
            if (student == null)
                return NotFound(new { message = "Student Not Found!!" });
            var halaqa = Unit.HalaqaRepo.GetById(studentHalaqaFromReq.HalaqaId);
            if (halaqa == null)
                return NotFound(new { message = "Halaqa Not Found!!" });
            var studentHalaqaFromDB = Unit.HalaqaStudentRepo.getStudentByHalaqaId(studentHalaqaFromReq.StudentId, studentHalaqaFromReq.HalaqaId);
            if (studentHalaqaFromDB != null)
                return BadRequest(new { message = "The student is already assign to this halaqa!!" });
            Unit.HalaqaStudentRepo.Add(Mapper.Map<HalaqaStudent>(studentHalaqaFromReq));
            Unit.Save();
            return Ok();
        }
        [HttpDelete]
        public IActionResult RemoveStudentFromHalaqa(int studentId, int halaqaId)
        {
            var studentHalaqaFromDB = Unit.HalaqaStudentRepo.getStudentByHalaqaId(studentId, halaqaId);
            if (studentHalaqaFromDB == null)
                return BadRequest(new { message = "The student is not assign to this halaqa!!" });
            Unit.HalaqaStudentRepo.RemoveStudentFromHalaqa(studentId, halaqaId);
            Unit.Save();
            return Ok();
        }
    }


}
