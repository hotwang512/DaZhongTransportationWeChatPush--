var selector = {
    $grid: function () { return $("#jqxTable") },
    $ExercisesName_Search: function () { return $("#ExercisesName_Search") },
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
        //loadData();
    }

    function addEvent() {
        //生效时间控件
        selector.$txtForm().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            language: 'zh-CN',
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });

        //生效时间控件
        selector.$txtTo().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            language: 'zh-CN',
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true,
        });
        selector.$btnSearch().click(function () {
            loadData();
        });

        selector.$btnExport().click(function () {
            var vguid = selector.$ExercisesName_Search().val();
            $.post("/ReportManagement/PsychologicalEvaluation/ExportPsychologicalEvaluationSource", { vguid: vguid, start: selector.$txtForm().val(), end: selector.$txtTo().val() }, function (date) {
                window.location.href = "/Temp/" + date;
            });
            //window.location.href = "/ReportManagement/PsychologicalEvaluation/ExportPsychologicalEvaluationSource?vguid=" + vguid + "&start=" + selector.$txtForm().val() + "&end=" + selector.$txtTo().val();
        });
    }
    function loadData() {
        var PersionSelectionRatioReport =
            {
                datafields:
                [
                    { name: "VGUID", type: 'string' },
                    { name: "Name", type: 'string' },
                    { name: 'ChangeDate', type: 'string' },
                    { name: 'ptScore', type: 'string' },
                    { name: 'phqScore', type: 'string' },
                    { name: 'ColorBlock', type: 'string' },
                    { name: 'Result', type: 'string' },
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { "vguid": selector.$ExercisesName_Search().val(), "start": selector.$txtForm().val(), "end": selector.$txtTo().val(), },
                url: "/ReportManagement/PsychologicalEvaluation/GetPsychologicalEvaluationSource"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(PersionSelectionRatioReport, {
            downloadComplete: function (data) {
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                width: "100%",
                height: 480,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { text: '人员姓名', width: 150, datafield: 'Name', align: 'center', cellsAlign: 'center' },
                  { text: '日期时间', width: 200, datafield: 'ChangeDate', align: 'center', cellsAlign: 'center' },
                  { text: 'PT成绩', width: 80, datafield: 'ptScore', align: 'center', cellsAlign: 'center' },// 
                  { text: 'PHQ成绩', width: 80, datafield: 'phqScore', align: 'center', cellsAlign: 'center' },
                  { text: '区块', width: 80, datafield: 'ColorBlock', align: 'center', cellsAlign: 'center', cellsRenderer: genderColorBlock },
                  { text: '结果', datafield: 'Result', align: 'left', cellsAlign: 'left' }
                ]
            });
    }
    function genderColorBlock(row, column, value, rowData) {
        return "<div style='height: 28px;width: 100%;margin-top: -10px;margin-bottom: -10px;background-color:" + value + "'>";
    }

}


$(function () {
    var page = new $page();
    page.init();

});