$(function () {
    const maxLayer = Number($(".messageLayout").attr("id").substring(13));
    for (let i = 0; i <= maxLayer; i++) {
        const left = `+=${i * 50}px`;
        const width = `${500 - i * 50}px`
        $(`.layer${i}`).css({ "left": left });
        $(`.layer${i}`).find(".messageContent").css("width", width);
    }
    for (let i = 0; i < maxLayer; i++) {
        $(`.layer${i}`).each(function () {
            if ($(this).next(".messageBoardID").length > 0) {
                const nextLayer = Number($(this).next(".messageBoardID").attr("class").split(' ')[0].substring(5));
                if (nextLayer > i) {
                    $(this).find(".messageFooter").append(`<div class="collapseMessage" style="cursor:pointer">收合</div>`);
                }
            }
        });
    }
});

$(".messageReplyBtn").click(function () {
    $(this).closest(".messageFooter").siblings(".messageReplay").slideToggle();
});

//$(".messageBoardVC").on("click", ".collapseMessage", function () {
//    const thisLayerNumber = Number($(this).closest(".messageBoardID").attr("class").split(' ')[0].substring(5));
//    if ($(this).closest(".messageBoardID").nextAll(".messageBoardID").length > 0) {
//        if ($(this).html() == "收合") {
//            $(this).closest(".messageBoardID").nextAll(".messageBoardID").each(function (event) {
//                const layerNumber = Number($(this).attr("class").split(' ')[0].substring(5));
//                if (layerNumber > thisLayerNumber) {
//                    console.log($(this).attr("class"));
//                    $(this).removeClass("d-none").addClass("d-none");
//                    console.log($(this).attr("class"));
//                }
//                else {
//                    return;
//                }
//            })
//            $(this).html("展開");
//        }
//        else {
//            $(this).closest(".messageBoardID").nextAll(".messageBoardID").each(function () {
//                const layerNumber = Number($(this).attr("class").split(' ')[0].substring(5));
//                if (layerNumber > thisLayerNumber) {
//                    console.log($(this).attr("class"));
//                    $(this).removeClass("d-none");
//                    console.log($(this).attr("class"));
//                    if ($(this).has(".collapseMessage").length > 0) {
//                        $(this).find(".collapseMessage").html("收合");
//                    }
//                }
//                else {
//                    return;
//                }
//            })
//            $(this).html("收合");
//        }
//    }
//});

