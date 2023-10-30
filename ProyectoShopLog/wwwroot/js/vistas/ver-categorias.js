class Categoria {
    constructor(categoriaId, nombre, descripcion, tipoMovimiento) {
        this.categoriaId = categoriaId;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.tipoMovimiento = tipoMovimiento;
    }

    static nuevo({ nombre, descripcion, tipoMovimiento }) {
        return new Categoria(null, nombre, descripcion, tipoMovimiento);
    }

    static editar({ categoriaId, nombre, descripcion, tipoMovimiento }) {
        return new Categoria(categoriaId, nombre, descripcion, tipoMovimiento);
    }
}

function renderCategoriasTable() {
    const categoriasDatatable = $('#categoriasTable').DataTable({
        responsive: true,
        ajax: {
            url: '/Categoria/Lista',
            type: "GET",
            datatype: "json"
        },
        drawCallback: function () {
        },
        "columns": [
            { "data": "categoriaId", "visible": false, "searchable": false },
            { "data": "tipoMovimiento" },
            { "data": "nombre" },
            { "data": "descripcion" },
            {
                "defaultContent": `<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>`,
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
                filename: 'reporte-categorias',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

    $("#categoriasTable tbody").on("click", ".btn-editar", function () {
        if ($(this).closest("tr").hasClass("child")) {
            filaSeleccionada = $(this).closest("tr").prev();
        } else {
            filaSeleccionada = $(this).closest("tr");
        }

        const data = categoriasDatatable.row(filaSeleccionada).data();
        openCategoriaDialog(Categoria.editar({
            categoriaId: data.categoriaId,
            nombre: data.nombre,
            descripcion: data.descripcion,
            tipoMovimiento: data.tipoMovimiento
        }))
    })
}


/**
 * 
 * @param {Categoria} categoria 
 */
function openCategoriaDialog(categoria) {
    const { categoriaIdElement, nombreElement, descripcionElement, tipoMovimientoElement } = getFormCategoriaElements();

    categoriaIdElement.value = categoria.categoriaId;
    nombreElement.value = categoria.nombre;
    descripcionElement.value = categoria.descripcion;
    tipoMovimientoElement.value = categoria.tipoMovimiento;

    const requiredElements = document.getElementsByClassName("input-validar");
    renderizarInvalidOrValidElements(requiredElements);

    $("#nuevaCategoriaDialog").modal("show");
}

function onNuevaCategoria() {
    openCategoriaDialog(Categoria.nuevo({
        nombre: '',
        descripcion: '',
        tipoMovimiento: ''
    }))
}

async function saveCategoria(categoria) {
    if (categoria.categoriaId) {
        return await httpPut("/Categoria/Actualizar", categoria);
    }

    return await httpPost("/Categoria/Crear", categoria);
}

/**
 * devuelve los elementos html del formulario categoria
 * 
 * @returns {{
 *  categoriaIdElement,
 *  tipoMovimientoElement,
 *  nombreElement,
 *  descripcionElement
 * }}
 */
function getFormCategoriaElements() {
    const categoriaIdElement = document.getElementById('categoriaId');
    const tipoMovimientoElement = document.getElementById('tipoMovimiento');
    const nombreElement = document.getElementById('nombre');
    const descripcionElement = document.getElementById('descripcion');

    return {
        categoriaIdElement,
        tipoMovimientoElement,
        nombreElement,
        descripcionElement
    }
}

async function onGuardarCategoria() {
    const requiredElements = document.getElementsByClassName("input-validar");
    renderizarInvalidOrValidElements(requiredElements);

    const existeElementosInvalidos = requiredElementsSonValidos(requiredElements);
    if (existeElementosInvalidos) {
        return;
    }

    const { categoriaIdElement, nombreElement, descripcionElement, tipoMovimientoElement } = getFormCategoriaElements();
    const categoriaId = categoriaIdElement.value;
    const nombre = nombreElement.value;
    const descripcion = descripcionElement.value;
    const tipoMovimiento = tipoMovimientoElement.value;

    try {
        $.LoadingOverlay("show");

        await saveCategoria(Categoria.editar({
            categoriaId: parseInt(categoriaId),
            nombre,
            descripcion,
            tipoMovimiento
        }));

        $.LoadingOverlay("hide");
        $("#nuevaCategoriaDialog").modal("hide");
        swal("Listo!", `Guardado con éxito`, "success");
        $("#categoriasTable").DataTable().ajax.reload();
    } catch (ex) {
        $.LoadingOverlay("hide");
        swal(`Se produjo un problema. Intente nuevamente.`, ex.message, "error")
    }
}

function addEvents() {
    const nuevaCategoriaElement = document.getElementById("nuevaCategoria");
    nuevaCategoriaElement.addEventListener('click', onNuevaCategoria);

    const guardarCategoriaElement = document.getElementById("guardarCategoria");
    guardarCategoriaElement.addEventListener('click', onGuardarCategoria);
}

function onReadyEvents() {
    renderCategoriasTable();

    addEvents();
}

$(document).ready(onReadyEvents);