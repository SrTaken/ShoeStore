using Model.Factura;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class FacturaManager
    {
        private IMongoCollection<Factura> _facturaCollection;

        public FacturaManager(MongoDBConnection connection)
        {
            _facturaCollection = connection.GetCollection<Factura>("factura");
        }

        public void InsertFactura(Factura factura)
        {
            _facturaCollection.InsertOne(factura);
        }
    }
}
