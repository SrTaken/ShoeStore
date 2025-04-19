using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Product;

namespace Model.Factura
{
    public class LineaFactura
    {
        [BsonElement("producto")]
        public ObjectId ProductoId { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }
        [BsonElement("variante")]
        public string Variante { get; set; }
        [BsonElement("talla")]
        public int Talla { get; set; }

        [BsonElement("descuento")]
        public double Descuento { get; set; }

        [BsonElement("precio_unitario")]
        public decimal PrecioUnitario { get; set; }
        [BsonElement("precio_final")]
        public decimal PrecioFinal { get; set; }

        [BsonElement("qty")]
        public int Cantidad { get; set; }

        [BsonElement("iva")]
        public IVA Iva { get; set; }
    }
}
