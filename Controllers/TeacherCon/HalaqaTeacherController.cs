using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs.HalaqaTeacherDtos;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.TeacherCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalaqaTeacherController : ControllerBase
    {
        IMapper Mapper;
        UnitOfWork Unit;
        public HalaqaTeacherController(IMapper _mapper, UnitOfWork _unit)
        {
            this.Mapper = _mapper;
            this.Unit = _unit;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public IActionResult GetAll()
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetAllWithIncludes();
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound("No Halaqa Teachers found.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("teacher/{teacherId}")]
        public IActionResult GetByTeacherId(int teacherId)
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetByTeacherId(teacherId);
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound($"No Halaqa Teachers found for Teacher ID: {teacherId}.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqa/{halaqaId}")]
        public IActionResult GetByHalaqaId(int halaqaId)
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetByHalaqaId(halaqaId);
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound($"No Halaqa Teachers found for Halaqa ID: {halaqaId}.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqaNotAssignForTeacher/{teacherId}")]
        public IActionResult GetHalaqasNotAssignToTeacher(int teacherId)
        {
            var halaqas = Unit.HalaqaRepo.GetHalaqasNotAssignToTeacher(teacherId);
            return Ok(Mapper.Map<List<HalaqaNamesListDTO>>(halaqas));
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqa/{halaqaId}/notAssignToTeacher")]
        public IActionResult GetTeachersNotAssignedToHalaqa(int halaqaId)
        {
            var teachers = Unit.HalaqaTeacherRepo.GetTeachersNotAssignedToHalaqa(halaqaId);
            if (teachers == null || teachers.Count == 0)
                return Ok(new List<TeacherDto>());
            var teacherDtos = Mapper.Map<List<TeacherDto>>(teachers);
            return Ok(teacherDtos);
        }
        //get teacher assigned to halaqa by halaqaId
        [Authorize(Roles = "Admin")]

        [HttpGet("halaqa/{halaqaId}/assignedTeachers")]
        public IActionResult GetTeachersAssignedToHalaqa(int halaqaId)
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetTeachersAssignedToHalaqa(halaqaId);
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return Ok(new List<TeacherDto>());
            var TeacherDtos = Mapper.Map<List<TeacherDto>>(halaqaTeachers);
            return Ok(TeacherDtos);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("teacher/{teacherId}/halaqa/{halaqaId}")]
        public IActionResult GetByIds(int teacherId, int halaqaId)
        {
            var halaqaTeacher = Unit.HalaqaTeacherRepo.GetByIds(teacherId, halaqaId);
            if (halaqaTeacher == null)
                return NotFound($"No Halaqa Teacher found for Teacher ID: {teacherId} and Halaqa ID: {halaqaId}.");
            var halaqaTeacherDto = Mapper.Map<HalaqaTeacherDto>(halaqaTeacher);
            return Ok(halaqaTeacherDto);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost()]
        public IActionResult Create(CreateHalaqaTeacherDto createHalaqaTeacherDto)
        {
            if (createHalaqaTeacherDto == null)
                return BadRequest("Invalid data.");
            var existHalaqa = Unit.HalaqaRepo.GetById(createHalaqaTeacherDto.HalaqaId);
            if (existHalaqa == null || existHalaqa.IsDeleted)
            {
                return NotFound($"Halaqa with ID: {createHalaqaTeacherDto.HalaqaId} does not exist or has been deleted.");
            }
            var existTeacher = Unit.TeacherRepo.GetById(createHalaqaTeacherDto.TeacherId);
            if (existTeacher == null || existTeacher.IsDeleted)
            {
                return NotFound($"Teacher with ID: {createHalaqaTeacherDto.TeacherId} does not exist or has been deleted.");
            }
            if (Unit.HalaqaTeacherRepo.Exists(createHalaqaTeacherDto.TeacherId, createHalaqaTeacherDto.HalaqaId))
            {
                return Conflict($"Halaqa Teacher with Teacher ID: {createHalaqaTeacherDto.TeacherId} and Halaqa ID: {createHalaqaTeacherDto.HalaqaId} already exists.");
            }
            var halaqaTeacher = Mapper.Map<HalaqaTeacher>(createHalaqaTeacherDto);
            Unit.HalaqaTeacherRepo.Add(halaqaTeacher);
            Unit.Save();
            return CreatedAtAction(nameof(GetByIds), new { teacherId = halaqaTeacher.TeacherId, halaqaId = halaqaTeacher.HalaqaId }, Mapper.Map<HalaqaTeacherDto>(halaqaTeacher));
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("teacher/{teacherId}/halaqa/{halaqaId}")]
        public IActionResult Delete(int teacherId, int halaqaId)
        {
            if (!Unit.HalaqaTeacherRepo.Exists(teacherId, halaqaId))
            {
                return NotFound($"Halaqa Teacher with Teacher ID: {teacherId} and Halaqa ID: {halaqaId} does not exist.");
            }
            Unit.HalaqaTeacherRepo.Delete(teacherId, halaqaId);
            Unit.Save();
            return NoContent();
        }
    }
}
