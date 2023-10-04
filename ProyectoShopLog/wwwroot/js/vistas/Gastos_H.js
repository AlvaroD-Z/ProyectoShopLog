const MODELO_BASE = {
    gastoId: 0,
    usuarioId: 0,
    nombre: "",
    descripcion: "",
    monto: 0,
    fechaDeIngreso: "",
    usuario: "",
}

let tablaData;

$(document).ready(function () {

    tablaData = $('#tbdata').DataTable({
        responsive: true,
         "ajax": {
             "url": '/AdmiGasto/Lista',
             "type": "GET",
             "datatype": "json"
         },
        /*"columnDefs": [
            { "defaultContent": "-", "targets": "_all" }
        ],*/
         "columns": [
             { "data": "gastoId", "visible": true, "searchable": false },
             { "data": "usuarioId", "visible": true, "searchable": false },
             { "data": "nombre" },
             { "data": "descripcion" },
             { "data": "monto" },
             { "data": "fechaDeIngreso" },
             { "data": "usuario", "visible": false, "searchable": false },
             {
                 "defaultContent": `<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>` +
                     `<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>`,
                 "orderable": false,
                 "searchable": false,
                 "width": "80px"
             }
         ],
         order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Usuarios',
                exportOptions: {
                    columns: [1, 3, 4]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function mostrarModal(modelo = MODELO_BASE) {
    $("#txtId").val(modelo.usuarioId)
    $("#txtCorreo").val(modelo.correo)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    $("#modalData").modal("show")
}

$("#btnNuevo").click(function (){
    mostrarModal()
})

$("#btnGuardar").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : ${inputs_sin_valor[0].correo}`;
        toastr.warning("", mensaje)
        $(`input[correo]= "${inputs_sin_valor[0].correo}"]`).focus()

        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idUsuario"] = parseInt($("#txtId").val())
    modelo["correo"] = $("#txtCorreo").val()
    modelo["idRol"] = $("#cboRol").val()

    const formData = new FormData();

    formData.append("foto", null)
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idUsuario == 0) {
        fetch("/Usuario/Crear", {
            method: "POST",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.estado) {
                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "El usuario fue creado", "success")
                } else {
                    swal("Lo sentimos, no se creo el usuario", responseJson.mensaje, "error")
                }
            })
    } else {
        fetch("/Usuario/Editar", {
            method: "PUT",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.estado) {
                    tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                    filaSeleccionada = null;
                    $("#modalData").modal("hide")
                    swal("Listo!", "El usuario fue editado", "success")
                } else {
                    swal("Lo sentimos no se pudo editar", responseJson.mensaje, "error")
                }
            })
    }
})

let filaSeleccionada
$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();
    mostrarModal(data)
})