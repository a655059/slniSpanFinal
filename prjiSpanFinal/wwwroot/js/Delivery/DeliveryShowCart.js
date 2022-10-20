$(function () {
    $(".itemCount").each(function (idx, ele) {
        if ($(this).val() <= 1) {
            $(this).siblings(".countMinus").attr("disabled", true).css("color", "#D0D0D0");
            $(this).val(1);
        }
        const maxValue = Number($(this).attr("max"));
        if ($(this).val() >= maxValue) {
            $(this).siblings(".countPlus").attr("disabled", true).css("color", "D0D0D0");
            $(this).val(maxValue);
            $(this).closest("div").siblings().removeClass("d-none");
        }
    });
    CalTotalPrice();
});



$(".selectAll").change(function () {
    if ($(this).prop("checked")) {
        $(this).closest("div").siblings().find(".selectItem").prop("checked", true);
    }
    else {
        $(this).closest("div").siblings().find(".selectItem").prop("checked", false);
    }
    CalTotalPrice();
});

$(".selectItem").change(function () {
    CalTotalPrice();
    let count = $(this).closest(".sellerLayout").find(".selectItem").length;
    let checkedCount = $(this).closest(".sellerLayout").find(".selectItem:checked").length;
    if (checkedCount != count) {
        $(this).closest(".sellerLayout").find(".selectAll").prop("checked", false);
    }
    else {
        $(this).closest(".sellerLayout").find(".selectAll").prop("checked", true);
    }
});
$(".itemCount").change(function () {
    if ($(this).val() <= 1) {
        $(this).siblings(".countMinus").attr("disabled", true).css("color", "#D0D0D0");
        $(this).siblings(".countPlus").attr("disabled", false).css("color", "#000000");
        $(this).closest("div").siblings().removeClass("d-none").addClass("d-none");
        $(this).val(1);
    }
    const maxValue = Number($(this).attr("max"));
    if ($(this).val() >= maxValue) {
        $(this).siblings(".countMinus").attr("disabled", false).css("color", "#000000");
        $(this).siblings(".countPlus").attr("disabled", true).css("color", "#D0D0D0");
        $(this).closest("div").siblings().removeClass("d-none");
        $(this).val(maxValue);
    }
    CalTotalPrice();
});

function CalTotalPrice() {
    let smallPrice = 0;
    $(".selectItem:checked").closest("div").siblings().find(".itemCount").each(function () {
        let itemCount = $(this).val();
        let itemPrice = $(this).closest(".choseCount").siblings().find(".itemPrice").html();
        smallPrice += Number(itemCount) * Number(itemPrice);
    });
    document.getElementById("smallPrice").innerHTML = smallPrice;
    let discount = 0;
    let discountPriceElement = document.getElementById("discountPrice");
    if (discountPriceElement.innerHTML != null) {
        discount = Number(discountPriceElement.innerHTML);
    }
    let totalPrice = smallPrice - Number(discount);
    document.getElementById("totalPrice").innerHTML = totalPrice;
}


$(".countMinus").click(function () {
    let value = Number($(this).siblings(".itemCount").val()) - 1;
    $(this).siblings(".itemCount").val(value);
    if ($(this).siblings(".itemCount").val() <= 1) {
        $(this).attr("disabled", true).css("color", "#D0D0D0");
    }
    const maxValue = Number($(this).siblings(".itemCount").attr("max"));
    if ($(this).siblings(".itemCount").val() < maxValue) {
        $(this).siblings(".countPlus").attr("disabled", false).css("color", "#000000");
        $(this).closest("div").siblings().removeClass("d-none").addClass("d-none");
    }
    CalTotalPrice();
});
$(".countPlus").click(function () {
    let value = Number($(this).siblings(".itemCount").val()) + 1;
    $(this).siblings(".itemCount").val(value);
    if ($(this).siblings(".itemCount").val() != 1) {
        $(this).siblings(".countMinus").attr("disabled", false).css("color", "#000000");
    }
    const maxValue = Number($(this).siblings(".itemCount").attr("max"));
    if ($(this).siblings(".itemCount").val() >= maxValue) {
        $(this).attr("disabled", true).css("color", "#D0D0D0");
        $(this).closest("div").siblings().removeClass("d-none");
    }
    CalTotalPrice();
});

$(".itemCount").on("blur", function () {
    if ($(this).val() <= 1) {
        $(this).val(1);
        $(this).siblings(".countMinus").attr("disabled", true).css("color", "#D0D0D0");
    }
    else {
        $(this).siblings(".countMinus").attr("disabled", false).css("color", "#000000");
    }
    CalTotalPrice();
});










