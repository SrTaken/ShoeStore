using Model.User;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class UsuarioManager
    {
        private static IMongoCollection<Usuario> _usuariosCollection;

        public UsuarioManager(MongoDBConnection connection)
        {
            _usuariosCollection = connection.GetCollection<Usuario>("usuario");
        }

        /// <summary>
        /// Metodo para hacer login con un usuario
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Usuario Login(string login, string password)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Login, login) & Builders<Usuario>.Filter.Eq(u => u.Password, password);
            return _usuariosCollection.Find(filter).FirstOrDefault();
        }

        public List<Usuario> GetAll()
        {
            return _usuariosCollection.Find(_ => true).ToList();
        }
    }
}
