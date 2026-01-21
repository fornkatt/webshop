using System.Text.Json;

namespace Webshop.Helpers;

internal class ConfigHelper
{
    private static AppSettings? _settings;

    private static AppSettings GetSettings()
    {
        if (_settings == null)
        {
            var json = File.ReadAllText("appsettings.json");
            _settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
        return _settings ?? throw new Exception("Failed to load appsettings.json, please add it to root");
    }

    public static (string Username, string Password) GetAdminCredentials()
    {
        var settings = GetSettings();

        // Faller tillbaka till default-värden om värden inte finns i appsettings.json för utvecklingsmiljöns skull
        return (settings.Admin?.Username ?? "admin", settings.Admin?.Password ?? "Admin123");
    }

    public static string? GetConnectionString()
    {
        var settings = GetSettings();

        return settings.ConnectionStrings?.WebshopDb;
    }

    internal class AppSettings
    {
        public AdminConfig? Admin { get; set; }
        public ConnectionStringsConfig? ConnectionStrings { get; set; }
    }

    internal class AdminConfig
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    internal class ConnectionStringsConfig
    {
        public string? WebshopDb { get; set; }
    }
}
