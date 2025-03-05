using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class Direccion
    {
        [BsonElement("cp")]
        public string CP { get; set; }
        [BsonElement("provincia")]
        public string Provincia { get; set; }
        [BsonElement("pais")]
        public string Pais { get; set; }
        [BsonElement("calle")]
        public string Calle { get; set; }
        [BsonElement("direccion_principal")]
        public string DireccionPrincipal { get; set; }
    }
}
