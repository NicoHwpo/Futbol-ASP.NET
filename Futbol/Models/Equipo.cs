using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Futbol.Models
{
    [BsonIgnoreExtraElements]
    public class Equipo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = String.Empty;

        [BsonElement("ciudad")]
        public string Ciudad { get; set; }

        [BsonElement("estadio")]
        public string Estadio { get; set; }

        [BsonElement("entrenador")]
        public string Entrenador { get; set; }

        [BsonElement("ligas")]
        public string[]? Ligas { get; set; }
    }
}
