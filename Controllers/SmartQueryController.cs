using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.BotDTOs;
using MotqenIslamicLearningPlatform_API.Services;
using System.Security.Claims;

namespace MotqenIslamicLearningPlatform_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SmartQueryController : ControllerBase
    {
        private readonly SmartQueryService _smartQueryService;

        public SmartQueryController(SmartQueryService smartQueryService)
        {
            _smartQueryService = smartQueryService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] SmartQueryRequest request)
        {
            try
            {

                //var claims = User.Claims;
                //foreach (var claim in claims)
                //{
                //    Console.WriteLine($"{claim.Type}: {claim.Value}");
                //}
                // Get user ID and role from JWT token using your custom claim names
                var Id = User.FindFirst("id")?.Value;
                var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(role))
                {
                    return Unauthorized("User information not found in token");
                }

                // Map your DTO to the service DTO
                var serviceRequest = new SmartQueryRequest
                {
                    Question = request.Question,
                    Id = Id,        // Map to service expected property
                    Role = role     // Map to service expected property
                };

                var response = await _smartQueryService.ProcessSmartQuery(serviceRequest);

                // Map service response back to your DTO
                var controllerResponse = new SmartQueryResponse
                {
                    Answer = response.Answer,
                    DataSource = response.DataSource
                };

                return Ok(controllerResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new SmartQueryResponse
                {
                    Answer = "I'm sorry, I encountered an error processing your question. Please try again.",
                    DataSource = "System Error"
                });
            }
        }

        [HttpPost("ask-simple")]
        public async Task<IActionResult> AskSimple([FromBody] string question)
        {
            try
            {
                // Get user ID and role from JWT token using your custom claim names
                var Id = User.FindFirst("id")?.Value;
                var role = User.FindFirst("role")?.Value;

                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(role))
                {
                    return Unauthorized("User information not found in token");
                }

                // Create service request with proper mapping
                var serviceRequest = new SmartQueryRequest
                {
                    Question = question,
                    Id = Id,        // Map to service expected property
                    Role = role     // Map to service expected property
                };

                var response = await _smartQueryService.ProcessSmartQuery(serviceRequest);
                return Ok(response.Answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "I'm sorry, I encountered an error processing your question. Please try again.");
            }
        }

        // Debug endpoint to help troubleshoot token issues
        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Claims = claims,
                Id = User.FindFirst("id")?.Value,
                Role = User.FindFirst("role")?.Value,
                StudentId = User.FindFirst("studentId")?.Value,
                TeacherId = User.FindFirst("teacherId")?.Value,
                ParentId = User.FindFirst("parentId")?.Value,
                IsAdmin = User.FindFirst("IsAdmin")?.Value
            });
        }

        // Test endpoint without authorization for debugging
        [HttpPost("ask-test")]
        [AllowAnonymous]
        public async Task<IActionResult> AskTest([FromBody] SmartQueryRequest request)
        {
            try
            {
                // For testing - use the values from the request body directly
                var serviceRequest = new SmartQueryRequest
                {
                    Question = request.Question,
                    Id = request.Id,        // Map from your DTO
                    Role = request.Role     // Map from your DTO
                };

                var response = await _smartQueryService.ProcessSmartQuery(serviceRequest);

                var controllerResponse = new SmartQueryResponse
                {
                    Answer = response.Answer,
                    DataSource = response.DataSource
                };

                return Ok(controllerResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new SmartQueryResponse
                {
                    Answer = "I'm sorry, I encountered an error processing your question. Please try again.",
                    DataSource = "System Error"
                });
            }
        }
    }
}


