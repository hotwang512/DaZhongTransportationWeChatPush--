var selector = {
    $grid: function () { return $("#ExerciseList") },

    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnImport: function () { return $("#btnImport") },
    $btnSearch: function () { return $("#btnSearch") },

    //查询
    $exerciseName_Search: function () { return $("#ExercisesName_Search") },
    $exerciseStstus_Search: function () { return $("#ExerciseStatus_Search") },
    $inputType_Search: function () { return $("#InputType_Search") },
    $effectiveDate_Search: function () { return $("#EffectiveDate_Search") },

    //页面-角色权限
    $ReadsPermission: function () { return $("#ReadsPermission") },
    $EditPermission: function () { return $("#EditPermission") },
    $DeletesPermission: function () { return $("#DeletesPermission") },
    $AddsPermission: function () { return $("#AddsPermission") },
    $SubmitPermission: function () { return $("#SubmitPermission") },
    $ApprovedPermission: function () { return $("#ApprovedPermission") },
    $ImportPermission: function () { return $("#ImportPermission") },
    $ExportPermission: function () { return $("#ExportPermission") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        LoadTable();

    }; //addEvent end

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        LoadTable();
    });

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        window.location.href = "/BasicDataManagement/ExerciseManagement/ExerciseDetail?isEdit=false";
    });

    //生效时间控件
    selector.$effectiveDate_Search().datepicker({
        format: "yyyy-mm-dd",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",

    });

    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'ExercisesName', type: 'string' },
                    { name: 'EffectiveDate', type: 'date' },
                    { name: 'Status', type: 'string' },
                    { name: 'TranslateStatus', type: 'string' },
                    { name: 'InputType', type: 'string' },
                    { name: 'TranslateInputType', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'Remarks', type: 'string' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { "ExercisesName": selector.$exerciseName_Search().val(), "Status": selector.$exerciseStstus_Search().val(), "InputType": selector.$inputType_Search().val(), "EffectiveDate": selector.$effectiveDate_Search().val() },
                url: "/BasicDataManagement/ExerciseManagement/GetExerciseListBySearch"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(UserInfoListSource, {
            downloadComplete: function (data) {
                UserInfoListSource.totalrecords = data.TotalRows;
            }
        });
        var tool = {

        }; // tool end       
        var index = 0;
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: true,
                width: "100%",
                height: 450,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '习题名称', datafield: 'ExercisesName', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  { text: '习题有效日期', datafield: 'EffectiveDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd" },
                  { text: '习题状态', datafield: 'TranslateStatus', align: 'center', cellsAlign: 'center' },
                  { text: '习题录入类型', datafield: 'TranslateInputType', align: 'center', cellsAlign: 'center' },
                  { text: '描述', datafield: 'Description', align: 'center', cellsAlign: 'center' },
                  { text: '备注', datafield: 'Remarks', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });
        index = 0;
    }

    //跳转编辑页面
    function detailFunc(row, column, value, rowData) {
        var container = "";        
        container = "<a href='ExerciseDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.ExercisesName + "</a>";
        return container;
    }
    function cellsRendererFunc(row, column, value, rowData) {
        return "<input class=\"jqx_datatable_checkbox\" index=\"" + row + "\" type=\"checkbox\"  style=\"margin:auto;width: 17px;height: 17px;\" />";
    }

    function rendererFunc() {
        var checkBox = "<div id='jqx_datatable_checkbox_all' class='jqx_datatable_checkbox_all' style='z-index: 999; margin-left:7px ;margin-top: 7px;'>";
        checkBox += "</div>";
        return checkBox;
    }

    function renderedFunc(element) {
        var grid = selector.$grid();
        element.jqxCheckBox();
        element.on('change', function (event) {
            var checked = element.jqxCheckBox('checked');

            if (checked) {
                var rows = grid.jqxDataTable('getRows');
                for (var i = 0; i < rows.length; i++) {
                    grid.jqxDataTable('selectRow', i);
                    grid.find(".jqx_datatable_checkbox").attr("checked", "checked")
                }
            } else {
                grid.jqxDataTable('clearSelection');
                grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked")
            }
        });
        return true;
    }

};


$(function () {
    var page = new $page();
    page.init();

})


