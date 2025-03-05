using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Product
{
    public class Producto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("marca")]
        public string Marca { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("categorias")]
        public List<ObjectId> Categorias { get; set; }

        [BsonElement("iva")]
        public ObjectId Iva { get; set; }

        [BsonElement("variantes")]
        public List<VarianteProducto> Variantes { get; set; }
    }
}
