using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Product
{
    public class Categoria
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nom")]
        public string Nombre { get; set; }

        [BsonElement("categoria_padre")]
        public ObjectId? CategoriaPadre { get; set; } // Puede ser null si no tiene padre
    }

}
