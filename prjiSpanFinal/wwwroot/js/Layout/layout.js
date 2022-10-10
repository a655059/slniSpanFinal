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
        //window.scrollTo({ top: 0, behavior: 'smooth' });
    });
});


//$(".notification").hover(
//    function () {
//        $('#collapseExample1').collapse('show');
//    }, function () {
//        $('#collapseExample1').collapse('hide');
//    }
//);

//let cost = 0;

//$(`#notification`).mouseenter(function () {
//     cost++;
//     checkcollapes();
// });
//$(`#collapseExample1`).mouseenter(function () {
//    cost++;
//    checkcollapes();
//});
//$(`#notification`).mouseleave(function () {
//        cost--;
//    checkcollapes();
//    });
//$(`#collapseExample1`).mouseleave(function () {
//    cost--;
//    checkcollapes();
//});

//function checkcollapes() {
//    if (cost <= 0) {
//        $(`#collapseExample1`).hide();
//    }
//    else if (cost > 0) {
//        $(`#collapseExample1`).show();
//    }
//    console.log(cost);
//}



const h1 = $("#header1");
const h2 = document.getElementById("header2");
sizeble();
addEventListener('resize', async () =>
    await sizeble());
function sizeble() {
    h2.style.marginTop = (h1.height() + 16) + "px";
}