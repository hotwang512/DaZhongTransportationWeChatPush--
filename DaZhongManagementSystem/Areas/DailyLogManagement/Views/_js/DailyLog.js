var selector = {
    $grid: function () { return $("#DailyLogList") },
    $logDialog: function () { return $("#LogDialog") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnCancel: function () { return $("#btnCancel") },

    //查询
    $EventType_Search: function () { return $("#EventType_Search") },
    $VcrtUser_Search: function () { return $("#VcrtUser_Search") },
    $DateBegin_Search: function () { return $("#DateBegin_Search") },
    $DateEnd_Search: function () { return $("#DateEnd_Search") },

    $eventType: function () { return $("#eventType") },
    $createdUser: function () { return $("#createdUser") },
    $logData: function () { return $("#logData") },
    $page: function () { return $("#page") },
    $createdTime: function () { return $("#createdTime") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        LoadTable();

    }; //addEvent end

    selector.$btnSearch().on('click', function () {
        LoadTable();
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$EventType_Search().val("");
        selector.$VcrtUser_Search().val("");
        selector.$DateBegin_Search().val("");
        selector.$DateEnd_Search().val("");
    });

    //生效时间控件
    selector.$DateBegin_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
   
    //双击编辑事件
    selector.$grid().on('rowDoubleClick', function (event) {
        var args = event.args;
        var key = args.key;
        var row = args.row;
        //selector.$Vguid().val(row.Vguid);//Vguid
        $.ajax({
            url: "/DailyLogManagement/LogManagement/GetDailyDetail",
            data: { vguid: row.Vguid },
            type: "post",
            success: function (msg) {
                selector.$eventType().val(msg.EventType);
                selector.$createdUser().val(msg.CreatedUser);
                selector.$page().val(msg.Page);
                selector.$createdTime().val(msg.CreatedDate);
                selector.$logData().val(msg.LogData);
                //弹出编辑框
                selector.$myModalLabel_title().text("查看日志详细信息");
                selector.$logDialog().modal({ backdrop: 'static', keyboard: false });
                selector.$logDialog().modal('show');
            }
        });
    });

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        selector.$logDialog().modal('hide');
    });

    //生效时间控件
    selector.$DateEnd_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //加载团队表
    function LoadTable() {

        var LogListSource =
            {
                datafields:
                [
                    //{ name: "checkbox", type: null },
                    { name: 'EventType', type: 'string' },
                    { name: 'LogMessage', type: 'string' },
                    { name: 'LogData', type: 'string' },
                    { name: 'LogUser', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'CreatedUser', type: 'string' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "Vguid",//主键
                async: true,
                data: { "EventType": selector.$EventType_Search().val(), "LogUser": selector.$VcrtUser_Search().val(), "BeginDate": selector.$DateBegin_Search().val(), "EndDate": selector.$DateEnd_Search().val() },
                url: "/DailyLogManagement/LogManagement/GetLogListBySearch"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(LogListSource, {
            downloadComplete: function (data) {
                LogListSource.totalrecords = data.TotalRows;
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
                  { text: '事件类型', width: '200px', datafield: 'EventType', align: 'center', cellsAlign: 'center' },
                  { text: '消息', datafield: 'LogMessage', align: 'center', cellsAlign: 'center' },
                  //{ text: '数据', datafield: 'LogData', align: 'center', cellsAlign: 'center' },
                  { text: '用户', datafield: 'CreatedUser', align: 'center', cellsAlign: 'center' },
                  { text: '操作时间', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: 'Vguid', datafield: 'Vguid', hidden: true }
                ]
            });
        index = 0;
    }

};


$(function () {
    var page = new $page();
    page.init();

})


