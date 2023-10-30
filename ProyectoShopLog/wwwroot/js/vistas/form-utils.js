
/**
 * 
 * @param {HTMLCollectionOf} collection 
 */
function renderizarInvalidOrValidElements(collection) {
    for (const element of collection) {
        if (element.value.trim() === "") {
            element.classList.remove('is-valid');
            element.classList.add('is-invalid');
        } else {
            element.classList.remove('is-invalid');
            element.classList.add("is-valid");
        }
    }
}

function requiredElementsSonValidos(collection) {
    for (const element of collection) {
        if (element.value.trim() === "") {
            return true;
        }
    }

    return false;
}

/**
 * Formatea una fecha para ser usado en un input date.
 * 
 * @param {Date} date
 * @returns {string} 
 */
function formatDateForInput(date) {
    return date.toISOString().substring(0, 10);
}