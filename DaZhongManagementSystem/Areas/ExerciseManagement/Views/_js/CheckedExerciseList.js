var selector = {
    $grid: function () { return $("#ExerciseList") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnDelete: function () { return $("#btnDelete") },

    //查询
    $exerciseName_Search: function () { return $("#ExercisesName_Search") },
    $exerciseStstus_Search: function () { return $("#ExerciseStatus_Search") },
    $inputType_Search: function () { return $("#InputType_Search") },
    $effectiveDate_Search: function () { return $("#EffectiveDate_Search") },
    $createdTimeStart_Search: function () { return $("#CreatedTimeStart_Search") },
    $createTimeEnd_Search: function () { return $("#CreateTimeEnd_Search") },

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

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$exerciseName_Search().val("");
        selector.$inputType_Search().val("");
        selector.$effectiveDate_Search().val("");
        selector.$createdTimeStart_Search().val("");
        selector.$createTimeEnd_Search().val("");
    });

    //生效时间控件
    selector.$effectiveDate_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });

    //创建起始时间
    selector.$createdTimeStart_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true,
        endDate: new Date()
    }).on('changeDate', function (e) {
        var startTime = e.date;
        selector.$createTimeEnd_Search().datetimepicker('setStartDate', startTime);
    });

    //创建截止时间
    selector.$createTimeEnd_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true,
        endDate: new Date()
    }).on('changeDate', function (e) {
        var startTime = e.date;
        selector.$createdTimeStart_Search().datetimepicker('setEndDate', startTime);
    });;
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.Vguid);
            }
        })
        if (selection.length < 1) {
            jqxNotification("请选择您要删除的习题！", null, "error");
        } else {
            WindowConfirmDialog(deleted, "您确定要删除选中的习题？", "确认框", "确定", "取消");
        }
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
            data: {
                "ExercisesName": selector.$exerciseName_Search().val().trim().trim(),
                "Status": selector.$exerciseStstus_Search().val(),
                "InputType": selector.$inputType_Search().val(),
                "EffectiveDate": selector.$effectiveDate_Search().val(),
                "CreatedTimeStart": selector.$createdTimeStart_Search().val(),
                "CreatedTimeEnd": selector.$createTimeEnd_Search().val()
            },
            url: "/ExerciseManagement/CheckedExercise/GetExerciseListBySearch"    //获取数据源的路径
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
                    { text: '习题名称', width: 350, datafield: 'ExercisesName', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                    { text: '习题有效日期', width: 180, datafield: 'EffectiveDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                    { text: '习题状态', width: 150, datafield: 'TranslateStatus', align: 'center', cellsAlign: 'center' },
                    { text: '习题录入类型', width: 150, datafield: 'TranslateInputType', align: 'center', cellsAlign: 'center' },
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
        container = "<a href='CheckedDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.ExercisesName + "</a>";
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

//删除
function deleted() {
    showLoading();//显示加载等待框
    selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.Vguid);
        }
    });
    $.ajax({
        url: "/ExerciseManagement/CheckedExercise/DeletedExercise",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("删除失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("删除成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}


$(function () {
    var page = new $page();
    page.init();

})


