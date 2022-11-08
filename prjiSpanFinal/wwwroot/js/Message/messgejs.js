let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let isconnectionstart = false;
let memacc = $("#msgmemid").val();
let truememacc = $("#msgmemacc").val();
let activec = "99999";
let mempic = $("#header1mempic").attr("src");
let histmultichat = "";
const chatbotid = "8";


//給想要加入跳出聊天框功能的人用的，傳入想開啟對話的帳號就好
function happy_popup(acc) {
    if (!$("#collapseExample").hasClass("show")) {
        $(".chatroom").trigger("click");
    }
    opencertaindialog(acc);
}












async function connectionstart() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}

$("#msgenter").click(function () {
    if ($("#msgtextinput").val() == "") {
        return;
    }
    let today = new Date();
    let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    let message;
    let indexofmsg = $("#msgtextinput").val().indexOf("/Item/Index?id=");
    let inputmsg = $("#msgtextinput").val();
    if (indexofmsg != -1) {
        message = "tyui" + timestamp;
        for (let i = indexofmsg + 15; i < inputmsg.length; i++) {
            if (isNaN(inputmsg[i]) || inputmsg[i] == "")
            {

            }
            else {
                message = message + inputmsg[i];
            }
        }
    }
    else if (inputmsg.includes("www.youtube.com")) {
        message = "ghjk" + timestamp + inputmsg;
    }
    else {
        message = "qwer" + timestamp + inputmsg;
    }
    $("#msgtextinput").val("");
    connection.invoke("SendMessage", memacc, message, activec).catch(function (err) {
        return console.error(err.toString());
    });
    dialogsort(activec);
})

$("#msgupload").on("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        let today = new Date();
        let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
        const uploaded_image = reader.result;
        let message = "zxcv" + timestamp + `${uploaded_image}`;
        connection.invoke("SendMessage", memacc, message, activec).catch(function (err) {
            return console.error(err.toString());
        });
        dialogsort(activec);
        
    });
    reader.readAsDataURL(this.files[0]);
})

$(".msgemoji").click(function () {
    let today = new Date();
    let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    let message = "asdf" + timestamp + $(event.currentTarget).attr("src")[$(event.currentTarget).attr("src").length - 5];
    connection.invoke("SendMessage", memacc, message, activec).catch(function (err) {
        return console.error(err.toString());
    });
    $("#collapseemoji").removeClass("show");
    dialogsort(activec);
})

function loadactivemsg(scid) {
    if (scid == 99999) {
        $("#messagebody").html("");
        activec = scid;
        $("#messagebody").append(histmultichat);
        $(`.msgcid[value="${activec}"]`).siblings("a").children().eq(1).children("span").remove();
        refreshtimestamp();
        $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
    }
    else {
        $.ajaxSetup({
            async: false
        });
        $.getJSON(`/Msgapi/Getchat`, { scid: scid, sid: memacc }, function (data) {
            $("#messagebody").html("");
            activec = scid;
            for (let i = 0; i < data.length; i++) {
                msgheader = data[i].msg.substr(0, 4);
                msgtimestamp = data[i].msg.substr(4, 17);
                msgbody = data[i].msg.substr(21);
                if (data[i].sendFrom == memacc) {
                    $("#messagebody").append(MyMessagePack(msgheader, msgbody, msgtimestamp));
                }
                else {
                    $("#messagebody").append(CMessagePack(msgheader, msgbody, msgtimestamp));
                }
            }
            $(`.msgcid[value="${activec}"]`).siblings("a").children().eq(1).children("span").remove();
            refreshtimestamp();
            $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
        });
        $.ajaxSetup({
            async: true
        });
    }
}

$(document).on('click', ".msgopendialog", function () {
    let scid = $(this).siblings("input").val();
    loadactivemsg(scid);
});


