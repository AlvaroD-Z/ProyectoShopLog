const dateFormateador = new Intl.DateTimeFormat('es-PE');

class Movimiento {

    constructor(gastoId, usuarioId, nombre, descripcion, monto, fechaDeIngreso, categoriaId, tipoMovimiento) {
        this.gastoId = gastoId;
        this.usuarioId = usuarioId;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.monto = monto;
        this.fechaDeIngreso = fechaDeIngreso;
        this.categoriaId = categoriaId;
        this.tipoMovimiento = tipoMovimiento;
    }

    static nuevo(nombre, descripcion, monto, fechaDeIngreso, categoriaId, tipoMovimiento) {
        return new Movimiento(null, null, nombre, descripcion, monto, fechaDeIngreso, categoriaId, tipoMovimiento);
    }
}


/**
 * guarda un movimiento
 * 
 * @param {Movimiento} movimiento 
 * @returns {{gastoId}}
 */
async function saveMovimiento(movimiento) {
    if (movimiento.gastoId) {
        return await httpPut("/AdmiGasto/Actualizar", movimiento);
    }

    return await httpPost("/AdmiGasto/Crear", movimiento);
}

/**
 * 
 * @param {{gastoId: number}}
 * @returns 
 */
async function eliminarMovimiento({ gastoId }) {
    return await httpPut("/AdmiGasto/Eliminar", { gastoId });
}

const MODELO_BASE = Movimiento.nuevo(
    "",
    "",
    null,
    formatDateForInput(new Date()),
    null
);

let tablaData;

$(document).ready(function () {

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/AdmiGasto/Lista',
            "type": "GET",
            "datatype": "json"

        },
        drawCallback: function () {
        },
        "columns": [
            { "data": "gastoId", "visible": false, "searchable": false },
            { "data": "usuarioId", "visible": false, "searchable": false },
            { "data": "tipoMovimiento" },
            { "data": "categoriaNombre" },
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
                filename: 'Reporte Gastos',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },

    });

})

/**
 * 
 * @param {Movimiento} movimiento 
 */
async function confirmarEliminacionMovimiento(movimiento) {
    $('#confirmEliminarGastoId').val(movimiento.gastoId);

    $("#confirmarModal").modal("show");
}

async function mostrarModal(movimiento = MODELO_BASE) {
    await renderizarCategoriaByTipoMovimiento(movimiento.tipoMovimiento);
    if (movimiento.gastoId) {
        $("#dateFecha").prop("disabled", true);
    } else {
        $("#dateFecha").prop("disabled", false);
    }

    $("#gastoId").val(movimiento.gastoId)
    $("#tipoMovimiento").val(movimiento.tipoMovimiento)
    $("#categoria").val(movimiento.categoriaId)
    $("#txtNombre").val(movimiento.nombre)
    $("#txtDescripcion").val(movimiento.descripcion)
    $("#numMonto").val(movimiento.monto)
    $("#dateFecha").val(formatDateForInput(new Date(movimiento.fechaDeIngreso)))

    const requiredElements = document.getElementsByClassName("input-validar");
    renderizarInvalidOrValidElements(requiredElements);
    $("#modalData").modal("show")
}

$("#btnNuevo").click(async function () {
    mostrarModal()
})

$("#btnConfirmarEliminar").click(async function () {
    $.LoadingOverlay("show");

    try {
        const gastoIdAEliminar = parseInt($("#confirmEliminarGastoId").val());
        await eliminarMovimiento({
            gastoId: gastoIdAEliminar
        });

        $.LoadingOverlay("hide");
        $("#confirmarModal").modal("hide");
        swal("Listo!", `Eliminado con éxito`, "success");
        $("#tbdata").DataTable().ajax.reload();
    } catch (ex) {
        $.LoadingOverlay("hide");
        swal(`Se produjo un problema. Intente nuevamente.`, ex.message, "error")
    }
});

$("#btnGuardar").click(async function () {
    const requiredElements = document.getElementsByClassName("input-validar");
    renderizarInvalidOrValidElements(requiredElements);

    const existeElementosInvalidos = requiredElementsSonValidos(requiredElements);
    if (existeElementosInvalidos) {
        return;
    }

    const movimiento = new Movimiento(
        parseInt($("#gastoId").val()),
        null,
        $("#txtNombre").val(),
        $("#txtDescripcion").val(),
        parseInt($("#numMonto").val()),
        $("#dateFecha").val(),
        parseInt($("#categoria").val()),
        $("#tipoMovimiento").val()
    );

    $.LoadingOverlay("show");

    try {
        await saveMovimiento(movimiento);

        $.LoadingOverlay("hide");
        $("#modalData").modal("hide");
        swal("Listo!", `Guardado con éxito`, "success");
        $("#tbdata").DataTable().ajax.reload();
    } catch (ex) {
        $.LoadingOverlay("hide");
        swal(`Se produjo un problema. Intente nuevamente.`, ex.message, "error")
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

    mostrarModal(new Movimiento(
        data.gastoId,
        data.usuarioId,
        data.nombre,
        data.descripcion,
        data.monto,
        data.fechaDeIngreso,
        data.categoriaId,
        data.tipoMovimiento))
})

$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();
    console.log({ data });

    confirmarEliminacionMovimiento(
        new Movimiento(
            data.gastoId,
            data.usuarioId,
            data.nombre,
            data.descripcion,
            data.monto,
            data.fechaDeIngreso,
            data.categoriaId,
            data.tipoMovimiento)
    );
})