using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.HalaqaCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassScheduleController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassScheduleController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("halaqa/{halaqaId:int}")]
        public IActionResult GetAllForHalaqa(int halaqaId, bool includeDeleted = false)
        {
            var schedules = _unitOfWork.ClassScheduleRepo.GetAllIncludeHalaqa()
                .Where(cs => cs.HalaqaId == halaqaId && (includeDeleted || !cs.IsDeleted));
            var result = _mapper.Map<IEnumerable<ClassScheduleDto>>(schedules);
            return Ok(result);
        }


        [HttpGet("halaqa/{halaqaId:int}/{id:int}")]
        public IActionResult GetById(int halaqaId, int id)
        {
            var schedule = _unitOfWork.ClassScheduleRepo.GetByIdIncludeHalaqa(id);
            if (schedule == null || schedule.HalaqaId != halaqaId)
                return NotFound();
            var result = _mapper.Map<ClassScheduleDto>(schedule);
            return Ok(result);
        }

        [HttpPost("halaqa/{halaqaId:int}")]
        public IActionResult Create(int halaqaId, [FromBody] CreateClassScheduleDto dto)
        {
            if (!ModelState.IsValid || dto.HalaqaId != halaqaId)
                return BadRequest(ModelState);

            var schedule = _mapper.Map<ClassSchedule>(dto);
            _unitOfWork.ClassScheduleRepo.Add(schedule);
            _unitOfWork.Save();

            var result = _mapper.Map<ClassScheduleDto>(schedule);
            return CreatedAtAction(nameof(GetById), new { halaqaId = halaqaId, id = schedule.Id }, result);
        }

        [HttpPut("halaqa/{halaqaId:int}/{id:int}")]
        public IActionResult Update(int halaqaId, int id, [FromBody] UpdateClassScheduleDto dto)
        {
            if (!ModelState.IsValid || dto.HalaqaId != halaqaId || dto.Id != id)
                return BadRequest(ModelState);

            var schedule = _unitOfWork.ClassScheduleRepo.GetById(id);
            if (schedule == null || schedule.HalaqaId != halaqaId)
                return NotFound();

            _mapper.Map(dto, schedule);
            _unitOfWork.ClassScheduleRepo.Edit(schedule);
            _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("halaqa/{halaqaId:int}/{id:int}")]
        public IActionResult Delete(int halaqaId, int id)
        {
            var schedule = _unitOfWork.ClassScheduleRepo.GetById(id);
            if (schedule == null || schedule.HalaqaId != halaqaId)
                return NotFound();

            schedule.IsDeleted = true;
            _unitOfWork.ClassScheduleRepo.Edit(schedule);
            _unitOfWork.Save();
            return Ok(new { message = "Schedule deleted successfully." });
        }

        [HttpPut("halaqa/{halaqaId:int}/restore/{id:int}")]
        public IActionResult Restore(int halaqaId, int id)
        {
            var schedule = _unitOfWork.ClassScheduleRepo.GetById(id);
            if (schedule == null || schedule.HalaqaId != halaqaId)
                return NotFound();

            if (!schedule.IsDeleted)
                return BadRequest("Schedule is not deleted.");

            _unitOfWork.ClassScheduleRepo.Restor(id);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}
