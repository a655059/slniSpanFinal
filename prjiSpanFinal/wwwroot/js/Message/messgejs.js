let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let memacc = $("#msgmemid").val();
let activec = "3";
let mempic = $("#header1mempic").attr("src");

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

$("#msgenter").click(function () {
    if ($("#msgtextinput").val() == "") {
        return;
    }
    let today = new Date();
    let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    let message = "qwer" + timestamp + $("#msgtextinput").val();
    $("#msgtextinput").val("");
    connection.invoke("SendMessage", memacc, message, activec).catch(function (err) {
        return console.error(err.toString());
    });
});


$(".msgopendialog").click(function (event) {
    $.get(`/Controllers/MsgApi?cid=${$(this).siblings("input").val()}&&id=${memacc}`, function (data) {
        let obj = JSON.parse(data);

    });
});


connection.on("ReceiveMessage", function (sendFrom, message, sendTo, msgid) {
    if (message == "") {
        return;
    }
    if (memacc != sendTo && memacc != sendFrom) {
        return;
    }
    let msgheader = message.substr(0, 4);
    let msgtimestamp = message.substr(4, 17);
    let msgbody = message.substr(21);
    
    if (sendFrom == memacc) {
        if (msgheader == "qwer") {
            $("#messagebody").append(MyMessagePack(msgbody, msgtimestamp));
        }



        connection.invoke("HaveReadMessage", msgid).catch(function (err) {
            return console.error(err.toString());
        });
        $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
    }

    else if (sendFrom == activec) {
        if (msgheader == "qwer") {
            $("#messagebody").append(CMyMessagePack(msgbody, msgtimestamp));
        }
    }
    else {
        console.log($(`.msgcid[value="${sendFrom}"]`).siblings("a").children(1).children("span"));
    }

    refreshtimestamp();
});


setInterval(refreshtimestamp, 1000);

function timestamptodatetime(timestamp) {
    let indate = new Date();
    indate.setHours(timestamp.substr(0, 2));
    indate.setMinutes(timestamp.substr(2, 2));
    indate.setSeconds(timestamp.substr(4, 2));
    indate.setMilliseconds(timestamp.substr(6, 3));
    indate.setFullYear(timestamp.substr(9, 4));
    indate.setMonth(timestamp.substr(13, 2)-1);
    indate.setDate(timestamp.substr(15, 2));
    return indate;
}
function refreshtimestamp() {
    let hiddens = document.getElementsByClassName("msgbodytimestamp");
    let timeval;
    let indate;
    let today = new Date();
    let diff;
    for (let i = 0; i < hiddens.length; i++) {
        timeval = $(hiddens[i]).val();
        indate = timestamptodatetime(timeval);
        console.log(indate);
        diff = today - indate;
        if (diff > 86400000 && diff < 864000000) {
            $(hiddens[i]).siblings("p").text(Math.floor(diff / 86400000) + "天前");
        }
        else if (diff > 3600000) {
            $(hiddens[i]).siblings("p").text(Math.floor(diff / 3600000) + "小時前");
        }
        else if (diff > 60000) {
            $(hiddens[i]).siblings("p").text(Math.floor(diff / 60000) + "分鐘前");
        }
        else if (diff < 60000) {
            $(hiddens[i]).siblings("p").text("現在");
        }
        else {
            $(hiddens[i]).siblings("p").text(+timeval.substr(0, 2) + ":" + timeval.substr(2, 2) + " , " + +timeval.substr(13, 2) + "/" + timeval.substr(15, 2));
        }
    }
    
    
}

function MyMessagePack(msg,time) {
    let str = `<div class="d-flex flex-row justify-content-end">
                <div>
                    <input type="hidden" class="msgbodytimestamp" value="${time}" />
                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">${msg}</div>
                    <p class="small me-3 mb-3 rounded-3 text-muted"></p>
                </div>
                <img src="${mempic}" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
               </div>`
    return str;
}
function CMessagePack(msg, time) {
    let str = `<div class="d-flex flex-row justify-content-start">
               <img src="${mempic}" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
                 <div>
                    <input type="hidden" class="msgbodytimestamp" value="${time}" />
                    <div class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">${msg}</div>
                    <p class="small ms-3 mb-3 rounded-3 text-muted float-end"></p>
                 </div>
               </div>`
    return str;
}
    //let time = new Date();
    //let hm = time.getHours() + ":" + (time.getMinutes() < 10 ? '0' : '') + time.getMinutes();
    //let head = message.substr(0, 5);
    //let body = message.substr(5);

    //if (curUser == server) {
    //    if (head == "ihtgs") {
    //        if (user == server) {
    //            if (isSticker) {
    //                $(`#m${sendTo}`).prepend(`<div class="clearfix mt-1 rounded">
    //                <img src="${body}" class="${frame}" style="float:right;cursor:default;width:100px"/>
    //            </div>`);
    //            } else {
    //                $(`#m${sendTo}`).prepend(`<div class="clearfix mt-1 rounded">
    //                <img src="${body}" class="w-25 imgAnime" style="float:right"/>
    //            </div>`);
    //            }
    //        }
    //        else {
    //            if ($(`#u${user}`)[0] == null) {
    //                $("#listUser").append(`<div id="u${user}" class="border border-1" onclick='hideOther("${user}")'>${user} &emsp; <i onclick='hideSelf("${user}")' class="Xbox bi bi-x-square-fill"></i></div>`);
    //                $("#divParent").append(`<div id="m${user}" hidden></div>`);
    //            }
    //            $(`#u${user}`).attr("class", "border border-1 neon-effect");
    //            if (isSticker) {
    //                $(`#m${user}`).prepend(`<div class="clearfix mt-1">
    //                <img src="${body}" class="${frame}" style="cursor:default;width:100px"/>
    //            </div>`);
    //            } else {
    //                $(`#m${user}`).prepend(`<div class="clearfix mt-1">
    //                <img src="${body}" class="w-25 imgAnime"/>
    //            </div>`);
    //            }
    //        }
    //    }
    //    else {
    //        if (user == server) {
    //            $(`#m${sendTo}`).prepend(`<div class="clearfix mt-1">
    //                <div class="shadow rounded p-1 pull-right bg-light">
    //                    <h6>${hm}</h6><h4 class="text-success">客服：${message}</h4>
    //                </div>
    //             </div>`);
    //        }
    //        else
    //            if ($(`#u${user}`)[0] == null) {
    //                $("#listUser").append(`<div id="u${user}" class="border border-1" onclick='hideOther("${user}")'>${user} &emsp; <i onclick='hideSelf("${user}")' class="Xbox bi bi-x-square-fill"></i></div>`);
    //                $("#divParent").append(`<div id="m${user}" hidden></div>`);
    //            }
    //        $(`#u${user}`).attr("class", "border border-1 neon-effect");
    //        $(`#m${user}`).prepend(`<div class="clearfix mt-1">
    //            <div class="shadow rounded p-1 pull-left bg-primary">
    //                <h6 class="text-white">${hm}</h6><h4 class="text-black">${user}：${message}</h4>
    //            </div>
    //        </div>`);
    //    }
    //}
    //else {
    //    if (head == "ihtgs") { SendImg(); }
    //    else {
    //        if ($("#userInput")[0].value == server) {
    //            if ($("#userInput")[0].value == user) { SendMes("客服"); }
    //            else { ReceiveMes(user); }
    //        }
    //        else {
    //            if ($("#userInput")[0].value == user) { SendMes("您"); }
    //            else { ReceiveMes("客服"); }
    //        }
    //    }
    //}





