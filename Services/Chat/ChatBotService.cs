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
            apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini API Key is not configured.");
            model = configuration["Gemini:Model"] ?? "gemini-2.0-flash";
        }

        public async Task<string> GetChatResponse(string userMessage)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = userMessage }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error from Gemini API: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);

            var reply = doc.RootElement
                           .GetProperty("candidates")[0]
                           .GetProperty("content")
                           .GetProperty("parts")[0]
                           .GetProperty("text")
                           .GetString();

            return reply;
        }
    }
}
