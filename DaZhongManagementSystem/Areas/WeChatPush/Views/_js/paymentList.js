var selector = {
    //表格
    $grid: function () { return $("#jqxTable") },
    //按钮
    $btnExport: function () { return $("#btnExport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    //查询条件
    $RedpacketType: function () { return $("#RedpacketType") },
    $txtOperationTimeFrom: function () { return $("#txtOperationTimeFrom") },
    $txtOperationTimeTo: function () { return $("#txtOperationTimeTo") },
    $txtName: function () { return $("#txtName") },
    $txtMobilePhone: function () { return $("#txtMobilePhone") }


};



var $page = function () {
    this.init = function () {
        addEvent();
    }

    function addEvent() {
        initTable();


    }


    //生效时间控件
    selector.$txtOperationTimeFrom().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    //生效时间控件
    selector.$txtOperationTimeTo().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    selector.$txtOperationTimeFrom().on('changeDate', function (e) {
        selector.$txtOperationTimeTo().datetimepicker('setStartDate', e.date);
    });

    //点击搜索按钮
    selector.$btnSearch().on("click", function () {
        initTable();
    });
    //点击重置按钮
    selector.$btnReset().on("click", function () {
        selector.$RedpacketType().val("");
        selector.$txtOperationTimeFrom().val("");
        selector.$txtOperationTimeTo().val("");
        selector.$txtName().val("");
        selector.$txtMobilePhone().val("");
    });
    //初始化表格
    function initTable() {
        var source =
            {
                datafields:
                [
                    //{ name: "checkbox", type: null },
                    { name: 'UserID', type: 'string' },
                    { name: 'Name', type: 'string' },
                    { name: 'RedpacketMoney', type: 'string' },
                  //  { name: 'ReceiveDate', type: 'date' },
                    { name: 'RedpacketStatus', type: 'string' },
                 //   { name: 'transRedpacketStatus', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'Reson', type: 'string' },
                    { name: 'VGUID', type: 'string' },
                ],
                datatype: "json",
                id: "Vguid",//主键
                async: true,
                data: {
                    "Name": selector.$txtName().val().trim(),
                    "UserID": selector.$txtMobilePhone().val().trim(),
                    "RedpacketStatus": selector.$RedpacketType().val(),
                    "ReceiveDateFrom": selector.$txtOperationTimeFrom().val(),
                    "ReceiveDateTo": selector.$txtOperationTimeTo().val()
                },
                url: "/WeChatPush/PaymentOperation/GetPaymentInfos"    //获取数据源的路径
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
                height: 450,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { text: '姓名', width: '16%', datafield: 'Name', align: 'center', cellsAlign: 'center' },
                  { text: '手机号', width: '16%', datafield: 'UserID', align: 'center', cellsAlign: 'center' },
                  { text: '金额(元)', width: '16%', datafield: 'RedpacketMoney', align: 'center', cellsAlign: 'center', cellsformat: "d2" },
                  { text: '付款时间', width: '16%', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: '付款状态', width: '16%', datafield: 'RedpacketStatus', align: 'center', cellsAlign: 'center', cellsRenderer: showFunc },
                 // { text: '领取时间/退款时间', width: '15%', datafield: 'ReceiveDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: '失败原因', width: '20%', datafield: 'Reson', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });

        function showFunc(row, column, value, rowData) {
            var container = "";
            switch (rowData.RedpacketStatus) {
                case 1:
                    container = "<span>付款成功</span>";
                    break;
                case 2:
                    container = "<span>付款失败</span>";
                    break;
            }
            return container;
        }
    }





};



$(function () {
    var page = new $page();
    page.init();
});