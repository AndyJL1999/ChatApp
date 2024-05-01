var connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl).build();

connection.start().catch(error => console.log(error));

function sendMessageToHub(message) {
    connection.invoke('SendMessage', message)
        .catch(error => console.log(error));
}