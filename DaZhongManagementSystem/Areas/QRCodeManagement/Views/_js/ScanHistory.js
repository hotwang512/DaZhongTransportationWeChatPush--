var selector = {
    $grid: function () { return $("#scanHistoryList") },
    //按钮
    $btnDelete: function () { return $("#btnDelete") },
    $btnExport: function () { return $("#btnExport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $txtCreatedDateFrom: function () { return $("#txtCreatedDateFrom") },
    $txtCreatedDateTo: function () { return $("#txtCreatedDateTo") },
    $txtMachineCode: function () { return $("#txtMachineCode") },
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
            jqxNotification("请选择您要删除的扫码历史！", null, "error");
        } else {
            WindowConfirmDialog(deleted, "您确定要删除选中的扫码历史？", "确认框", "确定", "取消");
        }
    });
    //导出按钮事件
    selector.$btnExport().on('click', function () {
        var data = { "CreatedDateFrom": selector.$txtCreatedDateFrom().val(), "CreatedDateTo": selector.$txtCreatedDateTo().val(), "MachineCode": selector.$txtMachineCode().val() };
        window.location.href = "/QRCodeManagement/ScanHistory/Export?searchParams=" + JSON.stringify(data);
    });
    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$txtCreatedDateFrom().val("");
        selector.$txtCreatedDateTo().val("");
        selector.$txtMachineCode().val("");
    });

    //生效时间控件
    selector.$txtCreatedDateFrom().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    //生效时间控件
    selector.$txtCreatedDateTo().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });

    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //加载表格
    function initTable() {
        var source =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'MachineCode', type: 'string' },
                    { name: 'SystemID', type: 'string' },
                    { name: 'ScanUser', type: 'string' },
                    { name: 'Data', type: 'string' },
                    { name: 'User', type: 'string' },
                    { name: 'ScanTime', type: 'date' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { "CreatedDateFrom": selector.$txtCreatedDateFrom().val(), "CreatedDateTo": selector.$txtCreatedDateTo().val(), "MachineCode": selector.$txtMachineCode().val() },
                url: "/QRCodeManagement/ScanHistory/GetScanHistoryListBySearch"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(source, {
            downloadComplete: function (data) {
                source.totalrecords = data.TotalRows;
            }
        });
        //信息列表
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
                  { text: '扫码机器码', width: 350, datafield: 'MachineCode', align: 'center', cellsAlign: 'center' }, // ,cellsRenderer: detailFunc 
                  { text: '系统编号', width: 200, datafield: 'SystemID', align: 'center', cellsAlign: 'center' },
                  { text: '扫码人员', width: 200, datafield: 'ScanUser', align: 'center', cellsAlign: 'center' },
                  { text: '扫码数据', datafield: 'Data', align: 'center', cellsAlign: 'center' },
                  { text: '扫码数据用户', width: 200, datafield: 'User', align: 'center', cellsAlign: 'center' },
                //  { text: '年龄', width: 180, datafield: 'Age', align: 'center', cellsAlign: 'center' },
                  { text: '扫码日期', width: 200, datafield: 'ScanTime', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", },
                //  { text: '状态', width: 150, datafield: 'TranslateStatus', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });
    }

    ////跳转编辑页面
    //function detailFunc(row, column, value, rowData) {
    //    var container = "";
    //    container = "<a href='DraftDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
    //    return container;
    //}
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
        url: "/QRCodeManagement/ScanHistory/DeletedScanHistory",
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

});


