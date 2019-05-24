var selector = {

    $txtForm: function () { return $("#txtForm") },
    $txtTo: function () { return $("#txtTo") },
    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnExport: function () { return $("#btnExport") }
};

var $page = function () {

    this.init = function () {
        addEvent();
    }

    function addEvent() {

        selector.$btnSearch().click(function () {
            var startDate = selector.$txtForm().val();
            var endDate = selector.$txtTo().val();
            window.location.href = "/ReportManagement/RideCheckFeedbackReport/SelectionRatioReport?startDate=" + startDate + "&endDate=" + endDate;
        });

        selector.$btnExport().click(function () {
            var startDate = selector.$txtForm().val();
            var endDate = selector.$txtTo().val();
            window.location.href = "/ReportManagement/RideCheckFeedbackReport/ExportSelectionRatioReport?startDate=" + startDate + "&endDate=" + endDate;
        });
        //生效时间控件
        selector.$txtForm().datetimepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            language: 'zh-CN',
            //startView: 'month',
            minView: 'month',
            maxView: 'decade',
            orientation: "bottom right",
            showMeridian: true
        });

        //生效时间控件
        selector.$txtTo().datetimepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            language: 'zh-CN',
            //startView: 'month',
            minView: 'month',
            maxView: 'decade',
            orientation: "bottom right",
            showMeridian: true,
        });
    }
}


$(function () {
    var page = new $page();
    page.init();

});