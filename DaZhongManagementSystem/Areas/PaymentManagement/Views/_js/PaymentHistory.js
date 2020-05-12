var selector = {
    //表格
    $grid: function () { return $("#datatable") },
    //查询条件
    $txtName: function () { return $("#txtName") },
    $txtMobilePhone: function () { return $("#txtMobilePhone") },
    $txtUserIDNo: function () { return $("#txtUserIDNo") },
    $txtJobNumber: function () { return $("#txtJobNumber") },
    $txtStatus: function () { return $("#txtStatus") },
    $txtTransactionId: function () { return $("#txtTransactionId") },
    $txtPaymentForm: function () { return $("#txtPaymentForm") },
    $txtPaymentTo: function () { return $("#txtPaymentTo") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnInsert: function () { return $("#btnInsert") },
    $btnExport: function () { return $("#btnExport") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        initTable();

        //查询
        selector.$btnSearch().on("click", function () {
            initTable();
        });

        //重置
        selector.$btnReset().on("click", function () {
            selector.$txtName().val("");
            selector.$txtMobilePhone().val("");
            selector.$txtUserIDNo().val("");
            selector.$txtJobNumber().val("");
            selector.$txtStatus().val("1"),
            selector.$txtTransactionId().val("");
            selector.$txtPaymentForm().val("");
            selector.$txtPaymentTo().val("");
        });

        //生效时间控件
        selector.$txtPaymentForm().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });
        //生效时间控件
        selector.$txtPaymentTo().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });
        $(".glyphicon-arrow-left").text("<<");
        $(".glyphicon-arrow-right").text(">>");

        selector.$btnInsert().on('click', function () {
            var selection = [];
            var revenueStatus = 0;
            var grid = selector.$grid();
            var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
            checedBoxs.each(function () {
                var th = $(this);
                if (th.is(":checked")) {
                    var index = th.attr("index");
                    var data = grid.jqxDataTable('getRows')[index];
                    selection.push(data.RevenueStatus);
                }
            });
            if (selection.length == 0) {
                jqxNotification("请选择要插入的数据！", null, "error");
                return false;
            }
            for (var i = 0; i < selection.length; i++) {
                if (selection[i] == "已匹配") {
                    revenueStatus++;
                }
            }
            if (revenueStatus == 0) {
                WindowConfirmDialog(approved, "您确定要插入选中的数据？", "确认框", "确定", "取消");
            } else {
                jqxNotification("所选择的数据营收状态不正确!", null, "error");
            }

        });

        //删除
        selector.$btnDelete().on("click", function () {
            var selection = [];
            var grid = selector.$grid();
            var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
            checedBoxs.each(function () {
                var th = $(this);
                if (th.is(":checked")) {
                    var index = th.attr("index");
                    var data = grid.jqxDataTable('getRows')[index];
                    selection.push(data.VGUID);
                }
            });
            if (selection.length < 1) {
                jqxNotification("请选择您要删除的数据！", null, "error");
            } else {
                WindowConfirmDialog(deleted, "您确定要删除选中的数据？", "确认框", "确定", "取消");
            }

        });
        //导出
        selector.$btnExport().on("click", function () {
            var data = {
                "Name": selector.$txtName().val().trim(),
                "PhoneNumber": selector.$txtMobilePhone().val().trim(),
                "IDNumber": selector.$txtUserIDNo().val().trim(),
                "JobNumber": selector.$txtJobNumber().val().trim(),
                "PaymentStatus": selector.$txtStatus().val(),
                "TransactionID": selector.$txtTransactionId().val().trim(),
                "PayDateFrom": selector.$txtPaymentForm().val(),
                "PayDateTo": selector.$txtPaymentTo().val()
            };
            window.location.href = "/PaymentManagement/PaymentHistory/Export?searchParas=" + JSON.stringify(data);
        });
        //初始化表格
        function initTable() {

            var source =
                {
                    datafields:
                    [
                        { name: "checkbox", type: null },
                        { name: 'Name', type: 'string' },
                        { name: 'IDNumber', type: 'string' },
                        { name: 'JobNumber', type: 'string' },
                        { name: 'PhoneNumber', type: 'string' },
                        { name: 'OrganizationName', type: 'string' },
                        { name: 'ActualAmount', type: 'number' },
                        { name: 'PaymentAmount', type: 'number' },
                        { name: 'CompanyAccount', type: 'number' },
                        { name: 'PaymentBrokers', type: 'string' },
                        { name: 'TransactionID', type: 'string' },
                        { name: 'PayDate', type: 'date' },
                        { name: 'PaymentStatus', type: 'string' },
                        { name: 'PaymentStatusName', type: 'string' },
                        { name: 'RevenueType', type: 'string' },
                        { name: 'RevenueStatus', type: 'string' },
                        { name: 'VGUID', type: 'string' },
                    ],
                    datatype: "json",
                    id: "VGUID",//主键
                    async: true,
                    data: {
                        "Name": selector.$txtName().val().trim(),
                        "PhoneNumber": selector.$txtMobilePhone().val().trim(),
                        "IDNumber": selector.$txtUserIDNo().val().trim(),
                        "JobNumber": selector.$txtJobNumber().val().trim(),
                        "PaymentStatus": selector.$txtStatus().val(),
                        "TransactionID": selector.$txtTransactionId().val().trim(),
                        "PayDateFrom": selector.$txtPaymentForm().val(),
                        "PayDateTo": selector.$txtPaymentTo().val()
                    },
                    url: "/PaymentManagement/PaymentHistory/GetAllPaymentHistoryInfo"    //获取数据源的路径
                };
            var typeAdapter = new $.jqx.dataAdapter(source, {
                downloadComplete: function (data) {
                    source.totalrecords = data.TotalRows;
                }
            });
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
                      { text: '人员姓名', datafield: 'Name', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '部门', datafield: 'OrganizationName', width: 150, align: 'center', cellsAlign: 'center' },
                      { text: '工号', datafield: 'JobNumber', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '手机号', datafield: 'PhoneNumber', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '应付款(:元)', cellsFormat: "d2", width: 110, datafield: 'PaymentAmount', align: 'center', cellsAlign: 'center' },
                      { text: '实际付款(:元)', width: 110, cellsFormat: "d2", datafield: 'ActualAmount', align: 'center', cellsAlign: 'center' },
                      { text: '公司到账(:元)', cellsFormat: "d2", width: 110, datafield: 'CompanyAccount', align: 'center', cellsAlign: 'center' },
                      { text: '支付中间商', datafield: 'PaymentBrokers', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '微信支付单号', datafield: 'TransactionID', minwidth: 220, align: 'center', cellsAlign: 'center' },
                      { text: '付款时间', datafield: 'PayDate', width: 150, align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                      { text: '营收类型', datafield: 'RevenueType', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '支付状态', datafield: 'PaymentStatusName', width: 100, align: 'center', cellsAlign: 'center' },
                      //{ text: '营收状态', datafield: 'RevenueStatus', width: 100, align: 'center', cellsAlign: 'center' },
                      //{ text: '操作', datafield: '', align: 'center', cellsAlign: 'center', cellsRenderer: showFunc },
                      { text: 'VGUID', datafield: 'VGUID', hidden: true }
                    ]
                });
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
                        grid.find(".jqx_datatable_checkbox").attr("checked", "checked");
                    }
                } else {
                    grid.jqxDataTable('clearSelection');
                    grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked");
                }
            });
            return true;
        }

        function showFunc(row, column, value, rowData) {
            var container = "";
            var num = new Date() - new Date(rowData.PayDate);
            var dateSub = (num / (1000 * 3600 * 24));   //求得相差的天数
            if (rowData.PaymentStatus == "支付成功" && dateSub <= 1) {
                container = "<a href='#' onclick=\"refund('" + rowData.TransactionID + "','" + rowData.ActualAmount + "','" + rowData.PayDate + "')\"  style=\"text-decoration: underline;color: #333;\">退款</a>";
            }
            return container;
        }
    }; //addEvent end


};
var transactionId;
var total;
function refund(transaction_id, total_fee, payDate) {
    transactionId = transaction_id;
    total = total_fee;
    var num = new Date() - new Date(payDate);
    var dateSub = (num / (1000 * 3600 * 24));   //求得相差的天数
    if (dateSub > 1) {
        jqxNotification("时间已经操作一天，不能进行退款！", null, "error");
        return;
    }
    WindowConfirmDialog(ref, "确定要退款吗?", "确认框", "确定", "取消");

}

