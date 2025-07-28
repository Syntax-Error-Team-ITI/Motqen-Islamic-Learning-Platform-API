using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.HalaqaDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.StudentDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.TeacherDTOs;
using MotqenIslamicLearningPlatform_API.Models.HalaqaModel;
using MotqenIslamicLearningPlatform_API.Services;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.HalaqaCon
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HalaqaController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IRoomService roomService;
        private readonly IMapper _mapper;
        public HalaqaController(IMapper mapper, UnitOfWork unitOfWork, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            this.roomService = roomService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("names")]
        public IActionResult GetHalaqaNamesList()
        {
            var teacherId = Convert.ToInt32(User.FindFirst("id")?.Value);
            var halaqas = _unitOfWork.HalaqaRepo.GetHalaqasAssignToTeacher(teacherId);
            return Ok(_mapper.Map<List<HalaqaNamesListDTO>>(halaqas));
        }
        //[Authorize(Roles = "Teacher,Admin")]

        [HttpGet]
        public IActionResult GetAll(bool includeDeleted = false)
        {
            var halaqas = _unitOfWork.HalaqaRepo.GetAllIncludeSubject(includeDeleted);
            var result = _mapper.Map<IEnumerable<HalaqaDto>>(halaqas);
            return Ok(result);
        }
        //[Authorize(Roles = "Teacher,Admin")]

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var halaqa = _unitOfWork.HalaqaRepo.GetByIdIncludeSubjectAndClassSchedules(id);
            if (halaqa == null)
                return NotFound(new { message = "Halaqa not found." });
            var result = _mapper.Map<HalaqaDetailsDto>(halaqa);
            return Ok(result);
            //return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHalaqaDto halaqaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await roomService.CreateRoom(new CreateRoomRequest { Name = halaqaDto.Name, Description = halaqaDto.Description });

            var halaqa = _mapper.Map<Halaqa>(halaqaDto);

            halaqa.HostLiveLink = room[0];
            halaqa.GuestLiveLink = room[1];
            halaqa.RoomId = room[2];

            _unitOfWork.HalaqaRepo.Add(halaqa);
            _unitOfWork.Save();

            var result = _mapper.Map<HalaqaDto>(halaqa);

            return CreatedAtAction(nameof(GetById), new { id = halaqa.Id }, result);
        }
        //[Authorize(Roles = "Teacher,Admin")]

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
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = _unitOfWork.HalaqaRepo.SoftDelete(id);
            if (!success)
                return NotFound(new { message = "Halaqa not found." });

            _unitOfWork.Save();
            return Ok(new { message = "Halaqa deleted successfully." });
        }
        [Authorize(Roles = "Admin")]

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

        [HttpGet("{halaqaId:int}/students")]
        public IActionResult getAllStudentsForHalaqa(int halaqaId)
        {
            var students = _unitOfWork.HalaqaStudentRepo.getAllStudentsByHalaqaId(halaqaId: halaqaId);
            return Ok(_mapper.Map<List<StudentHalaqaDisplayDTO>>(students));
        }

        [HttpGet("{halaqaId:int}/teachers")]
        public IActionResult GetTeachers(int halaqaId)
        {
            var teachers = _unitOfWork.HalaqaTeacherRepo.GetByHalaqaId(halaqaId);
            return Ok(_mapper.Map<List<TeacherDto>>(teachers));
        }
    }
}
