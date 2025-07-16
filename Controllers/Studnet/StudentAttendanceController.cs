using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs.StudentAttendanceDtos;
using MotqenIslamicLearningPlatform_API.Migrations;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }

        public StudentAttendanceController(IMapper _mapper , UnitOfWork _unit)
        {
            this.Unit = _unit;
            this.Mapper = _mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var attendances = Unit.StudentAttendanceRepo.GetAllWithInclude();
            var result = Mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var attendance = Unit.StudentAttendanceRepo.GetByIdWithInclude(id);
            if (attendance == null)
                return NotFound();

            var result = Mapper.Map<StudentAttendanceDto>(attendance);
            return Ok(result);
        }

        [HttpGet("byStudentId/{studentId:int}")]
        public IActionResult GetByStudentId(int studentId)
        {
            var attendance = Unit.StudentAttendanceRepo.GetByStudentId(studentId);
            var result = Mapper.Map<IEnumerable<StudentAttendanceDto>>(attendance);
            return Ok(result);
        }

        [HttpGet("byHalaqaId/{halaqaId:int}")]
        public IActionResult GetByHalaqaId(int halaqaId)
        {
            var attendance = Unit.StudentAttendanceRepo.GetByHalaqaId(halaqaId);
            var result = Mapper.Map<IEnumerable<StudentAttendanceDto>>(attendance);
            return Ok(result);
        }

        [HttpGet("byComposite/{studentId:int}/{halaqaId:int}")]
        public IActionResult GetByStudentIdAndHalaqaId(int studentId, int halaqaId)
        {
            var attendance = Unit.StudentAttendanceRepo.GetByStudentIdAndHalaqaId(studentId, halaqaId);
            var result = Mapper.Map<IEnumerable<StudentAttendanceDto>>(attendance);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CreateStudentAttendanceDto attendanceDto)
        {
            if (attendanceDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (attendanceDto.AttendanceDate > DateTime.UtcNow)
                return BadRequest("Attendance date cannot be in the future.");

            var existingHalaqa = Unit.HalaqaRepo.GetById(attendanceDto.HalaqaId);
            var existingStudent = Unit.StudentRepo.GetById(attendanceDto.StudentId);

            if (existingHalaqa == null || existingStudent == null)
                return NotFound("Halaqa or Student not found.");

            var existingAttendance = Unit.StudentAttendanceRepo
                .GetByComposite(attendanceDto.StudentId, attendanceDto.HalaqaId, attendanceDto.AttendanceDate);

            if (existingAttendance != null)
                return Conflict("Attendance record already exists for this student and halaqa.");

            var attendance = Mapper.Map<StudentAttendance>(attendanceDto);
            Unit.StudentAttendanceRepo.Add(attendance);
            Unit.Save();

            var result = Mapper.Map<StudentAttendanceDto>(attendance);
            return CreatedAtAction(nameof(GetById), new { id = attendance.Id }, result);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateStudentAttendanceDto attendanceDto)
        {
            if (attendanceDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAttendance = Unit.StudentAttendanceRepo.GetByIdWithInclude(id);
            if (existingAttendance == null)
                return NotFound();

            Mapper.Map(attendanceDto, existingAttendance);
            Unit.StudentAttendanceRepo.Edit(existingAttendance);
            Unit.Save();

            var result = Mapper.Map<StudentAttendanceDto>(existingAttendance);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var attendance = Unit.StudentAttendanceRepo.GetByIdWithInclude(id);
            if (attendance == null)
                return NotFound();

            Unit.StudentAttendanceRepo.HardDelete(id);
            Unit.Save();
            return NoContent();
        }
    }

}
