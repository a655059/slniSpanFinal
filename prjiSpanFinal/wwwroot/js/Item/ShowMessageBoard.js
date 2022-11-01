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
            if ($(this).next(".messageBoardID") != null) {
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

$(".messageBoardVC").on("click", ".collapseMessage", function () {
    const thisIndex = Number($(".messageBox .messageBoardID").index($(this).closest(".messageBoardID")));

    const thisLayerNumber = Number($(this).closest(".messageBoardID").attr("class").split(' ')[0].substring(5));
    let count = 0;
    $(this).closest(".messageBoardID").nextAll(".messageBoardID").each(function () {

        const layerNumber = Number($(this).attr("class").split(' ')[0].substring(5));
        if (layerNumber > thisLayerNumber) {
            count++;
        }
        else {
            return false;
        }
    })
    if ($(this).html() == "展開") {
        $(".messageBox .messageBoardID").slice(thisIndex + 1, thisIndex + count + 1).removeClass("d-none");
        $(this).html("收合");
        $(".messageBox .messageBoardID").slice(thisIndex + 1, thisIndex + count + 1).find(".collapseMessage").html("收合");
    } else {
        $(".messageBox .messageBoardID").slice(thisIndex + 1, thisIndex + count + 1).removeClass("d-none").addClass("d-none");
        $(this).html("展開");
    }
});
