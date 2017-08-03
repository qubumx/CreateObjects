using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Text;

namespace CrearObjetos.BLL
{
    public class CrearSP:Instance<CrearSP>
    {
        #region Variables
        public String rutaCreacionSP { get; set; }
        public String baseDatos { get; set; }
        public String nombreTabla { get; set; }
        public String esquemaTabla { get; set; }
        public String autor { get; set; }
        public String sistema { get; set; }
        private StringBuilder cuerpoSP { get; set; }
        public String cuerpoFinalSP { get; set; }
        private StringBuilder campos { get; set; }
        private StringBuilder condicion { get; set; }
        private String accionCRUD { get; set; }
        //private Utilerias objUtilerias { get; set; }
        public Response<InformacionTablaDTO> respuestaTabla { get; set; }

        #endregion

        #region Constructor
        public CrearSP()
        {
            this.rutaCreacionSP = String.Empty;
            this.baseDatos = String.Empty;
            this.esquemaTabla = String.Empty;
            this.nombreTabla = String.Empty;
            this.autor = String.Empty;
            this.sistema = String.Empty;
            this.cuerpoSP = new StringBuilder();
            this.campos = new StringBuilder();
            this.condicion = new StringBuilder();
            this.accionCRUD = String.Empty;
            //this.objUtilerias = new Utilerias();
            this.respuestaTabla = new Response<InformacionTablaDTO>();
            this.cuerpoFinalSP = String.Empty;
        }
        #endregion

        #region CRUD

        public Response<String> SPSCRUD(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuario, BaseDatosDTO baseDatos)
        {
            Response<String> responseCRUD = new Response<string>();
            Response<String> response = new Response<string>();

            responseCRUD = CrearSPSelect(esquema, tabla,proyecto,usuario,baseDatos );
            this.cuerpoFinalSP += responseCRUD.ResponseType;

            responseCRUD = CrearSPDelete(esquema, tabla, proyecto, usuario, baseDatos);
            this.cuerpoFinalSP += responseCRUD.ResponseType;

            responseCRUD = CrearSPUpdate(esquema, tabla, proyecto, usuario, baseDatos);
            this.cuerpoFinalSP += responseCRUD.ResponseType;

            responseCRUD = CrearSPInsert(esquema, tabla, proyecto, usuario, baseDatos);
            this.cuerpoFinalSP += responseCRUD.ResponseType;

            responseCRUD = CrearSPFilterSelect(esquema, tabla, proyecto, usuario, baseDatos);
            this.cuerpoFinalSP += responseCRUD.ResponseType;

            response.ResponseType = this.cuerpoFinalSP;
            return response;
        }

        private void CrearSPEncabezado(ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            this.cuerpoSP.Append("USE [" + baseDatos.NombreBaseDatos + "]" + Environment.NewLine); 
            this.cuerpoSP.Append("GO" + Environment.NewLine);
            this.cuerpoSP.Append("-- =============================================" + Environment.NewLine);
            //this.cuerpoSP.Append("-- Autor          :" + usuarioProyecto.NombreColaborador + Environment.NewLine);
            this.cuerpoSP.Append("-- Fecha Creacion :" + System.DateTime.Now.ToString() + Environment.NewLine);
            this.cuerpoSP.Append("-- Sistema        :" + proyecto.NombreProyecto + Environment.NewLine);
            this.cuerpoSP.Append("-- Descripcion    :" + "" + this.accionCRUD + " tabla " + this.nombreTabla + Environment.NewLine);
            this.cuerpoSP.Append("-- =============================================" + Environment.NewLine + Environment.NewLine);
        }

