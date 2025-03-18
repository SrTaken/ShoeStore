﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cesta
{
    public class ItemCesta
    {
        [BsonElement("producto")]
        public ObjectId TallaId { get; set; }

        [BsonElement("qty")]
        public int Cantidad { get; set; }

        [BsonIgnore]
        public string Nombre { get; set; }
        [BsonIgnore]
        public string Imagen { get; set; }
        [BsonIgnore]
        public decimal Precio { get; set; }
        [BsonIgnore]
        public decimal IVA { get; set; }
    }
}
