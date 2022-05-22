using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ProcrastinationBlocker.Common;

namespace ProcrastinationBlocker.Forest.API
{
    public class ForestRestApiHelper
    {
        private string Token { get; }
        private IHttpClient HttpClient { get; }

        private const string ForestUrl = "https://c88fef96.forestapp.cc";

        internal ForestRestApiHelper(string token, IHttpClient httpClient)
        {
            Token = token;
            HttpClient = httpClient;
        }

        public ForestRestApiHelper(string token) : this(token, new HttpClientWrapper())
        {
                
        }

        public TimeSpan GetFocusedTimeSince(DateTime startTime)
        {
            var query = $"from_date={startTime:O}";
            var uri = new Uri($"{ForestUrl}/api/v1/plants{(string.IsNullOrEmpty(query) ? string.Empty : "?" + query)}");
            var message = GetRequestMessage(HttpMethod.Get, uri);
            var response = HttpClient.Send(message);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unable to retrieve plants info. Response does not indicate success.", null, response.StatusCode);
            }

            var plants = JsonSerializer.Deserialize<IEnumerable<PlantDto>>(response.Content.ReadAsStream());

            if (plants == null)
            {
                return TimeSpan.Zero;
            }

            var focusTime = TimeSpan.Zero;  
            foreach (var plant in plants)
            {
                if (plant.StartTime == null || plant.EndTime == null)
                {
                    continue;
                }

                var elapsed = (DateTime)plant.EndTime - (DateTime)plant.StartTime;
                focusTime = focusTime.Add(elapsed);
            }

            return focusTime;
        }

        private HttpRequestMessage GetRequestMessage(HttpMethod method, Uri uri)
        {
            var message = new HttpRequestMessage(method, uri);
            message.Headers.Add("Accept", "application/json");
            message.Headers.Add("Cookie", $"remember_token={Token}");
            return message;
        }
    }

    internal class TreeDto
    {
        [JsonPropertyName("created_at")]
        public DateTime? Created { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? Updated { get; set; }
        [JsonPropertyName("tree_type")]
        public int? Type { get; set; }
        [JsonPropertyName("is_dead")]
        public bool? Dead { get; set; }
        [JsonPropertyName("phase")]
        public int? Phase { get; set; }
    }

    internal class PlantDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("tag")]
        public int? Tag { get; set; }
        [JsonPropertyName("is_success")]
        public bool? Success { get; set; }
        [JsonPropertyName("start_time")]
        public DateTime? StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? Created { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? Updated { get; set; }
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
        [JsonPropertyName("has_left")]
        public bool? HasLeft { get; set; }
        [JsonPropertyName("deleted")]
        public bool? Deleted { get; set; }
        [JsonPropertyName("theme")]
        public int? Theme { get; set; }
        [JsonPropertyName("cheating")]
        public bool? Cheating { get; set; }
        [JsonPropertyName("tree_type_gid")]
        public int? TreeTyepId { get; set; }
        [JsonPropertyName("tree_count")]
        public int? TreeCount { get; set; }
        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("trees")]
        public IEnumerable<TreeDto>? Trees { get; set; }
    }
}
