using Model.Cesta;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class CestaManager
    {
        private static IMongoCollection<Cesta> _cestaCollection;

        public CestaManager(MongoDBConnection connection)
        {
            _cestaCollection = connection.GetCollection<Cesta>("cesta");
        }

        public Cesta GetCestaByUsuarioId(ObjectId usuarioId)
        {
            return _cestaCollection.Find(c => c.UsuarioId == usuarioId).FirstOrDefault();
        }

        public void CrearCesta(Cesta cesta)
        {
            _cestaCollection.InsertOne(cesta);
        }

        public void ActualizarCesta(Cesta cesta)
        {
            var filter = Builders<Cesta>.Filter.Eq(c => c.Id, cesta.Id);
            _cestaCollection.ReplaceOne(filter, cesta);
        }

        public void EliminarCesta(Cesta cesta)
        {
            var filter = Builders<Cesta>.Filter.Eq(c => c.Id, cesta.Id);
            _cestaCollection.DeleteOne(filter);
        }
    }

}
