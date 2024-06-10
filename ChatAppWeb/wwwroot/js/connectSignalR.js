const currentUserId = document.getElementById('userId').value;
const channelName = document.getElementById('channelName').value;

channelId = document.getElementById('channelId').value;

var connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl + 'message?channelId=' + channelId, {
        accessTokenFactory: () => getToken().then(function (token) {
            return token;
        })
    })
    .build();

connection.start().catch(error => console.log(error));

function sendMessageToHub(message) {
    connection.invoke('SendMessage', message)
        .catch(error => console.log(error));
}