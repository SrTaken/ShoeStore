using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Model.Product
{
    public class TallaVarianteProducto :INotifyPropertyChanged
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("talla")]
        public int Talla { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
