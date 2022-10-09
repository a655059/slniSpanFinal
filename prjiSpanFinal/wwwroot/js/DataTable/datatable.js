let today = new Date();
let startdatestring = (today.getFullYear() - 1) + "-" + (today.getMonth() + 1).toString().padStart(2, '0') + "-" + today.getDate().toString().padStart(2, '0');
let enddatestring = today.getFullYear() + "-" + (today.getMonth() + 1).toString().padStart(2, '0') + "-" + today.getDate().toString().padStart(2, '0');
$("#startDate").val(startdatestring);
$("#endDate").val(enddatestring);

function sortctrl(event) {
    if (!$(event.target).hasClass("sorting_asc")) {
        $(event.target).removeClass("sorting_desc");
        $(event.target).addClass("sorting_asc");
        $(event.target).siblings().removeClass("sorting_desc").removeClass("sorting_asc");
    }
    else {
        $(event.target).removeClass("sorting_asc");
        $(event.target).addClass("sorting_desc");
        $(event.target).siblings().removeClass("sorting_desc").removeClass("sorting_asc");
    }
}

$("#orderid").click(sortctrl);
$("#ordertime").click(sortctrl);
$("#ordersales").click(sortctrl);