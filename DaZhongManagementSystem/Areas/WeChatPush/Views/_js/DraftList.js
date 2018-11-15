var selector = {
    $grid: function () { return $("#DraftInfoList") },

    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnSubmit: function () { return $("#btnSubmit") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $title_Search: function () { return $("#Title_Search") },
    $pushType_Search: function () { return $("#PushType_Search") },
    $isImportant_Search: function () { return $("#IsImportant_Search") },
    $effectiveDate_Search: function () { return $("#EffectiveDate_Search") }
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
        selector.$effectiveDate_Search().val("");
    });

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        window.location.href = "/WeChatPush/DraftList/DraftDetail?isEdit=false";
    });

    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        var selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox_a:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.VGUID);
            }
        })
        if (selection.length < 1) {
            jqxNotification("请选择您要删除的数据！", null, "error");
        } else {
            WindowConfirmDialog(dele, "您确定要删除选中的数据？", "确认框", "确定", "取消");
        }
    });

    //提交按钮事件
    selector.$btnSubmit().on('click', function () {
        selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox_a:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.VGUID);
            }
        })
        if (selection.length < 1) {
            jqxNotification("请选择您要提交的数据！", null, "error");
        } else {
            WindowConfirmDialog(submit, "您确定要提交选中的数据？", "确认框", "确定", "取消");
        }
    });

    //生效时间控件
    selector.$effectiveDate_Search().datetimepicker({
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
                    { name: 'ExercisesVGUID', type: 'string' },
                    { name: 'Message', type: 'string' }
                ],
                datatype: "json",
                id: "Vguid",//主键
                async: true,
                data: {
                    "Title": selector.$title_Search().val(),
                    "PushType": selector.$pushType_Search().val(),
                    "Important": selector.$isImportant_Search().val(),
                    "PeriodOfValidity": selector.$effectiveDate_Search().val()
                },
                url: "/WeChatPush/DraftList/GetWeChatPushListBySearch"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(DraftListSource, {
            downloadComplete: function (data) {
                DraftListSource.totalrecords = data.TotalRows;
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
                  { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '推送标题', datafield: 'Title', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  { text: '推送类型', datafield: 'TranslatePushType', align: 'center', cellsAlign: 'center' },
                  { text: '推送消息类型', width: 150, datafield: 'TranslateMessageType', align: 'center', cellsAlign: 'center' },
                  { text: '是否定时发送', width: 150, datafield: 'Timed', align: 'center', cellsAlign: 'center', cellsRenderer: TranslateTimed },
                  { text: '定时发送时间', width: 150, datafield: 'TimedSendTime', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", cellsRenderer: translateTime },
                  { text: '推送有效时间', width: 150, datafield: 'PeriodOfValidity', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm", cellsRenderer: translateTime },
                  //{ text: '推送时间', datafield: 'PushDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd" },
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
        container = "<a href='DraftDetail?Vguid=" + rowData.VGUID + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
        return container;
    }
    function cellsRendererFunc(row, column, value, rowData) {
        return "<input class=\"jqx_datatable_checkbox_a\" index=\"" + row + "\" type=\"checkbox\"  style=\"margin:auto;width: 17px;height: 17px;\" />";
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
                    grid.find(".jqx_datatable_checkbox_a").attr("checked", "checked")
                }
            } else {
                grid.jqxDataTable('clearSelection');
                grid.find(".jqx_datatable_checkbox_a").removeAttr("checked", "checked")
            }
        });
        return true;
    }

};


//删除
function dele() {
    showLoading();//显示加载等待框
    selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox_a:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.VGUID);
        }
    });
    $.ajax({
        url: "/WeChatPush/DraftList/DeletePushList",
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
    })
}

//提交
function submit() {
    selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox_a:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.VGUID);
        }
    })
    $.ajax({
        url: "/WeChatPush/DraftList/SubmitDraftList",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            if (msg.isSuccess) {
                jqxNotification("提交成功！", null, "success");
                selector.$grid().jqxDataTable('updateBoundData');
            }
            else {
                jqxNotification("提交失败！", null, "error");
            }
        }
    })
}


$(function () {
    var page = new $page();
    page.init();

})
