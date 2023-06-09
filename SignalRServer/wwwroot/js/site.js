﻿const connection = new signalR.HubConnectionBuilder()
    .withUrl("/learning-hub")
    .configureLogging(signalR.LogLevel.Information)
    .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

const messages = document.getElementById('signalr-message-panel');

connection.on("ReceiveMessage", (message) => {
    const div = document.createElement('div');
    div.textContent = message;
    messages.prepend(div);
});

connection.on("PrivateMessage", (message) => {
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

const selftBtn = document.getElementById('btn-self-message');
const selfMsg = document.getElementById('self-message');
selftBtn.addEventListener('click', () => {
    connection.invoke("SendToCaller", selfMsg.value).catch(err => console.error(err.toString()));
})

const connectionIdBtn = document.getElementById('btn-conn-id');
connectionIdBtn.addEventListener('click', () => {
    connection.invoke("GetConnectionId").catch(err => console.error(err.toString()));
})

const userId = document.getElementById('user-id-input');
const privateMsg = document.getElementById('private-message');
const privateMsgBtn = document.getElementById('btn-private=-message');
privateMsgBtn.addEventListener('click', () => {
    connection.invoke("SendMessageTo", privateMsg.value, userId.value).catch(err => console.error(err.toString()));
})

const groupMsg = document.getElementById('group-message');
const groupName = document.getElementById('group-name');
const groupMsgBtn = document.getElementById('btn-group-message');
groupMsgBtn.addEventListener('click', () => {
    connection.invoke("SendToGroup", groupMsg.value, groupName.value).catch(err => console.error(err.toString()));
})

const joinGroupName = document.getElementById('group-to-join');
const joinGroupBtn = document.getElementById('btn-join-group');
joinGroupBtn.addEventListener('click', () => {
    connection.invoke("JoinGroup", joinGroupName.value).catch(err => console.error(err.toString()));
})

const leaveGroupName = document.getElementById('group-to-leave');
const leaveGroupBtn = document.getElementById('btn-leave-group');
leaveGroupBtn.addEventListener('click', () => {
    connection.invoke("LeaveGroup", leaveGroupName.value).catch(err => console.error(err.toString()));
})

async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
}

connection.onclose(async () => {
    await start();
});

start();