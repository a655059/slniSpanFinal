
let connection3 = new signalR.HubConnectionBuilder().withUrl("/selectedBiddingItemsCountdownHub").build();

connection3.on("ShowItemCountdown", function (remainingTimes) {
    $(".biddingItemLayout").each(function (index,element) {
        $(this).find(".remainingTime").html(remainingTimes[index]);
    });
});

connection3.start().then(function () {
    console.log("connect3 start");
    let biddingIDs = [];
    $(".biddingItemLayout").each(function () {
        biddingIDs.push(Number($(this).attr("id")));
    });
    connection3.invoke("SelectedBiddingItemsCountdown", biddingIDs)
}).catch(function (err) {
    console.log(err);
});


