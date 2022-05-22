using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProcrastinationBlocker.Service
{
    public class Configuration
    {
        public IEnumerable<string> BlockedHosts { get; }
        public string AuthenticationCookie { get; }
        public string RedirectTo { get; }
        public int WaitTimeMinutes { get; }
        public int HoursRequired { get; }

        private Configuration(IEnumerable<string> blockedHosts, string authenticationCookie, int waitTimeMinutes, string redirectTo, int hoursRequired)
        {
            BlockedHosts = blockedHosts;
            AuthenticationCookie = authenticationCookie;
            WaitTimeMinutes = waitTimeMinutes;
            RedirectTo = redirectTo;
            HoursRequired = hoursRequired;
        }

        public static Configuration LoadConfiguration()
        {
            var location = AppDomain.CurrentDomain.BaseDirectory;
            var json = File.ReadAllText(Path.Combine(location, "config.json"));
            var configurationDto = JsonSerializer.Deserialize<ConfigurationDto>(json);

            if (configurationDto == null)
            {
                throw new ApplicationException("Unable to deserialize config file.");
            }

            if (configurationDto.AuthenticationCookie == null)
            {
                throw new ApplicationException("Cookie is required.");
            }

            if (configurationDto.BlockedHosts == null)
            {
                throw new ApplicationException("List of hosts to block is required.");
            }

            return new Configuration(
                configurationDto.BlockedHosts ?? new string[] { },
                configurationDto.AuthenticationCookie,
                configurationDto.WaitTimeMinutes ?? 15,
                configurationDto.RedirectTo ?? "127.0.0.1",
                configurationDto.HoursRequired ?? 3);
        }
    }

    public class ConfigurationDto   
    {
        [JsonPropertyName("blocked-hosts")]
        public string[]? BlockedHosts { get; set; }
        [JsonPropertyName("cookie")]
        public string? AuthenticationCookie { get; set; }
        [JsonPropertyName("wait-time-minutes")]
        public int? WaitTimeMinutes { get; set; }
        [JsonPropertyName("redirect-to")]
        public string? RedirectTo { get; set; }
        [JsonPropertyName("hours-required")]
        public int? HoursRequired { get; set; }
    }
}
