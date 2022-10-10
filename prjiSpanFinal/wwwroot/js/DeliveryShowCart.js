CalTotalPrice();


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
        $(this).closest(".form-check").siblings().find(".selectAll").prop("checked", false);
    }
    else {
        $(this).closest(".form-check").siblings().find(".selectAll").prop("checked", true);
    }
    
});






function CalTotalPrice() {
    let smallPrice = 0;
    $(".selectItem:checked").closest("div").siblings().find(".itemCount").each(function () {
        let itemCount = $(this).val();
        let itemPrice = $(this).closest("div").siblings().find(".itemPrice").html();
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
    CalTotalPrice();
});
$(".countPlus").click(function () {
    let value = Number($(this).siblings(".itemCount").val()) + 1;
    $(this).siblings(".itemCount").val(value);
    if ($(this).siblings(".itemCount").val() != 1) {
        $(this).siblings(".countMinus").attr("disabled", false).css("color", "#000000");
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

$(function () {
    $(".itemCount").each(function (idx, ele) {
        if ($(this).val() <= 1) {
            $(this).siblings(".countMinus").attr("disabled", true).css("color", "#D0D0D0");
        }
        else {
            $(this).siblings(".countMinus").attr("disabled", false).css("color", "#000000");
        }
    });
});






