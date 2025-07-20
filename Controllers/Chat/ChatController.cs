using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.Services.Chat;

namespace MotqenIslamicLearningPlatform_API.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatBotService _chatBot;

        public ChatController(ChatBotService chatBot)
        {
            _chatBot = chatBot;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] string message)
        {
            var reply = await _chatBot.GetChatResponse(message);
            return Ok(reply);
        }
    }
}
