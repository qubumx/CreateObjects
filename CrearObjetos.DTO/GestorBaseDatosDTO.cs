using System;

namespace CrearObjetos.DTO
{
    public class GestorBaseDatosDTO
    {
        public String Servidor { get; set; }
        public String NombreUsuario { get; set; }
        public String Contrasenia { get; set; }
        public String NombreServicio { get; set; }
        public Int32 Puerto { get; set; }
        public EnumGestorBaseDatos GestorBaseDatos { get; set; }
    }
}
