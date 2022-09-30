$(".smallPhoto>div").mouseenter(function () {
    $(this).children().css("border-color", "orange").end().siblings().children().css("border-color", "#E0E0E0");
    let imgSrc = $(this).children().attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});

$(".purchaseStyle").mouseenter(function () {
    $(this).removeClass("btn-outline-secondary").addClass("btn-outline-warning").closest("div").siblings().children("label").removeClass("btn-outline-warning").addClass("btn-outline-secondary");
    let imgSrc = $(this).siblings(".itemStyleImg").attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});
$(".purchaseStyle").click(function () {
    let price = $(this).siblings(".price").html();
    $(".itemPrice").html(price);
    $(this).removeClass("btn-outline-warning").removeClass("btn-outline-secondary").addClass("btn-outline-danger").closest("div").siblings().children("label").removeClass("btn-outline-danger").addClass("btn-outline-secondary");
    let qty = $(this).siblings(".qty").html();
    $(".itemRemainingQty").html("還剩" + qty + "件");
    $("input[name='purchaseCount']").attr("max", qty)
});
$("input[type='submit']").click(function () {
    if ($("input[name='purchaseStyle']:checked").length != 1) {
        alert("請選擇一個規格");
    }
});


