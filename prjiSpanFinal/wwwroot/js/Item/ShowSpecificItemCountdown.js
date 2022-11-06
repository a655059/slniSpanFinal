


let biddingID = 0;
let connection4 = new signalR.HubConnectionBuilder().withUrl("/specificItemCountdownHub").build();

connection4.on("ShowSpecificItemCountdown", function (remainingTime, id) {
    if (id == biddingID) {
        $(".biddingItemCountdown").find(".remainingTime2").html(remainingTime);
    }
});




connection4.start().then(function () {
    console.log("connect4 start");
    biddingID = Number($(".biddingItemCountdown").attr("id"));
    connection4.invoke("SpecificItemCountdown", biddingID);
}).catch(function (err) {
    console.log(err);
});


//window.document.body.onbeforeunload = function () {
//    connection4.stop().then(function () {
//        connection4 = null;
//    });
//    return "確定要離開嗎?"
//};
