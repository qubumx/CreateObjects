using System;
using CrearObjetos.DTO.Utilerias;
using CrearObjetos.DTO;
using System.Data.SqlClient;

namespace CrearObjetos.DLL
{
    public class UtileriasDLL : Instance<UtileriasDLL>
    {
        string query = string.Empty;

        public Response<bool> ValidarCadenaConexion(string cadena, EnumGestorBaseDatos gestorBaseDatos)
        {
            Response<bool> respuestaValidarCadenaConexion = new Response<bool>();
            try
            {
                switch (gestorBaseDatos)
                {
                    case EnumGestorBaseDatos.MicrosoftSQLServer:
                        using (var context = DAL.DAL.ContextSQL(cadena))
                        {
                            respuestaValidarCadenaConexion.UserMessage = "La conexión al gestor de base de datos fue satisfactoria.";
                            respuestaValidarCadenaConexion.StatusType = StatusType.Ok;
                            respuestaValidarCadenaConexion.ResponseType = true;
                        }
                        break;
                    case EnumGestorBaseDatos.Oracle:
                        using (var context = DAL.DAL.ContextOracle(cadena))
                        {
                            respuestaValidarCadenaConexion.UserMessage = "La conexión al gestor de base de datos fue satisfactoria.";
                            respuestaValidarCadenaConexion.StatusType = StatusType.Ok;
                            respuestaValidarCadenaConexion.ResponseType = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                respuestaValidarCadenaConexion.ListError.Add(e.Message);
                respuestaValidarCadenaConexion.UserMessage = "No se pudo establecer la conexión al gestor de base de datos, verifique su información";
                respuestaValidarCadenaConexion.StatusType = StatusType.Error;
                respuestaValidarCadenaConexion.ResponseType = false;
            }

            return respuestaValidarCadenaConexion;
        }

        public Response<BaseDatosDTO> ObtenerBaseDatos(string cadena)
        {
            Response<BaseDatosDTO> ObjBaseDatos = new Response<BaseDatosDTO>();
           
            using (var context = DAL.DAL.ContextSQL(cadena))
            {
                try
                {
                    query = "SELECT name NombreBaseDatos FROM sys.databases ORDER BY name; ";

                    ObjBaseDatos.ListRecords = context.Sql(query).QueryMany<BaseDatosDTO>();
                    ObjBaseDatos.RecordsCount = ObjBaseDatos.ListRecords.Count;

                    if (ObjBaseDatos.RecordsCount > 0)
                    {
                        ObjBaseDatos.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjBaseDatos.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                    query = string.Empty;
                }
                catch (SqlException sqlex)
                {
                    ObjBaseDatos.StatusType = StatusType.Error;
                    ObjBaseDatos.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "ObtenerBaseDatos", "Utilerias", "Administrador");

                }
                catch (Exception ex)
                {
                    ObjBaseDatos.StatusType = StatusType.Error;
                    ObjBaseDatos.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerBaseDatos", "Utilerias", "Administrador");
                }
            }
            return ObjBaseDatos;
        }

        public Response<EsquemaDTO> ObtenerEsquemas(GestorBaseDatosDTO informacioGestorBaseDatos, string cadena)
        {
            Response<EsquemaDTO> ObjEsquema = new Response<EsquemaDTO>();
            using (var context = DAL.DAL.ContextSQL(cadena))
            {
                try
                {
                    query = "SELECT DISTINCT(TABLE_SCHEMA) AS NombreEsquema FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG='" + informacioGestorBaseDatos.NombreBaseDatos + "' ORDER BY TABLE_SCHEMA";

                    ObjEsquema.ListRecords = context.Sql(query).QueryMany<EsquemaDTO>();
                    ObjEsquema.RecordsCount = ObjEsquema.ListRecords.Count;

                    if (ObjEsquema.RecordsCount > 0)
                    {
                        ObjEsquema.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjEsquema.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                    query = string.Empty;
                }
                catch (SqlException sqlex)
                {
                    ObjEsquema.StatusType = StatusType.Error;
                    ObjEsquema.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "ObtenerEsquemas", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjEsquema.StatusType = StatusType.Error;
                    ObjEsquema.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerEsquemas", "Utilerias", "Administrador");
                }
            }
            return ObjEsquema;
        }

        public Response<TablaDTO> ObtenerTablas(GestorBaseDatosDTO informacioGestorBaseDatos, string cadena)
        {
            Response<TablaDTO> ObjTablas = new Response<TablaDTO>();
            using (var context = DAL.DAL.ContextSQL(cadena))
            {
                try
                {
                    query = "SELECT TABLE_SCHEMA Esquema, TABLE_NAME AS NombreTabla FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG='" + informacioGestorBaseDatos.NombreBaseDatos + "' AND TABLE_SCHEMA='" + informacioGestorBaseDatos.NombreEsquema + "' ORDER BY TABLE_NAME";
                    
                    ObjTablas.ListRecords = context.Sql(query).QueryMany<TablaDTO>();
                    ObjTablas.RecordsCount = ObjTablas.ListRecords.Count;

                    if (ObjTablas.RecordsCount > 0)
                    {
                        ObjTablas.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjTablas.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                }
                catch (SqlException sqlex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "ObtenerTablas", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerTablas", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        public Response<TablaDTO> ObtenerTablasOracle(GestorBaseDatosDTO informacioGestorBaseDatos, String cadenaConexion)
        {
            Response<TablaDTO> ObjTablas = new Response<TablaDTO>();
            using (var context = DAL.DAL.ContextOracle(cadenaConexion))
            {
                try
                {
                    //ObjTablas.ListRecords = context.Sql(@"SELECT OWNER AS Esquema, OBJECT_NAME AS NombreTabla FROM ALL_OBJECTS WHERE OWNER=@Esquema AND OBJECT_TYPE='TABLE'")
                    //                        .Parameter("Esquema", informacioGestorBaseDatos.NombreUsuario)                                            
                    //                        .QueryMany<TablaDTO>();
                    string query = "SELECT OWNER AS Esquema, OBJECT_NAME AS NombreTabla FROM ALL_OBJECTS WHERE OWNER='" + informacioGestorBaseDatos.NombreUsuario + "' OBJECT_TYPE='TABLE';";
                    ObjTablas.ListRecords = context.Sql(query)                                                                                    
                                              .QueryMany<TablaDTO>();


                    if (ObjTablas.RecordsCount > 0)
                    {
                        ObjTablas.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjTablas.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                }
                catch (SqlException sqlex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "ObtenerTablas", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerTablas", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        public Response<ProyectoDTO> ObtenerProyectos(ProyectoDTO proyecto)
        {
            Response<ProyectoDTO> ObjTablas = new Response<ProyectoDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[ProyectosFilSel]")
                                            .Parameter("NombreProyecto", proyecto.NombreProyecto)
                                            .QueryMany<ProyectoDTO>();
                    if (ObjTablas.RecordsCount > 0)
                    {
                        ObjTablas.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjTablas.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                }
                catch (SqlException sqlex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "ObtenerProyectos", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerProyectos", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        //public Response<UsuarioProyectoDTO> ObtenerUsuariosProyecto(ProyectoDTO proyecto)
        //{
        //    Response<UsuarioProyectoDTO> ObjTablas = new Response<UsuarioProyectoDTO>();
        //    using (var context = DAL.DAL.Context())
        //    {
        //        try
        //        {
        //            ObjTablas.ListRecords = context.StoredProcedure("[dbo].[UsuariosProyectoFilSel]")
        //                                    .Parameter("ProyectoId", proyecto.ProyectoId)
        //                                    .QueryMany<UsuarioProyectoDTO>();
        //            if (ObjTablas.RecordsCount > 0)
        //            {
        //                ObjTablas.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
        //            }
        //            else
        //            {
        //                ObjTablas.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
        //            }
        //        }
        //        catch (SqlException sqlex)
        //        {
        //            ObjTablas.StatusType = StatusType.Error;
        //            ObjTablas.UserMessage = "Error SQL, Al obtener las bases de datos.";
        //            Log.LogFile(sqlex.Message, "ObtenerUsuariosProyecto", "Utilerias", "Administrador");
        //        }
        //        catch (Exception ex)
        //        {
        //            ObjTablas.StatusType = StatusType.Error;
        //            ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
        //            Log.LogFile(ex.Message, "ObtenerUsuariosProyecto", "Utilerias", "Administrador");
        //        }
        //    }
        //    return ObjTablas;
        //}

        //public Response<InformacionTablaDTO> LeerCamposTabla(EsquemaDTO esquema, TablaDTO tabla)
        public Response<InformacionTablaDTO> LeerCamposTabla(ProyectoDTO proyecto)
        {
            Response<InformacionTablaDTO> ObjTablas = new Response<InformacionTablaDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[InformacionTablaFilSel]")
                                            .Parameter("Tabla", proyecto.NombreEsquema + "." + proyecto.NombreTabla)
                                            .QueryMany<InformacionTablaDTO>();
                    if (ObjTablas.RecordsCount > 0)
                    {
                        ObjTablas.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjTablas.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }
                }
                catch (SqlException sqlex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SQL, Al obtener las bases de datos.";
                    Log.LogFile(sqlex.Message, "LeerCamposTabla", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "LeerCamposTabla", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        public String DevuelveTipo(String tipoSql)
        {
            switch (tipoSql)
            {
                case "bigint": return "Int64";
                case "binary": return "Byte[]";
                case "bit": return "Boolean";
                case "char": return "String";
                //case "cursor": return "Tipo de dato no soportado";
                case "date": return "DateTime";
                case "datetime": return "DateTime";
                case "smalldatetime": return "DateTime";
                case "decimal": return "Decimal";
                case "float": return "Double";
                //case "image": return "Tipo de dato no soportado";
                case "int": return "Int32";
                case "money": return "Decimal";
                case "nchar": return "String";
                //case "ntext": return "Tipo de dato no soportado";
                case "numeric": return "Decimal";
                case "real": return "Single";
                case "rowversion": return "Byte[]";
                case "smallint": return "Int16";
                case "smallmoney": return "Decimal";
                case "sql_variant": return "Object";
                case "time": return "TimeSpan";
                case "tinyint": return "Byte";
                case "uniqueidentifier": return "Guid";
                case "varbinary": return "Byte[]";
                case "varchar": return "String";
                case "nvarchar": return "String";
                default:
                    break;
            }
            return "Tipo de dato no soportado";
        }
    }
}