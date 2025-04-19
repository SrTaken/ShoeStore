using Model.Product;
using Model.User;
using MongoDB.Bson;
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
        public Producto GetProductoById(ObjectId productoId)
        {
            return _productoCollection.Find(p => p.Id == productoId).FirstOrDefault();
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

        public List<Producto> GetProductosByNombre(string nombre)
        {
            return _productoCollection.Find(p => p.Nombre.ToLower().Contains(nombre.ToLower())).ToList();
        }

        public List<Producto> GetProductosByRangoDePrecio(double minPrecio, double maxPrecio)
        {
            return _productoCollection.Find(p => p.Variantes.Any(v => v.Precio >= minPrecio && v.Precio <= maxPrecio)).ToList();
        }

        public List<Producto> GetProductosByTalla(string talla)
        {
            int tint;
            if (int.TryParse(talla, out tint))
            {
                return _productoCollection.Find(p => p.Variantes.Any(v => v.Tallas.Any(t => t.Talla == tint))).ToList();
            }
            return new List<Producto>();
        }

        public List<string> GetAllMarcas()
        {
            return _productoCollection.Distinct<string>("marca", Builders<Producto>.Filter.Empty).ToList();
        }

        public List<Producto> GetFilteredProducts(Categoria selectedCategory, string nombre, double minPrecio,
    double maxPrecio, string talla, List<string> marcas, int currentPage, int itemsPerPage)
        {
            var filteredProducts = GetAll();

            if (selectedCategory != null)
            {
                var categoriasFiltradas = _categoriaManager.GetCategoriasYSubcategorias(selectedCategory);
                filteredProducts = filteredProducts
                    .Where(p => p.Categorias.Any(c => categoriasFiltradas.Contains(c)))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            filteredProducts = filteredProducts
                .Where(p => p.Variantes.Any(v => v.Precio >= minPrecio && v.Precio <= maxPrecio))
                .ToList();

            if (!string.IsNullOrEmpty(talla) && int.TryParse(talla, out int tint))
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Variantes.Any(v => v.Tallas.Any(t => t.Talla == tint)))
                    .ToList();
            }

            if (marcas != null && marcas.Count > 0)
            {
                filteredProducts = filteredProducts
                    .Where(p => marcas.Contains(p.Marca))
                    .ToList();
            }

            return filteredProducts
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }

        public void ActualizarStockTalla(ObjectId productoId, ObjectId tallaId, int nuevoStock)
        {
            var filter = Builders<Producto>.Filter.And(
                Builders<Producto>.Filter.Eq(p => p.Id, productoId),
                Builders<Producto>.Filter.ElemMatch(p => p.Variantes, v => v.Tallas.Any(t => t.Id == tallaId))
            );

            var update = Builders<Producto>.Update.Set("variantes.$[].tallas.$[t].stock", nuevoStock);

            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("t._id", tallaId))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            _productoCollection.UpdateOne(filter, update, updateOptions);
        }
        public void VerificarStockSuficiente(ObjectId tallaId, int cantidadARestar)
        {
            var producto = _productoCollection.Find(p => p.Variantes.Any(v => v.Tallas.Any(t => t.Id == tallaId))).FirstOrDefault();

            if (producto == null)
            {
                throw new Exception("NoProdFound");
            }

            var talla = producto.Variantes.SelectMany(v => v.Tallas).FirstOrDefault(t => t.Id == tallaId);

            if (talla == null)
            {
                throw new Exception("NoSizeFromProdFound");
            }

            if (talla.Stock - cantidadARestar < 0)
            {
                throw new Exception("NotEnoughtStock");
            }
        }

        public void RestarStockTalla(ObjectId tallaId, int cantidadARestar)
        {
            var filter = Builders<Producto>.Filter.ElemMatch(p => p.Variantes,
                v => v.Tallas.Any(t => t.Id == tallaId));

            var update = Builders<Producto>.Update.Inc("variantes.$[].tallas.$[t].stock", -cantidadARestar);

            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("t._id", tallaId))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            _productoCollection.UpdateOne(filter, update, updateOptions);
        }


        public Producto GetProductoByTallaId(ObjectId tallaId)
        {
            return _productoCollection.Find(p => p.Variantes.Any(v => v.Tallas.Any(t => t.Id == tallaId))).FirstOrDefault();
        }

        public long GetTotalProductos()
        {
            return _productoCollection.CountDocuments(_ => true);
        }

    }

}