connection.on("ReceiveMessage", async function (sendFrom, message, sendTo, msgid) {
    if (message == "") {
        return;
    }
    if (memacc != sendTo && memacc != sendFrom && sendTo != 99999) {
        return;
    }
    let msgheader = message.substr(0, 4);
    let msgtimestamp = message.substr(4, 17);
    let msgbody = message.substr(21);
    let shortbody;
    if (msgheader == "asdf") {
        shortbody = "貼圖";
    }
    else if (msgheader == "zxcv") {
        shortbody = "照片";
    }
    else if (msgheader == "tyui") {
        shortbody = "本站產品";
    }
    else if (msgheader == "ghjk") {
        shortbody = "YT影片";
    }
    else {
        if (msgbody.length > 8) {
            shortbody = msgbody.substr(0, 8) + "...";
        }
        else {
            shortbody = msgbody;
        }

    }
    if (sendTo == 99999)
    {
        if (activec == 99999) {
            if (sendFrom == memacc) {
                $("#messagebody").append(MyMessagePack(msgheader, msgbody, msgtimestamp));
                histmultichat = histmultichat + MyMessagePack(msgheader, msgbody, msgtimestamp);
            }
            else {
                $("#messagebody").append(CMessagePack(msgheader, msgbody, msgtimestamp, multiid = sendFrom));
                histmultichat = histmultichat + CMessagePack(msgheader, msgbody, msgtimestamp, multiid = sendFrom);
            }
            $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
            $(`.msgcid[value="${sendTo}"]`).siblings("a").children().eq(1).children("input").val(msgtimestamp);
            $(`.msgcid[value="${sendTo}"]`).siblings("a").children().eq(0).children().eq(1).children().eq(1).html(shortbody);
            
        }
        else {
            let t = $(`.msgcid[value="99999"]`).siblings("a").children().eq(1).children("span");
            if (t.html() != null) {
                let v = parseInt(t.html()) + 1;
                t.html(v);
            }
            else {
                $(`.msgcid[value="99999"]`).siblings("a").children().eq(1).append(`<span class="badge bg-danger rounded-pill float-end">1</span>`);
            }
            $(`.msgcid[value="99999"]`).siblings("a").children().eq(1).children("input").val(msgtimestamp);
            $(`.msgcid[value="99999"]`).siblings("a").children().eq(0).children().eq(1).children().eq(1).html(shortbody);
            histmultichat = histmultichat + CMessagePack(msgheader, msgbody, msgtimestamp, multiid = sendFrom);
        }
    }
    else if (sendFrom == memacc) {
        $("#messagebody").append(MyMessagePack(msgheader, msgbody, msgtimestamp));

        $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
        $(`.msgcid[value="${sendTo}"]`).siblings("a").children().eq(1).children("input").val(msgtimestamp);
        $(`.msgcid[value="${sendTo}"]`).siblings("a").children().eq(0).children().eq(1).children().eq(1).html(shortbody);
        if (sendTo == chatbotid) {
            chatbotreply(msgheader, msgbody);
        }
    }

    else if (sendFrom == activec) {
        $("#messagebody").append(CMessagePack(msgheader, msgbody, msgtimestamp));
        $("#messagebody").animate({ scrollTop: $("#messagebody").prop("scrollHeight") }, 1000);
        $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(1).children("input").val(msgtimestamp);
        $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(0).children().eq(1).children().eq(1).html(shortbody);
        $.getJSON(`/Msgapi/HaveRead`, { msgid: msgid }, function () {});
    }
    else {
        if ($(`.msgcid[value="${sendFrom}"]`).length == 0) {
            $.getJSON(`/Msgapi/GetmembyId`, { id: sendFrom }, function (data) {
                if (data == undefined) {
                    return;
                }
                if (data.memberId == memacc || data.memberId == 1 || $(`.msgcid[value="${data.memberId}"]`).length != 0) {
                    return;
                }
                $("#msgopendialogbody").prepend(CMessageDialog(data.memberId, data.memPic, msgbody, msgtimestamp, data.memberAcc, 1));
            });
        }
        else {
            let t = $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(1).children("span");
            if (t.html() != null) {
                let v = parseInt(t.html()) + 1;
                t.html(v);
            }
            else {
                $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(1).append(`<span class="badge bg-danger rounded-pill float-end">1</span>`);
            }
            $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(1).children("input").val(msgtimestamp);
            $(`.msgcid[value="${sendFrom}"]`).siblings("a").children().eq(0).children().eq(1).children().eq(1).html(shortbody);
        }
    }
    refreshtimestamp();
    if (sendFrom == memacc) {
        dialogsort(activec);
    }
    else {
        if (sendTo == 99999) {
            dialogsort(sendTo);
        }
        else {
            dialogsort(sendFrom);
        }
    }
});



