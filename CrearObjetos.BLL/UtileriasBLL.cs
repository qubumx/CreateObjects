using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Configuration;

namespace CrearObjetos.BLL
{
    public class UtileriasBLL : Instance<UtileriasBLL>
    {
        public Response<Boolean> ValidarConexion(GestorBaseDatosDTO informacioGestorBaseDatos)
        {

            Response<Boolean> respuesta = new Response<Boolean>();
            Response<String> respuestaCadenaConexion = new Response<String>();

            if (informacioGestorBaseDatos == null)
            {
                respuesta.UserMessage = "No fue capturada la información del gestor de base de datos.";
                respuesta.ResponseType = false;
                respuesta.StatusType = StatusType.Error;
                respuesta.ListError.Add("El parametro informacioGestorBaseDatos no debe de ser nulo");
                return respuesta;
            }

            switch (informacioGestorBaseDatos.GestorBaseDatos)
            {
                case EnumGestorBaseDatos.MicrosoftSQLServer:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);
                    respuesta = CrearObjetos.DLL.UtileriasDLL.Instances.ValidarCadenaConexion(respuestaCadenaConexion.ResponseType.ToString(), informacioGestorBaseDatos.GestorBaseDatos);
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);
                    respuesta = CrearObjetos.DLL.UtileriasDLL.Instances.ValidarCadenaConexion(respuestaCadenaConexion.ResponseType.ToString(), informacioGestorBaseDatos.GestorBaseDatos);
                    break;
            }

            return respuesta;
        }
        
        public Response<BaseDatosDTO> ObtenerBaseDatos()
        {
            Response<BaseDatosDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerBaseDatos();
            return response;
        }

        public Response<EsquemaDTO> ObtenerEsquemas(BaseDatosDTO baseDatos)
        {
            Response<EsquemaDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerEsquemas(baseDatos);
            return response;
        }

        public Response<TablaDTO> ObtenerTablas(BaseDatosDTO baseDatos, EsquemaDTO esquemaTabla)
        {
            Response<TablaDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerTablas(baseDatos, esquemaTabla);
            return response;
        }

        public Response<TablaDTO> ObtenerTablasOracle(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<String> respuestaCadenaConexion = new Response<String>();
            respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);

            Response<TablaDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerTablasOracle(informacioGestorBaseDatos, respuestaCadenaConexion.ResponseType);
            return response;
        }

        public Response<ProyectoDTO> ObtenerProyectos(ProyectoDTO proyecto)
        {
            Response<ProyectoDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerProyectos(proyecto);
            return response;
        }

        public Response<UsuarioProyectoDTO> ObtenerUsuariosProyecto(ProyectoDTO proyecto)
        {
            Response<UsuarioProyectoDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerUsuariosProyecto(proyecto);
            return response;
        }

        public Response<InformacionTablaDTO> LeerCamposTabla(EsquemaDTO esquema, TablaDTO tabla)
        {
            Response<InformacionTablaDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.LeerCamposTabla(esquema, tabla);
            return response;
        }

        public String DevuelveTipo(String tipoSql)
        {
            String response = CrearObjetos.DLL.UtileriasDLL.Instances.DevuelveTipo(tipoSql);
            return response;
        }

        private Response<String> ArmarCadenaConexionMicrosoftSQLServer(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<String> respuestaSQL = new Response<String>();
            String cadenaConexion = ConfigurationManager.AppSettings["ConexionSQL"].ToString();

            //Data Source=SERVIDOR:PUERTO/NOMBRE_SERVICIO; Persist Security Info=True; User ID=NOMBRE_USUARIO; Password=CONTRASENIA
            cadenaConexion = cadenaConexion.Replace("SERVIDOR", informacioGestorBaseDatos.Servidor);
            cadenaConexion = cadenaConexion.Replace("PUERTO", informacioGestorBaseDatos.Puerto.ToString());
            cadenaConexion = cadenaConexion.Replace("NOMBRE_SERVICIO", informacioGestorBaseDatos.NombreServicio);
            cadenaConexion = cadenaConexion.Replace("NOMBRE_USUARIO", informacioGestorBaseDatos.NombreUsuario);
            cadenaConexion = cadenaConexion.Replace("CONTRASENIA", informacioGestorBaseDatos.Contrasenia);
            respuestaSQL.ResponseType = cadenaConexion;

            return respuestaSQL;
        }

        private Response<String> ArmarCadenaConexionOracle(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<String> respuestaOracle = new Response<string>();

            String cadenaConexion = ConfigurationManager.AppSettings["ConexionOracle"].ToString();

            //Data Source=SERVIDOR:PUERTO/NOMBRE_SERVICIO; Persist Security Info=True; User ID=NOMBRE_USUARIO; Password=CONTRASENIA
            cadenaConexion = cadenaConexion.Replace("SERVIDOR", informacioGestorBaseDatos.Servidor);
            cadenaConexion = cadenaConexion.Replace("PUERTO", informacioGestorBaseDatos.Puerto.ToString());
            cadenaConexion = cadenaConexion.Replace("NOMBRE_SERVICIO", informacioGestorBaseDatos.NombreServicio);
            cadenaConexion = cadenaConexion.Replace("NOMBRE_USUARIO", informacioGestorBaseDatos.NombreUsuario);
            cadenaConexion = cadenaConexion.Replace("CONTRASENIA", informacioGestorBaseDatos.Contrasenia);
            respuestaOracle.ResponseType = cadenaConexion;

            return respuestaOracle;
        }
    }
}