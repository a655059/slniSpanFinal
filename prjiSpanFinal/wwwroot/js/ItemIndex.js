﻿$(".smallPhoto>div").mouseenter(function () {
    $(this).children().css("border-color", "orange").end().siblings().children().css("border-color", "#E0E0E0");
    let imgSrc = $(this).children().attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});

$(".purchaseStyle").mouseenter(function () {
    let imgSrc = $(this).siblings(".itemStyleImg").attr("src");
    $(".bigPhoto img").attr("src", imgSrc);
});

$(".purchaseStyle").click(function () {
    let price = $(this).siblings(".price").html();
    $(".itemPrice").html(price);
    $(this).removeClass("btn-outline-secondary").addClass("btn-danger").closest("div").siblings().children("label").removeClass("btn-danger").addClass("btn-outline-secondary");
    let qty = $(this).siblings(".qty").html();
    $(".itemRemainingQty").html("還剩" + qty + "件");
    $("input[name='purchaseCount']").attr("max", qty)
});

$(".buyDirectly").click(function () {
    if ($("input[name='purchaseStyle']:checked").length != 1) {
        alert("請選擇一個規格");
        return false;
    }
});

$(".addToCart").click(function () {
    if ($("input[name='purchaseStyle']:checked").length != 1) {
        alert("請選擇一個規格");
        return false;
    }
    else {
        $(".dollarIcon").animate(
            {
                top: "-=400",
                opacity: "0"
            },
            1000,
        )
    }
});

$("#nextPage").click(function () {
    let page = Number($("#page").html()) + 1;
    $("#page").html(page);
    if (Number($("#page").html()) != 1) {
        $("#previousPage").attr("disabled", false).css("background-color", "#FFA042").css("color", "black");
    }
    if ($("#page").html() == $("#totalPage").html()) {
        $("#nextPage").attr("disabled", true).css("background-color", "#FFDCB9").css("color", "#D0D0D0");
    }
    $(".sellerCard").animate({ left: "-=1050px" });
});

$("#previousPage").click(function () {
    let page = Number($("#page").html()) - 1;
    $("#page").html(page);
    if (Number($("#page").html()) == 1) {
        $("#previousPage").attr("disabled", true).css("background-color", "#FFDCB9").css("color", "#D0D0D0");
    }
    if ($("#page").html() != $("#totalPage").html()) {
        $("#nextPage").attr("disabled", false).css("background-color", "#FFA042").css("color", "black");
    }
    $(".sellerCard").animate({ left: "+=1050px" });
});



