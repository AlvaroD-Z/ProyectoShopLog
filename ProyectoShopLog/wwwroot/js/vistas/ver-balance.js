const monedaSolFormateador = new Intl.NumberFormat("es-PE", {
    style: 'currency',
    currency: 'PEN',
});

const saldoPositivoClass = "alert-success";
const saldoNegativoClass = "alert-danger";
const saldoResultanteLabel = "SALDO RESULTANTE";
const totalLabel = "TOTAL";

/**
 * invoca un servicio web, utilizando http
 * para obtener los gastos
 * 
 * @returns {{
 *  ingresos: Array<gastoId, usuarioId, nombre, descripcion, monto, fechaDeIngreso, usuario>, 
 *  gastos: Array<gastoId, usuarioId, nombre, descripcion, monto, fechaDeIngreso, usuario>, 
 *  totalIngresos: number, 
 *  totalGastos: number, 
 *  saldoResultante: number
 * }}
 */
async function fetchBalance({ fechaInicio, fechaFin }) {
    const fetchParametros = new URLSearchParams({
        fechaInicio,
        fechaFin
    });
    const httpResponse = await fetch(`/AdmiGasto/GetBalance?${fetchParametros}`);
    const responseData = await httpResponse.json();
    return responseData;
}

/**
 * crea una fila(HTML) para la tabla,
 * muestra nombre y monto.
 * 
 * @param {HTMLTableElement} tableElement
 * @param {{nombre, monto}}
 */
function crearFilaCeldas(tableElement, { categoriaNombre, nombre, monto }) {
    const tableBody = tableElement.getElementsByTagName("tbody")[0];
    const row = tableBody.insertRow();

    row.insertCell(0)
        .appendChild(document.createTextNode(categoriaNombre))
    row.insertCell(1)
        .appendChild(document.createTextNode(nombre))
    row.insertCell(2)
        .appendChild(document.createTextNode(monedaSolFormateador.format(monto)))
}

/**
 * crea el footer para la tabla
 * mostrando el total de montos.
 * 
 * @param {HTMLTableElement} tableElement 
 * @param {number} total 
 */
function crearFooter(tableElement, total) {
    const footer = tableElement.createTFoot();
    const row = footer.insertRow(0);

    row.classList.add("font-weight-bold", "text-primary");

    row.insertCell(0);
    row.insertCell(1)
        .appendChild(document.createTextNode(totalLabel));
    row.insertCell(2)
        .appendChild(document.createTextNode(monedaSolFormateador.format(total)));
}

/**
 * elimina body y footer de una tabla.
 * 
 * @param {HTMLTableElement} ingresosTableElement 
 */
function clearTableElements(ingresosTableElement) {
    ingresosTableElement.removeChild(ingresosTableElement.getElementsByTagName("tbody")[0]);
    ingresosTableElement.appendChild(document.createElement('tbody'));

    const tFootElement = ingresosTableElement.getElementsByTagName("tfoot")[0];

    if (tFootElement) {
        ingresosTableElement.removeChild(tFootElement);
    }
}

/**
 * renderiza la tabla ingresos.
 * 
 * @param {{
 *  ingresos: Array<{nombre, monto, categoriaNombre}>, 
 *  totalIngresos: number
 * }}  
 */
function renderizarTablaIngresos({ ingresos, totalIngresos }) {
    const ingresosTableElement = document.getElementById("ingresosTable");
    clearTableElements(ingresosTableElement);

    for (const ingreso of ingresos) {
        crearFilaCeldas(ingresosTableElement, {
            categoriaNombre: ingreso.categoriaNombre,
            nombre: ingreso.nombre,
            monto: ingreso.monto
        });
    }

    crearFooter(ingresosTableElement, totalIngresos);
}


/**
 * renderiza la tabla gastos.
 * 
 * @param {{
 *  gastos: Array<{nombre, monto}>,
 *  totalGastos: number
 * }} 
 */
function renderizarTablaGastos({ gastos, totalGastos }) {
    const gastosTableElement = document.getElementById("gastosTable");
    clearTableElements(gastosTableElement);

    for (const gasto of gastos) {
        crearFilaCeldas(gastosTableElement, {
            categoriaNombre: gasto.categoriaNombre,
            nombre: gasto.nombre,
            monto: gasto.monto
        });
    }

    crearFooter(gastosTableElement, totalGastos);
}

/**
 * 
 * @param {{saldoResultante: number}} 
 */
function renderizarSaldoResultante({ saldoResultante }) {
    const alertSaldoResultanteElement = document.getElementById("balanceSaldoResultante");
    const saldoIsPositivo = saldoResultante > 0;
    const alertClassAUsar = saldoIsPositivo ? saldoPositivoClass : saldoNegativoClass;

    alertSaldoResultanteElement.classList.add(alertClassAUsar);
    alertSaldoResultanteElement.innerHTML = `${saldoResultanteLabel}: ${monedaSolFormateador.format(saldoResultante)}`;
}

/**
 * 
 * @param {{
 *  ingresos: Array<gastoId, usuarioId, nombre, descripcion, monto, fechaDeIngreso, usuario>, 
 *  gastos: Array<gastoId, usuarioId, nombre, descripcion, monto, fechaDeIngreso, usuario>, 
 *  totalIngresos: number, 
 *  totalGastos: number, 
 *  saldoResultante: number
 * }} balance 
 */
function renderizarBalance(balance) {
    renderizarSaldoResultante({
        saldoResultante: balance.saldoResultante
    });
    renderizarTablaIngresos({
        ingresos: balance.ingresos,
        totalIngresos: balance.totalIngresos
    });
    renderizarTablaGastos({
        gastos: balance.gastos,
        totalGastos: balance.totalGastos
    });
}

async function onClickObtenerBalance() {
    const fechaInicioBalanceElement = document.getElementById("fechaInicioBalance");
    const fechaFinBalanceElement = document.getElementById("fechaFinBalance");

    const fechaInicioBalance = fechaInicioBalanceElement.value;
    const fechaFinBalance = fechaFinBalanceElement.value;

    if (fechaInicioBalance && fechaFinBalance) {
        const balance = await fetchBalance({
            fechaInicio: fechaInicioBalance,
            fechaFin: fechaFinBalance
        });

        renderizarBalance(balance);
    }
}

async function onReadyAddEvents() {
    const obtenerBalanceElement = document.getElementById("obtenerBalance");

    obtenerBalanceElement.addEventListener('click', onClickObtenerBalance);
}

$(document).ready(onReadyAddEvents);
