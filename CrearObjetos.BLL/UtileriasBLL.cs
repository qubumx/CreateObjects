using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;

namespace CrearObjetos.BLL
{
    public class UtileriasBLL : Instance<UtileriasBLL>
    {
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
    }
}