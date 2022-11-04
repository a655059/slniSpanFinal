let connection2 = new signalR.HubConnectionBuilder().withUrl("/countdownHub").build();

connection2.on("Countdown", function (time) {
    $(".remainingTime").html(time);
    console.log(time);
});

connection2.start().then(function () {
    console.log("connect2 start");
    connection2.invoke("Countdown", 3);
}).catch(function (err) {
    console.log(err);
});