//$(".msgemoji").click(function () {
//    let src = "";
//    if ($(event.target).hasClass("msgemoji1")) {
//        src = "/img/sleepinpark.jpg";
//    }
//    else if ($(event.target).hasClass("msgemoji2")) {
//        src = "/img/lapsleepinpark.jpg";
//    }
//    else if ($(event.target).hasClass("msgemoji3")) {
//        src = "/img/pien.jpg";
//    }
//    $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
//                                <div>
//                                    <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary"><img src="`+ src +`" /></p>
//                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
//                                </div>
//                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
//                            </div>`);
//    $("#collapseemoji").removeClass("show");
//    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
//});

//$("#msgenter").click(function () {
//    let msg = $("#msgtextinput").val();
//    $("#msgtextinput").val("");
//    if (msg.includes(`Item/Index/2113`) == true) {
//        msg = `<a href="/Item/Index/2113">
//    <div style="max-width:400px; max-height:600px;background-color:white;">
//        <div style="margin:6px">
//            <div class="sq align-self-center "> <img class="img-fluid align-self-center mr-2 mr-md-4 pl-0 p-0 mb-2" src="/img/product01.png" width="135" height="135" /> </div>
//            <div class="media-body text-right">
//                <div class=""> <h6 class="mb-0">【JUST台灣現貨】獨家顏色！台灣製造30000mAh PD/QC快充3A 行動電源 保固一年 行動電源 行動充 隨身充</h6></div>
//                <div class="row flex-column flex-md-row">
//                    <div class="col"> <small>規格 : 樣式1</small></div>
//                    <div class="col"> <small>數量 : 55</small></div>
//                    <div class="col"><h6 class="mb-0">$499</h6></div>
//                </div>
//            </div>
//        </div>
//    </div>
//</a>`;
//    }
//    else if (msg.includes("退貨") == true) {
//        setTimeout(function () {
//            $("#messagebody").append(`<div class="d-flex flex-row justify-content-start">
//                                <img src="/img/robot.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
//                                <div>
//                                    <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">您可以在會員中心的購買清單中點擊「申請退貨」，</br>來進行退貨流程。</p>
//                                    <p class="small ms-3 mb-3 rounded-3 text-muted float-end">16:01 PM | 7月 10</p>
//                                </div>
//                            </div>`);
//        },2000);
//    }
//    $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
//                                <div>
//                                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">`+ msg + `</div>
//                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
//                                </div>
//                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
//                            </div>`);
//    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
//});

//$("#msgchattarget6").click(function () {
//    $("#messagebody").empty();
//    $("#messagebody").append(`<div class="d-flex flex-row justify-content-start">
//                                <img src="/img/robot.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
//                                <div>
//                                    <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">您好請問我可以做些什麼?</p>
//                                    <p class="small ms-3 mb-3 rounded-3 text-muted float-end">16:01 PM | 7月 10</p>
//                                </div>
//                            </div>`);
//});

//const image_input = document.querySelector("#msgupload");
//image_input.addEventListener("change", function () {
//    const reader = new FileReader();
//    reader.addEventListener("load", () => {
//        let msg = `<img id="msgdisplayimg" style="width: 259px;height: 194px;border: 1px solid black;background-position: center;background-size: cover;"></img>`;
//        $("#messagebody").append(`<div class="d-flex flex-row justify-content-end">
//                                <div>
//                                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">`+ msg + `</div>
//                                    <p class="small me-3 mb-3 rounded-3 text-muted">現在</p>
//                                </div>
//                                <img src="/img/Member/pekopeko.jpg" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
//                            </div>`);
//        const uploaded_image = reader.result;
//        document.querySelector("#msgdisplayimg").style.backgroundImage = `url(${uploaded_image})`;
//    });
//    reader.readAsDataURL(this.files[0]);
//    $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
//});