setInterval(refreshtimestamp, 60000);

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
        diff = today - indate;
        if (diff > 864000000) {
            $(hiddens[i]).siblings("p").text(`${indate.getMonth()+1}/${indate.getDate()}`);
        }
        else if (diff > 86400000) {
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

function dialogsort(id) {
    $(`.msgcid[value="${id}"]`).parent().prependTo("#msgopendialogbody");
}

function MyMessagePack(head, msg, time) {
    if (head == "asdf") {
        let src = "/img/emoji/emoji" + msg + ".jpg";
        msg = `<img class="msgdisplayemoji" src="` + src + `" />`;
    }
    else if (head == "zxcv") {
        msg = `<img src="` + msg + `" class="msgdisplayimg" style="max-width: 380px;max-height: 500px;border: 1px solid black;">`;
    }
    else if (head == "tyui") {
        getid = msg;
        $.getJSON(`/Msgapi/GetProductbyId`, { id: getid }, function (data) {
            if (data == undefined) {
                return;
            }
            msg = `<a href="/Item/Index?id=` + msg + `" style="text-decoration:none">
                <div style="max-width:380px; max-height:600px;background-color:white;">
                    <div style="margin:6px">
                        <div class="sq align-self-center "> <img class="img-fluid align-self-center mr-2 mr-md-4 pl-0 p-0 mb-2" src="data:image;base64,${data.pic}" width="135" height="135" /> </div>
                        <div class="media-body text-right">
                            <div class=""> <h6 class="mb-0">${data.productName}</h6></div>
                            <div class="row flex-column flex-md-row">
                                <div class="col"> <small>規格 : ${data.style}</small></div>
                                <div class="col"> <small>數量 : ${data.quantity}</small></div>
                                <div class="col"><h6 class="mb-0">$${data.unitPrice}</h6></div>
                            </div>
                        </div>
                    </div>
                </div>
            </a>`;
        });
    }
    else if (head == "ghjk") {
        msg = `<iframe width="352" height="198" src="${msg}" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>`;
    }
    else {
    }
    let str = `<div class="d-flex flex-row justify-content-end messageboxes">
                <div>
                    <input type="hidden" class="msgbodytimestamp" value="${time}" />
                    <div class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" style="max-width:400px;">${msg}</div>
                    <p class="small me-3 mb-3 rounded-3 text-muted"></p>
                </div>
                <img src="${mempic}" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
               </div>`
    return str;


}
function CMessagePack(head, msg, time, multiid = "0") {
    if (head == "asdf") {
        let src = "/img/emoji/emoji" + msg + ".jpg";
        msg = `<img class="msgdisplayemoji" src="` + src + `" />`;
    }
    else if (head == "zxcv") {
        msg = `<img src="` + msg + `" class="msgdisplayimg" style="max-width: 380px;max-height: 500px;border: 1px solid black;">`;
        
    }
    else if (head == "tyui") {
        getid = msg;
        $.ajaxSetup({
            async: false
        });
        $.getJSON(`/Msgapi/GetProductbyId`, { id: getid }, function (data) {
            if (data == undefined) {
                return;
            }
            msg = `<a href="/Item/Index?id=` + msg + `" style="text-decoration:none">
                <div style="max-width:380px; max-height:600px;background-color:white;">
                    <div style="margin:6px">
                        <div class="sq align-self-center "> <img class="img-fluid align-self-center mr-2 mr-md-4 pl-0 p-0 mb-2" src="data:image;base64,${data.pic}" width="135" height="135" /> </div>
                        <div class="media-body text-right">
                            <div class=""> <h6 class="mb-0">${data.productName}</h6></div>
                            <div class="row flex-column flex-md-row">
                                <div class="col"> <small>規格 : ${data.style}</small></div>
                                <div class="col"> <small>數量 : ${data.quantity}</small></div>
                                <div class="col"><h6 class="mb-0">$${data.unitPrice}</h6></div>
                            </div>
                        </div>
                    </div>
                </div>
            </a>`;

        });
        $.ajaxSetup({
            async: true
        });
    }
    else if (head == "ghjk") {
        msg = `<iframe width="352" height="198" src="${msg}" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>`;
    }
    else {
    }

    let path = "";
    if (multiid == "0") {
        let t = $(`.msgcid[value="${activec}"]`).siblings("a").children(0).children(0).children("img");
        path = t.attr("src");
    }
    else {
        $.getJSON(`/Msgapi/GetmembyId`, { id: multiid }, function (data) {
            if (data == undefined) {
                return;
            }
            path = `data:image;base64,${data.memPic}`;
        });
    }

    let str = `<div class="d-flex flex-row justify-content-start messageboxes">
               <img src="${path}" alt="avatar 1" style="width: 45px; height: 45px; border-radius: 50%;">
                 <div>
                    <input type="hidden" class="msgbodytimestamp" value="${time}" />
                    <div class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #dae0e6;">${msg}</div>
                    <p class="small ms-3 mb-3 rounded-3 text-muted float-end"></p>
                 </div>
               </div>`
    return str;
}

//$(document).on("load", ".messageboxes", function () {
//    $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
//})

$(document).on("load", ".msgdisplayimg", function () {
    $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
})

$(document).on("load", ".msgdisplayemoji", function () {
    $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
})

//$(document).on("change", ".msgdisplayemoji", function () {
//    $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
//})

function CMessageDialog(id, img, msg, time, acc, read = 1, head="qwer") {
    if (head == "asdf") {
        msg = "貼圖";
    }
    else if (head == "zxcv") {
        msg = "照片";
    }
    else if (head == "tyui") {
        msg = "本站產品";
    }
    else if (head == "ghjk") {
        msg = "YT影片";
    }
    else {
    }
    let str = `<li class="p-2 border-bottom msgopendialogli">
                <input type="hidden" class="msgcid" value="${id}" />
                <a href="#!" style="text-decoration:none" class="d-flex justify-content-between msgopendialog">
                    <div class="d-flex flex-row">
                        <div>
                            <img src="data:image;base64,${img}" alt="avatar" class="d-flex align-self-center me-3" style="width: 45px; height: 45px; border-radius: 50%;">
                            <span class="badge bg-success badge-dot"></span>
                        </div>
                        <div class="pt-1">
                            <p class="fw-bold mb-0">${acc}</p>
                            <p class="small text-muted">${msg}</p>
                        </div>
                    </div>
                    <div class="pt-1">
                        <input type="hidden" class="msgbodytimestamp" value="${time}" />
                        <p class="small text-muted mb-1">現在</p>
                        <span class="badge bg-danger rounded-pill float-end">${read}</span>
                    </div>
                    </a>
                </li>`
    return str;
}

//$.ajaxSetup({
//    async: false
//});

$("#msgtextinput").on("keypress", function (e) {
    if (e.which == 13) {
        $("#msgenter").trigger("click");
    }
});

function pageload() {
    if ($("#msgopendialogbody").html == "") {
    }
    else {
        loadactivemsg($(".msgcid").val());
    }
}
if (memacc != null) {
    connectionstart();
    isconnectionstart = true;
    let today = new Date();
    let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    $("#msgopendialogbody").children().eq(0).children("a").children().eq(1).children("input").val(timestamp);
    pageload();
}

$(".chatroom").click(function () {
    if (memacc != null) {
        pageload();
        $("#messagebody").scrollTop($("#messagebody").prop("scrollHeight"));
    }
})

$("#msgautoComplete").on("input",async () => {
    $("#msgautoCompletebox").css("display", "block");
    let htmlDatas;
    $.getJSON(`/Msgapi/AutoComplete`, { keyword: $("#msgautoComplete").val() }, function (data) {
        htmlDatas = data.map(data => {
            if (data != "admin" && data != truememacc && data != "chatbot") {
                return (
                    `<button type="button" onclick="read(event)" class="list-group-item list-group-item-action">${data}</button>`
                );
            }
        })
        $("#msgautoCompletebox").html(htmlDatas.join(""));
    });
    
})

function read(evt) {
    $("#msgautoComplete").val(evt.target.textContent);
    $("#msgautoCompletebox").css("display", "none");
}

$("#msgsearch").click(function () {
    if ($("#msgautoComplete").val() == "") {
        return;
    }
    opencertaindialog($("#msgautoComplete").val());
})

function opencertaindialog(acc) {
    let today = new Date();
    let timestamp = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    $.getJSON(`/Msgapi/GetmembyAcc`, { acc: acc }, function (data) {
        if (data == undefined) {
            return;
        }
        if (data.memberId == memacc) {
            return;
        }
        if (data.memberId == 1) {
            data.memberAcc = "線上客服";
        }
        else if (data.memberId == 8) {
            data.memberAcc = "智慧客服";
        }
        if ($(`.msgcid[value="${data.memberId}"]`).length != 0) {
            $(`.msgcid[value="${data.memberId}"]`).siblings().trigger("click");
        }
        else {
            $("#msgopendialogbody").prepend(CMessageDialog(data.memberId, data.memPic, "", timestamp, data.memberAcc, ""));
            $("#msgopendialogbody").children().eq(0).children("a").trigger("click");
        }
    });
}

function chatbotreply(header, msgbody) {
    let bothead = "qwer";
    let today = new Date();
    let bottime = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    let botmsg;
    if (header == "asdf") {
        bothead = "asdf";
        botmsg = msgbody;
    }
    else if (header == "zxcv") {
        botmsg = "很酷的照片哦";
    }
    else if (header == "tyui") {
        botmsg = "我上次買過，很棒哦快點買"
    }
    else if (header == "ghjk") {
        botmsg = "很酷的影片哦";
    }
    else {
        if (msgbody.includes("退貨")) {
            botmsg = `您可以在會員中心的購買清單中點擊「申請退貨」，</br > 來進行退貨流程。`;
        }
        else if (msgbody.includes("買東西") || msgbody.includes("購買")) {
            botmsg = `瀏覽商品頁面後點擊加入購物車，</br >並在購物車頁面確認無誤後按下結帳即可。`;
        }
        else {
            botmsg = "我看不懂你在說甚麼，請換個說法試試看。";
        }
    }
    let message = bothead + bottime + botmsg;
    setTimeout(function () {
        connection.invoke("SendMessage", chatbotid, message, memacc).catch(function (err) {
            return console.error(err.toString());
        });
        dialogsort(activec);
    },1500);
    
}