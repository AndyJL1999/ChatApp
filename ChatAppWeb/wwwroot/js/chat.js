const loadMore = document.getElementById('loadMoreContainer');
const loadButton = document.getElementById('loadBtn');
const threadContainer = document.getElementById('messagesContainer');
const messageInput = document.getElementById('message');
const sendButton = document.getElementById('sendButton');
const messageList = JSON.parse((document.getElementById('messages').value));
const recipient = JSON.parse((document.getElementById('recipient').value));
const channelContainer = document.getElementById('channelContainer');

var messageCountLimit = 0;

connection.on('ReceiveMessage', addMessageToChat);

$(document).ready(function () {
    messageList.reverse();

    if (messageList.length >= 20)
        loadMore.hidden = false;

    if (messageList.length <= 100)
        window.scrollTo(0, document.documentElement.scrollHeight);

    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;
});

function sendMessage() {
    var content = document.getElementById('message').value;
    let message = { userId: currentUserId, channelId: channelId, channelType: 'Chat', content: content };
    
    sendMessageToHub(message);
}

function addMessageToChat(message, sentAt) { 
    createChatElement(message, sentAt, true);

    messageList.unshift(
        {
            id: message.id,
            userName: message.userName,
            userId: message.userId,
            content: message.content,
            sentAt: message.sentAt
        }
    );

    updateChannelList(message.content);

    messageInput.value = '';

    window.scrollTo(0, document.documentElement.scrollHeight);
}

function updateChannelList(lastMessage) {
    for (var i = 0; i < channelList.length; i++) {
        if (channelList[i].id === channelId) {
            channelList[i].lastMessage = lastMessage;
            createChannelElement(lastMessage, i, channelName);
            break;
        }
    }
}

function createChatElement(message, sentAt, isNewMessage) {
    let isCurrentUserMessage = currentUserId === message.userId;

    let container = document.createElement('div');
    let contentConatiner = document.createElement('div');
    let timeStamp = document.createElement('span');

    container.className = isCurrentUserMessage ? 'row align-items-center justify-content-start flex-row-reverse' : 'row align-items-center justify-content-start';

    contentConatiner.className = 'chat col-auto';
    contentConatiner.style = isCurrentUserMessage ? 'background: dodgerblue' : 'background: limegreen';
    contentConatiner.innerHTML = message.content;

    timeStamp.className = 'row form-floating me-1 text-black-50 justify-content-end';
    timeStamp.innerHTML = sentAt;

    contentConatiner.appendChild(timeStamp);
    container.appendChild(contentConatiner);

    if (isNewMessage)
        threadContainer.appendChild(container);
    else
        threadContainer.insertBefore(container, threadContainer.firstChild);
}

function loadInMoreMessages() {
    for (let i = messageCountLimit; i < messageCountLimit + 20; i++) { // Load 20 messages at a time

        if (i !== messageList.length) { // if there is a message then display it
            var timeSent = new Date(messageList[i].sentAt);
            createChatElement(messageList[i], timeSent.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true }), false);
            threadContainer.insertBefore(loadMore, threadContainer.firstChild);
        }
        else { 
            break;
        }
    }
    
}

function loadMessagesFromDB() {
    $.ajax({
        type: "POST",
        url: "./Chat/LoadMoreMessages",
        data: JSON.stringify({ channelId: channelId, messages: messageList }),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                var oldMessageCount = messageList.length;

                messageList.push(...data.newMessageList);

                if (messageList.length !== oldMessageCount) // If messageList has changed => load new messages
                    loadInMoreMessages();
            }
        }
    });
}

loadButton.addEventListener("click", function (event) {
    threadContainer.removeChild(threadContainer.firstElementChild);
    messageCountLimit = messageCountLimit + 20;

    if (messageCountLimit < messageList.length) {
        loadInMoreMessages();
    } else {
        loadMessagesFromDB();
    }
    event.preventDefault();
});

sendButton.addEventListener("click", function (event) {
    sendMessage();
    event.preventDefault();
});

messageInput.addEventListener("keyup", function () {
    sendButton.disabled = messageInput.value.trim() === '';
});

function saveData() {
    var chatVm =
    {
        currentUserId: currentUserId,
        channel: {
            id: channelId,
            name: channelName,
            type: 'Chat'
        },
        recipient: recipient,
        messages: messageList.slice(0, 100)
    };

    $.ajax({
        type: "POST",
        url: "./Chat/OnLeave",
        data: JSON.stringify({ chatVM: chatVm, channels: channelList }),
        contentType: "application/json; charset=utf-8"
    });
}
