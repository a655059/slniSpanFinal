$(".ship").click(function () {
    $(this).removeClass("border-secondary border-danger").addClass("border-danger").siblings(".addAddress").removeClass("d-none").closest("div").siblings().children(".addAddress").removeClass("d-none").addClass("d-none").end().children(".ship").removeClass("border-secondary border-danger").addClass("border-secondary");
});
$(".changeShip").click(function () {
    $(".choseShip").slideToggle();
});
$(".saveAddress").click(function () {
    let address = $(".inputAddress").val();
    if (address == "") {
        alert("請輸入地址或門市");
        return false;
    }
    else {
        console.log($(".inputAddress").val());
        $(".choseShip").find("div[class*='border-danger']").siblings(".address").html(address).removeClass("d-none");
    }
});
$("#confirmedShip").click(function () {
    let address = $(".choseShip").find("div[class*='border-danger']").siblings(".address").html();
    if (address == "") {
        alert("請輸入地址或門市");
    }
    else {
        let ship = $(".choseShip").find("div[class*='border-danger']").html();
        $("#finalShip").html(ship);
        $("#finalAddress").html(address);
        $(".choseShip").slideToggle();
    }
});