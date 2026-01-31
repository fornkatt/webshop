using System.Text.Json;

namespace Webshop.Helpers;

internal static class ConfigHelper
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

    public static string? GetWebshopConnectionString()
    {
        var settings = GetSettings();

        return settings.ConnectionStrings?.WebshopDb;
    }

    public static string? GetMongoConnectionString()
    {
        var settings = GetSettings();
        return settings.ConnectionStrings?.MongoDb;
    }

    public static string GetMongoDatabaseName()
    {
        var settings = GetSettings();
        return settings.MongoDb?.DatabaseName ?? "WebshopLogs";
    }

    internal class AppSettings
    {
        public AdminConfig? Admin { get; set; }
        public ConnectionStringsConfig? ConnectionStrings { get; set; }
        public MongoDbConfig? MongoDb { get; set; }
    }

    internal class AdminConfig
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    internal class ConnectionStringsConfig
    {
        public string? WebshopDb { get; set; }
        public string? MongoDb { get; set; }
    }

    internal class MongoDbConfig
    {
        public string? DatabaseName { get; set; }
    }
}
