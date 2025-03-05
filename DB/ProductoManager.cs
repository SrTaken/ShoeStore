using Model.Product;
using Model.User;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class ProductoManager
    {
        private static IMongoCollection<Producto> _productoCollection;

        private static CategoriaManager _categoriaManager;
        public ProductoManager(MongoDBConnection connection)
        {
            _productoCollection = connection.GetCollection<Producto>("producto");
            _categoriaManager = new CategoriaManager(connection);
        }
        public List<Producto> GetAll()
        {
            return _productoCollection.Find(_ => true).ToList();
        }

        public List<Producto> GetAllProdByCategoria(Categoria selectedCategory)
        {
            return _productoCollection.Find(p => p.Categorias.Contains(selectedCategory.Id)).ToList();
        }

        public List<Producto> GetProductosByCategoria(Categoria categoria)
        {
            var productos = GetAllProdByCategoria(categoria);
            var childCategories = _categoriaManager.GetChildCategorias(categoria);

            foreach (var child in childCategories)
            {
                productos.AddRange(GetProductosByCategoria(child));
            }

            return productos;
        }
    }
}
