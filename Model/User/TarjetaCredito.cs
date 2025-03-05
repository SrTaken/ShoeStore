using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class TarjetaCredito
    {
        [BsonElement("nombre")]
        public string Nombre { get; set; }
        [BsonElement("numero")]
        public string Numero { get; set; }
        [BsonElement("cvc")]
        public string CVC { get; set; }
        [BsonElement("fecha_caducidad")]
        public string Fecha_caducidad { get; set; }
    }

}
