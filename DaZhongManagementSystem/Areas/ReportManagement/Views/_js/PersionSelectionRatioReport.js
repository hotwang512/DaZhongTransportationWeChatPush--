var selector = {
    $grid: function () { return $("#jqxTable") },
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
        loadData();
    }

    function addEvent() {

        selector.$btnSearch().click(function () {
            loadData();
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
    function loadData() {
        var PersionSelectionRatioReport =
            {
                datafields:
                [
                    { name: "VGUID", type: 'string' },
                    { name: "IDNumber", type: 'string' },
                    { name: 'Name', type: 'string' },
                    { name: 'Sex', type: 'string' },
                    { name: 'PhoneNumber', type: 'PhoneNumber' },
                    { name: 'Counts', type: 'Counts' },
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { "startDate": selector.$txtForm().val(), "endDate": selector.$txtTo().val() },
                url: "/ReportManagement/RideCheckFeedbackReport/getPersionSelectionRatioReport"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(PersionSelectionRatioReport, {
            downloadComplete: function (data) {
                //UserInfoListSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                //pageable: false,
                width: "100%",
                height: 480,
                //pageSize: 10,
                //serverProcessing: true,
                //pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { text: '人员姓名', width: 150, datafield: 'Name', align: 'center', cellsAlign: 'center' },
                  { text: '身份证号', width: 200, datafield: 'IDNumber', align: 'center', cellsAlign: 'center' },
                  { text: '性别', width: 80, datafield: 'Sex', align: 'center', cellsAlign: 'center' },//, cellsRenderer: genderTranslate 
                  { text: '手机号码', datafield: 'PhoneNumber', align: 'center', cellsAlign: 'center' },
                  { text: '跳车单数量', datafield: 'Counts', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'vguid', hidden: true }
                ]
            });

    }

}


$(function () {
    var page = new $page();
    page.init();

});