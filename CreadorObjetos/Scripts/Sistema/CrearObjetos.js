var GestionarCrearObjetos = {
    Inicial: function () {

        GestionarCrearObjetos.InicializarControles();
        //GestionarCrearObjetos.CargarBaseDatos();
        //GestionarCrearObjetos.CargarProyectos();
        ////////////GestionarCrearObjetos.ConfiguracionCodeMirror();

        $("#ddlBaseDatos").change(function () {
            GestionarCrearObjetos.CargarEsquemas($(this).val());
        });

        $("#ddlEsquema").change(function () {
            GestionarCrearObjetos.CargarTablas($('#ddlBaseDatos').val(), $(this).val());
        });

        $("#btnGenerObjetos").click(function () {
            GestionarCrearObjetos.GenerarObjetos();
        });

        $("#ddlProyecto").change(function () {
            GestionarCrearObjetos.CargarUsuariosProyecto($(this).val());
        });

        $("#chkSQL").click(function () {
            GestionarCrearObjetos.GestorBDSQL();
        });

        $("#chkOracle").click(function () {
            GestionarCrearObjetos.GestorBDOracle();
        });

        $("#btnConectarBD").click(function () {
            GestionarCrearObjetos.ConectarBD();
        });

    },

    GestorBDSQL: function () {
        $('#divInfoBD').slideUp();
        $('#divNombreServicio').slideUp();
        $('#divPuerto').slideUp();

        $('#txtNombreServicio').val('');
        $('#txtPuerto').val('');
    },

    GestorBDOracle: function () {
        $('#divInfoBD').slideUp();
        $('#divNombreServicio').slideDown();
        $('#divPuerto').slideDown();

        $('#txtNombreServicio').val('');
        $('#txtPuerto').val('');
    },

    ConectarBD: function () {
        GestorBaseDatos = {
            Servidor: $("#txtNombreServidor").val(),
            NombreUsuario: $("#txtNombreUsuario").val(),
            Contrasenia: $("#txtContrasenia").val(),
            GestorBaseDatos: $("#chkSQL").is(":checked") ? 1 : 2,
            NombreServicio: $('#txtNombreServicio').val(),
            Puerto: $('#txtPuerto').val(),
        }

        $.ajax({
            type: 'GET',
            url: 'Home/ValidarConexion',
            contentType: "application/json;",
            dataType: "json",
            data: GestorBaseDatos,
            success: function (response) {
                if (response.ResponseType === true) {

                    alert(response.UserMessage);

                    $('#divInfoProyecto').show();
                    $('#divUtilerias').show();
                    
                    switch (GestorBaseDatos.GestorBaseDatos) {
                        case 1:
                            GestionarCrearObjetos.CargarBaseDatos();
                            break;
                        case 2:
                            GestionarCrearObjetos.CargarTablasOracle();
                            break;
                    }
                }
                else {
                    alert(response.UserMessage);
                }
            },
            complete: function () {

            },            
            error: function (xhr, textStatus, err) {
                console.log("readyState: " + xhr.readyState);
                console.log("responseText: " + xhr.responseText);
                console.log("status: " + xhr.status);
                console.log("text status: " + textStatus);
                console.log("error: " + err);
            }
        });
    },

    InicializarControles: function () {
        $('#PintarObjetos').slideUp();
        $("#divEsquema").slideUp();
        $("#divTabla").slideUp();
        //$("#divUsuariosProyecto").slideUp();

        $("#ddlBaseDatos").val('');
        $("#ddlEsquema").val('');
        $("#ddlTabla").val('');
        $("#ddlProyecto").val('');
        $("#ddlUsuariosProyecto").val('');
    },

    CargarBaseDatos: function () {
        $.ajax({
            type: 'GET',           
            url: 'Home/ObtenerBaseDatos',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,            
            data: GestorBaseDatos,
            success: function (response) {
                if (response.ListError === null) {
                    $('#divInfoBD').show();
                    $("#ddlBaseDatos").html('');
                    $("#ddlBaseDatos").append('<option value="">Seleccionar una Base de Datos</option>');
                    $.each(response.ListRecords, function (indice, basedatos) {
                        $("#ddlBaseDatos").append('<option value=' + basedatos.NombreBaseDatos + '>' + basedatos.NombreBaseDatos + '</option>');
                    });
                }
                else {
                    //GestionarCrearObjetos.ModalMensajesInfo("Error Base de Datos", "Error al obtener información de las bases de datos.");

                    alert(response.UserMessage);
                }
            },
            complete: function () {
                debugger;
            },
            error: function (xhr, textStatus, err) {
                console.log("readyState: " + xhr.readyState);
                console.log("responseText: " + xhr.responseText);
                console.log("status: " + xhr.status);
                console.log("text status: " + textStatus);
                console.log("error: " + err);
            }
        });
    },

    CargarEsquemas: function (basedatos) {
        if (basedatos != "") {

            GestorBaseDatos = {
                Servidor: $("#txtNombreServidor").val(),
                NombreUsuario: $("#txtNombreUsuario").val(),
                Contrasenia: $("#txtContrasenia").val(),
                GestorBaseDatos: $("#chkSQL").is(":checked") ? 1 : 2,
                NombreServicio: $('#txtNombreServicio').val(),
                Puerto: $('#txtPuerto').val(),
                NombreBaseDatos: basedatos
            };
            
            $.ajax({
                type: 'GET',
                url: 'Home/ObtenerEsquemas',
                contentType: "application/json; charset=utf-8",
                dataType: "json",               
                data:GestorBaseDatos,
                success: function (response) {
                    if (response.ListRecords != null) {
                        if (response.ListRecords.length > 0) {
                            $("#divEsquema").slideDown();
                            $("#ddlEsquema").html('');
                            $("#ddlEsquema").append('<option value="">Seleccionar un Esquema</option>');
                            $.each(response.ListRecords, function (indice, esquema) {
                                $("#ddlEsquema").append('<option value=' + esquema.NombreEsquema + '>' + esquema.NombreEsquema + '</option>');
                            });
                        }
                        else {
                            //GestionarCrearObjetos.ModalMensajesInfo("Error Esquemas", "No existen esquemas en la base de datos " + basedatos + ".");

                            alert("No existen esquemas en la base de datos " + basedatos + ".");
                        }
                    }
                    else {
                        alert("No existen esquemas en la base de datos " + basedatos + ".");
                    }
                },
                //Mensaje de error en caso de fallo
                error: function (xhr, ajaxOptions, thrownError) {
                    debugger;
                    alert("Error " + xhr.status + " " + thrownError);
                }
            });
        } else {
            $("#divEsquema").slideUp();
            $("#divTabla").slideUp();
        }
    },

    CargarTablas: function (basedatos, esquema) {
        GestorBaseDatos = {
            Servidor: $("#txtNombreServidor").val(),
            NombreUsuario: $("#txtNombreUsuario").val(),
            Contrasenia: $("#txtContrasenia").val(),
            GestorBaseDatos: $("#chkSQL").is(":checked") ? 1 : 2,
            NombreServicio: $('#txtNombreServicio').val(),
            Puerto: $('#txtPuerto').val(),
            NombreBaseDatos: basedatos,
            NombreEsquema: esquema
        };

        if (esquema != "") {
            $.ajax({
                type: 'GET',                
                url: 'Home/ObtenerTablas',
                contentType: "application/json; charset=utf-8",
                dataType: "json",                
                data: GestorBaseDatos,
                success: function (response) {
                    if (response.ListRecords != null) {
                        if (response.ListRecords.length > 0) {
                            $("#divTabla").slideDown();
                            $("#ddlTabla").html('');
                            $("#ddlTabla").append('<option value="">Seleccionar una Tabla</option>');
                            $.each(response.ListRecords, function (indice, tabla) {
                                $("#ddlTabla").append('<option value=' + tabla.NombreTabla + '>' + tabla.NombreTabla + '</option>');
                            });
                        }
                        else {
                            //GestionarCrearObjetos.ModalMensajesInfo("Error Tabla", "Error al obtener información de la Tabla.");
                            alert("Error al obtener información del esquema " + esquema + ".");
                        }
                    }
                    else {
                        alert("Error al obtener información del esquema " + esquema + ".");
                    }
                },
                //Mensaje de error en caso de fallo
                error: function (xhr, ajaxOptions, thrownError) {
                    debugger;
                    alert("Error " + xhr.status + " " + thrownError);
                }
            });
        } else {
            $("#divTabla").slideUp();
        }
    },

    CargarTablasOracle: function () {

        $.ajax({
            type: 'GET',
            //Llamado al metodo en el controlador
            url: 'Home/ObtenerTablasOracle',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //Parametros que se envian al metodo del controlador
            data: GestorBaseDatos,
            //En caso de resultado exitoso
            success: function (response) {
                if (response.ListRecords != null) {
                    if (response.ListRecords.length > 0) {

                        $('#divInfoBD').slideDown();
                        $('#divBaseDatos').slideUp();
                        $('#divEsquema').slideUp();
                        $('#divTabla').slideDown();

                        $("#ddlTabla").html('');
                        $("#ddlTabla").append('<option value="">Seleccionar una Tabla</option>');
                        $.each(response.ListRecords, function (indice, tabla) {
                            $("#ddlTabla").append('<option value=' + tabla.NombreTabla + '>' + tabla.NombreTabla + '</option>');
                        });
                    }
                    //else {
                    //    //GestionarCrearObjetos.ModalMensajesInfo("Error Tabla", "Error al obtener información de la Tabla.");
                    //    alert("Error al obtener información del esquema " + esquema + ".");
                    //}
                }
                //else {
                //    alert("Error al obtener información del esquema " + esquema + ".");
                //}
            },
            //Mensaje de error en caso de fallo
            error: function (xhr, ajaxOptions, thrownError) {
                debugger;
                alert("Error " + xhr.status + " " + thrownError);
            }
        });
    },

    //CargarProyectos: function () {
    //    $.ajax({
    //        type: 'GET',
    //        //Llamado al metodo en el controlador
    //        url: 'Home/ObtenerProyectos',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        async: true,
    //        //En caso de resultado exitoso
    //        success: function (response) {
    //            if (response.ListError === null) {
    //                $("#divUsuariosProyecto").slideDown();
    //                $("#ddlProyecto").html('');
    //                $("#ddlProyecto").append('<option value="">Seleccionar un Proyecto</option>');
    //                $.each(response.ListRecords, function (indice, proyecto) {
    //                    $("#ddlProyecto").append('<option value=' + proyecto.ProyectoId + '>' + proyecto.NombreProyecto + '</option>');
    //                });
    //            }
    //            else {
    //                //GestionarCrearObjetos.ModalMensajesInfo("Error Base de Datos", "Error al obtener información de las bases de datos.");
    //                alert("Error al obtener información del proyecto.");
    //            }
    //        },
    //        complete: function () {
    //            debugger;
    //        },
    //        //Mensaje de error en caso de fallo
    //        //error: function (xhr, ajaxOptions, thrownError) {
    //        //    debugger;
    //        //    alert("Error " + xhr.status + " " + thrownError + " " + xhr.responseText);
    //        error: function (xhr, textStatus, err) {
    //            console.log("readyState: " + xhr.readyState);
    //            console.log("responseText: " + xhr.responseText);
    //            console.log("status: " + xhr.status);
    //            console.log("text status: " + textStatus);
    //            console.log("error: " + err);

    //        }
    //    });
    //},

    //CargarUsuariosProyecto: function (proyectoId) {
    //    if (proyectoId != "") {
    //        $.ajax({
    //            type: 'GET',
    //            //Llamado al metodo en el controlador
    //            url: 'Home/ObtenerUsuariosProyecto',
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            //Parametros que se envian al metodo del controlador
    //            data: { proyectoId: proyectoId },
    //            //En caso de resultado exitoso
    //            success: function (response) {
    //                if (response.ListRecords != null) {
    //                    if (response.ListRecords.length > 0) {
    //                        $("#divUsuariosProyecto").slideDown();
    //                        $("#ddlUsuariosProyecto").html('');
    //                        $("#ddlUsuariosProyecto").append('<option value="">Seleccionar un Usuario del Proyecto</option>');
    //                        $.each(response.ListRecords, function (indice, tabla) {
    //                            $("#ddlUsuariosProyecto").append('<option value=' + tabla.UsuarioProyectoId + '>' + tabla.NombreColaborador + '</option>');
    //                        });
    //                    }
    //                    else {
    //                        //GestionarCrearObjetos.ModalMensajesInfo("Error Tabla", "Error al obtener información de la Tabla.");
    //                        alert("Error al obtener información del proyecto");
    //                    }
    //                }
    //                else {
    //                    alert("Error al obtener información del proyecto");
    //                }
    //            },
    //            //Mensaje de error en caso de fallo
    //            error: function (xhr, ajaxOptions, thrownError) {
    //                debugger;
    //                alert("Error " + xhr.status + " " + thrownError);
    //            }
    //        });
    //    }
    //    else {
    //        $("#divUsuariosProyecto").slideDown();
    //    }
    //},

    GenerarObjetos: function () {
        //var objetos = $('.form-control');

        //for (var i = 0; i < objetos.length; i++) {
        //    if (objetos[i].value === '') {
        //        objetos[i].focus();
        //        return;
        //    }
        //}

        GestorBaseDatos = {
            Servidor: $("#txtNombreServidor").val(),
            NombreUsuario: $("#txtNombreUsuario").val(),
            Contrasenia: $("#txtContrasenia").val(),
            GestorBaseDatos: $("#chkSQL").is(":checked") ? 1 : 2,
            NombreServicio: $('#txtNombreServicio').val(),
            Puerto: $('#txtPuerto').val(),
            NombreBaseDatos: $("#ddlBaseDatos").val(),
            NombreEsquema: $("#ddlEsquema").val(),
            NombreTabla: $("#ddlTabla").val(),
            NombreProyecto: $("#txtProyecto").val(),
            NombreColaborador: $("#txtColaborador").val(),
        };

        $.ajax({
            type: 'GET',
            url: 'Home/GenerarObjetos',
            contentType: "application/json; charset=utf-8",
            dataType: "json",            
            data:GestorBaseDatos,
            success: function (response) {
                var errores = '';
                if (response.responseCrearSP.StatusType === 1) {
                    $.each(response.responseCrearSP.ListError, function (indie, errorSP) {
                        errores += errorSP + "\n";
                    });
                }

                if (response.responseCrearDTO.StatusType === 1) {
                    $.each(response.responseCrearDTO.ListError, function (indie, errorDTO) {
                        errores += errorDTO + "\n";
                    });
                }
                debugger;

                if (response.responseCrearDLL.StatusType === 1) {
                    $.each(response.responseCrearDLL.ListError, function (indie, errorDLL) {
                        errores += errorDLL + "\n";
                    });
                }

                //if ($('#chkWS').is(':checked')) {
                //    if (response.ObjCrearWSDTO === 1) {
                //        $.each(response.ObjCrearWSDTO.AsigaErrorSistema, function (indie, errorWSDTO) {
                //            errores += errorWSDTO + "\n";
                //        });
                //    }
                //}


                if (errores === '') {
                    alert("Se crearon satisfactoriamente los objetos.");
                    $('#PintarObjetos').slideDown();
                    $('#codeDTO').html(response.responseCrearDTO.ResponseType);
                    $('#codeBussines').html(response.responseCrearDLL.ResponseType);
                    $('#codeSP').html(response.responseCrearSP.ResponseType);

                    if ($('#chkWS').is(':checked')) {
                        //$('#pnlWSDTO').css('display', 'block');
                        //$('#pnlWSDLL').css('display', 'block');
                        //$('#pnlWSBLL').css('display', 'block');

                        //$('#codeWSDTO').html(response.ObjCrearWSDTO.cuerpoFinalWSDTO);
                        //$('#codeWSDLL').html(response.ObjCrearWSDLL.cuerpoFinalWSDLL);
                        //$('#codeWSBLL').html(response.ObjCrearWSBLL.cuerpoFinalWSBLL);
                    }
                    else {
                        //$('#pnlWSDTO').css('display', 'none');
                        //$('#pnlWSDLL').css('display', 'none');
                        //$('#pnlWSBLL').css('display', 'none');
                    }
                }
                else {
                    $('#PintarObjetos').slideUp();
                    alert("Ocurrieron los siguientes errores." + errores);
                }
            },
            //Mensaje de error en caso de fallo
            error: function (xhr, ajaxOptions, thrownError) {
                debugger;
                alert("Error " + xhr.status + " " + thrownError);
            }
        });

    },

    //ConfiguracionCodeMirror: function () {
    //    var editorDTO = CodeMirror.fromTextArea(document.getElementById("codeDTO"), {
    //        lineNumbers: true,
    //        //theme: "night",
    //        textWrapping: true,
    //        mode: "xml",
    //        htmlMode: true,
    //        extraKeys: {
    //            "F11": function (cm) {
    //                cm.setOption("fullScreen", !cm.getOption("fullScreen"));
    //            },
    //            "Esc": function (cm) {
    //                if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
    //            }
    //        }
    //    });

    //    var editorBussines = CodeMirror.fromTextArea(document.getElementById("codeBussines"), {
    //        lineNumbers: true,
    //        //theme: "night",
    //        textWrapping: true,
    //        mode: "xml",
    //        htmlMode: true,
    //        extraKeys: {
    //            "F11": function (cm) {
    //                cm.setOption("fullScreen", !cm.getOption("fullScreen"));
    //            },
    //            "Esc": function (cm) {
    //                if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
    //            }
    //        }
    //    });

    //    var editorSP = CodeMirror.fromTextArea(document.getElementById("codeSP"), {
    //        lineNumbers: true,
    //        //theme: "night",
    //        textWrapping: true,
    //        mode: "xml",
    //        htmlMode: true,
    //        extraKeys: {
    //            "F11": function (cm) {
    //                cm.setOption("fullScreen", !cm.getOption("fullScreen"));
    //            },
    //            "Esc": function (cm) {
    //                if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
    //            }
    //        }
    //    });
    //}


    //ModalMensajesInfo: function (titulo, textoMensaje) {
    //    $(".TituloModalInfo").html(titulo);
    //    $("#bodyMensajes").html('');
    //    $("#bodyMensajes").html(textoMensaje);
    //    $("#MensajesInfo").modal(
    //     {
    //         backdrop: 'static',
    //         keyboard: false
    //     });
    //},
}

let GestorBaseDatos;