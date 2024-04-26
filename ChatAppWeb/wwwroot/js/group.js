﻿const currentUserId = document.getElementById('userId').value;
const groupId = document.getElementById('channelId').value;
const threadContainer = document.getElementById('messagesContainer');
const messageInput = document.getElementById('message');
const sendButton = document.getElementById('sendButton');
const messageList = JSON.parse((document.getElementById('messages').value));

var connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl).build();

connection.start().catch(error => console.log(error));

connection.on('ReceiveMessage', addMessageToGroup);

function sendMessageToHub(message) {
    connection.invoke('SendMessage', message)
        .catch(error => console.log(error));
}

$(document).ready(function () {
    window.scrollTo(0, document.documentElement.scrollHeight);

    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;
});

function sendMessage() {
    var content = document.getElementById('message').value;
    let message = { userId: currentUserId, channelId: groupId, channelType: 'Group', content: content };

    sendMessageToHub(message);
}

function addMessageToGroup(message, sentAt) {
    let isCurrentUserMessage = currentUserId === message.userId;

    let container = document.createElement('div');
    let contentConatiner = document.createElement('div');
    let timeStamp = document.createElement('span');
    let namePlate = document.createElement('label');

    container.className = isCurrentUserMessage ? 'row align-items-center justify-content-start flex-row-reverse' : 'row align-items-center justify-content-start form-floating';

    contentConatiner.className = 'chat col-auto';
    contentConatiner.style = isCurrentUserMessage ? 'background: dodgerblue' : 'background: limegreen';
    contentConatiner.innerHTML = message.content;

    timeStamp.className = 'row form-floating me-1 text-black-50 justify-content-end';
    timeStamp.innerHTML = sentAt;

    namePlate.className = 'ms-4 mt-1 text-decoration-underline ps-3';
    namePlate.innerHTML = message.userName;

    contentConatiner.appendChild(timeStamp);
    container.appendChild(contentConatiner);

    if (isCurrentUserMessage !== true)
        container.appendChild(namePlate);

    threadContainer.appendChild(container);

    messageList.push(
        {
            id: message.id,
            userName: message.userName,
            userId: message.userId,
            content: message.content,
            sentAt: message.sentAt
        }
    );

    messageInput.value = '';

    window.scrollTo(0, document.documentElement.scrollHeight);
}

sendButton.addEventListener("click", function (event) {
    sendMessage();
    event.preventDefault();
});

messageInput.addEventListener("keyup", function () {
    sendButton.disabled = messageInput.value.trim() === '';
});

window.onbeforeunload = function () {
    $.ajax({
        type: "POST",
        url: "/UserHome/Group/OnLeave",
        data: JSON.stringify({ channelId: groupId, messages: messageList }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                console.log("Messages saved!")
            }
        }
    });
}