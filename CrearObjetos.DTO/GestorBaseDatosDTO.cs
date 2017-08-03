using System;

namespace CrearObjetos.DTO
{
    public class GestorBaseDatosDTO : BaseDatosDTO
    {
        public string Servidor { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string NombreServicio { get; set; }
        public Int32 Puerto { get; set; }
        public EnumGestorBaseDatos GestorBaseDatos { get; set; }
    }
}
