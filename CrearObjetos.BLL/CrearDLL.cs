using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Text;

namespace CrearObjetos.BLL
{
    public class CrearDLL: Instance<CrearDLL>
    {
        private StringBuilder cuerpoBusiness { get; set; }
        public String cuerpoFinalBusiness { get; set; }
        public String rutaCreacionBusiness { get; set; }
        public String nombreBaseDatos { get; set; }
        public String esquemaTabla { get; set; }
        public String nombreTabla { get; set; }
        public Response<InformacionTablaDTO> ResponseTabla { get; set; }

        public CrearDLL()
        {
            this.cuerpoBusiness = new StringBuilder();
            this.rutaCreacionBusiness = String.Empty;
            this.nombreBaseDatos = String.Empty;
            this.esquemaTabla = String.Empty;
            this.nombreTabla = String.Empty;
            this.ResponseTabla = new Response<InformacionTablaDTO>();
            this.cuerpoFinalBusiness = String.Empty;
        }

        public Response<String> ConstruccionBusiness(ProyectoDTO proyecto)
        {
            Response<String> response = new Response<String>();            

            this.ResponseTabla = UtileriasBLL.Instances.LeerCamposTabla(proyecto);

            if (this.ResponseTabla.StatusType == StatusType.Ok)
            {
                this.cuerpoBusiness.Append("using System;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using System.Collections.Generic;" + Environment.NewLine);                
                this.cuerpoBusiness.Append("using System.Data.SqlClient;" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.Bussiness.Genericas;" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.DTO.Genericas;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using Ingram_DLL.General;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using Ingram_Obj.Utilities;" + Environment.NewLine);

                this.cuerpoBusiness.Append("using Ingram_Obj." + proyecto.NombreTabla + ";" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.DAL;" + Environment.NewLine);
                this.cuerpoBusiness.Append("" + Environment.NewLine);
                this.cuerpoBusiness.Append("namespace Ingram_DLL." + proyecto.NombreTabla + "" + Environment.NewLine);
                this.cuerpoBusiness.Append("{" + Environment.NewLine);
                this.cuerpoBusiness.Append("    public class " + proyecto.NombreTabla + "DLL : Instance<" + proyecto.NombreTabla + "DLL>" + Environment.NewLine);
                this.cuerpoBusiness.Append("    {" + Environment.NewLine);

                CrearBusinessSelect(proyecto);
                CrearBusinessInsert(proyecto);
                CrearBusinessUpdate(proyecto);
                CrearBusinessDelete(proyecto);
                CrearBusinessFilterSelect(proyecto);

                this.cuerpoBusiness.Append("    }" + Environment.NewLine);
                this.cuerpoBusiness.Append("}" + Environment.NewLine);

                //string directorioRaiz = "C:\\BUSINESS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\BUSINESS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //rutaCreacionBusiness = directorioTabla;

                //if (File.Exists(this.rutaCreacionBusiness + @"\Gestionar" + this.nombreTabla + ".cs"))
                //{
                //    File.Delete(this.rutaCreacionBusiness + @"\Gestionar" + this.nombreTabla + ".cs");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionBusiness + @"\Gestionar" + this.nombreTabla + ".cs"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoBusiness.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();
                this.cuerpoFinalBusiness = this.cuerpoBusiness.ToString();
                this.cuerpoBusiness.Clear();
                //        //this.ResponseTabla.EstatusBS = EstatusBS.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.ResponseTabla.EstatusBS = EstatusBS.Error;
                //        this.ResponseTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear la capa de negocio de la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + proyecto.NombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + proyecto.NombreTabla + " filtrada.", "ConstruccionBusiness", "CrearDLL", "Administrador");
            }

            response.ResponseType = this.cuerpoFinalBusiness;
            return response;
        }

        private void CrearBusinessSelect(ProyectoDTO proyecto)
        {
            this.cuerpoBusiness.Append("        public Response<" + proyecto.NombreTabla + "Obj> " + proyecto.NombreTabla + "Sel(String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + proyecto.NombreTabla + "Obj> ObjResponse = new Response<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            //this.cuerpoBusiness.Append("                    ObjResponse.ResponseType = context.StoredProcedure(\"" + this.esquemaTabla + ".Sp_ps_" + this.nombreTabla + "\")" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.ListRecords = context.StoredProcedure(\"" + proyecto.NombreEsquema + ".Sp_ps_" + proyecto.NombreTabla + "\")" + Environment.NewLine);
            this.cuerpoBusiness.Append("                            .QueryMany<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + proyecto.NombreTabla + "Sel\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + proyecto.NombreTabla + "Sel\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessInsert(ProyectoDTO proyecto)
        {

            this.cuerpoBusiness.Append("        public Response<" + proyecto.NombreTabla + "Obj> " + proyecto.NombreTabla + "Ins(" + proyecto.NombreTabla + "Obj Obj" + proyecto.NombreTabla + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + proyecto.NombreTabla + "Obj> ObjResponse = new Response<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + proyecto.NombreEsquema + ".Sp_pi_" + proyecto.NombreTabla + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((!item.EsPK) && (item.NombreColumna != "Activo") && (item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioUpd") && (item.NombreColumna != "FechaUpd"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + proyecto.NombreTabla + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + proyecto.NombreTabla + "Ins\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + proyecto.NombreTabla + "Ins\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessUpdate(ProyectoDTO proyecto)
        {

            this.cuerpoBusiness.Append("        public Response<" + proyecto.NombreTabla + "Obj> " + proyecto.NombreTabla + "Upd(" + proyecto.NombreTabla + "Obj Obj" + proyecto.NombreTabla + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + proyecto.NombreTabla + "Obj> ObjResponse = new Response<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + proyecto.NombreEsquema + ".Sp_pu_" + proyecto.NombreTabla + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioIns") && (item.NombreColumna != "FechaUpd"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + proyecto.NombreTabla + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + proyecto.NombreTabla + "Upd\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + proyecto.NombreTabla + "Upd\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessDelete(ProyectoDTO proyecto)
        {

            this.cuerpoBusiness.Append("        public Response<" + proyecto.NombreTabla + "Obj> " + proyecto.NombreTabla + "Del(" + proyecto.NombreTabla + "Obj Obj" + proyecto.NombreTabla + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + proyecto.NombreTabla + "Obj> ObjResponse = new Response<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + proyecto.NombreEsquema + ".Sp_pd_" + proyecto.NombreTabla + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if (item.EsPK)
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + proyecto.NombreTabla + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
                else if (item.NombreColumna == "UsuarioUpd")
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + proyecto.NombreTabla + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + proyecto.NombreTabla + "Del\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + proyecto.NombreTabla + "Del\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessFilterSelect(ProyectoDTO proyecto)
        {
            this.cuerpoBusiness.Append("        public Response<" + proyecto.NombreTabla + "Obj> " + proyecto.NombreTabla + "FilSel(" + proyecto.NombreTabla + "Obj Obj" + proyecto.NombreTabla + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + proyecto.NombreTabla + "Obj> ObjResponse = new Response<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.ListRecords = context.StoredProcedure(\"" + proyecto.NombreEsquema + ".Sp_pfs_" + proyecto.NombreTabla + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((item.NombreColumna != "UsuarioIns") && (item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioUpd") && (item.NombreColumna != "FechaUpd") && (item.NombreColumna != "Activo"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + proyecto.NombreTabla + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .QueryMany<" + proyecto.NombreTabla + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + proyecto.NombreTabla + "FilIns\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + proyecto.NombreTabla + "FilIns\", \"Log_" + proyecto.NombreTabla + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }
    }
}