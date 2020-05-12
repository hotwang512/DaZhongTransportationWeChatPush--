var selector = {
    $grid: function () { return $("#DraftInfoList") },

    //按钮
    $btnSend: function () { return $("#btnSend") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $title_Search: function () { return $("#Title_Search") },
    $pushType_Search: function () { return $("#PushType_Search") },
    $isImportant_Search: function () { return $("#IsImportant_Search") },
    $pushDate_Search: function () { return $("#PushDate_Search") }
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
        selector.$title_Search().val("");
        selector.$pushType_Search().val("");
        selector.$isImportant_Search().val("");
        selector.$pushDate_Search().val("");
    });

    //生效时间控件
    selector.$pushDate_Search().datetimepicker({
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

        var DraftListSource =
        {
            datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'PushType', type: 'string' },
                    { name: 'TranslatePushType', type: 'string' },
                    { name: 'Title', type: 'string' },
                    { name: 'MessageType', type: 'string' },
                    { name: 'TranslateMessageType', type: 'string' },
                    { name: 'Timed', type: 'string' },
                    { name: 'TimedSendTime', type: 'date' },
                    { name: 'PeriodOfValidity', type: 'date' },
                    { name: 'PushDate', type: 'date' },
                    { name: 'PushPeople', type: 'string' },
                    { name: 'VGUID', type: 'string' },
                    { name: 'Status', type: 'string' },
                    { name: 'TranslateStatus', type: 'string' },
                    { name: 'Message', type: 'string' }
                ],
            datatype: "json",
            id: "Vguid",//主键
            async: true,
            data: {
                "Title": selector.$title_Search().val().trim(),
                "PushType": selector.$pushType_Search().val(),
                "Important": selector.$isImportant_Search().val(),
                "PushDate": selector.$pushDate_Search().val()
            },
            url: "/WeChatPush/PushedList/GetWeChatPushListBySearch"    //获取数据源的路径
        };
        var typeAdapter = new $.jqx.dataAdapter(DraftListSource, {
            downloadComplete: function (data) {
                DraftListSource.totalrecords = data.TotalRows;
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
                    { text: '推送标题', datafield: 'Title', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                    { text: '推送类型', datafield: 'TranslatePushType', align: 'center', cellsAlign: 'center' },
                    { text: '推送消息类型', datafield: 'TranslateMessageType', align: 'center', cellsAlign: 'center' },
                    { text: '是否定时发送', datafield: 'Timed', align: 'center', cellsAlign: 'center', cellsRenderer: TranslateTimed },
                    { text: '定时发送时间', datafield: 'TimedSendTime', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", cellsRenderer: translateTime },
                    { text: '推送有效时间', datafield: 'PeriodOfValidity', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", cellsRenderer: translateTime },
                    { text: '推送时间', datafield: 'PushDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", cellsRenderer: translateTime },
                    { text: '推送人员', datafield: 'PushPeople', align: 'center', cellsAlign: 'center' },
                    { text: '推送内容', datafield: 'Message', align: 'center', cellsAlign: 'center', hidden: true },
                    { text: 'VGUID', datafield: 'VGUID', hidden: true }
                ]
            });
        index = 0;
    }

    function translateTime(row, column, value, rowData) {
        var container = "";
        if (value.indexOf("0001-01-01") == 0) {
            container = "";
        } else {
            container = value;
        }
        return container;
    }

    //翻译是否定时
    function TranslateTimed(row, column, value, rowData) {
        var container = "";
        if (rowData.Timed == false) {
            container = "否";
        }
        else {
            container = "是";
        }
        return container;
    }

    function detailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='PushedDetail?Vguid=" + rowData.VGUID + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
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
