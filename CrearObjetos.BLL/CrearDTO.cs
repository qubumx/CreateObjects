using CrearObjetos.DTO;
using CrearObjetos.DTO.Utilerias;
using System;
using System.Text;

namespace CrearObjetos.BLL
{
    public class CrearDTO:Instance<CrearDTO>
    {

        
        private StringBuilder cuerpoDTO { get; set; }
        public String cuerpoFinalDTO { get; set; }
        public String rutaCreacionDTO { get; set; }


        public CrearDTO()
        {
            this.cuerpoDTO = new StringBuilder();
            this.rutaCreacionDTO = String.Empty;
            this.cuerpoFinalDTO = String.Empty;
        }

        //public Response<InformacionTablaDTO> ConstruccionDTO(EsquemaDTO esquema, TablaDTO tabla)
        //public Response<String> ConstruccionDTO(EsquemaDTO esquema, TablaDTO tabla)
        public Response<String> ConstruccionDTO(ProyectoDTO proyecto)
        {
            Response<InformacionTablaDTO> responseTabla = new Response<InformacionTablaDTO>();
            //responseTabla = UtileriasBLL.Instances.LeerCamposTabla(esquema, tabla);
            responseTabla = UtileriasBLL.Instances.LeerCamposTabla(proyecto);

            Response<String> response = new Response<String>();

            if (responseTabla.StatusType == StatusType.Ok)
            {
                this.cuerpoDTO.Append("using System;" + Environment.NewLine);
                //this.cuerpoDTO.Append("using BS.DTO.Genericas;" + Environment.NewLine);
                this.cuerpoDTO.Append("" + Environment.NewLine);
                //this.cuerpoDTO.Append("namespace " + this.nombreBaseDatos + ".DTO." + tabla.NombreTabla + "" + Environment.NewLine);
                String tabla1 = proyecto.NombreTabla;
                tabla1 = tabla1.Remove(0, 3);
                this.cuerpoDTO.Append("namespace Ingram_Obj." + tabla1 + "" + Environment.NewLine);
                this.cuerpoDTO.Append("{" + Environment.NewLine);
                //this.cuerpoDTO.Append("     public class " + tabla.NombreTabla + " : Auditoria" + Environment.NewLine);
                this.cuerpoDTO.Append("     public class " + tabla1 + "Obj" + Environment.NewLine);
                this.cuerpoDTO.Append("         {" + Environment.NewLine);

                //this.responseTabla.ListRecords.RemoveAll(tbl => (tbl.NombreColumna == "UsuarioIns") || (tbl.NombreColumna == "FechaIns") || (tbl.NombreColumna == "UsuarioUpd") || (tbl.NombreColumna == "FechaUpd"));


                foreach (InformacionTablaDTO item in responseTabla.ListRecords)
                {
                    //objUtilerias.tipoSql = item.TipoDato;

                    if (item.EsPK)
                    {
                        //if (item.CampoNulo)
                        //{
                        this.cuerpoDTO.Append("             public " + UtileriasBLL.Instances.DevuelveTipo(item.TipoDato) + "? " + item.NombreColumna + " { get; set; } // Es PK" + Environment.NewLine);
                        //}
                        //else
                        //{
                        //    this.cuerpoDTO.Append("             public " + UtileriasBLL.Instances.DevuelveTipo() + " " + item.NombreColumna + "  { get; set; } // Es PK" + Environment.NewLine);
                        //}
                    }
                    else
                    {
                        if (item.CampoNulo)
                        {
                            this.cuerpoDTO.Append("             public " + UtileriasBLL.Instances.DevuelveTipo(item.TipoDato) + "? " + item.NombreColumna + " { get; set; }" + Environment.NewLine);
                        }
                        else
                        {
                            this.cuerpoDTO.Append("             public " + UtileriasBLL.Instances.DevuelveTipo(item.TipoDato) + " " + item.NombreColumna + "  { get; set; }" + Environment.NewLine);
                        }
                    }
                }
                this.cuerpoDTO.Append("         }" + Environment.NewLine);
                this.cuerpoDTO.Append("}" + Environment.NewLine);

                //string directorioRaiz = "C:\\DTO\\";
                //if (!Directory.Exists(directorioRaiz))
                //{
                //    Directory.CreateDirectory(directorioRaiz);
                //}

                //string directorioTabla = "C:\\DTO\\" + tabla.NombreTabla;
                //if (!Directory.Exists(directorioTabla))
                //{
                //    Directory.CreateDirectory(directorioTabla);
                //}

                //this.rutaCreacionDTO = directorioTabla;

                //if (File.Exists(this.rutaCreacionDTO + @"\" + tabla.NombreTabla + ".cs"))
                //{
                //    File.Delete(this.rutaCreacionDTO + @"\" + tabla.NombreTabla + ".cs");
                //}

                //using (FileStream fileStream = File.Create(this.rutaCreacionDTO + @"\" + tabla.NombreTabla + ".cs"))
                //{
                //    byte[] texto = new UTF8Encoding(true).GetBytes(this.cuerpoDTO.ToString());
                //    try
                //    {
                //        fileStream.Write(texto, 0, texto.Length);
                //        fileStream.Flush();
                this.cuerpoFinalDTO = this.cuerpoDTO.ToString();
                this.cuerpoDTO.Clear();
                //        //this.responseTabla.EstatusBS = EstatusBS.Ok;
                //    }
                //    catch (Exception errorCrearAchivo)
                //    {
                //        this.responseTabla.EstatusBS = EstatusBS.Error;
                //        this.responseTabla.AsigaErrorSistema(errorCrearAchivo, "Error al crear el DTO con la información de la tabla " + tabla.NombreTabla + ".");
                //    }
                //}
            }
            else
            {
                response.StatusType = StatusType.Error;
                response.UserMessage = "No existe información de la tabla " + proyecto.NombreTabla + " filtrada.";
                Log.LogFile("No existe información de la tabla " + proyecto.NombreTabla + " filtrada.", "LeerCamposTabla", "Utilerias", "Administrador");
            }
            //return responseTabla;
            
            response.ResponseType = this.cuerpoFinalDTO;
            return response;
        }
    }
}
