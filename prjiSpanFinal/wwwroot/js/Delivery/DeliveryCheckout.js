$(function () {
    $("#finalShipperFee1").html($("#finalShipperFee").html());
    CalTotalPriceIncludeDiscountAndFee()
    
});

$(".showCalTotalPriceVC").on("mouseenter", ".divDiscount", function () {
    $(".removeCoupon").removeClass("d-none");
});
$(".showCalTotalPriceVC").on("mouseleave", ".divDiscount", function () {
    $(".removeCoupon").addClass("d-none");
});
$(".showCalTotalPriceVC").off("click", ".removeCoupon").on("click", ".removeCoupon", function () {
    $(".divDiscount").addClass("d-none");
    $("input[class*='couponInput']:checked").prop("checked", false);
    $(".discountPrice").html(0);
    
    CalTotalPriceIncludeDiscountAndFee();
});




function CalTotalPriceIncludeDiscountAndFee() {
    let smallPrice = 0;
    $(".purchaseCount").each(function () {
        let purchaseCount = Number($(this).html());
        let unitPrice = Number($(this).closest("div").siblings().find(".unitPrice").html());
        smallPrice += purchaseCount * unitPrice;
    })
    $("#smallPrice").html(smallPrice);
    const shipperFee = Number($("#finalShipperFee1").html());
    const paymentFee = Number($("#finalPaymentFee").html());
    let totalPrice = 0;
    if ($(".couponInput:checked").length > 0) {
        let isFreeShipperFee = $(".couponInput:checked").siblings().find(".isFreeShipperFee").attr("id");
        if (isFreeShipperFee == "freeShipperFee") {
            totalPrice = smallPrice + shipperFee + paymentFee - shipperFee;
        }
        else {
            const discount = Number($(".discountPrice").html());
            $(".purchaseCount").each(function () {
                let purchaseCount = Number($(this).html());
                let unitPrice = Number($(this).closest("div").siblings().find(".unitPrice").html());
                totalPrice += purchaseCount * Math.ceil(Number(unitPrice * discount/10));
            })
            totalPrice = totalPrice + shipperFee + paymentFee;
        }
    }
    else {
        totalPrice = smallPrice + shipperFee + paymentFee;
    }
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
        let isFreeShipperFee = $(".couponInput:checked").siblings().find(".isFreeShipperFee").attr("id");
        $("#finalShipper").html(ship);
        $("#finalShipperID").html(shipperID);
        $(".finalShipperFee").html(shipFee);
        $("#address").val(address);
        $(".choseShip").slideToggle();
        $(".finalShipInfo").removeClass("d-none");
        $(".calFeePrice").removeClass("d-none");
        if ($(".divDiscount").attr("class").split(' ').indexOf("d-none") < 0 && isFreeShipperFee == "freeShipperFee") {
            $(".discountPrice").html(shipFee);
        }
        CalTotalPriceIncludeDiscountAndFee();
    }
});
$(".payment").change(function () {
    const paymentFee = $(".payment:checked").siblings("label").find(".paymentFee").html();
    $("#finalPaymentFee").html(paymentFee);
    $(".calPaymentFeePrice").removeClass("d-none");
    CalTotalPriceIncludeDiscountAndFee();
});

$(".showCalTotalPriceVC").off("click", "#previousStore").on("click", "#previousStore", function () {
    changePage("Prev");
});

$(".showCalTotalPriceVC").on("click", ".selectCoupon", function () {
    $(".showCalTotalPriceVC").css({ "position": "relative", "z-index": "1" });
    $(".couponModal").show();
});

$(".showCalTotalPriceVC").on("click", ".cancel", function () {
    $(".couponModal").hide();
    $(".showCalTotalPriceVC").css({ "position": "sticky" });
    $("input[class*='couponInput']:checked").prop("checked", false);
});

$(".showCalTotalPriceVC").on("click", ".chose", function () {
    if ($(".couponInput:checked").length < 1) {
        Swal.fire("請選擇一張折價券");
    }
    else {
        let isZeroMinimum = $(".couponInput:checked").siblings(".coupon").find(".isZeroMinimum").attr("id");
        if (isZeroMinimum == "notZeroMinimum") {
            let smallPrice = Number($("#smallPrice").html());
            let minimum = Number($(".couponInput:checked").siblings(".coupon").find(".isFreeShipperFee").html());
            if (smallPrice < minimum) {
                Swal.fire("消費金額不足，無法使用此優惠券");
                return false;
            }
        }
        $(".couponModal").hide();
        $(".showCalTotalPriceVC").css({ "position": "sticky" });
        $(".selectedCouponID").attr("id", $(".couponInput:checked").attr("id"));
        $(".selectedCouponID").html($(".couponInput:checked").siblings().find(".couponCode").html());
        $(".divDiscount").removeClass("d-none");
        let isFreeShipperFee = $(".couponInput:checked").siblings().find(".isFreeShipperFee").attr("id");
        if (isFreeShipperFee == "freeShipperFee") {
            let shipperFee = $(".finalShipperFee").html();
            $(".freeShipper").show();
            $(".notFreeShipper").hide();
            $(".discountPrice").html(shipperFee);
        }
        else {
            $(".freeShipper").hide();
            $(".notFreeShipper").show();
            $(".discountPrice").html($(".couponInput:checked").siblings().find(".goDiscount").html());
        }
        CalTotalPriceIncludeDiscountAndFee();
    }
});
