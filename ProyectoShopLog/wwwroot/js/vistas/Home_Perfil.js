$(document).ready(function () {

    $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/ObtenerUsuario")
        .then(response => {
            $(".container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.Reject(response);
        })
        .then(responseJson => {
            console.log(responseJson)

            if (responseJson.estado) {
                const d = responseJson.objeto

                $("#txtCorreo").val(d.correo)
                $("#txtRol").val(d.nombreRol)


            } else {
                swal("Lo sentimos", responseJson.mensaje,"error")
            }
        })

})

$("#btnGuardarCambios").click(function () {

    if ($("#txtCorreo").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Correo")
        $("#txtCorreo").focus()
        return;
    }


    swal({
        title: "¿Desea guardar los cambios?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {

            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");

                let modelo = {
                    correo: $("#txtCorreo").val().trim()
                }

                fetch("/Home/GuardarPerfil", {
                    method: "POST",
                    headers: { "Content-Type": "application/json; charset = utf-8" },
                    body: JSON.stringify(modelo)
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            
                            swal("Listo!", "Los cambios fueron guardados", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }
        })
})

$("#btnGuardarCambiosContra").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    

    if (inputs_sin_valor.length > 0) {
        const mensaje = 'Debe completar el campo: "${inputs_sin_valor[0].name}"';
        toastr.warning("", mensaje)
        $('input[name="${inputs_sin_valor[0].name}"]').focus()
        return;
    }

    if ($("#passNuevaContra").val().trim() != $("#passNuevaContraAga").val().trim()) {
        toastr.warning("", "Las contraseñas no coinciden")
        return;
    }

    let modelo = {
        claveActual: $("#txtContraAnt").val().trim(),
        claveNueva: $("#passNuevaContra").val().trim()
    }

    


    swal({
        title: "Se cambiará la contraseña ¿Desea guardar los cambios?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {

            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");

                fetch("/Usuario/CambiarClave", {
                    method: "POST",
                    headers: { "Content-Type": "application/json; charset = utf-8" },
                    body: JSON.stringify(modelo)
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {


                            swal("Listo!", "La contraseña fue cambiada", "success")
                            $("input.input-validar").val("");
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }
        })

})