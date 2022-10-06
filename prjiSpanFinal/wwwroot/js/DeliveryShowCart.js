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

$(".itemCount").on("input",function () {
    CalTotalPrice();
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
CalTotalPrice();