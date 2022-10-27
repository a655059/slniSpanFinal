$(function () {
    ControlCommentPage();
});

function ControlCommentPage() {
    let currentCommentPage = Number($(".currentCommentPage").html());
    let totalCommentPage = Number($(".totalCommentPage").html());
    if (currentCommentPage == 1) {
        $("#commentPrevioustPage").attr("disabled", true).css("color", "#ADADAD");
    }
    else {
        $("#commentPrevioustPage").attr("disabled", false).css("color", "#000000");
    }
    if (currentCommentPage == totalCommentPage) {
        $("#commentNextPage").attr("disabled", true).css("color", "#ADADAD");
    }
    else {
        $("#commentNextPage").attr("disabled", false).css("color", "#000000");
    }
}

//$("#commentNextPage").click(function () {
//    let currentCommentPage = Number($(".currentCommentPage").html());

//    @{ int nextPage = Model + 10; }
//    $(".partialView").load('@Url.Action("Comment", "Item", new { id = nextPage })');

//});
//$("#commentPrevioustPage").click(function () {
//    @{ int previousPage = Model - 10; ;
//}
//        $(".partialView").load('@Url.Action("Comment", "Item", new { id = previousPage })');
//    });