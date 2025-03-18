using Model.Product;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class IVAManager
    {
        private IMongoCollection<IVA> _ivaCollectiom;

        public IVAManager(MongoDBConnection connection)
        {
            _ivaCollectiom = connection.GetCollection<IVA>("iva");
        }

        public IVA GetIVA(ObjectId id)
        {
            return _ivaCollectiom.Find<IVA>(i => i.Id == id).FirstOrDefault();
        }
    }
}
