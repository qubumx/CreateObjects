using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO
{
    public class InformacionTablaDTO
    {
        public string NombreColumna { get; set; }
        public string TipoDato { get; set; }
        public Int32 LongitudMaxima { get; set; }
        public bool CampoNulo { get; set; }
        public bool EsPK { get; set; }
        public bool EsFK { get; set; }
    }
}
