
var connection = new signalR.HubConnectionBuilder().withUrl("http://127.0.0.1:5163/chat").build();

connection.on("Message", function (time, message) {
    $("#messageList").append('<li>' + message + '</li>');
});

connection.start().then(function() {

}).catch(function(err) {
    alert(err.toString());
});

function sendMessage() {
    var userName = $("#userName").val();
    var message = $("#message").val();

    connection.invoke("SendMessage",
    {
        userName: userName,
        message: message
    }).catch(function (err) {
        alert(err.toString());
    });

}
