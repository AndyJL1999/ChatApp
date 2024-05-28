
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
})

