
/**
 * agrega un option element a una select.
 * 
 * @param {HTMLSelectElement} selectElement 
 * @param {{value: string, text: string}} 
 */
function agregarOpcionASelect(selectElement, { value, text }) {
    const optionElement = document.createElement('option');

    optionElement.value = value;
    optionElement.text = text;

    selectElement.options.add(optionElement);
}

/**
 * Elimina todas las opciones de una select.
 * 
 * @param {HTMLSelectElement} selectElement 
 */
function clearOptionsSelect(selectElement) {
    selectElement.options.length = 0;
}

async function renderizarCategoria(tipoMovimiento) {
    const categoriaElement = document.getElementById("categoria");
    clearOptionsSelect(categoriaElement);

    const categorias = await httpGet("/Categoria/ListaByTipoMovimiento", { tipoMovimiento });

    for (const categoria of categorias) {
        agregarOpcionASelect(categoriaElement, { value: categoria.categoriaId, text: categoria.nombre });
    }
}


async function renderizarCategoriaByTipoMovimiento(tipoMovimiento) {
    return await renderizarCategoria(tipoMovimiento);
}

function onChangeTipoMovimiento(event) {
    const tipoMovimiento = event.target.value;
    renderizarCategoriaByTipoMovimiento(tipoMovimiento)
}

function onReadyAddEvents() {
    const tipoMovimientoElement = document.getElementById("tipoMovimiento");
    tipoMovimientoElement.addEventListener('change', onChangeTipoMovimiento);
}

$(document).ready(onReadyAddEvents);
