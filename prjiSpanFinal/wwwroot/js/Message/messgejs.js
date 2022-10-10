$(".msgemoji").click(function () {
    let src = "";
    if ($(event.target).hasClass("msgemoji1")) {
        src = "/img/sleepinpark.jpg";
    }
    else if ($(event.target).hasClass("msgemoji2")) {
        src = "/img/lapsleepinpark.jpg";
    }
    else if ($(event.target).hasClass("msgemoji3")) {
        src = "/img/pien.jpg";
    }
    $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
                                <div>
                                    <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary"><img src="`+ src +`" /></p>
                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
                                </div>
                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
                            </div>`);
    $("#collapseemoji").removeClass("show");
    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
});

$("#msgenter").click(function () {
    let msg = $("#msgtextinput").val();
    $("#msgtextinput").val("");
    if (msg.includes(`Item/Index/2113`) == true) {
        msg = `<a href="/Item/Index/2113">
    <div style="max-width:400px; max-height:600px;background-color:white;">
        <div style="margin:6px">
            <div class="sq align-self-center "> <img class="img-fluid align-self-center mr-2 mr-md-4 pl-0 p-0 mb-2" src="/img/product01.png" width="135" height="135" /> </div>
            <div class="media-body text-right">
                <div class=""> <h6 class="mb-0">【JUST台灣現貨】獨家顏色！台灣製造30000mAh PD/QC快充3A 行動電源 保固一年 行動電源 行動充 隨身充</h6></div>
                <div class="row flex-column flex-md-row">
                    <div class="col"> <small>規格 : 樣式1</small></div>
                    <div class="col"> <small>數量 : 55</small></div>
                    <div class="col"><h6 class="mb-0">$499</h6></div>
                </div>
            </div>
        </div>
    </div>
</a>`;
    }
    $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
                                <div>
                                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">`+ msg + `</div>
                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
                                </div>
                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
                            </div>`);
    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
});


const image_input = document.querySelector("#msgupload");
image_input.addEventListener("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        let msg = `<img id="msgdisplayimg" style="width: 259px;height: 194px;border: 1px solid black;background-position: center;background-size: cover;"></img>`;
        $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
                                <div>
                                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">`+ msg + `</div>
                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
                                </div>
                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
                            </div>`);
        const uploaded_image = reader.result;
        document.querySelector("#msgdisplayimg").style.backgroundImage = `url(${uploaded_image})`;
    });
    reader.readAsDataURL(this.files[0]);
    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
});