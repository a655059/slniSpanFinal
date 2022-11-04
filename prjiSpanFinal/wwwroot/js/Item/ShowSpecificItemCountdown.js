



let connection4 = new signalR.HubConnectionBuilder().withUrl("/specificItemCountdownHub").build();
//connection4.off("ShowSpecificItemCountdown", null);
connection4.on("ShowSpecificItemCountdown", function (remainingTime) {
    $(".biddingItemCountdown").find(".remainingTime").html(remainingTime);
});

connection4.start().then(function () {
    console.log("connect4 start");
    const biddingID = Number($(".biddingItemCountdown").attr("id"));
    console.log(biddingID);
    connection4.invoke("SpecificItemCountdown", biddingID);
}).catch(function (err) {
    console.log(err);
});


//window.document.body.onbeforeunload = function () {
//    connection4.invoke("SpecificItemCountdown", 0);
//    connection4.stop();
//    return "確定要離開嗎?"
//};
