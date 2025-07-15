using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.SubjectDtos;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.SubjectCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        UnitOfWork Unit { get; }
        IMapper Mapper { get; }
        public SubjectController(UnitOfWork _unit, IMapper _mapper)
        {
            this.Mapper = _mapper;
            this.Unit = _unit;
        }
        [HttpGet]
        public IActionResult GetAll(bool includeDelete = false)
        {
            var subjects = Unit.SubjectRepo.GetAll(includeDelete);
            var result = Mapper.Map<IEnumerable<SubjectDto>>(subjects);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var subject = Unit.SubjectRepo.GetById(id);
            if (subject == null)
            {
                return NotFound();
            }
            var result = Mapper.Map<SubjectDto>(subject);
            return Ok(result);
        }
        [HttpGet("deleted")]
        public IActionResult GetDeletedSubjects()
        {
            var deletedSubjects = Unit.SubjectRepo.GetDeleted();
            
            var result = Mapper.Map<IEnumerable<SubjectDto>>(deletedSubjects);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create(CreateSubjectDto subjectDto)
        {
            if (  subjectDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subject = Mapper.Map<Subject>(subjectDto);
            Unit.SubjectRepo.Add(subject);
            Unit.Save();
            var result = Mapper.Map<SubjectDto>(subject);
            return CreatedAtAction(nameof(GetById), new { id = subject.Id }, result);
        }
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateSubjectDto subjectDto)
        {
            
            if ( subjectDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSubject = Unit.SubjectRepo.GetById(id);
            if (existingSubject == null)
            {
                return NotFound();
            }
            Mapper.Map(subjectDto, existingSubject);
            Unit.SubjectRepo.Edit(existingSubject);
            Unit.Save();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var success = Unit.SubjectRepo.SoftDelete(id);
            if (!success)
            {
                return NotFound();
            }
            Unit.Save();
            return NoContent();
        }
        [HttpPut("restore/{id:int}")]
        public IActionResult Restore(int id)
        {
            var subject = Unit.SubjectRepo.GetById(id);
            if (subject == null)
            {
                return NotFound();
            }
            if (!subject.IsDeleted)
            {
                return BadRequest("Subject is not deleted.");
            }
            Unit.SubjectRepo.Restor(id);
            Unit.Save();
            return NoContent();
        }
    }
}
