using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrearObjetos.BLL;

namespace CreadorObjetos.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ValidarConexion(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<Boolean> ResponseValidarConexion = UtileriasBLL.Instances.ValidarConexion(informacioGestorBaseDatos);
            return Json(ResponseValidarConexion, JsonRequestBehavior.AllowGet);     
        }

        [HttpGet]
        public JsonResult ObtenerBaseDatos()
        {
            Response<BaseDatosDTO> ResponseBaseDatos = UtileriasBLL.Instances.ObtenerBaseDatos();
            return Json(ResponseBaseDatos, JsonRequestBehavior.AllowGet);                        
        }

        [HttpGet]
        public JsonResult ObtenerEsquemas(String baseDatos)
        {            
            BaseDatosDTO objBaseDatos = new BaseDatosDTO { NombreBaseDatos = baseDatos };
            Response<EsquemaDTO> ResponseEsquema = UtileriasBLL.Instances.ObtenerEsquemas(objBaseDatos);
            return Json(ResponseEsquema, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerTablas(String baseDatos, String esquema)
        {
            //Utilerias ObjUtilerias = new Utilerias { baseDatos = baseDatos, esquemaTabla = esquema };
            BaseDatosDTO objBaseDatos = new BaseDatosDTO { NombreBaseDatos = baseDatos };
            EsquemaDTO objEsquema = new EsquemaDTO { NombreEsquema = esquema };
            Response<TablaDTO> ResponseTabla = UtileriasBLL.Instances.ObtenerTablas(objBaseDatos, objEsquema);
            return Json(ResponseTabla, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerTablasOracle(GestorBaseDatosDTO informacioGestorBaseDatos)
        {
            Response<TablaDTO> ResponseTabla = UtileriasBLL.Instances.ObtenerTablasOracle(informacioGestorBaseDatos);
            return Json(ResponseTabla, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerProyectos()
        {
            ProyectoDTO objProyecto = new ProyectoDTO { NombreProyecto = null };
            Response<ProyectoDTO> ResponseProyectos = UtileriasBLL.Instances.ObtenerProyectos(objProyecto);
            return Json(ResponseProyectos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerUsuariosProyecto(int proyectoId)
        {            
            ProyectoDTO objProyecto = new ProyectoDTO { ProyectoId = proyectoId };
            Response<UsuarioProyectoDTO> ResponseUsuarioProyecto = UtileriasBLL.Instances.ObtenerUsuariosProyecto(objProyecto);
            return Json(ResponseUsuarioProyecto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerarObjetos(String baseDatos, String esquema, String tabla, int proyectoId, int usuarioIdProyecto)
        {
            BaseDatosDTO objBaseDatos = new BaseDatosDTO { NombreBaseDatos = baseDatos };
            EsquemaDTO objEsquema = new EsquemaDTO { NombreEsquema = esquema };
            TablaDTO objTabla = new TablaDTO { NombreTabla = tabla };
            UsuarioProyectoDTO objUsuarioProyecto = new UsuarioProyectoDTO { ProyectoId = proyectoId, UsuarioProyectoId = usuarioIdProyecto };
            ProyectoDTO objProyecto = new ProyectoDTO { NombreProyecto = null, ProyectoId = proyectoId };


            Response<InformacionTablaDTO> ResponseTabla = new Response<InformacionTablaDTO>();
            Response<ProyectoDTO> ResponseProyectos = UtileriasBLL.Instances.ObtenerProyectos(objProyecto);
            IEnumerable<ProyectoDTO> proyecto = from pro in ResponseProyectos.ListRecords
                                             where pro.ProyectoId == proyectoId
                                             select pro;
            objProyecto.NombreProyecto = proyecto.Single().NombreProyecto;


            Response<UsuarioProyectoDTO> ResponseUsuarioProyecto = UtileriasBLL.Instances.ObtenerUsuariosProyecto(objProyecto);
            IEnumerable<UsuarioProyectoDTO> usuarioProyecto = from pro in ResponseUsuarioProyecto.ListRecords
                                                              where pro.UsuarioProyectoId == usuarioIdProyecto
                                                              select pro;
            objUsuarioProyecto.NombreColaborador = usuarioProyecto.Single().NombreColaborador;

            Response<String> responseCrearDTO = CrearDTO.Instances.ConstruccionDTO(objEsquema, objTabla);

            Response<String> responseCrearSP = CrearSP.Instances.SPSCRUD(objEsquema, objTabla, objProyecto, objUsuarioProyecto, objBaseDatos);

            Response<String> responseCrearDLL = CrearDLL.Instances.ConstruccionBusiness(objEsquema, objTabla);


            //CrearWSDTO ObjCrearWSDTO = new CrearWSDTO { esquemaTabla = esquema, nombreTabla = tabla, nombreBaseDatos = baseDatos };
            //ObjCrearWSDTO.ConstruccionWSDTO();

            //string cuerpoObjWSDLL = string.Empty;
            //CrearWSDLL ObjCrearWSDLL = new CrearWSDLL { esquemaTabla = esquema, nombreTabla = tabla, nombreBaseDatos = baseDatos };
            //ObjCrearWSDLL.ConstruccionWSDLL();

            //string cuerpoObjWSBLL = string.Empty;
            //CrearWSBLL ObjCrearWSBLL = new CrearWSBLL { esquemaTabla = esquema, nombreTabla = tabla, nombreBaseDatos = baseDatos };
            //ObjCrearWSBLL.ConstruccionWSBLL();

            //return Json(new { ObjCrearSP, ObjCrearDTO, ObjCrearBusiness, ObjCrearWSDTO, ObjCrearWSDLL, ObjCrearWSBLL }, JsonRequestBehavior.AllowGet);
            return Json(new { responseCrearDTO, responseCrearSP, responseCrearDLL }, JsonRequestBehavior.AllowGet);
        }

    }
}
