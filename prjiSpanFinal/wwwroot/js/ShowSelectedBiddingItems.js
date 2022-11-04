$(function () {
    $(".VCLayout_").each(function () {
        let i = 10;
        $(this).find(".biddingItemLayout").each(function () {
            $(this).css("left", i);
            i += 210;
        });
    });
    ControlPage();
});

function ControlPage() {
    $(".page").each(function () {
        let currentPage = Number($(this).html());
        let totalPage = Number($(this).siblings(".totalPage").html());
        if (currentPage == 1) {
            $(this).closest("div").siblings(".previousPage").attr("disabled", true).css("background-color", "#FFDCB9").css("color", "#D0D0D0");
        }
        else {
            $(this).closest("div").siblings(".previousPage").attr("disabled", false).css("background-color", "#FFA042").css("color", "black");
        }
        if (currentPage == totalPage) {
            $(this).closest("div").siblings(".nextPage").attr("disabled", true).css("background-color", "#FFDCB9").css("color", "#D0D0D0");
        }
        else {
            $(this).closest("div").siblings(".nextPage").attr("disabled", false).css("background-color", "#FFA042").css("color", "black");
        }
    });
}

$(".nextPage").click(function () {
    const page = Number($(this).siblings("div").find(".page").html()) + 1;
    $(this).siblings("div").find(".page").html(page);
    $(this).closest("div").siblings(".VCLayout").find(".productCardLayout").animate({ left: "-=1050px" });
    ControlPage();
});

$(".previousPage").click(function () {
    const page = Number($(this).siblings("div").find(".page").html()) - 1;
    $(this).siblings("div").find(".page").html(page);
    $(this).closest("div").siblings(".VCLayout").find(".productCardLayout").animate({ left: "+=1050px" });
    ControlPage();
});