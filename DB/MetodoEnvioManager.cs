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
    public class MetodoEnvioManager
    {
        private static IMongoCollection<MetodoEnvio> _metodosEnvioCollection;
        public MetodoEnvioManager(MongoDBConnection connection)
        {
            _metodosEnvioCollection = connection.GetCollection<MetodoEnvio>("metodo_envio");
        }
        public List<MetodoEnvio> GetMetodosEnvio()
        {
            return _metodosEnvioCollection.Find(new BsonDocument()).ToList();
        }
        public MetodoEnvio GetMetodoEnvio(ObjectId id)
        {
            return _metodosEnvioCollection.Find(m => m.Id == id).FirstOrDefault();
        }
    }
}
