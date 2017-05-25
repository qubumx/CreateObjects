using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO
{
    public class InformacionTablaDTO
    {
        public String NombreColumna { get; set; }
        public String TipoDato { get; set; }
        public Int32 LongitudMaxima { get; set; }
        public Boolean CampoNulo { get; set; }
        public Boolean EsPK { get; set; }
        public Boolean EsFK { get; set; }
    }
}
