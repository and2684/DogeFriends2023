using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ImageService.Models
{
    public class ImageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string UID { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public string Base64Data { get; set; } = string.Empty;

        public bool IsMain { get; set; }
    }
}
