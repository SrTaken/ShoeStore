using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.User;

namespace Model
{
    public class Empresa
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("ubicacion")]
        public Direccion Ubicacion { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("logo")]
        public string logoURL { get; set; }

        [BsonElement("cif")]
        public string cif { get; set; }
    }
}
