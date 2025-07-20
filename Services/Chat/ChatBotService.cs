using System.Text.Json;

namespace MotqenIslamicLearningPlatform_API.Services.Chat
{
    public class ChatBotService
    {
        private readonly HttpClient client;
        private readonly string apiKey;
        private readonly string model;

        public ChatBotService(IConfiguration configuration)
        {
            client = new HttpClient();
            apiKey = configuration["OpenAI:ApiKey"] ?? throw new ArgumentNullException("API Key is not configured.");
            model = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }


        public async Task<string> GetChatResponse(string userMessage)
        {

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = userMessage }
                },
                max_tokens = 1000
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error from OpenAI API: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);
            var reply = doc.RootElement
                           .GetProperty("choices")[0]
                           .GetProperty("message")
                           .GetProperty("content")
                           .GetString();

            return reply;

        }
    }
}
