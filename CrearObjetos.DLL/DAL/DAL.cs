using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DLL.DAL
{
    public static class DAL
    {
        public static IDbContext Context()
        {
            return new DbContext().ConnectionStringName("ConexionBD", new SqlServerProvider());
        }
    }
}
