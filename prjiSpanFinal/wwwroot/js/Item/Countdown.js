


let connection2 = new signalR.HubConnectionBuilder().withUrl("/countdownHub").build();

let upload = 0;
connection2.on("ShowUploadItem", function (productName, sellerID) {
    if (upload == 0) {
        upload = 1;
        happy_sendnoti(2, sellerID, "競標商品上架通知", "/Home/Index", `你的競標商品${productName}已上架`);
    }
    
});

let pull = 0;
connection2.on("ShowPullItem", function (productName, sellerID) {
    if (pull == 0) {
        pull = 1;
        happy_sendnoti(2, sellerID, "競標結果", "/Home/Index", `你的競標商品${productName}已結束，無人參加此商品的競標`);
    }
    
});

connection2.on("ToGetItemMember", function (productName, buyerID, price) {
    happy_sendnoti(2, buyerID, "競標結果", "/Delivery/ShowCart", `你以 ${price} 元得標 ${productName} 商品，請查看你的購物車`);
});



connection2.start().then(function () {
    console.log("connect2 start");
    connection2.invoke("Countdown");
}).catch(function (err) {
    console.log(err);
});

