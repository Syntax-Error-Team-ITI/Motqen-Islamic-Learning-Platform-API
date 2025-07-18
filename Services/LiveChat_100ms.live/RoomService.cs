using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace MotqenIslamicLearningPlatform_API.Services
{
    public interface IRoomService
    {
        Task<List<string>> CreateRoom(CreateRoomRequest roomData);
        List<RoomCodeInfo> GetRoomCodes(string roomId);
        IEnumerable<RoomInfo> ListActiveRooms();
        string GenerateManagementToken();
        bool EndRoom(string roomId);
        List<string> GetRoomJoinLink(string roomId);
        Task<bool> DisableRoom(string roomId, bool disable);

    }

    public class RoomService : IRoomService
    {
        private readonly string _apiBaseUrl;
        private readonly string _motqenUrl;
        private readonly string _managementToken;
        private readonly string _appAccessKey;
        private readonly string _templateId;
        private readonly string _appSecret;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IConfiguration config, ILogger<RoomService> logger)
        {
            _apiBaseUrl = config["100ms:ApiBaseUrl"];
            _motqenUrl = config["100ms:MotqenUrl"];
            _appAccessKey = config["100ms:AppAccessKey"];
            _templateId = config["100ms:TemplateId"];
            _appSecret = config["100ms:AppSecret"];
            _managementToken = GenerateManagementToken();
            _logger = logger;

            ValidateConfig();
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrEmpty(_appSecret))
                throw new ArgumentNullException(nameof(_appSecret), "AppSecret is missing in configuration");

            // Add other validations as needed
        }

        public string GenerateManagementToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTimeOffset.UtcNow;
            var unixTimeSeconds = now.ToUnixTimeSeconds();

            var claims = new[]
            {
                new Claim("iat", unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                new Claim("exp", now.AddDays(7).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim("type", "management"),
                new Claim("version", "2", ClaimValueTypes.Integer32),
                new Claim("nbf", unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                new Claim("access_key", _appAccessKey)
            };

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(claims: claims, signingCredentials: credentials));
        }

        public async Task<List<string>> CreateRoom(CreateRoomRequest roomData)
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest("rooms", Method.Post);

            request.AddHeader("Authorization", $"Bearer {_managementToken}");
            request.AddHeader("Content-Type", "application/json");

            var roomRequest = new
            {
                roomData.Name,
                roomData.Description,
                template_id = _templateId,
                recording_info = new { enabled = roomData.RecordingEnabled }
            };

            request.AddJsonBody(roomRequest);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to create room: {ResponseContent}", response.Content);
                throw new Exception($"Failed to create room: {response.StatusCode}");
            }

            // Parse the response to get just the room ID
            var responseObject = JObject.Parse(response.Content);
            var roomId = responseObject["id"]?.ToString();
            var joinLinks = GetRoomJoinLink(roomId);
            joinLinks.Add(roomId);

            if (string.IsNullOrEmpty(roomId))
            {
                throw new Exception("Room ID not found in response");
            }

            return joinLinks;
        }

        public async Task<bool> DisableRoom(string roomId, bool disable)
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest($"rooms/{roomId}", Method.Post);

            request.AddHeader("Authorization", $"Bearer {_managementToken}");
            request.AddHeader("Content-Type", "application/json");

            var disableRequest = new
            {
                enabled = !disable  
            };

            request.AddJsonBody(disableRequest);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to disable room {RoomId}: {ResponseContent}", roomId, response.Content);
                throw new Exception($"Failed to disable room: {response.StatusCode}");
            }
            return true;
        }
        public List<string> GetRoomJoinLink(string roomId)
        {
            var roomCodes = GetRoomCodes(roomId);
            // Validate we got at least 2 codes
            if (roomCodes == null || roomCodes.Count < 2)
            {
                _logger.LogError("Insufficient room codes returned for room {RoomId}", roomId);
                throw new Exception("Failed to generate room links - insufficient room codes");
            }

            List<string> links = new List<string> {
                $"{_motqenUrl}/meeting/{roomCodes[0].Code}",
                $"{_motqenUrl}/meeting/{roomCodes[1].Code}"
            };
            return links;
        }

        public List<RoomCodeInfo> GetRoomCodes(string roomId)
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest($"room-codes/room/{roomId}", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_managementToken}");
            request.AddHeader("Content-Type", "application/json");


            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to get room codes: {ResponseContent}", response.Content);
                throw new Exception($"Failed to get room: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<ApiResponse<RoomCodeInfo>>(response.Content)?.Data
                ?? new List<RoomCodeInfo>();
        }

        public IEnumerable<RoomInfo> ListActiveRooms()
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest("rooms", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_managementToken}");
            request.AddQueryParameter("active", "true");

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to list rooms: {ResponseContent}", response.Content);
                throw new Exception($"Failed to list rooms: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<RoomsPaginatedResponse>(response.Content)?.Data
                ?? new List<RoomInfo>();
        }

        public bool EndRoom(string roomId)
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest($"rooms/{roomId}/end", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_managementToken}");

            var response = client.Execute(request);
            return response.IsSuccessful;
        }
    }
    public class CreateRoomRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RecordingEnabled { get; set; }
    }
    public class DisableRoomRequest
    {
        [Required]
        public string RoomId { get; set; }

        public bool Disable { get; set; } = true;
    }
    // Support both response formats
    public class RoomsPaginatedResponse
    {
        [JsonProperty("data")]
        public List<RoomInfo> Data { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
    public class RoomInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("recording_enabled")]
        public bool RecordingEnabled { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("participant_count")]
        public int ParticipantCount { get; set; }
    }
    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }
    public class RoomCodeInfo
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
