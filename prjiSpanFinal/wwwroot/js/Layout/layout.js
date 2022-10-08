$(function () {
    $('#BackTop').click(function () {
        $('html,body').animate({ scrollTop: 0 }, 333);
    });
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('#BackTop').fadeIn(222);
        } else {
            $('#BackTop').stop().fadeOut(222);
        }
    }).scroll();
});

const h1 = $("#header1");
const h2 = document.getElementById("header2");
sizeble();
addEventListener('resize', async () =>
    await sizeble());
function sizeble() {
    h2.style.marginTop = (h1.height() + 16) + "px";
}