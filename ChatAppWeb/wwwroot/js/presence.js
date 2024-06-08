const channelList = JSON.parse((document.getElementById('channelList').value));

function getToken() {

    return new Promise(function (resolve) {
        $.ajax({
            type: "GET",
            url: "/UserAccount/Auth/GetTokenForPresenceHub",
            data: {},
            contentType: "application/json",
            success: function (data) {
                if (data.success) {
                    resolve(data.token);
                }
            }
        });
    });
}

getToken().then(function (token) {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl + 'presence', {
            accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build();

    connection.start().catch(error => console.log(error));

    connection.on('NewMessageRecieved', messageRecieved);
})

function messageRecieved(content, channelId, name) {
    for (var i = 0; i < channelList.length; i++) {
        if (channelList[i].id === channelId) {
            channelList[i].lastMessage = content;
            createChannelElement(content, i, name);
            break;
        }
    }
}

function createChannelElement(lastMessage, channelIndex, channelName) {
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
