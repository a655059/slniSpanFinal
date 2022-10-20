$(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('#BackTop').fadeIn(400);
        } else {
            $('#BackTop').fadeOut(400);
        }
    });
    $('#BackTop').click(function () {
        $('html,body').animate({ scrollTop: 0 }, 333);
    });
});
const h1 = $("#header1");
const h2 = document.getElementById("header2");
sizeble();
addEventListener('resize', async () =>
    await sizeble());
function sizeble() {
    h2.style.marginTop = (h1.height() + 16) + "px";
}