var ConfirmCustom = function (mensaje, callbackOk, callbackCancel, TxtBtnOk, TxtBtnFail) {
    if (TxtBtnOk == "" || TxtBtnOk == undefined)
        TxtBtnOk = "Aceptar";
    if (TxtBtnFail == "" || TxtBtnFail == undefined)
        TxtBtnFail = "Cancelar";
    bootbox.dialog({
        message: mensaje,
        closeButton: false,
        buttons:
        {
            "success":
            {
                "label": TxtBtnOk,
                "className": "btn-sm btn-success",
                "callback": function () {
                    if (callbackOk != "") {
                        var call = $.Callbacks();
                        call.add(callbackOk);
                        call.fire();
                    }
                }
            },
            "danger":
            {
                "label": TxtBtnFail,
                "className": "btn-sm btn-danger",
                "callback": function () {
                    if (callbackCancel != "") {
                        var call = $.Callbacks();
                        call.add(callbackCancel);
                        call.fire();
                    }
                }
            }
        }
    });
}
var ExitoCustom = function (mensaje, callback, bootbox) {
    if (mensaje == "" || mensaje == undefined)
        mensaje = '<div class="bootbox-msg">El cambio se realizó correctamente.</div>';
    else
        mensaje = "<div class='bootbox-msg'>" + mensaje + "</div>";
    if (bootbox == "" || bootbox == undefined)
        bootbox = false;
    if (bootbox) {
        $.notify({
            message: mensaje
        },
        {
            onShow: callback
        });
    }
    else {
        showDialog(mensaje, callback);
    }
}
var ErrorCustom = function (mensaje, callback, bootbox) {
    if (mensaje == "" || mensaje == undefined)
        mensaje = '<div class="bootbox-msg">Ocurrió un error al procesar la petición, intente de nuevo más tarde.</div>';
    else
        mensaje = "<div class='bootbox-msg'>" + mensaje + "</div>";
    if (bootbox == "" || bootbox == undefined)
        bootbox = false;
    if (bootbox) {
        $.notify({
            message: mensaje
        }
        , {
            // settings
            type: 'danger',
            onShow: callback
        });
    } else {
        showErrorDialog(mensaje, callback);
    }
}
var showDialog = function (mensaje, callback) {
    bootbox.dialog({
        message: mensaje,
        closeButton: false,
        buttons:
        {
            "success":
            {
                "label": "<i class='icon-remove'></i> Cerrar",
                "className": "btn-success",
                "callback": function () {
                    if (callback != "") {
                        var call = $.Callbacks();
                        call.add(callback);
                        call.fire();
                    }
                }
            }
        }
    });
}
var showErrorDialog = function (mensaje, callback) {
    bootbox.dialog({
        message: mensaje,
        closeButton: false,
        buttons:
        {
            "danger":
            {
                "label": "<i class='icon-remove'></i> Cerrar",
                "className": "btn-danger",
                "callback": function () {
                    if (callback != "") {
                        var call = $.Callbacks();
                        call.add(callback);
                        call.fire();
                    }
                }
            }
        }
    });
}