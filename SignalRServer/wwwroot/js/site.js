const connection = new signalR.HubConnectionBuilder()
    .withUrl("/learning-hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const messages = document.getElementById('signalr-message-panel');

connection.on("ReceiveMessage", (message) => {
    const div = document.createElement('div');
    div.textContent = message;
    messages.prepend(div);
});

const broadcastBtn = document.getElementById('btn-broadcast');
const broadcast = document.getElementById('broadcast');
broadcastBtn.addEventListener('click', () => {
    connection.invoke("BroadcastMessage", broadcast.value).catch(err => console.error(err.toString()));
});

const othersBtn = document.getElementById('btn-others');
const othersMsg = document.getElementById('others-msg');
othersBtn.addEventListener('click', () => {
    connection.invoke("SendToOthers", othersMsg.value).catch(err => console.error(err.toString()));
})


async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();