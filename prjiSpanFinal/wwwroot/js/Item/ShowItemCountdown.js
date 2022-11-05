
let biddingIDs = [];

let connection3 = new signalR.HubConnectionBuilder().withUrl("/selectedBiddingItemsCountdownHub").build();

connection3.on("ShowItemCountdown", function (remainingTimes, ids) {
    $(".biddingItemLayout").each(function (index, element) {
        let biddingID = Number($(this).attr("id"));
        for (let i = 0; i < ids.length; i++) {
            if (ids[i] == biddingID) {
                $(this).find(".remainingTime").html(remainingTimes[i]);
            }
        }
    });
});

connection3.start().then(function () {
    console.log("connect3 start");
    $(".biddingItemLayout").each(function () {
        biddingIDs.push(Number($(this).attr("id")));
    });
    connection3.invoke("SelectedBiddingItemsCountdown", biddingIDs)
}).catch(function (err) {
    console.log(err);
});


