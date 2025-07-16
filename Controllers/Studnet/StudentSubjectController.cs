using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentSubjectDtos;
using MotqenIslamicLearningPlatform_API.Migrations;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectController : ControllerBase
    {
        IMapper Mapper;
        UnitOfWork Unit;
        public StudentSubjectController(IMapper _mapper, UnitOfWork _unit)
        {
            this.Mapper = _mapper;
            this.Unit = _unit;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var studentSubjects = Unit.StudentSubjectRepo.GetAllWithInclude();
            if (studentSubjects == null || !studentSubjects.Any())
                return NotFound("No student subjects found.");

            var result = Mapper.Map<List<StudentSubjectDto>>(studentSubjects);
            return Ok(result);
        }

        [HttpGet("byStudentId/{studentId:int}")]
        public IActionResult GetByStudentId(int studentId)
        {
            var studentSubjects = Unit.StudentSubjectRepo.GetByStudentId(studentId);
            if (studentSubjects == null || !studentSubjects.Any())
                return NotFound($"No subjects found for student with ID {studentId}.");

            var result = Mapper.Map<List<StudentSubjectDto>>(studentSubjects);
            return Ok(result);
        }

        [HttpGet("bySubjectId/{subjectId:int}")]
        public IActionResult GetBySubjectId(int subjectId)
        {
            var studentSubjects = Unit.StudentSubjectRepo.GetBySubjectId(subjectId);
            if (studentSubjects == null || !studentSubjects.Any())
                return NotFound($"No students found for subject with ID {subjectId}.");

            var result = Mapper.Map<List<StudentSubjectDto>>(studentSubjects);
            return Ok(result);
        }

        [HttpGet("byComposite/{studentId:int}/{subjectId:int}")]
        public IActionResult GetByIds(int studentId, int subjectId)
        {
            var studentSubject = Unit.StudentSubjectRepo.GetByIds(studentId, subjectId);
            if (studentSubject == null)
                return NotFound($"No student-subject relationship found for Student ID {studentId} and Subject ID {subjectId}.");

            var result = Mapper.Map<StudentSubjectDto>(studentSubject);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CreateStudentSubjectDto studentSubjectDto)
        {
            if (studentSubjectDto == null)
                return BadRequest("StudentSubject data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingStudentSubject = Unit.StudentSubjectRepo.GetByIds(studentSubjectDto.StudentId, studentSubjectDto.SubjectId);
            if (existingStudentSubject != null)
                return Conflict($"Student with ID {studentSubjectDto.StudentId} is already assigned to Subject with ID {studentSubjectDto.SubjectId}.");

            var existingStudent = Unit.StudentRepo.GetById(studentSubjectDto.StudentId);
            if (existingStudent == null)
                return NotFound($"Student with ID {studentSubjectDto.StudentId} not found.");

            var existingSubject = Unit.SubjectRepo.GetById(studentSubjectDto.SubjectId);
            if (existingSubject == null)
                return NotFound($"Subject with ID {studentSubjectDto.SubjectId} not found.");

            var studentSubject = Mapper.Map<Models.StudentModel.StudentSubject>(studentSubjectDto);
            Unit.StudentSubjectRepo.Add(studentSubject);
            Unit.Save();

            return CreatedAtAction(nameof(GetByIds), new { studentId = studentSubject.StudentId, subjectId = studentSubject.SubjectId }, studentSubjectDto);
        }

        [HttpDelete("delete/{studentId:int}/{subjectId:int}")]
        public IActionResult Delete(int studentId, int subjectId)
        {
            var studentSubject = Unit.StudentSubjectRepo.GetByIds(studentId, subjectId);
            if (studentSubject == null)
                return NotFound($"No student-subject relationship found for Student ID {studentId} and Subject ID {subjectId}.");

            Unit.StudentSubjectRepo.Delete(studentId, subjectId);
            Unit.Save();

            return NoContent();
        }
    }
}
