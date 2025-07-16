using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.TeacherAttendanceDtos;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.TeacherCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAttendanceController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }

        public TeacherAttendanceController(IMapper _mapper, UnitOfWork _unit)
        {
            this.Unit = _unit;
            this.Mapper = _mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var attendances = Unit.TeacherAttendanceRepo.GetAllWithInclude();
            var result = Mapper.Map<IEnumerable<TeacherAttendanceDto>>(attendances);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var attendance = Unit.TeacherAttendanceRepo.GetByIdWithInclude(id);
            if (attendance == null)
                return NotFound();

            var result = Mapper.Map<TeacherAttendanceDto>(attendance);
            return Ok(result);
        }
        [HttpGet("byComposite/{teacherId:int}/{halaqaId:int}")]
        public IActionResult GetByTeacherIdAndHalaqaId(int teacherId, int halaqaId)
        {
            var attendance = Unit.TeacherAttendanceRepo.GetByTeacherIdAndHalaqaId(teacherId, halaqaId);
            
            var result = Mapper.Map< IEnumerable <TeacherAttendanceDto>>(attendance);
            return Ok(result);
        }

        [HttpGet("byTeacherId/{teacherId:int}")]
        public IActionResult GetByTeacherId(int teacherId)
        {
            var attendance = Unit.TeacherAttendanceRepo.GetByTeacherId(teacherId);

            var result = Mapper.Map<IEnumerable< TeacherAttendanceDto>>(attendance);
            return Ok(result);
        }
        [HttpGet("byHalaqaId/{halaqaId:int}")]
        public IActionResult GetByHalaqaId(int halaqaId)
        {
            var attendance = Unit.TeacherAttendanceRepo.GetByHalaqaId(halaqaId);

            var result = Mapper.Map<IEnumerable<TeacherAttendanceDto>>(attendance);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create(CreateTeacherAttendanceDto attendanceDto)
        {
            if (attendanceDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (attendanceDto.AttendanceDate > DateTime.UtcNow)
            {
                return BadRequest("Attendance date cannot be in the future.");
            }
            var existingHalaqa = Unit.HalaqaRepo.GetById(attendanceDto.HalaqaId);
            var existingTeacher = Unit.TeacherRepo.GetById(attendanceDto.TeacherId);
            if (existingHalaqa == null || existingTeacher == null)
            {
                return NotFound("Halaqa or Teacher not found.");
            }
            
            var existingAttendance = Unit.TeacherAttendanceRepo.GetByComposite(attendanceDto.TeacherId, attendanceDto.HalaqaId, attendanceDto.AttendanceDate);
            if (existingAttendance != null)
            {
                return Conflict("Attendance record already exists for this teacher and halaqa.");
            }
            
            var attendance = Mapper.Map<TeacherAttendance>(attendanceDto);
            Unit.TeacherAttendanceRepo.Add(attendance);
            Unit.Save();
            var result = Mapper.Map<TeacherAttendanceDto>(attendance);
            return CreatedAtAction(nameof(GetById), new {  id = attendance.Id}, result);
        }


        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateTeacherAttendanceDto attendanceDto)
        {
            if (attendanceDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingAttendance = Unit.TeacherAttendanceRepo.GetByIdWithInclude(id);
            if (existingAttendance == null)
            {
                return NotFound();
            }
            Mapper.Map(attendanceDto, existingAttendance);
            Unit.TeacherAttendanceRepo.Edit(existingAttendance);
            Unit.Save();
            var result = Mapper.Map<TeacherAttendanceDto>(existingAttendance);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var attendance = Unit.TeacherAttendanceRepo.GetByIdWithInclude( id );
            if (attendance == null)
            {
                return NotFound();
            }
           Unit.TeacherAttendanceRepo.HardDelete(id);
            Unit.Save();
            return NoContent();

        }
    }
}
