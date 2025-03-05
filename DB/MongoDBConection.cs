using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DB
{
    public class MongoDBConnection
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoDBConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["mongodbconectionstring"];
            string db = ConfigurationManager.AppSettings["dbname"];
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(db);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }

}
