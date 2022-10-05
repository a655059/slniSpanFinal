$(".selectAll").change(function () {
    if ($(this).prop("checked")) {
        $(this).closest("div").siblings().find(".selectItem").prop("checked", true);
    }
    else {
        $(this).closest("div").siblings().find(".selectItem").prop("checked", false);
    }
});

function() {

}