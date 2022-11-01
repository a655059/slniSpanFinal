$(function () {
    const maxLayer = Number($(".messageLayout").attr("id").substring(13));
    for (let i = 0; i <= maxLayer; i++) {
        const left = `+=${i * 50}px`;
        const width = `${500 - i*50}px`
        $(`.layer${i}`).css({ "left": left });
        $(`.layer${i}`).find(".messageContent").css("width", width);
    }
});

$(".messageReplyBtn").click(function () {
    $(this).closest(".messageFooter").siblings(".messageReplay").slideToggle();
});

