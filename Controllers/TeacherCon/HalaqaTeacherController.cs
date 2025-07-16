using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet()]
        public IActionResult GetAll()
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetAllWithIncludes();
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound("No Halaqa Teachers found.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [HttpGet("teacher/{teacherId}")]
        public IActionResult GetByTeacherId(int teacherId)
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetByTeacherId(teacherId);
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound($"No Halaqa Teachers found for Teacher ID: {teacherId}.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [HttpGet("halaqa/{halaqaId}")]
        public IActionResult GetByHalaqaId(int halaqaId)
        {
            var halaqaTeachers = Unit.HalaqaTeacherRepo.GetByHalaqaId(halaqaId);
            if (halaqaTeachers == null || halaqaTeachers.Count == 0)
                return NotFound($"No Halaqa Teachers found for Halaqa ID: {halaqaId}.");
            var halaqaTeacherDtos = Mapper.Map<List<HalaqaTeacherDto>>(halaqaTeachers);
            return Ok(halaqaTeacherDtos);
        }
        [HttpGet("teacher/{teacherId}/halaqa/{halaqaId}")]
        public IActionResult GetByIds(int teacherId, int halaqaId)
        {
            var halaqaTeacher = Unit.HalaqaTeacherRepo.GetByIds(teacherId, halaqaId);
            if (halaqaTeacher == null)
                return NotFound($"No Halaqa Teacher found for Teacher ID: {teacherId} and Halaqa ID: {halaqaId}.");
            var halaqaTeacherDto = Mapper.Map<HalaqaTeacherDto>(halaqaTeacher);
            return Ok(halaqaTeacherDto);
        }
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
