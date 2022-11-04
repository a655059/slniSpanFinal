



let connection4 = new signalR.HubConnectionBuilder().withUrl("/specificItemCountdownHub").build();
//connection4.off("ShowSpecificItemCountdown", null);
connection4.on("ShowSpecificItemCountdown", function (remainingTime) {
    $(".biddingItemCountdown").find(".remainingTime").html(remainingTime);
    console.log(remainingTime);
});

connection4.start().then(function () {
    console.log("connect4 start");
    const biddingID = Number($(".biddingItemCountdown").attr("id"));
    connection4.invoke("SpecificItemCountdown", biddingID);
}).catch(function (err) {
    console.log(err);
});