var outTradeNo = "";
//退款
function ref() {
    showLoading();
    $.ajax({
        url: "/PaymentManagement/PaymentHistory/Refund",
        data: { transaction_id: transactionId, total_fee: total, tradeNo: outTradeNo },
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg.ResponseInfo) {
                case "1":
                    jqxNotification("退款成功!", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
                default:
                    jqxNotification(msg.ResponseInfo, null, "error");
                    outTradeNo = msg.ReturnMsg;  //退款单号
                    break;
            }
            closeLoading();
        }
    });
}
//删除
function deleted() {
    var selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.VGUID);
        }
    });
    $.ajax({
        url: "/PaymentManagement/PaymentHistory/DeletePaymentHistory",
        data: { vguidList: selection },
        type: "post",
        dataType: "json",
        traditional: true,
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("删除失败!", null, "error");
                    break;
                case "1":
                    selector.$grid().jqxDataTable('updateBoundData');
                    jqxNotification("删除成功!", null, "success");
                    break;
            }
        }

    });
}
//补插
function approved() {
    var selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.VGUID);
        }
    });
    $.ajax({
        url: "/PaymentManagement/PaymentHistory/Insert2Revenue",
        data: { vguidList: selection },
        type: "post",
        dataType: "json",
        traditional: true,
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("插入失败!", null, "error");
                    break;
                case "1":
                    selector.$grid().jqxDataTable('updateBoundData');
                    jqxNotification("插入成功!", null, "success");
                    break;
            }
        }

    });
}
$(function () {
    var page = new $page();
    page.init();

});
