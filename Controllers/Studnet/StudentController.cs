using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }

        public StudentController(UnitOfWork unit, IMapper mapper)
        {
            Unit = unit;
            Mapper = mapper;
        }

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult GetAllStudents(bool includeDeleted = false)
        {
            var students = Unit.StudentRepo.GetAllWithMinimalDataInclude(0,includeDeleted);

            if (students == null || !students.Any())
                return NotFound(new { message = "No students found" });

            return Ok(Mapper.Map<List<StudentListDTO>>(students));
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("notInHalaqa/{halaqaId:int}")]
        public IActionResult GetStudentNotInHalqa(int halaqaId, bool includeDeleted = false)
        {
            var students = Unit.StudentRepo.GetAllWithMinimalDataInclude(halaqaId, includeDeleted);

            if (students == null || !students.Any())
                return NotFound(new { message = "No students found" });

            return Ok(Mapper.Map<List<StudentShortDisplayDTO>>(students));
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("{studentId}")]
        public IActionResult GetSpecificStudentDetails(int studentId , bool includeDeleted = false)
        {
            var student = Unit.StudentRepo.GetSpecificStudentDetailsById(studentId, includeDeleted);
            if (student == null)
                return NotFound(new { message = "Student Not Found!!" });
            return Ok(Mapper.Map<StudentDetailedDisplayDTO>(student));
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqa/{halaqaId}/all-students")]
        public IActionResult getAllStudentsForHalaqa(int halaqaId,bool includeDeleted = false)
        {
            var students = Unit.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqaId: halaqaId);
            return Ok(Mapper.Map<List<StudentHalaqaDisplayDTO>>(students));
        }
        [Authorize(Roles = "Admin , Student")]

        [HttpGet("{studentId}/all-halaqa")]
        public IActionResult getAllHalaqaForStudent(int studentId,bool includeDeleted = false)
        {
            ICollection<Models.HalaqaModel.HalaqaStudent>? halaqa = Unit.HalaqaStudentRepo.getAllHalaqaByStudentId(studentId: studentId);
            if (halaqa == null)
                return NotFound();
            return Ok(Mapper.Map<List<HalaqaStudentDisplayHalaqaDTO>>(halaqa));
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
