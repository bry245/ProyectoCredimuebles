
let chatHistory = [];

async function GPTChat(mensaje) {
    let productosResponse, ventasResponse;
    let productosString = "", ventasString = "";

    try {
        // Solicita los datos de productos
        const productosRes = await fetch('/Chat/TraerDatosDB');
        if (!productosRes.ok) throw new Error((await productosRes.json()).message);
        productosResponse = await productosRes.json();
        productosString = JSON.stringify(productosResponse, null, 2);

        // Solicita los datos de ventas
        const ventasRes = await fetch('/Chat/ObtenerVentasTotales');
        if (!ventasRes.ok) throw new Error((await ventasRes.json()).message);
        ventasResponse = await ventasRes.json();
        ventasString = JSON.stringify(ventasResponse, null, 2);

        console.log("Datos de productos:", productosString);
        console.log("Datos de ventas:", ventasString);

    } catch (error) {
        console.error("Error en la solicitud:", error);
        return;
    }

    const systemMessage = {
        role: "system",
        content: `Dame tu respuesta enfocada en los datos productos ventas y en un formato claro que tenga saltos de l�nea, que tenga bullet points si es necesario, y que sea f�cil de leer, bas�ndote siempre en la suma de los datos de cada venta o producto. Aqu� est�n los datos en formato JSON de los productos:\n${productosString}\n\nY aqu� est�n los datos en formato JSON de las ventas totales:\n${ventasString}. No respondas esto directamente`
    };

    const userMessage = { role: "user", content: mensaje };

    chatHistory.push(systemMessage);
    chatHistory.push(userMessage);

    const res = await fetch('https://api.openai.com/v1/chat/completions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + API_KEY
        },
        body: JSON.stringify({
            model: "gpt-3.5-turbo",
            messages: chatHistory,
            max_tokens: 500,
            temperature: 0.7,
        })
    });

    if (!res.ok) {
        const error = await res.json();
        throw new Error(error.error.message);
    }

    const response = await res.json();
    const botMessage = response.choices[0].message.content;

    chatHistory.push({ role: "assistant", content: botMessage });

    return botMessage;
}

async function generarPDF() {
    try {
        const res = await fetch('/Chat/GenerarPDF', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                productosJson: productosString,  // Usa la variable que contiene los datos de productos
                ventasJson: ventasString         // Usa la variable que contiene los datos de ventas
            })
        });

        if (!res.ok) {
            const error = await res.json();
            throw new Error(error.message);
        }

        // Convertir la respuesta a un Blob para descargar el PDF
        const blob = await res.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'InformeAnual.pdf';
        document.body.appendChild(a);
        a.click();
        a.remove();
    } catch (error) {
        console.error("Error al generar el PDF:", error.message);
        displayMessage("Error al generar el PDF: " + error.message, 'admin');
    }
}

const enviar = document.querySelector('#generate');

enviar.addEventListener('click', async () => {
    let pregunta = $("#prompt").val();
    if (!pregunta) return;

    displayMessage(pregunta, 'user');
    $("#prompt").val("");

    try {
        const respuesta = await GPTChat(pregunta);
        const formattedResponse = respuesta.replace(/\n/g, '<br>');
        displayMessage(formattedResponse, 'admin');

        // Llamar a la funci�n para generar el PDF
        if (pregunta.toLowerCase().includes('generar pdf')) {
            await generarPDF();
        }

    } catch (error) {
        console.error("Error:", error.message);
        displayMessage("Error al obtener la respuesta: " + error.message, 'admin');
    }
});

function displayMessage(message, sender) {
    const now = new Date(Date.now()).toLocaleString();
    const messageHtml = `
        <li class="${sender === 'user' ? 'right' : ''}">
            <div class="conversation-list">
                <div class="ctext-wrap">
                    <div class="conversation-name">${sender === 'user' ? 'Usuario' : 'Administrador'}</div>
                    <div class="ctext-wrap-content">
                        <p class="mb-0">${message}</p>
                    </div>
                    <p class="chat-time mb-0"><i class="mdi mdi-clock-outline align-middle me-1"></i> ${now}</p>
                </div>
            </div>
        </li>
    `;
    $(".list-unstyled").append(messageHtml);
    $(".chat-conversation").scrollTop($(".chat-conversation")[0].scrollHeight);
}
