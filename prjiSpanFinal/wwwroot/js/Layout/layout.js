let mymemid = $("#msgmemid").val();
let header2exist = $("#header2").length != 0;

$(document).ready(function () {
    if (header2exist) {
        getShoppingCart();
        GetSearchDetail();
    }
})


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
    loadHeight();
    //sizeble();
    addEventListener('resize', async () =>
        await sizeble());
    function sizeble() {
        h2.style.marginTop = (h1.height()+15) + "px";
    }
    function loadHeight() {
        h2.style.marginTop = 53 + "px";
    }
}

//ShoppingCart
function getShoppingCart() {
    $.post("/Home/ShoppingCartStockDisplay", function (data) {
        if (data == 0) {
            $("#ShoppingCartIcon").html("");
        }
        else {
            res = `<span class="qty" id="itemCountInCart">${data}</span>`;
            $("#ShoppingCartIcon").html(res);
        }
    })
}

//搜尋
let searchkeyword = "";

if (header2exist) {
    let searchInput = document.querySelector("#SearchInputTxT");
    let searchBtn = document.querySelector("#SearchInputbtn");
    
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
    searchBtn.addEventListener("click", () => {
        searchkeyword = searchInput.value;
        GetSearchDetail();
    })

    $(document).click(function (e) {
        var inputFocused = $("#SearchInputTxT").is(':focus');
        var clickedInsideAcDiv = $(e.target).closest('#LayoutSearchSugBox').length > 0;
        if (inputFocused || clickedInsideAcDiv) {
            $('#LayoutSearchSugBox').show();
        }
        if (!inputFocused && !clickedInsideAcDiv) {
            $('#LayoutSearchSugBox').hide();
        }
    });

    $("#LayoutSearchSugBox").on("click", ".LayoutSearchresult", function (evt) {
        let Textvalue = $(this).text();
        location.href=`/Category/SearchResult/?keyword=${Textvalue}`
    })
}
        //搜尋紀錄


function GetSearchDetail() {
    $.post(`/Home/GetSearchDetail`, { key: searchkeyword }, function (data) {

        if (data.length > 0) {
            searchAutoComplete = data;
            $("#LayoutSearchSugBox").html("");
            let SugsData = "";
            for (let i = 0; i < data.length; i++) {
                SugsData += `<li class="LayoutSearchresult"><i class="fa-regular fa-clock"></i>${data[i]}</li>`;
            }
            $("#LayoutSearchSugBox").html(SugsData);
        }

    })
}

//notification
function ticktotimestamp(today) {
    
    tms = today.getHours().toString().padStart(2, '0') + today.getMinutes().toString().padStart(2, '0') + today.getSeconds().toString().padStart(2, '0') + today.getMilliseconds().toString().padStart(3, '0') + today.getFullYear().toString() + (today.getMonth() + 1).toString().padStart(2, '0') + today.getDate().toString().padStart(2, '0');
    return tms;
}


if (mymemid == null) {
    $(".notification").css("display", "none");
}
else {
    function happy_sendnoti(type, sendtomemberid, text, link, content = "") {
        $.getJSON("/MsgApi/SendNoti", { type: type, id: sendtomemberid, text:text,link:link, content:content }, function (data) {

        })
    }


    $("#notificationlistbox").html("");
    $.getJSON("/MsgApi/GetNotificationbyID", { id: mymemid }, function (data) {
        let hrcount = 0;
        for (let i = 0; i < data.length; i++) {
            if (data[i].haveRead == false) {
                hrcount++;
            }
            $("#notificationlistbox").append(`<div class="notification-item-listitem">
                                        <a href="${data[i].link}" class="d-flex flex-wrap justify-content-end linknoline notification-link">
                                            <div class="d-flex flex-row w-100">
                                                <div style="float:left;">
                                                    <img src="data:image;base64,${data[i].iconPic}" alt="avatar" class="d-flex align-self-center me-3">
                                                    <span class="badge bg-success badge-dot"></span>
                                                </div>
                                                <div>
                                                        <div class="pt-1 w-100">
                                                            <p class="mb-0">${data[i].text}</p>
                                                        </div>
                                                        <div class="pt-1 w-100">
                                                    <p class="mb-0 small">${data[i].textContent}</p>
                                                        </div>
                                                </div>
                                            </div>
                                            <div class="pt-1">
                                                <input type="hidden" class="msgbodytimestamp" value="${ticktotimestamp(new Date(data[i].time))}"/>
                                                <p class="small text-muted mb-1"></p>
                                            </div>
                                        </a>
                                    </div>`);
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

$(".searchcart-Cart1").click(async function () {
    const response = await fetch("/Delivery/ShowcartCondition");
    const data = await response.text();
    if (data == "0") {
        Swal.fire({
            title: "請先登入再查看購物車",
            showDenyButton: false,
            showCancelButton: true,
            confirmButtonText: "登入",
        }).then((result) => {
            if (result.isConfirmed) {
                location.href = "/Member/Login";
            }
        })
    }
    else if (data == "2") {
        Swal.fire("購物車裡沒有東西喔!", "快去購物吧!")
    }
    else {
        location.href = "/Delivery/ShowCart";
    }
});
