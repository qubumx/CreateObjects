using System;
using CrearObjetos.DTO.Utilerias;
using CrearObjetos.DTO;
using System.Data.SqlClient;

namespace CrearObjetos.DLL
{
    public class UtileriasDLL : Instance<UtileriasDLL>
    { 

        public Response<Boolean> ValidarCadenaConexion(String cadena, EnumGestorBaseDatos gestorBaseDatos)
        {
            Response<Boolean> respuestaValidarCadenaConexion = new Response<Boolean>();
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

        public Response<BaseDatosDTO> ObtenerBaseDatos()
        {
            Response<BaseDatosDTO> ObjBaseDatos = new Response<BaseDatosDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjBaseDatos.ListRecords = context.StoredProcedure("[dbo].[BasesDatosSel]").QueryMany<BaseDatosDTO>();
                    ObjBaseDatos.RecordsCount = ObjBaseDatos.ListRecords.Count;
                    if (ObjBaseDatos.RecordsCount > 0)
                    {
                        ObjBaseDatos.UserMessage = "Se obtuvo correctamente la información correspondiente a la base de datos.";
                    }
                    else
                    {
                        ObjBaseDatos.UserMessage = "No se cuenta con información correspondiente a la base de datos.";
                    }

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

        public Response<EsquemaDTO> ObtenerEsquemas(BaseDatosDTO baseDatos)
        {
            Response<EsquemaDTO> ObjTablas = new Response<EsquemaDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[EsquemasFilSel]")
                                            .Parameter("BaseDatos", baseDatos.NombreBaseDatos)
                                            .QueryMany<EsquemaDTO>();
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
                    Log.LogFile(sqlex.Message, "ObtenerEsquemas", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerEsquemas", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        public Response<TablaDTO> ObtenerTablas(BaseDatosDTO baseDatos, EsquemaDTO esquemaTabla)
        {
            Response<TablaDTO> ObjTablas = new Response<TablaDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[TablasFilSel]")
                                            .Parameter("BaseDatos", baseDatos.NombreBaseDatos)
                                            .Parameter("EsquemaTabla", esquemaTabla.NombreEsquema)
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

        public Response<UsuarioProyectoDTO> ObtenerUsuariosProyecto(ProyectoDTO proyecto)
        {
            Response<UsuarioProyectoDTO> ObjTablas = new Response<UsuarioProyectoDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[UsuariosProyectoFilSel]")
                                            .Parameter("ProyectoId", proyecto.ProyectoId)
                                            .QueryMany<UsuarioProyectoDTO>();
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
                    Log.LogFile(sqlex.Message, "ObtenerUsuariosProyecto", "Utilerias", "Administrador");
                }
                catch (Exception ex)
                {
                    ObjTablas.StatusType = StatusType.Error;
                    ObjTablas.UserMessage = "Error SYS, Al obtener las bases de datos.";
                    Log.LogFile(ex.Message, "ObtenerUsuariosProyecto", "Utilerias", "Administrador");
                }
            }
            return ObjTablas;
        }

        public Response<InformacionTablaDTO> LeerCamposTabla(EsquemaDTO esquema, TablaDTO tabla)
        {
            Response<InformacionTablaDTO> ObjTablas = new Response<InformacionTablaDTO>();
            using (var context = DAL.DAL.Context())
            {
                try
                {
                    ObjTablas.ListRecords = context.StoredProcedure("[dbo].[InformacionTablaFilSel]")
                                            .Parameter("Tabla", esquema.NombreEsquema + "." + tabla.NombreTabla)
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