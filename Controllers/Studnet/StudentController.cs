using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }
        public StudentController(UnitOfWork unit, IMapper mapper)
        {
            Unit = unit;
            Mapper = mapper;
        }
        [HttpGet("{halaqaId}")]
        public IActionResult getAllStudentsForHalaqa(int halaqaId,bool includeDeleted = false)
        {
            var students = Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(parentId: halaqaId);
            return Ok(Mapper.Map<List<StudentHalaqaDisplayDTO>>(students));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var student = Unit.StudentRepo.GetById(id);
            if (student == null)
                return NotFound(new { message = "Student Not Found!!" });
            Unit.StudentRepo.SoftDelete(id);
            Unit.Save();
            return Ok();
        }
        [HttpPut("restore")]
        public IActionResult Restore(int id)
        {
            var student = Unit.StudentRepo.GetById(id);
            if (student == null)
                return NotFound(new { message = "Student Not Found!!" });
            if (!student.IsDeleted)
                return BadRequest(new {message = "Student is already deleted"});
            Unit.StudentRepo.Restor(id);
            Unit.Save();
            return Ok();
        }
       
    }
}
