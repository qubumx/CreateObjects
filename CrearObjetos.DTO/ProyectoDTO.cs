using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO
{
    public class ProyectoDTO
    {
        public int ProyectoId { get; set; }
        public String NombreProyecto { get; set; }
        public Boolean Activo { get; set; }
        public String UsuarioIns { get; set; }
        public DateTime FechaIns { get; set; }
        public String UsuarioUpd { get; set; }
        public DateTime FechaUpd { get; set; }
    }
}