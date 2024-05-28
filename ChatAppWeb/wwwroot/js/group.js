const loadMore = document.getElementById('loadMoreContainer');
const loadButton = document.getElementById('loadBtn');
const threadContainer = document.getElementById('messagesContainer');
const messageInput = document.getElementById('message');
const sendButton = document.getElementById('sendButton');
const messageList = JSON.parse((document.getElementById('messages').value));
const channelList = JSON.parse((document.getElementById('channelList').value));
const recipients = JSON.parse((document.getElementById('recipients').value));
const channelContainer = document.getElementById('channelContainer');

var messageCountLimit = 0;

connection.on('ReceiveMessage', addMessageToGroup);

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
    let message = { userId: currentUserId, channelId: channelId, channelType: 'Group', content: content };

    sendMessageToHub(message);
}

function addMessageToGroup(message, sentAt) {
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
            createChannelElement(lastMessage, i);
            break;
        }
    }
}

function createChannelElement(lastMessage, channelIndex) {
    let channelComponent = document.createElement('div');
    channelComponent.className = 'row align-items-center';

    let channelImg = document.createElement('img');
    channelImg.src = 'https://placehold.co/500x500/png';
    channelImg.className = 'col-4 mb-3';
    channelImg.style = 'width:30%; border-radius:50%;';

    let channelInfoComponent = document.createElement('div');
    channelInfoComponent.className = 'container col-8 m-0 p-0';

    let channelNavLink = document.createElement('a');
    channelNavLink.className = 'nav-link';

    let channelNameSpan = document.createElement('span');
    channelNameSpan.className = 'fw-bold d-inline-block text-truncate w-100';
    channelNameSpan.innerHTML = channelName;

    let channelLastMessage = document.createElement('p');
    channelLastMessage.className = 'fs-6 text-truncate';
    channelLastMessage.innerHTML = lastMessage;

    channelNavLink.appendChild(channelNameSpan);
    channelNavLink.appendChild(channelLastMessage);
    channelInfoComponent.appendChild(channelNavLink);

    channelComponent.appendChild(channelImg);
    channelComponent.appendChild(channelInfoComponent);

    channelContainer.replaceChild(channelComponent, channelContainer.children[channelIndex]);
}

function createChatElement(message, sentAt, isNewMessage) {
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

    if (isNewMessage)
        threadContainer.appendChild(container);
    else
        threadContainer.insertBefore(container, threadContainer.firstChild);
}

function loadInMoreMessages() {
    if (messageCountLimit < messageList.length) {
        for (let i = messageCountLimit; i < messageCountLimit + 20; i++) { // Load 20 messages at a time

            if (i !== messageList.length) { // if there is a message then display it
                var timeSent = new Date(messageList[i].sentAt);
                createChatElement(messageList[i], timeSent.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true }), false);
            }
            else {
                break;
            }
        }

        threadContainer.insertBefore(loadMore, threadContainer.firstChild);
    }
}

function loadMessagesFromDB() {
    $.ajax({
        type: "POST",
        url: "./Group/LoadMoreMessages",
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
    var groupVm =
    {
        currentUserId: currentUserId,
        channel: {
            id: channelId,
            name: channelName,
            type: 'Group'
        },
        recipients: recipients,
        messages: messageList.slice(0, 100)
    };

    $.ajax({
        type: "POST",
        url: "./Group/OnLeave",
        data: JSON.stringify({ groupVM: groupVm, channels: channelList }),
        contentType: "application/json; charset=utf-8"
    });
}
