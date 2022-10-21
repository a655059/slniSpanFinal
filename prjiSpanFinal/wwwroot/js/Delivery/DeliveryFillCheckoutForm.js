$(".changeShip").click(function () {
    $(".choseShip").slideToggle();
});

$(".ship").click(function () {
    $(this).removeClass("border-danger").addClass("border-danger").siblings(".addAddress").removeClass("d-none").closest("div").siblings().children(".addAddress").removeClass("d-none").addClass("d-none").end().children(".ship").removeClass("border-danger");
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


//以下 Google Maps
let storeLocations;
let map;
let shipperName;
$(".addAddress").click(async function () {
    shipperName = $(this).siblings().find(".shipperName").html();
    const _url = `GetShipperLocation?shipperName=${shipperName}`;
    let response = await fetch(_url);
    let data = await response.json();
    storeLocations = JSON.parse(data);
    
});


function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 24.137519, lng: 120.686687 },
        zoom:12,
    });
    console.log(shipperName);
    if (shipperName == "7-11取貨") {
        let marker_config = storeLocations.map(function (value, index) {
            let newObject = { position: { lat: value.X, lng: value.Y }, map: map, title: value.POIName };
            return newObject;
        });
        //let marker_configs = "[" + marker_config.join(",") + "]";
        storeLocations = marker_config;
        console.log(marker_config);
    }
    
    //let markers = [];
    //storeLocations.forEach(function (e,i) {
    //    markers[i] = new google.maps.Marker(e);
    //    markers[i].setMap(map);
    //});

    //let marker = new google.maps.Marker({
    //    position: { lat: 24.137519, lng: 120.686687 },
    //    map: map,
    //});
};
window.initMap = initMap;