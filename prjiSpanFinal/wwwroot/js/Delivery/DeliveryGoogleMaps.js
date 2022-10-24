
//ispan: 25.036266945607213, 121.5441415608662
//以下 Google Maps

let currentInfoWindow = "";

$(".addAddress").click(async function () {
    let shipperName = $(this).siblings().find(".shipperName").html();
    const _url = `GetShipperLocation?shipperName=${shipperName}`;
    console.log(shipperName);
    let response = await fetch(_url);
    let data = await response.json();
    let storeLocations = JSON.parse(data);
    let storeInfo;
    let map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 24.137519, lng: 120.686687 },
        zoom:16,
    });
    const locationButton = document.createElement("button");
    locationButton.textContent = "🧿";
    locationButton.style.fontSize = "2em";
    locationButton.style.opacity = "0.5";
    locationButton.style.marginRight = "12px";
    locationButton.style.marginTop = "10px";
    locationButton.style.border = "1px solid transparent";
    locationButton.style.backgroundColor = "transparent";
    locationButton.classList.add("custom-map-control-button");
    map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(locationButton);
    locationButton.addEventListener("click", () => {
        // Try HTML5 geolocation.
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }
    });
    const imgSrc = "https://localhost:44330/img/";
    if (shipperName == "7-11取貨") {
        let marker_config = storeLocations.map(function (value, index) {
            let newObject = { position: { lat: value.Y, lng: value.X }, map: map, title: value.POIName };
            return newObject;
        });
        let info_config = storeLocations.map(function (value, index) {
            let contentString = `<div style="display:flex">
                                    <div style="width:100px;height:100px;">
                                        <img src="${imgSrc}payment5.png" alt="7-11" style="width:100%;aspect-ratio:1/1;object-fit:contain" />
                                    </div>
                                    <div>
                                        <h4>${value.POIName}門市</h4>
                                        <h6>${value.Telno}</h6>
                                        <h6 class="storeAddress">${value.Address}</h6>
                                    </div>
                                </div>`;
            return contentString;
        });
        storeLocations = marker_config;
        storeInfo = info_config;
    }
    else if (shipperName == "OK便利店取貨") {
        let marker_config = storeLocations.map(function (value, index) {
            let newObject = { position: { lat: value.lat, lng: value.lng }, map: map, title: value.name };
            return newObject;
        });
        let info_config = storeLocations.map(function (value, index) {
            let contentString = `<div style="display:flex">
                                    <div style="width:100px;height:100px;">
                                        <img src="${imgSrc}payment8.png" alt="OK便利店" style="width:100%;aspect-ratio:1/1;object-fit:contain" />
                                    </div>
                                    <div>
                                        <h4>${value.name}</h4>
                                        <h6 class="storeAddress">${value.address}</h6>
                                    </div>
                                </div>`;
            return contentString;
        });
        storeLocations = marker_config;
        storeInfo = info_config;
    }
    else if (shipperName == "全家取貨") {
        let marker_config = storeLocations.map(function (value, index) {
            let newObject = { position: { lat: value.py, lng: value.px }, map: map, title: value.NAME };
            return newObject;
        });
        let info_config = storeLocations.map(function (value, index) {
            let contentString = `<div style="display:flex">
                                    <div style="width:100px;height:100px;">
                                        <img src="${imgSrc}payment6.png" alt="全家" style="width:100%;aspect-ratio:1/1;object-fit:contain" />
                                    </div>
                                    <div>
                                        <h4>${value.NAME}</h4>
                                        <h6>${value.TEL}</h6>
                                        <h6 class="storeAddress">${value.addr}</h6>
                                    </div>
                                </div>`;
            return contentString;
        });
        storeLocations = marker_config;
        storeInfo = info_config;
    }
    else {
        let marker_config = storeLocations.map(function (value, index) {
            let newObject = { position: { lat: value.lat, lng: value.lng }, map: map, title: value.name };
            return newObject;
        });
        let info_config = storeLocations.map(function (value, index) {
            let contentString = `<div style="display:flex">
                                    <div style="width:100px;height:100px;">
                                        <img src="${imgSrc}payment7.png" alt="萊爾富" style="width:100%;aspect-ratio:1/1;object-fit:contain" />
                                    </div>
                                    <div>
                                        <h4>${value.name}</h4>
                                        <h6>${value.phone}</h6>
                                        <h6 class="storeAddress">${value.address}</h6>
                                    </div>
                                </div>`;
            return contentString;
        });
        storeLocations = marker_config;
        storeInfo = info_config;
    }
    let markers = [];
    let infos = [];
    storeInfo.forEach(function (e, i) {
        infos[i] = new google.maps.InfoWindow({
            content: e
        });
    });
    storeLocations.forEach(function (e, i) {
        markers[i] = new google.maps.Marker(e);
        markers[i].setMap(map);
        markers[i].addListener("click", function () {
            infos[i].open(map, markers[i]);
            let infoContent = infos[i].getContent();
            let storeNameIndex = infoContent.indexOf("h4");
            let storeName = infoContent.substring(storeNameIndex).split('<')[0].split('>')[1];
            let storeAddressIndex = infoContent.indexOf("storeAddress");
            let storeAddress = infoContent.substring(storeAddressIndex).split('<')[0].split('>')[1];
            $(".inputAddress").val(storeName + "    " + storeAddress);
        });
    });
    
});

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service failed."
            : "Error: Your browser doesn't support geolocation."
    );
    infoWindow.open(map);
}

//function initMap() {
    
//};
//window.initMap = initMap;