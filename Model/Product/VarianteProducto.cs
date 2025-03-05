using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Product
{
    public class VarianteProducto
    {
        [BsonElement("color")]
        public string Color { get; set; }

        [BsonElement("precio")]
        public double Precio { get; set; }

        [BsonElement("descuento")]
        public double Descuento { get; set; }

        [BsonElement("imagenes")]
        public List<string> Imagenes { get; set; }

        [BsonElement("tallas")]
        public List<TallaVarianteProducto> Tallas { get; set; }
    }

}
