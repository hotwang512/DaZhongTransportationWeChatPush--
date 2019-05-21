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
        //生效时间控件
        selector.$txtForm().datetimepicker({
            format: "yyyy-mm",
            autoclose: true,
            language: 'zh-CN',
            startView: 'year',
            minView: 'year',
            maxView: 'decade',
            orientation: "bottom right",
            showMeridian: true
        });

        //生效时间控件
        selector.$txtTo().datetimepicker({
            format: "yyyy-mm",
            autoclose: true,
            language: 'zh-CN',
            startView: 'year',
            minView: 'year',
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