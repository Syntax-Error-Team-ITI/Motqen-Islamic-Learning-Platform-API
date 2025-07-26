using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.UserCon
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Admin)]
    public class UsersController(UnitOfWork unit) : ControllerBase
    {
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string userId)
        {
            var user = await unit.UserRepo.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var getAll = await unit.UserRepo.GetAll();
            if (getAll == null)
                return NotFound("No users found.");

            return Ok(await unit.UserRepo.GetAll());
        }

        [HttpDelete("SoftDelete")]
        public async Task<IActionResult> SoftDelete(string userId)
        {
            var softDeleteTask = await unit.UserRepo.SoftDelete(userId);

            if (softDeleteTask == null)
                return NotFound("User not found.");
            else if (softDeleteTask == false)
                return BadRequest("User already isDeleted");
            else
                return Ok(softDeleteTask);
        }

        [HttpGet("Restore")]
        public async Task<IActionResult> Restore(string userId)
        {
            var restoreTask = await unit.UserRepo.Restore(userId);

            if (restoreTask == null)
                return NotFound("User not found.");
            else if (restoreTask == false)
                return BadRequest("User is not deleted!");
            else
                return Ok(restoreTask);
        }

        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDelete(string userId)
        {
            var hardDeleteTask = await unit.UserRepo.HardDelete(userId);

            if (!hardDeleteTask)
                return BadRequest("User is not found.");

            else
                return Ok("User deleted from the database.");
        }
    }
}
