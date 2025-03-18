using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cesta
{
    public class MetodoEnvio
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("precio_base")]
        public decimal PrecioBase { get; set; }

        [BsonElement("precio_minimo")]
        public decimal PrecioMinimo { get; set; }

        [BsonElement("iva")]
        public ObjectId Iva { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MetodoEnvio envio &&
                   Id.Equals(envio.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
