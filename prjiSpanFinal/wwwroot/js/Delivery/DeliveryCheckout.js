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
$(".selectCoupon").click(function () {
    $(".showCalTotalPriceVC").css("position", "relative");
});
$("#exampleModal").on("hidden.bs.modal", function () {
    $(".showCalTotalPriceVC").css("position", "sticky").css("top", "80px");
});

//DeliveryFillCheckoutForm ViewComponent

$(".ship").click(function () {
    $(this).removeClass("border-secondary border-danger").addClass("border-danger").siblings(".addAddress").removeClass("d-none").closest("div").siblings().children(".addAddress").removeClass("d-none").addClass("d-none").end().children(".ship").removeClass("border-secondary border-danger").addClass("border-secondary");
});
$(".changeShip").click(function () {
    $(".choseShip").slideToggle();
});
$(".saveAddress").click(function () {
    let address = $("#inputAddress").val();
    if (address == "") {
        alert("請輸入地址或門市");
        return false;
    }
    else {
        $(".choseShip").find("div[class*='border-danger']").siblings(".address").html(address).removeClass("d-none");
    }
});
$("#confirmedShip").click(function () {
    let address = $(".choseShip").find("div[class*='border-danger']").siblings(".address").html();
    if (address == "") {
        alert("請輸入地址或門市");
    }
    else {
        let ship = $(".choseShip").find("div[class*='border-danger']").html();
        $("#finalShip").html(ship);
        $("#finalAddress").html(address);
        $(".choseShip").slideToggle();
    }
});