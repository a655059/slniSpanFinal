


$(".smallPhoto>div").mouseenter(function () {
    $(this).children().css("border-color", "orange").end().siblings().children().css("border-color", "#E0E0E0");
    let imgSrc = $(this).children().attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});

$(".purchaseStyle").mouseenter(function () {
    let imgSrc = $(this).siblings(".itemStyleImg").attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});



$(".buyDirectly").click(function () {
    if ($("input[name='purchaseStyle']:checked").length != 1) {
        alert("請選擇一個規格");
        return false;
    }
});










