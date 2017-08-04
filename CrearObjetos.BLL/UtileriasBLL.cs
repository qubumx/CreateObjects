using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Configuration;

namespace CrearObjetos.BLL
{
    public class UtileriasBLL : Instance<UtileriasBLL>
    {
        public Response<bool> ValidarConexion(GestorBaseDatosDTO informacioGestorBaseDatos)
        {

            Response<bool> respuesta = new Response<bool>();
            Response<string> respuestaCadenaConexion = new Response<string>();

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
                    respuestaCadenaConexion = ArmarCadenaConexionMicrosoftSQLServer(informacioGestorBaseDatos);
                    //respuesta = DLL.UtileriasDLL.Instances.ValidarCadenaConexion(respuestaCadenaConexion.ResponseType.ToString(), informacioGestorBaseDatos.GestorBaseDatos);
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);
                    //respuesta = DLL.UtileriasDLL.Instances.ValidarCadenaConexion(respuestaCadenaConexion.ResponseType.ToString(), informacioGestorBaseDatos.GestorBaseDatos);
                    break;
            }
                        
            return DLL.UtileriasDLL.Instances.ValidarCadenaConexion(respuestaCadenaConexion.ResponseType.ToString(), informacioGestorBaseDatos.GestorBaseDatos);
        }
        
        public Response<BaseDatosDTO> ObtenerBaseDatos(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<string> respuestaCadenaConexion = new Response<string>();

            switch (informacioGestorBaseDatos.GestorBaseDatos)
            {
                case EnumGestorBaseDatos.MicrosoftSQLServer:
                    respuestaCadenaConexion = ArmarCadenaConexionMicrosoftSQLServer(informacioGestorBaseDatos);                    
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);                    
                    break;
            }

            return DLL.UtileriasDLL.Instances.ObtenerBaseDatos(respuestaCadenaConexion.ResponseType);
        }

        public Response<EsquemaDTO> ObtenerEsquemas(GestorBaseDatosDTO informacioGestorBaseDatos)
        {

            Response<EsquemaDTO> response = new Response<EsquemaDTO>();
            Response <string> respuestaCadenaConexion = new Response<string>();

            switch (informacioGestorBaseDatos.GestorBaseDatos)
            {
                case EnumGestorBaseDatos.MicrosoftSQLServer:
                    respuestaCadenaConexion = ArmarCadenaConexionMicrosoftSQLServer(informacioGestorBaseDatos);
                    respuestaCadenaConexion.ResponseType= respuestaCadenaConexion.ResponseType.Replace("master", informacioGestorBaseDatos.NombreBaseDatos);
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);
                    break;
            }

            return DLL.UtileriasDLL.Instances.ObtenerEsquemas(informacioGestorBaseDatos, respuestaCadenaConexion.ResponseType);
        }

        public Response<TablaDTO> ObtenerTablas(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<TablaDTO> response = new Response<TablaDTO>();
            Response<string> respuestaCadenaConexion = new Response<string>();

            switch (informacioGestorBaseDatos.GestorBaseDatos)
            {
                case EnumGestorBaseDatos.MicrosoftSQLServer:
                    respuestaCadenaConexion = ArmarCadenaConexionMicrosoftSQLServer(informacioGestorBaseDatos);
                    respuestaCadenaConexion.ResponseType = respuestaCadenaConexion.ResponseType.Replace("master", informacioGestorBaseDatos.NombreBaseDatos);
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(informacioGestorBaseDatos);
                    break;
            }

            return DLL.UtileriasDLL.Instances.ObtenerTablas(informacioGestorBaseDatos, respuestaCadenaConexion.ResponseType);
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

        //public Response<UsuarioProyectoDTO> ObtenerUsuariosProyecto(ProyectoDTO proyecto)
        //{
        //    Response<UsuarioProyectoDTO> response = CrearObjetos.DLL.UtileriasDLL.Instances.ObtenerUsuariosProyecto(proyecto);
        //    return response;
        //}

        //public Response<InformacionTablaDTO> LeerCamposTabla(EsquemaDTO esquema, TablaDTO tabla)
        public Response<InformacionTablaDTO> LeerCamposTabla(ProyectoDTO proyecto)
        {
            //Response<InformacionTablaDTO> response = DLL.UtileriasDLL.Instances.LeerCamposTabla(esquema, tabla);
            Response<InformacionTablaDTO> response = new Response<InformacionTablaDTO>();
           
            Response<string> respuestaCadenaConexion = new Response<string>();

            switch (proyecto.GestorBaseDatos)
            {
                case EnumGestorBaseDatos.MicrosoftSQLServer:
                    respuestaCadenaConexion = ArmarCadenaConexionMicrosoftSQLServer(proyecto);
                    respuestaCadenaConexion.ResponseType = respuestaCadenaConexion.ResponseType.Replace("master", proyecto.NombreBaseDatos);
                    response = DLL.UtileriasDLL.Instances.LeerCamposTablaSQL(proyecto, respuestaCadenaConexion.ResponseType);
                    break;
                case EnumGestorBaseDatos.Oracle:
                    respuestaCadenaConexion = ArmarCadenaConexionOracle(proyecto);
                    break;
            }
           
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

            //Data Source = SERVIDOR; Initial Catalog = BASE_DATOS; User Id = NOMBRE_USUARIO; Password = CONTRASENIA
            cadenaConexion = cadenaConexion.Replace("SERVIDOR", informacioGestorBaseDatos.Servidor);           
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