        private Response<String> CrearSPSelect(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            Response<String> response = new Response<string>();

            this.accionCRUD = "Seleccionar los registros de la ";
            CrearSPEncabezado(proyecto, usuarioProyecto, baseDatos);
            //this.respuestaTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema,tabla);

            if (this.respuestaTabla.StatusType == StatusType.Ok)
            {
                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);

                this.cuerpoSP.Append("CREATE PROCEDURE [" + esquema.NombreEsquema + "].[Sp_ps_" + tabla1 + "]" + Environment.NewLine);
                this.cuerpoSP.Append("AS" + Environment.NewLine);
                this.cuerpoSP.Append("BEGIN" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT ON;" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN TRY" + Environment.NewLine);
                this.cuerpoSP.Append("              SELECT" + Environment.NewLine);

                int contador = 0;
                foreach (InformacionTablaDTO item in this.respuestaTabla.ListRecords)
                {
                    this.cuerpoSP.Append(contador == 0 ? "                       " + item.NombreColumna + "" + Environment.NewLine : "                       ," + item.NombreColumna + "" + Environment.NewLine);
                    contador++;
                }

                this.cuerpoSP.Append("              FROM" + Environment.NewLine);
                this.cuerpoSP.Append("                      [" + esquema.NombreEsquema + "].[" + tabla.NombreTabla + "] WITH (NOLOCK)" + Environment.NewLine);
                //this.cuerpoSP.Append("          END TRY" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("              EXEC bs_Admin.ErrorSel" + Environment.NewLine);
                //this.cuerpoSP.Append("          END CATCH" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT OFF;" + Environment.NewLine);
                this.cuerpoSP.Append("END" + Environment.NewLine);
                this.cuerpoSP.Append("GO" + Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);

                //string directorioRaiz = "C:\\SPS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\SPS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}
                //this.rutaCreacionSP = directorioTabla;

                //if (File.Exists(this.rutaCreacionSP + @"\" + this.nombreTabla + "Sel.sql"))
                //{
                //    File.Delete(this.rutaCreacionSP + @"\" + this.nombreTabla + "Sel.sql");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionSP + @"\" + this.nombreTabla + "Sel.sql"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoSP.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();    
                
    
                response.ResponseType = cuerpoSP.ToString();
                //this.cuerpoFinalSP = cuerpoSP.ToString();
                this.cuerpoSP.Clear();
                
                //        //this.respuestaTabla.StatusType = StatusType.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.respuestaTabla.StatusType = StatusType.Error;
                //        this.respuestaTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el procedimiento almacenado para la selección de información en la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                //this.respuestaTabla.StatusType = StatusType.Error;
                //this.respuestaTabla.AsigaErrorSistema("No existe información de la tabla " + this.nombreTabla + " filtrada.");

                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + this.nombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + this.nombreTabla + " filtrada.", "CrearSPSelect", "Utilerias", "Administrador");
            }
            return response;
        }

        private Response<String> CrearSPInsert(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            Response<String> response = new Response<string>();

            this.accionCRUD = "Ingresar un registro a la ";
            CrearSPEncabezado(proyecto, usuarioProyecto, baseDatos);

            if (this.respuestaTabla.StatusType == StatusType.Ok)
            {
                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);

                InformacionTablaDTO objTabla = this.respuestaTabla.ListRecords.Find(tbl => tbl.EsPK == true);
                //this.objUtilerias.tipoSql = objTabla.TipoDato;

                this.cuerpoSP.Append("CREATE PROCEDURE [" + esquema.NombreEsquema + "].[Sp_pi_" + tabla1 + "]" + Environment.NewLine);

                //this.respuestaTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);
                this.respuestaTabla.ListRecords.RemoveAll(tbl => (tbl.EsPK == true) || (tbl.NombreColumna == "UsuarioUpd") || (tbl.NombreColumna == "FechaUpd") || (tbl.NombreColumna == "Activo") || (tbl.NombreColumna == "FechaIns"));
                int contador = 0;
                foreach (InformacionTablaDTO item in this.respuestaTabla.ListRecords)
                {
                    if (item.TipoDato == "varchar")
                    {
                        this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                    }
                    else if (item.TipoDato == "nvarchar")
                    {
                        this.cuerpoSP.Append(contador == 0 ? "  @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(MAX)" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(MAX)" + Environment.NewLine);
                    }
                    else
                    {
                        this.cuerpoSP.Append(contador == 0 ? "  @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "" + Environment.NewLine);
                    }
                    this.campos.Append(contador == 0 ? "                            " + item.NombreColumna + Environment.NewLine : "                            ," + item.NombreColumna + Environment.NewLine);
                    this.condicion.Append(contador == 0 ? "                            @" + item.NombreColumna + Environment.NewLine : "                            ,@" + item.NombreColumna + Environment.NewLine);
                    contador++;

                }
                this.cuerpoSP.Append("AS" + Environment.NewLine);
                this.cuerpoSP.Append("BEGIN" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT ON;" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN TRY" + Environment.NewLine);
                this.cuerpoSP.Append("              INSERT INTO" + Environment.NewLine);
                this.cuerpoSP.Append("                          [" + esquema.NombreEsquema + "].[" + tabla.NombreTabla + "] " + Environment.NewLine);
                this.cuerpoSP.Append("                          (" + Environment.NewLine);
                this.cuerpoSP.Append(this.campos.ToString());
                this.cuerpoSP.Append("                          )" + Environment.NewLine);
                this.cuerpoSP.Append("              VALUES" + Environment.NewLine);
                this.cuerpoSP.Append("                          (" + Environment.NewLine);
                this.cuerpoSP.Append(this.condicion.ToString());
                this.cuerpoSP.Append("                          )" + Environment.NewLine);
                //this.cuerpoSP.Append("          END TRY" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("              EXEC bs_Admin.ErrorSel" + Environment.NewLine);
                //this.cuerpoSP.Append("          END CATCH" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT OFF;" + Environment.NewLine);
                this.cuerpoSP.Append("END" + Environment.NewLine);
                this.cuerpoSP.Append("GO" + Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);

                //string directorioRaiz = "C:\\SPS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\SPS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //this.rutaCreacionSP = directorioTabla;

                //if (File.Exists(this.rutaCreacionSP + @"\" + this.nombreTabla + "Ins.sql"))
                //{
                //    File.Delete(this.rutaCreacionSP + @"\" + this.nombreTabla + "Ins.sql");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionSP + @"\" + this.nombreTabla + "Ins.sql"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoSP.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();

                response.ResponseType = this.cuerpoSP.ToString();
                //this.cuerpoFinalSP += this.cuerpoSP.ToString();
                this.cuerpoSP.Clear();
                this.campos.Clear();
                this.condicion.Clear();
                //        //this.respuestaTabla.StatusType = StatusType.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.respuestaTabla.StatusType = StatusType.Error;
                //        this.respuestaTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el procedimiento almacenado para la inserción de información en la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                //this.respuestaTabla.StatusType = StatusType.Error;
                //this.respuestaTabla.AsigaErrorSistema("No existe información de la tabla " + this.nombreTabla + " filtrada.");

                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + this.nombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + this.nombreTabla + " filtrada.", "CrearSPInsert", "Utilerias", "Administrador");
            }
            return response;
        }

        private Response<String> CrearSPUpdate(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            Response<String> response = new Response<String>();
            this.accionCRUD = "Actualizar un registro de la ";
            CrearSPEncabezado(proyecto, usuarioProyecto, baseDatos);

            //this.respuestaTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);
            if (this.respuestaTabla.StatusType == StatusType.Ok)
            {
                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);

                InformacionTablaDTO objTabla = this.respuestaTabla.ListRecords.Find(tbl => tbl.EsPK == true);
                //this.objUtilerias.tipoSql = objTabla.TipoDato;

                this.cuerpoSP.Append("CREATE PROCEDURE [" + esquema.NombreEsquema + "].[Sp_pu_" + tabla1 + "]" + Environment.NewLine);

                this.respuestaTabla.ListRecords.RemoveAll(tbl => (tbl.NombreColumna == "UsuarioIns" || tbl.NombreColumna == "FechaIns"));
                int contador = 0;
                foreach (InformacionTablaDTO item in this.respuestaTabla.ListRecords)
                {
                    if (item.EsPK)
                    {
                        this.condicion.Append("                     " + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine);
                        if (item.TipoDato == "int")
                        {
                            this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + Environment.NewLine);
                        }
                        else
                        {
                            this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                        }
                        contador = 0;
                    }
                    else if (item.NombreColumna == "FechaUpd")
                    {
                        this.campos.Append(contador == 0 ? "        " + item.NombreColumna + " = GETDATE()" + Environment.NewLine : "                     ," + item.NombreColumna + " = GETDATE()" + Environment.NewLine);
                    }
                    else
                    {
                        if (item.TipoDato == "varchar")
                        {
                            this.cuerpoSP.Append("  ,@" + item.NombreColumna + "              " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                        }
                        else if (item.TipoDato == "nvarchar")
                        {
                            this.cuerpoSP.Append("  ,@" + item.NombreColumna + "              " + item.TipoDato.ToUpper() + "(MAX)" + Environment.NewLine);
                        }
                        else
                        {
                            this.cuerpoSP.Append(contador == 0 ? "  @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "" + Environment.NewLine);
                        }
                        this.campos.Append(contador == 0 ? "                      " + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine : "                     ," + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine);
                        contador++;
                    }
                }

                this.cuerpoSP.Append("AS" + Environment.NewLine);
                this.cuerpoSP.Append("BEGIN" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT ON;" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN TRY" + Environment.NewLine);
                this.cuerpoSP.Append("              UPDATE" + Environment.NewLine);
                this.cuerpoSP.Append("                    [" + esquema.NombreEsquema + "].[" + tabla.NombreTabla + "]" + Environment.NewLine);
                this.cuerpoSP.Append("              SET" + Environment.NewLine);
                this.cuerpoSP.Append(this.campos.ToString());
                this.cuerpoSP.Append("              WHERE" + Environment.NewLine);
                this.cuerpoSP.Append("" + this.condicion.ToString() + "" + Environment.NewLine);
                //this.cuerpoSP.Append("          END TRY" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("              EXEC bs_Admin.ErrorSel" + Environment.NewLine);
                //this.cuerpoSP.Append("          END CATCH" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT OFF;" + Environment.NewLine);
                this.cuerpoSP.Append("END" + Environment.NewLine);
                this.cuerpoSP.Append("GO" + Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);

                //string directorioRaiz = "C:\\SPS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\SPS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //this.rutaCreacionSP = directorioTabla;

                //if (File.Exists(this.rutaCreacionSP + @"\" + this.nombreTabla + "Upd.sql"))
                //{
                //    File.Delete(this.rutaCreacionSP + @"\" + this.nombreTabla + "Upd.sql");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionSP + @"\" + this.nombreTabla + "Upd.sql"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoSP.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();

                response.ResponseType = this.cuerpoSP.ToString();
                //this.cuerpoFinalSP += this.cuerpoSP.ToString();
                this.cuerpoSP.Clear();
                this.campos.Clear();
                this.condicion.Clear();
                //        this.respuestaTabla.StatusType = StatusType.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.respuestaTabla.StatusType = StatusType.Error;
                //        this.respuestaTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el procedimiento almacenado para la actualización de información en la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                //this.respuestaTabla.StatusType = StatusType.Error;
                //this.respuestaTabla.AsigaErrorSistema("No existe información de la tabla " + this.nombreTabla + " filtrada.");

                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + this.nombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + this.nombreTabla + " filtrada.", "CrearSPUpdate", "CrearSP", "Administrador");
            }
            return response;
        }

        private Response<String> CrearSPDelete(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            Response<String> response = new Response<String>();
            this.accionCRUD = "Eliminar (Eliminación Lógica) un registro de la ";
            CrearSPEncabezado(proyecto, usuarioProyecto, baseDatos);
            //this.respuestaTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);

            if (this.respuestaTabla.StatusType == StatusType.Ok)
            {
                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);

                InformacionTablaDTO objTabla = this.respuestaTabla.ListRecords.Find(tbl => tbl.EsPK == true);
                //this.objUtilerias.tipoSql = objTabla.TipoDato;

                this.cuerpoSP.Append("CREATE PROCEDURE [" + esquema.NombreEsquema + "].[Sp_pd_" + tabla1 + "]" + Environment.NewLine);

                this.respuestaTabla.ListRecords.RemoveAll(tbl => (tbl.NombreColumna == "UsuarioIns" || tbl.NombreColumna == "FechaIns"));
                int contador = 0;
                foreach (InformacionTablaDTO item in this.respuestaTabla.ListRecords)
                {
                    if (item.EsPK)
                    {
                        this.condicion.Append("                     " + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine);
                        if (item.TipoDato == "int")
                        {
                            this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + Environment.NewLine);
                        }
                        else
                        {
                            this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine : "  ,@" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                        }
                        contador = 0;
                    }
                    else if (item.TipoDato == "bit" && item.NombreColumna.Split('_')[1] == "Activo")
                    {
                        this.campos.Append(contador == 0 ? "                      " + item.NombreColumna + " = 0" + Environment.NewLine : "                     ," + item.NombreColumna + " = 0" + Environment.NewLine);
                        contador++;
                    }
                    else if (item.NombreColumna == "UsuarioUpd")
                    {
                        this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "            " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine : "  ,@" + item.NombreColumna + "            " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                        this.campos.Append(contador == 0 ? "                      " + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine : "                     ," + item.NombreColumna + " = @" + item.NombreColumna + "" + Environment.NewLine);
                        contador++;
                    }
                    else if (item.NombreColumna == "FechaUpd")
                    {
                        this.campos.Append(contador == 0 ? "        " + item.NombreColumna + " = GETDATE()" + Environment.NewLine : "                     ," + item.NombreColumna + " = GETDATE()" + Environment.NewLine);
                        contador++;
                    }
                }

                this.cuerpoSP.Append("AS" + Environment.NewLine);
                this.cuerpoSP.Append("BEGIN" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT ON;" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN TRY" + Environment.NewLine);
                this.cuerpoSP.Append("              UPDATE" + Environment.NewLine);
                this.cuerpoSP.Append("                    [" + esquema.NombreEsquema + "].[" + tabla.NombreTabla + "]" + Environment.NewLine);
                this.cuerpoSP.Append("              SET" + Environment.NewLine);
                this.cuerpoSP.Append(this.campos.ToString());
                this.cuerpoSP.Append("              WHERE" + Environment.NewLine);
                this.cuerpoSP.Append("" + this.condicion.ToString() + "" + Environment.NewLine);
                //this.cuerpoSP.Append("          END TRY" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("              EXEC bs_Admin.ErrorSel" + Environment.NewLine);
                //this.cuerpoSP.Append("          END CATCH" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT OFF;" + Environment.NewLine);
                this.cuerpoSP.Append("END" + Environment.NewLine);
                this.cuerpoSP.Append("GO" + Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);


                //string directorioRaiz = "C:\\SPS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\SPS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //this.rutaCreacionSP = directorioTabla;

                //if (File.Exists(this.rutaCreacionSP + @"\" + this.nombreTabla + "Del.sql"))
                //{
                //    File.Delete(this.rutaCreacionSP + @"\" + this.nombreTabla + "Del.sql");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionSP + @"\" + this.nombreTabla + "Del.sql"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoSP.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();

                response.ResponseType = this.cuerpoSP.ToString();
                //this.cuerpoFinalSP += this.cuerpoSP.ToString();
                this.cuerpoSP.Clear();
                this.campos.Clear();
                this.condicion.Clear();
                //        this.respuestaTabla.StatusType = StatusType.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.respuestaTabla.StatusType = StatusType.Error;
                //        this.respuestaTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el procedimiento almacenado para la eliminación de información en la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                //this.respuestaTabla.StatusType = StatusType.Error;
                //this.respuestaTabla.AsigaErrorSistema("No existe información de la tabla " + this.nombreTabla + " filtrada.");

                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + this.nombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + this.nombreTabla + " filtrada.", "CrearSPDelete", "CrearSP", "Administrador");
            }
            return response;
        }

        private Response<String> CrearSPFilterSelect(EsquemaDTO esquema, TablaDTO tabla, ProyectoDTO proyecto, UsuarioProyectoDTO usuarioProyecto, BaseDatosDTO baseDatos)
        {
            Response<String> response = new Response<String>();
            this.accionCRUD = "Seleccionar registros filtrados de la ";
            CrearSPEncabezado(proyecto, usuarioProyecto, baseDatos);
            //this.respuestaTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);

            if (this.respuestaTabla.StatusType == StatusType.Ok)
            {

                String tabla1 = tabla.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);

                this.cuerpoSP.Append("CREATE PROCEDURE [" + esquema.NombreEsquema + "].[Sp_pfs_" + tabla1 + "]" + Environment.NewLine);
                //this.cuerpoSP.Append("  @Filtro VARCHAR(MAX)" + Environment.NewLine);

                int contador = 0;
                foreach (InformacionTablaDTO item in this.respuestaTabla.ListRecords)
                {
                    if ((item.NombreColumna != "Activo") && (item.NombreColumna != "UsuarioIns") && (item.NombreColumna != "FechaIns") && (item.NombreColumna != "UsuarioUpd") && (item.NombreColumna != "FechaUpd"))
                    {
                        if (item.TipoDato == "varchar")
                        {
                            this.cuerpoSP.Append(contador == 0 ? "  @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine : "  ,@" + item.NombreColumna + "          " + item.TipoDato.ToUpper() + "(" + item.LongitudMaxima + ")" + Environment.NewLine);
                        }
                        else if (item.TipoDato == "nvarchar")
                        {
                            this.cuerpoSP.Append(contador == 0 ? "  @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + "(MAX)" + Environment.NewLine : "  ,@" + item.NombreColumna + "          " + item.TipoDato.ToUpper() + "(MAX)" + Environment.NewLine);
                        }
                        else
                        {
                            this.cuerpoSP.Append(contador == 0 ? "   @" + item.NombreColumna + "             " + item.TipoDato.ToUpper() + Environment.NewLine : "  ,@" + item.NombreColumna + "          " + item.TipoDato.ToUpper() + Environment.NewLine);
                        }

                        this.condicion.Append(contador == 0 ? "                     (" + item.NombreColumna + " = " + item.NombreColumna + " OR @" + item.NombreColumna + " IS NULL)" + Environment.NewLine : "                     AND (" + item.NombreColumna + " = " + item.NombreColumna + " OR @" + item.NombreColumna + " IS NULL)" + Environment.NewLine);
                    }
                    this.campos.Append(contador == 0 ? " " + item.NombreColumna + "" + Environment.NewLine : "                       ," + item.NombreColumna + "" + Environment.NewLine);
                    contador++;
                }
                this.cuerpoSP.Append("AS" + Environment.NewLine);
                this.cuerpoSP.Append("BEGIN" + Environment.NewLine);
                this.cuerpoSP.Append("      SET NOCOUNT ON;" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN TRY" + Environment.NewLine);
                this.cuerpoSP.Append("              SELECT" + Environment.NewLine);
                this.cuerpoSP.Append("                      " + this.campos);
                this.cuerpoSP.Append("              FROM" + Environment.NewLine);
                this.cuerpoSP.Append("                      [" + esquema.NombreEsquema + "].[" + tabla.NombreTabla + "] WITH (NOLOCK)" + Environment.NewLine);
                this.cuerpoSP.Append("              WHERE" + Environment.NewLine);
                this.cuerpoSP.Append("" + this.condicion);
                //this.cuerpoSP.Append("          END TRY" + Environment.NewLine);
                //this.cuerpoSP.Append("          BEGIN CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("              EXEC bs_Admin.ErrorSel" + Environment.NewLine);
                //this.cuerpoSP.Append("          END CATCH" + Environment.NewLine);
                //this.cuerpoSP.Append("      SET NOCOUNT OFF;" + Environment.NewLine);
                this.cuerpoSP.Append("END" + Environment.NewLine);
                this.cuerpoSP.Append("GO" + Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);
                this.cuerpoSP.Append(Environment.NewLine);

                //string directorioRaiz = "C:\\SPS\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\SPS\\" + this.nombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //this.rutaCreacionSP = directorioTabla;

                //if (File.Exists(this.rutaCreacionSP + @"\" + this.nombreTabla + "FilSel.sql"))
                //{
                //    File.Delete(this.rutaCreacionSP + @"\" + this.nombreTabla + "FilSel.sql");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionSP + @"\" + this.nombreTabla + "FilSel.sql"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoSP.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();

                response.ResponseType = this.cuerpoSP.ToString();
                //this.cuerpoFinalSP += this.cuerpoSP.ToString();
                this.cuerpoSP.Clear();
                this.campos.Clear();
                this.condicion.Clear();
                //        this.respuestaTabla.StatusType = StatusType.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.respuestaTabla.StatusType = StatusType.Ok;
                //        this.respuestaTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el procedimiento almacenado para la selección de información en la tabla " + this.nombreTabla + ".");
                //    }
                //}
            }
            else
            {
                //this.respuestaTabla.StatusType = StatusType.Error;
                //this.respuestaTabla.AsigaErrorSistema("No existe información de la tabla " + this.nombreTabla + " filtrada.");

                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + this.nombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + this.nombreTabla + " filtrada.", "CrearSPFilterSelect", "CrearSP", "Administrador");
            }
            return response;
        }
        #endregion
    }
}
