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

        public Response<String> ConstruccionBusiness(EsquemaDTO esquema, TablaDTO tabla)
        {
            Response<String> response = new Response<String>();            

            this.ResponseTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);

            if (this.ResponseTabla.StatusType == StatusType.Ok)
            {
                this.cuerpoBusiness.Append("using System;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using System.Collections.Generic;" + Environment.NewLine);                
                this.cuerpoBusiness.Append("using System.Data.SqlClient;" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.Bussiness.Genericas;" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.DTO.Genericas;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using Ingram_DLL.General;" + Environment.NewLine);
                this.cuerpoBusiness.Append("using Ingram_Obj.Utilities;" + Environment.NewLine);
                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);
                this.cuerpoBusiness.Append("using Ingram_Obj." + tabla1 + ";" + Environment.NewLine);
                //this.cuerpoBusiness.Append("using BS.DAL;" + Environment.NewLine);
                this.cuerpoBusiness.Append("" + Environment.NewLine);
                this.cuerpoBusiness.Append("namespace Ingram_DLL." + tabla1 + "" + Environment.NewLine);
                this.cuerpoBusiness.Append("{" + Environment.NewLine);
                this.cuerpoBusiness.Append("    public class " + tabla1 + "DLL : Instance<" + tabla1 + "DLL>" + Environment.NewLine);
                this.cuerpoBusiness.Append("    {" + Environment.NewLine);

                CrearBusinessSelect(esquema,tabla);
                CrearBusinessInsert(esquema, tabla);
                CrearBusinessUpdate(esquema, tabla);
                CrearBusinessDelete(esquema, tabla);
                CrearBusinessFilterSelect(esquema, tabla);

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
                response.UserMessage = "No existe información de la tabla " + tabla.NombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + tabla.NombreTabla + " filtrada.", "ConstruccionBusiness", "CrearDLL", "Administrador");
            }

            response.ResponseType = this.cuerpoFinalBusiness;
            return response;
        }

        private void CrearBusinessSelect(EsquemaDTO esquema, TablaDTO tabla)
        {
            String tabla1 = tabla.NombreTabla;
            tabla1 = tabla1.Remove(0, 3);

            this.cuerpoBusiness.Append("        public Response<" + tabla1 + "Obj> " + tabla1 + "Sel(String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + tabla1 + "Obj> ObjResponse = new Response<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            //this.cuerpoBusiness.Append("                    ObjResponse.ResponseType = context.StoredProcedure(\"" + this.esquemaTabla + ".Sp_ps_" + this.nombreTabla + "\")" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.ListRecords = context.StoredProcedure(\"" + esquema.NombreEsquema + ".Sp_ps_" + tabla1 + "\")" + Environment.NewLine);
            this.cuerpoBusiness.Append("                            .QueryMany<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + tabla1 + "Sel\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + tabla1 + "Sel\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessInsert(EsquemaDTO esquema, TablaDTO tabla)
        {
            String tabla1 = tabla.NombreTabla;
            tabla1 = tabla1.Remove(0, 3);

            this.cuerpoBusiness.Append("        public Response<" + tabla1 + "Obj> " + tabla1 + "Ins(" + tabla1 + "Obj Obj" + tabla1 + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + tabla1 + "Obj> ObjResponse = new Response<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + esquema.NombreEsquema + ".Sp_pi_" + tabla1 + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((!item.EsPK) && (item.NombreColumna != "Activo") && (item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioUpd") && (item.NombreColumna != "FechaUpd"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + tabla1 + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + tabla1 + "Ins\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + tabla1 + "Ins\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessUpdate(EsquemaDTO esquema, TablaDTO tabla)
        {
            String tabla1 = tabla.NombreTabla;
            tabla1 = tabla1.Remove(0, 3);

            this.cuerpoBusiness.Append("        public Response<" + tabla1 + "Obj> " + tabla1 + "Upd(" + tabla1 + "Obj Obj" + tabla1 + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + tabla1 + "Obj> ObjResponse = new Response<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + esquema.NombreEsquema + ".Sp_pu_" + tabla1 + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioIns") && (item.NombreColumna != "FechaUpd"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + tabla1 + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + tabla1 + "Upd\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + tabla1 + "Upd\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessDelete(EsquemaDTO esquema, TablaDTO tabla)
        {
            String tabla1 = tabla.NombreTabla;
            tabla1 = tabla1.Remove(0, 3);

            this.cuerpoBusiness.Append("        public Response<" + tabla1 + "Obj> " + tabla1 + "Del(" + tabla1 + "Obj Obj" + tabla1 + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + tabla1 + "Obj> ObjResponse = new Response<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.StatusResponse = context.StoredProcedure(\"" + esquema.NombreEsquema + ".Sp_pd_" + tabla1 + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if (item.EsPK)
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + tabla1 + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
                else if (item.NombreColumna == "UsuarioUpd")
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + tabla1 + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .Execute();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + tabla1 + "Del\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + tabla1 + "Del\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }

        private void CrearBusinessFilterSelect(EsquemaDTO esquema, TablaDTO tabla)
        {
            String tabla1 = tabla.NombreTabla;
            tabla1 = tabla1.Remove(0, 3);

            this.cuerpoBusiness.Append("        public Response<" + tabla1 + "Obj> " + tabla1 + "FilSel(" + tabla1 + "Obj Obj" + tabla1 + ", String UserName)" + Environment.NewLine);
            this.cuerpoBusiness.Append("        {" + Environment.NewLine);
            this.cuerpoBusiness.Append("            Response<" + tabla1 + "Obj> ObjResponse = new Response<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("            using (var context = DAL.Context())" + Environment.NewLine);
            this.cuerpoBusiness.Append("            {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                try" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    ObjResponse.ListRecords = context.StoredProcedure(\"" + esquema.NombreEsquema + ".Sp_pfs_" + tabla1 + "\")" + Environment.NewLine);

            foreach (InformacionTablaDTO item in this.ResponseTabla.ListRecords)
            {
                if ((item.NombreColumna != "UsuarioIns") && (item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioUpd") && (item.NombreColumna != "FechaUpd") && (item.NombreColumna != "Activo"))
                {
                    this.cuerpoBusiness.Append("                            .Parameter(\"" + item.NombreColumna + "\", Obj" + tabla1 + "." + item.NombreColumna + ")" + Environment.NewLine);
                }
            }

            this.cuerpoBusiness.Append("                            .QueryMany<" + tabla1 + "Obj>();" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (SqlException sqlex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de base de datos: \" + sqlex.Message.ToString(), \"" + tabla1 + "FilIns\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("                catch (Exception ex)" + Environment.NewLine);
            this.cuerpoBusiness.Append("                {" + Environment.NewLine);
            this.cuerpoBusiness.Append("                    Log.LogFile(\"Error de sistema: \" + ex.Message.ToString(), \"" + tabla1 + "FilIns\", \"Log_" + tabla1 + "DLL\", UserName);" + Environment.NewLine);
            this.cuerpoBusiness.Append("                }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            }" + Environment.NewLine);
            this.cuerpoBusiness.Append("            return ObjResponse;" + Environment.NewLine);
            this.cuerpoBusiness.Append("        }" + Environment.NewLine);
            this.cuerpoBusiness.Append(Environment.NewLine);
        }
    }
}