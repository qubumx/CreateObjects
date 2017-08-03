using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO
{
    public class TablaDTO
    {
        public string Esquema { get; set; }
        public string NombreTabla { get; set; }
        public string OtraPropiedad { get; set; }
        public List<InformacionTablaDTO> lstInInformacionTabla { get; set; }
    }
}