using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.TeacherCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }
        public TeacherController(UnitOfWork _unit, IMapper _mapper)
        {
            this.Unit = _unit;
            this.Mapper = _mapper;
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult GetAll(bool includeDelete = false)
        {
            var teachers = Unit.TeacherRepo.GetAll(includeDelete);
            var result = Mapper.Map<IEnumerable<TeacherDto>>(teachers);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var teacher = Unit.TeacherRepo.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            var result = Mapper.Map<TeacherDto>(teacher);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("deleted")]
        public IActionResult GetDeletedTeachers()
        {
            var deletedTeachers = Unit.TeacherRepo.GetDeleted();
            var result = Mapper.Map<IEnumerable<TeacherDto>>(deletedTeachers);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public IActionResult Create(CreateTeacherDto teacherDto)
        {
            if(teacherDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teacher = Mapper.Map<Teacher>(teacherDto);
            Unit.TeacherRepo.Add(teacher);
            Unit.Save();
            var result = Mapper.Map<TeacherDto>(teacher);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, result);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{id:int}")]
        public IActionResult Update(int id , UpdateTeacherDto teacherDto)
        {
            if(teacherDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingTeacher = Unit.TeacherRepo.GetById(id);
            if (existingTeacher == null)
            {
                return NotFound();
            }
            if(teacherDto.Id != id)
            {
                return BadRequest(new { message = "Teacher ID mismatch!" });
            }
            Mapper.Map(teacherDto, existingTeacher);
            Unit.TeacherRepo.Edit(existingTeacher);
            Unit.Save();
            var result = Mapper.Map<TeacherDto>(existingTeacher);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = Unit.TeacherRepo.SoftDelete(id);
            if (!success)
            {
                return NotFound(new { message = "Teacher Not Found!!" });
            }
            Unit.Save();
            return Ok(new { message = "Teacher deleted successfully" });
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("restore/{id:int}")]
        public IActionResult Restore(int id)
        {
            var teacher = Unit.TeacherRepo.GetById(id);
            if (teacher == null)
            {
                return NotFound(new { message = "Teacher Not Found!!" });
            }
            if (!teacher.IsDeleted )
            {
                return BadRequest(new { message = "Teacher is not deleted!" });
            }
            Unit.TeacherRepo.Restor(teacher.Id);
            Unit.Save();
            return Ok(new { message = "Teacher restored successfully"});
        }

    }
}
