$(function () {
    const checkoutPage = Number($("#checkoutPage").html());
    $(".progressBarStage ").slice(0, checkoutPage).addClass("finish");
    $(".bar").slice(0, checkoutPage + 1).addClass("barfinish");
    $(".progressBarStage").eq(checkoutPage).addClass("active");
});