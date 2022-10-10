$(".coupon").click(function () {
    let discount = $(this).children().eq(0).children().html();
    $(".discount").children().html(`-$${discount}`);
});
$(".cancel").click(() => {
    $(".discount").children().html(`0`);
    $("input[type='radio']").attr("checked", false);
});
$(".chose").click(() => {
    if ($("input[type='radio']:checked").length == 0) {
        alert("請選擇一個折價券");
    }
    else {
        let couponID = $("input[type='radio']:checked").siblings().children().eq(4).children().html();
        let discount = $("input[type='radio']:checked").siblings().children().eq(0).children().html();
        $(".divDiscount").removeClass("d-none");
        $(".selectedCouponID").children().html(`${couponID}`);
        $("#discountPrice").html(`${discount}`);
        let totalPrice = Number($("#smallPrice").html()) - Number(discount);
        $("#totalPrice").html(totalPrice);
    }
});
$(".divDiscount").mouseenter(function () {
    $(".removeCoupon").removeClass("d-none");
});
$(".divDiscount").mouseleave(function () {
    $(".removeCoupon").addClass("d-none");
});
$(".removeCoupon").click(function () {
    $(".divDiscount").toggleClass("d-none");
    $("input[type='radio']").attr("checked", false);
    $("#discountPrice").html("");
    $(".selectedCouponID").children().html("");
    $(".discount").children().html("0");
    CalTotalPrice();
});
