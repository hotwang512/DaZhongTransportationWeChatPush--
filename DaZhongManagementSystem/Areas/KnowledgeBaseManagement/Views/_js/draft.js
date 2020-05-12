var selector = {
    $grid: function () { return $("#knowledgeList") },
    $form: function () { return $("#UpLoadKnowledgeForm") },
    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnImport: function () { return $("#btnImport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnChecked: function () { return $("#btnChecked") },
    $btnDownload: function () { return $("#btnDownload") },//下载模板
    $importFile: function () { return $("#importFile") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $txtTitle: function () { return $("#txtTitle") },
    $txtRemark: function () { return $("#txtRemark") },
    $drdType: function () { return $("#drdType") },
    $txtCreateDate: function () { return $("#txtCreateDate") },
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
        initTable();

    }; //addEvent end

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        initTable();
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$txtTitle().val("");
        selector.$txtRemark().val("");
        selector.$drdType().val("");
        selector.$txtCreateDate().val("");
    });

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        window.location.href = "/KnowledgeBaseManagement/Draft/DraftDetail?isEdit=false";
    });

    //下载模板按钮事件
    selector.$btnDownload().on('click', function () {
        window.location.href = "/KnowledgeBaseManagement/Draft/DownLoadTemplate";
    });

    //导入习题按钮事件
    selector.$btnImport().on('click', function () {
        selector.$importFile().click();
    });
    //生效时间控件
    selector.$txtCreateDate().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //审核按钮事件
    selector.$btnChecked().on('click', function () {
        var selection = [];
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
        if (selection.length < 1) {
            jqxNotification("请选择您要提交的知识！", null, "error");
        } else {
            WindowConfirmDialog(checked, "您确定要提交选中的知识？", "确认框", "确定", "取消");
        }
    });

    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        var selection = [];
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
        if (selection.length < 1) {
            jqxNotification("请选择您要删除的知识！", null, "error");
        } else {
            WindowConfirmDialog(deleted, "您确定要删除选中的知识？", "确认框", "确定", "取消");
        }
    });

    //加载表格
    function initTable() {
        var source =
        {
            datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'Title', type: 'string' },
                    { name: 'Type', type: 'string' },
                    { name: 'Content', type: 'string' },
                    { name: 'TranslateType', type: 'string' },
                    { name: 'Status', type: 'string' },
                    { name: 'TranslateStatus', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'CreatedUser', type: 'date' },
                    { name: 'Remark', type: 'date' },
                    { name: 'Vguid', type: 'string' }
                ],
            datatype: "json",
            id: "VGUID",//主键
            async: true,
            data: {
                "Title": selector.$txtTitle().val().trim(),
                "Remark": selector.$txtRemark().val().trim(),
                "Type": selector.$drdType().val(),
                "CreatedDate": selector.$txtCreateDate().val()
            },
            url: "/KnowledgeBaseManagement/Draft/GetKnowledgeListBySearch"    //获取数据源的路径
        };
        var typeAdapter = new $.jqx.dataAdapter(source, {
            downloadComplete: function (data) {
                source.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: true,
                width: "100%",
                height: 438,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                    { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                    { text: '名称', width: 350, datafield: 'Title', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                    //  { text: '内容', datafield: 'Content', align: 'center', cellsAlign: 'center' },             
                    { text: '录入类型', width: 250, datafield: 'TranslateType', align: 'center', cellsAlign: 'center' },
                    { text: '创建人', width: 250, datafield: 'CreatedUser', align: 'center', cellsAlign: 'center' },
                    { text: '创建日期', width: 280, datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                    { text: '备注', datafield: 'Remark', align: 'center', cellsAlign: 'center' },
                    { text: '状态', width: 150, datafield: 'TranslateStatus', align: 'center', cellsAlign: 'center' },
                    { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });
    }
    //跳转编辑页面
    function detailFunc(row, column, value, rowData) {
        var container = "";
        if (rowData.Content.split(' ')[1] == "4") {
            container = "<a href='/ExerciseManagement/CheckedExercise/CheckedDetail?Vguid=" + rowData.Content.split(' ')[0] + "&isEdit=true&Type=4' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
        } else {
            container = "<a href='DraftDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
        }
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
        element.on('change', function () {
            var checked = element.jqxCheckBox('checked');

            if (checked) {
                var rows = grid.jqxDataTable('getRows');
                for (var i = 0; i < rows.length; i++) {
                    grid.jqxDataTable('selectRow', i);
                    grid.find(".jqx_datatable_checkbox").attr("checked", "checked");
                }
            } else {
                grid.jqxDataTable('clearSelection');
                grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked");
            }
        });
        return true;
    }

};

//审核
function checked() {
    showLoading();//显示加载等待框
    var selection = [];
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
        url: "/KnowledgeBaseManagement/Draft/SubmitKnowledgeBase",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("提交失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("提交成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

//删除
function deleted() {
    showLoading();//显示加载等待框
    var selection = [];
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
        url: "/KnowledgeBaseManagement/Draft/DeleteKnowledgeBase",
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
function fileUpload() {
    selector.$form().ajaxSubmit({
        url: "/KnowledgeBaseManagement/Draft/UpLoadKnowledge?knowledgeFile=importFile",
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("导入失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("导入成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
                case "2":
                    jqxNotification("表格为空！", null, "error");
                    break;
                case "3":
                    jqxNotification("模板错误！", null, "error");
                    break;
                default:
                    jqxNotification(msg.respnseInfo, null, "error");
                    break;
            }
            $("#importFile").remove();
            var inputObj = "<input type=\"file\" name=\"importFile\" id=\"importFile\" onchange=\"fileUpload()\" accept=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel\" style=\"display: none\" />";
            selector.$btnImport().after(inputObj);
        }
    });
}


$(function () {
    var page = new $page();
    page.init();

});


