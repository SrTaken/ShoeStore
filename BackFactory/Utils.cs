using DB;
using Model.Cesta;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackFactory
{
    public static class Utils
    {
        public static MongoDBConnection mongoDBConnection { get; set; }
        public static Usuario LoggedUser { get; set; }
        public static Cesta MyCesta { get; set; }

        //Managers
        public static CestaManager CestaManager { get; set; }
        public static UsuarioManager UsuarioManager { get; set; }
        public static ProductoManager ProductoManager { get; set; }
        public static CategoriaManager CategoriaManager { get; set; }
        public static MetodoEnvioManager MetodoEnvioManager { get; set; }
        public static IVAManager IVAManager { get; set; }
        public static EmpresaManager EmpresaManager { get; set; }
        public static FacturaManager FacturaManager { get; set; }
    }
}
