using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Cesta;

namespace Model.Factura
{
    public class Factura
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("empresa")]
        public Empresa Empresa { get; set; }

        [BsonElement("data")]
        public DateTime Fecha { get; set; }

        [BsonElement("precio_base")]
        public decimal PrecioBase { get; set; }

        [BsonElement("precio_final")]
        public decimal PrecioFinal { get; set; }

        [BsonElement("cliente")]
        public ClienteFactura Cliente { get; set; }

        [BsonElement("lineas")]
        public List<LineaFactura> Lineas { get; set; } = new List<LineaFactura>();

        [BsonElement("impuestos")]
        public decimal Impuestos { get; set; }

        [BsonElement("metodoEnvio")]
        public MetodoEnvio MetodoEnvio { get; set; }

    }
}
