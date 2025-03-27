using Model;
using Model.Cesta;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class EmpresaManager
    {

        private static IMongoCollection<Empresa> _empresaCollection;
        public EmpresaManager(MongoDBConnection connection) 
        {
            _empresaCollection = connection.GetCollection<Empresa>("empresa");
        }

        public Empresa GetEmpresa()
        {
            return _empresaCollection.Find(e => true).FirstOrDefault();
        }

    }
}
