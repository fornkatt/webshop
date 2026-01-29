using MongoDB.Driver;
using Webshop.Helpers;
using Webshop.Models;

namespace Webshop.Services;

internal sealed class MongoLogService
{
    private readonly IMongoCollection<ActionLog>? _logsCollection;

    internal MongoLogService()
    {
        try
        {
            var connectionString = ConfigHelper.GetMongoConnectionString();

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageHelper.ShowError("Ingen MongoDB connection sträng är konfigurerad. Loggning kommer vara avslagen.");
                return;
            }

            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(ConfigHelper.GetMongoDatabaseName());
            _logsCollection = database.GetCollection<ActionLog>("ActionLogs");
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Kunde inte initialisera MongoDB: {e.Message}");
        }
    }

    internal async Task LogActionAsync(string actionType, int? userId = null, string? name = null, string? email = null, string? details = null)
    {
        if (_logsCollection == null) return;

        try
        {
            var log = new ActionLog
            {
                ActionType = actionType,
                UserId = userId,
                Name = name,
                Email = email,
                Details = details,
                Timestamp = DateTime.Now
            };

            await _logsCollection.InsertOneAsync(log);
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Gick inte att logga händelse: {e.Message}");
        }
    }

    internal async Task<List<ActionLog>> GetRecentLogsAsync(int limit = 100)
    {
        if (_logsCollection == null) return new List<ActionLog>();

        try
        {
            return await _logsCollection
                .Find(_ => true)
                .SortByDescending(log => log.Timestamp)
                .Limit(limit)
                .ToListAsync();
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Kunde inte hämta loggar: {e.Message}");
            return new List<ActionLog>();
        }
    }
}
