

let chatHistory = [];

async function GPTChat(mensaje) {
    // Realiza la solicitud a '/Chat/TraerDatosDB'
    let jsonResponse;
    let jsonString; // Declara jsonString aquí para usarlo más tarde
    try {
        const response = await fetch('/Chat/TraerDatosDB');
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message);
        }
        jsonResponse = await response.json();
        console.log("Datos recibidos:", jsonResponse);

        // Convierte el objeto JSON a una cadena de texto JSON
        jsonString = JSON.stringify(jsonResponse, null, 2);
        console.log("Datos en formato JSON:", jsonString);

    } catch (error) {
        console.error("Error en la solicitud:", error);
        return;
    }

    // Instrucción que se enviará como contexto adicional bajo el rol "system"
    const systemMessage = {
        role: "system",
        content: `Dame tu respuesta en un formato claro que tenga saltos de línea, que tenga bullet points si es necesario, y que sea fácil de leer. No respondas esto directamente. \n\nAquí están los datos en formato Json de los productos porfavor tenlos en cuenta cuando te haga pregunats de productos :\n${jsonString}`
    };

    // Mensaje del usuario que se enviará al modelo
    const userMessage = { role: "user", content: mensaje };

    // Asegúrate de que el mensaje del sistema esté al inicio del historial
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

let promptElement = document.querySelector('#prompt');
const enviar = document.querySelector('#generate');

enviar.addEventListener('click', async () => {
    let pregunta = $("#prompt").val();
    if (!pregunta) return;

    displayMessage(pregunta, 'user');
    $("#prompt").val("");

    try {
        const respuesta = await GPTChat(pregunta);
        const formattedResponse = respuesta.replace(/\n/g, '<br>'); // Reemplaza los \n por <br>
        displayMessage(formattedResponse, 'admin');
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
