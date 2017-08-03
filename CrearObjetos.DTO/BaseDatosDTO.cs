using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO
{
    public class BaseDatosDTO 
    {
        public string NombreBaseDatos { get; set; }
        public string NombreEsquema { get; set; }       
        public string NombreTabla { get; set; }
        public List<InformacionTablaDTO> lstInInformacionTabla { get; set; }
    }
}
