////let connection2 = new signalR.HubConnectionBuilder().withUrl("/countdownHub").build();

//connection2.on("ShowItemCountdown", function (time) {
//    $(".remainingTime").html(time);
//    console.log(time);
//});

connection2.on("ShowUploadItem", function (productName) {
    console.log(`競標商品: ${productName}已上架`);
});

connection2.on("ShowPullItem", function(productName) {
    console.log(`競標商品: ${productName}已結標`);
});

connection2.start().then(function () {
    console.log("connect2 start");
    connection2.invoke("Countdown");
}).catch(function (err) {
    console.log(err);
});

