using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Webshop.Models;

internal class ActionLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("actionType")]
    public string ActionType { get; set; } = string.Empty;

    [BsonElement("userId")]
    public int? UserId { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("details")]
    public string? Details { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
