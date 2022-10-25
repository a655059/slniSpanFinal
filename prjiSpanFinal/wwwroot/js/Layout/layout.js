let mymemid = $("#msgmemid").val();
let header2exist = $("#header2").length != 0;


//回到頂端
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

//H2自動符合高度
if (header2exist) {
    const h1 = $("#header1");
    const h2 = document.getElementById("header2");
    sizeble();
    addEventListener('resize', async () =>
        await sizeble());
    function sizeble() {
        h2.style.marginTop = (h1.height() + 16) + "px";
    }
}



//搜尋
let searchInput = document.querySelector("#SearchInputTxT");
let searchBtn = document.querySelector("#SearchInputbtn");

if (header2exist) {
    searchInput.addEventListener("input", () => {
        let searchkeyword = searchInput.value;
        let url = `/Category/SearchResult/?keyword=${searchkeyword}`;
        searchBtn.setAttribute("href", url);
    })

    searchInput.addEventListener("keypress", event => {
        if (event.key === "Enter") {
            event.preventDefault();
            searchBtn.click();
        }
    })
}

//快速熱搜

let reqQty = 5;
TakePopItem();

function TakePopItem() {
    $.post(`/Home/TakePopItem`, { reqQty: reqQty }, function (data) {
        console.log(data);
        $("#searchkeyspace").html("");
        if (data.length > 0) {
            $("#searchkeyspace").append(SearchKeyWord(data))
        }
    })
}
function SearchKeyWord(data) {
    res = "";
    if (data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            res += `<a class="linknoline searchkeyword" href="/Category/SearchResult/?keyword=${data[i].productName}">${data[i].productName}</a>`
        }
    }
    return res;
}


//notification
function ticktotimestamp(today) {
    
    tms = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    console.log(tms)
    return tms;
}


if (mymemid == null) {
    $(".notification").css("display", "none");
}
else {
    function happy_sendnoti(type, sendtomemberid ,text, link) {
        $.getJSON("/MsgApi/SendNoti", { type: type, id: sendtomemberid, text:text,link:link }, function (data) {

        })
    }


    $("#notificationlistbox").html("");
    $.getJSON("/MsgApi/GetNotificationbyID", { id: mymemid }, function (data) {
        let hrcount = 0;
        console.log(data);
        for (let i = 0; i < data.length; i++) {
            if (data[i].haveRead == false) {
                hrcount++;
            }
            $("#notificationlistbox").append(`<li class="notification-item-listitem">
                                        <a href="${data[i].link}" class="d-flex justify-content-between linknoline">
                                            <div class="d-flex flex-row">
                                                <div>
                                                    <img src="data:image;base64,${data[i].iconPic}" alt="avatar" class="d-flex align-self-center me-3" style="width: 45px; height: 45px; border-radius: 50%;">
                                                    <span class="badge bg-success badge-dot"></span>
                                                </div>
                                                <div class="pt-1">
                                                    <p class="mb-0">${data[i].text}</p>
                                                </div>
                                            </div>
                                            <div class="pt-1">
                                                <input type="hidden" class="msgbodytimestamp" value="${ticktotimestamp(new Date(data[i].time))}"/>
                                                <p class="small text-muted mb-1"></p>
                                            </div>
                                        </a>
                                    </li>`);
        }
        if (hrcount != 0) {
            $(".notificationbell").append(`<div class="qty notificationqty">${hrcount}</div>`);
        }
        else {
            
        }
        
        refreshtimestamp();
    });

    $(".notificationbell").click(function () {
        $.getJSON("/MsgApi/HaveReadAllNoti", { id: mymemid }, function (data) {
        });
        $(".notificationqty").remove();
    })
}
