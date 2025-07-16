using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherSubjectDtos;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.TeacherCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherSubjectController : ControllerBase
    {
         IMapper Mapper;
         UnitOfWork Unit;
        public TeacherSubjectController(IMapper _mapper , UnitOfWork _unit)
        {
            this.Mapper = _mapper;
            this.Unit = _unit;

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var teacherSubjects = Unit.TeacherSubjectRepo.GetAllWithInclude();
            if (teacherSubjects == null || !teacherSubjects.Any())
            {
                return NotFound("No teacher subjects found.");
            }
            var result = Mapper.Map<List<TeacherSubjectDto>>(teacherSubjects);
            return Ok(result);
        }
        [HttpGet("byTeacherId/{teacherId:int}")]
        public IActionResult  GetByTeacherId(int teacherId)
        {
            var teacherSubjects = Unit.TeacherSubjectRepo.GetByTeacherId(teacherId);
            if (teacherSubjects == null || !teacherSubjects.Any())
            {
                return NotFound($"No subjects found for teacher with ID {teacherId}.");
            }
            var result = Mapper.Map<List<TeacherSubjectDto>>(teacherSubjects);
            return Ok(result);
        }
        [HttpGet("bySubjectId/{subjectId:int}")]
        public IActionResult GetBySubjectId(int subjectId)
        {
            var teacherSubjects = Unit.TeacherSubjectRepo.GetBySubjectId(subjectId);
            if (teacherSubjects == null || !teacherSubjects.Any())
            {
                return NotFound($"No teachers found for subject with ID {subjectId}.");
            }
            var result = Mapper.Map<List<TeacherSubjectDto>>(teacherSubjects);
            return Ok(result);
        }
        [HttpGet("byComposite/{teacherId:int}/{subjectId:int}")]
        public IActionResult GetByIds(int teacherId, int subjectId)
        {
            var teacherSubject = Unit.TeacherSubjectRepo.GetByIds(teacherId, subjectId);
            if (teacherSubject == null)
            {
                return NotFound($"No teacher-subject relationship found for Teacher ID {teacherId} and Subject ID {subjectId}.");
            }
            var result = Mapper.Map<TeacherSubjectDto>(teacherSubject);
            return Ok(result);
        }
        [HttpPost()]
        public IActionResult Create( CreateTeacherSubjectDto teacherSubjectDto)
        {
            if (teacherSubjectDto == null)
            {
                return BadRequest("TeacherSubject data is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingTeacherSubject = Unit.TeacherSubjectRepo.GetByIds(teacherSubjectDto.TeacherId, teacherSubjectDto.SubjectId);
            if (existingTeacherSubject != null)
            {
                return Conflict($"Teacher with ID {teacherSubjectDto.TeacherId} is already assigned to Subject with ID {teacherSubjectDto.SubjectId}.");
            }
            var existingTeacher = Unit.TeacherRepo.GetById(teacherSubjectDto.TeacherId);
            if (existingTeacher == null)
            {
                return NotFound($"Teacher with ID {teacherSubjectDto.TeacherId} not found.");
            }
            var existingSubject = Unit.SubjectRepo.GetById(teacherSubjectDto.SubjectId);
            if (existingSubject == null)
            {
                return NotFound($"Subject with ID {teacherSubjectDto.SubjectId} not found.");
            }
            var teacherSubject = Mapper.Map<Models.TeacherModel.TeacherSubject>(teacherSubjectDto);
            Unit.TeacherSubjectRepo.Add(teacherSubject);
            Unit.Save();
            return CreatedAtAction(nameof(GetByIds), new { teacherId = teacherSubject.TeacherId, subjectId = teacherSubject.SubjectId }, teacherSubjectDto);
        }
        [HttpDelete("delete/{teacherId:int}/{subjectId:int}")]
        public IActionResult Delete(int teacherId , int subjectId)
        {
            var teacherSubject = Unit.TeacherSubjectRepo.GetByIds(teacherId, subjectId);
            if (teacherSubject == null)
            {
                return NotFound($"No teacher-subject relationship found for Teacher ID {teacherId} and Subject ID {subjectId}.");
            }
            Unit.TeacherSubjectRepo.Delete(teacherId, subjectId);
            Unit.Save();
            return NoContent();
        }




    }
}
