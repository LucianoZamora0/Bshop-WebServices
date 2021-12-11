using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bshop_WebServices.Configuration
{
    /*Clase auxiliar para rescatar los valores predefinidos
     tales como los de conexion a DB, etc.*/
    public class ServicesConfig
    {
        public string Host { get; set; }
        public string DataBaseName { get; set; }
        public string DataBaseUser { get; set; }
        public string DataBasePwd { get; set; }

    }
}
