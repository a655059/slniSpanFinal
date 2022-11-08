$(function () {
    CalTotalPriceIncludeDiscountAndFee()
});
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
        Swal.fire("請選擇一個折價券");
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

function CalTotalPriceIncludeDiscountAndFee() {
    let smallPrice = 0;
    $(".purchaseCount").each(function () {
        let purchaseCount = Number($(this).html());
        let unitPrice = Number($(this).closest("div").siblings().find(".unitPrice").html());
        smallPrice += purchaseCount * unitPrice;
    })

    $("#smallPrice").html(smallPrice);
    const shipperFee = Number($("#finalShipperFee").html());
    const paymentFee = Number($("#finalPaymentFee").html());
    const totalPrice = smallPrice + shipperFee + paymentFee;
    $("#totalPrice").html(totalPrice);
};

//DeliveryFillCheckoutForm.js

$(".changeShip").click(function () {
    $(".choseShip").slideToggle();
});

$(".ship").click(function () {
    $(this).removeClass("border-danger").addClass("border-danger").siblings(".addAddress").removeClass("d-none").closest("div").siblings().children(".addAddress").removeClass("d-none").addClass("d-none").end().children(".ship").removeClass("border-danger");
});

$(".saveAddress").click(function () {
    let address = $(".inputAddress").val();
    if (address == "") {
        Swal.fire("請輸入地址或門市");
        return false;
    }
    else {
        $(".choseShip").find("div[class*='border-danger']").siblings(".address").html(address);
    }
});
$("#confirmedShip").click(function () {
    let address = $(".choseShip").find("div[class*='border-danger']").siblings(".address").html();
    if (address == "") {
        Swal.fire("請輸入地址或門市");
    }
    else {
        let ship = $(".choseShip").find("div[class*='border-danger']").find(".shipperName").html();
        let shipperID = $(".choseShip").find("div[class*='border-danger']").attr("id").substring(7);
        let shipFee = $(".choseShip").find("div[class*='border-danger']").find(".shipperFee").html();
        $("#finalShipper").html(ship);
        $("#finalShipperID").html(shipperID);
        $(".finalShipperFee").html(shipFee);
        $("#address").val(address);
        $(".choseShip").slideToggle();
        $(".finalShipInfo").removeClass("d-none");
        $(".calFeePrice").removeClass("d-none");
        CalTotalPriceIncludeDiscountAndFee();
    }
});
$(".payment").change(function () {
    const paymentFee = $(".payment:checked").siblings("label").find(".paymentFee").html();
    $("#finalPaymentFee").html(paymentFee);
    $(".calPaymentFeePrice").removeClass("d-none");
    CalTotalPriceIncludeDiscountAndFee();
});
