const currentUserId = document.getElementById('userId').value;
const channelId = document.getElementById('channelId').value;
const channelName = document.getElementById('channelName').value;

var connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl + 'message?channelId=' + channelId)
    .build();

connection.start().catch(error => console.log(error));

function sendMessageToHub(message) {
    connection.invoke('SendMessage', message)
        .catch(error => console.log(error));
}