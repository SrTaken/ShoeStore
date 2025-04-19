using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.User
{
    public class Usuario
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("nombre")]
        public string Nombre { get; set; }
        [BsonElement("login")]
        public string Login { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("telefono")]
        public string Telefono { get; set; }
        [BsonElement("sexo")]
        public string Sexo { get; set; }
        [BsonElement("direcciones")]
        public Direccion Direcciones { get; set; }
        [BsonElement("cc")]
        public TarjetaCredito CC { get; set; }
        [BsonElement("mail")]
        public string Mail { get; set; }
        [BsonElement("nie")]
        public string Nie { get; set; }


        public override string ToString()
        {
            return Login + " " + Password;
        }
    }
}
