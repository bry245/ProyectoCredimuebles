var API_KEY = "sk-YfMvKiClRugUGjFZOAdfqS9zk255pLjQndYvTtlX0HT3BlbkFJijdjk45Nn-9SJ9-0g2aph0PtHTow6vM_pEfzrnNowA"

let chatHistory = [];

async function GPTChat(mensaje) {
    chatHistory.push({ role: "user", content: mensaje });
    
    const res = await fetch('https://api.openai.com/v1/chat/completions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + API_KEY
        },
        body: JSON.stringify({
            model: "gpt-4",
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

const prompt = document.querySelector('#prompt');
const enviar = document.querySelector('#generate');

enviar.addEventListener('click', async () => {
    let pregunta = $("#prompt").val();
    if (!pregunta) return;

    displayMessage(pregunta, 'user');
    $("#prompt").val("");

    try {
        const respuesta = await GPTChat(pregunta);
        displayMessage(respuesta, 'admin');
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
