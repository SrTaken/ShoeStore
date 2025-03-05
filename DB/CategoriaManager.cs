using Model.Product;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class CategoriaManager
    {
        private static IMongoCollection<Categoria> _categoriaCollection;
        public  CategoriaManager(MongoDBConnection connection)
        {
            _categoriaCollection = connection.GetCollection<Categoria>("categoria");
        }

        public List<Categoria> GetAllCategorias()
        {
            return _categoriaCollection.Find(_ => true).ToList();
        }

        public List<Categoria> GetParentCategorias()
        {
            return _categoriaCollection.Find(c => c.CategoriaPadre == null).ToList();
        }
        public List<Categoria> GetChildCategorias(Categoria parent)
        {
            return _categoriaCollection.Find(c => c.CategoriaPadre == parent.Id).ToList();
        }
    }
}
