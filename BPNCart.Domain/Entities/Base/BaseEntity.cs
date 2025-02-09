using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPNCart.Domain.Entities.Base;
public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
