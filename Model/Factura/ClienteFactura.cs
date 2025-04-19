using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.User;

namespace Model.Factura
{
    public class ClienteFactura
    {
        [BsonElement("cliente")]
        public ObjectId ClienteId { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("cc_pago")]
        public TarjetaCredito CCPago { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("adresa")]
        public Direccion Direccion { get; set; }
        [BsonElement("nie")]
        public string Nie { get; set; }
        [BsonElement("mail")]
        public string Mail { get; set; }

    }
}
