/**
 * crea una solicitud http POST
 * 
 * @param {string} url 
 * @param {any} data 
 * @returns 
 */
async function httpPost(url, data) {
    const httpResponse = await fetch(url, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    const httpResponseBody = await httpResponse.json();
    if (httpResponse.ok) {
        return httpResponseBody;
    }

    return Promise.reject(httpResponseBody.mensaje);
}

/**
 * crea una solicitud http PUT
 * 
 * @param {string} url 
 * @param {any} data 
 * @returns 
 */
async function httpPut(url, data) {
    const httpResponse = await fetch(url, {
        method: "PUT",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    const httpResponseBody = await httpResponse.json();
    if (httpResponse.ok) {
        return httpResponseBody;
    }

    return Promise.reject(httpResponseBody.mensaje);
}

/**
 * crea una solicitud http GET
 * @param {string} url 
 * @param {any} params 
 * @returns 
 */
async function httpGet(url, params) {
    const fetchParametros = new URLSearchParams(params);
    const httpResponse = await fetch(`${url}?${fetchParametros}`);
    const responseData = await httpResponse.json();
    return responseData.data;
}