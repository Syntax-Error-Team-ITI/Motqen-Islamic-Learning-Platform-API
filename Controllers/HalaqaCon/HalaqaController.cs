using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.HalaqaCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalaqaController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HalaqaController(IMapper mapper, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(bool includeDeleted = false)
        {
            var halaqas = _unitOfWork.HalaqaRepo.GetAllIncludeSubject(includeDeleted);
            var result = _mapper.Map<IEnumerable<HalaqaDto>>(halaqas);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var halaqa = _unitOfWork.HalaqaRepo.GetByIdIncludeSubject(id);
            if (halaqa == null)
                return NotFound(new { message = "Halaqa not found." });
            var result = _mapper.Map<HalaqaDto>(halaqa);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateHalaqaDto halaqaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var halaqa = _mapper.Map<Halaqa>(halaqaDto);
            _unitOfWork.HalaqaRepo.Add(halaqa);
            _unitOfWork.Save();

            var result = _mapper.Map<HalaqaDto>(halaqa);
            return CreatedAtAction(nameof(GetById), new { id = halaqa.Id }, result);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateHalaqaDto halaqaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var halaqa = _unitOfWork.HalaqaRepo.GetById(id);
            if (halaqa == null)
                return NotFound(new { message = "Halaqa not found." });

            _mapper.Map(halaqaDto, halaqa);
            _unitOfWork.HalaqaRepo.Edit(halaqa);
            _unitOfWork.Save();

            var result = _mapper.Map<HalaqaDto>(halaqa);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = _unitOfWork.HalaqaRepo.SoftDelete(id);
            if (!success)
                return NotFound(new { message = "Halaqa not found." });

            _unitOfWork.Save();
            return Ok(new { message = "Halaqa deleted successfully." });
        }

        [HttpPut("restore/{id:int}")]
        public IActionResult Restore(int id)
        {
            var halaqa = _unitOfWork.HalaqaRepo.GetById(id);
            if (halaqa == null)
                return NotFound(new { message = "Halaqa not found." });

            if (!halaqa.IsDeleted)
                return BadRequest(new { message = "Halaqa is not deleted." });

            _unitOfWork.HalaqaRepo.Restor(id);
            _unitOfWork.Save();
            return NoContent();
        }

        [HttpGet("{id:int}/students")]
        public IActionResult getAllStudentsForHalaqa(int halaqaId, bool includeDeleted = false)
        {
            var students = _unitOfWork.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqaId: halaqaId);
            return Ok(_mapper.Map<List<StudentHalaqaDisplayDTO>>(students));
        }

        [HttpGet("{id:int}/teachers")]
        public IActionResult GetTeachers(int id)
        {
            var teachers = _unitOfWork.HalaqaTeacherRepo.GetByHalaqaId(id);
            return Ok(_mapper.Map<List<TeacherDto>>(teachers));
        }
    }
}
