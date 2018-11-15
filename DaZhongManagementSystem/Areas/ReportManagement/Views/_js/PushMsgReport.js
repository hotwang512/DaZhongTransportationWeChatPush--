

var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有元素选择器
    var selector = {
        $grid: function () { return $("#UserInfoList") },

        $btnSearch: function () { return $("#btnSearch") },
        $btnReset: function () { return $("#btnReset") },
        $btnExport: function () { return $("#btnExport") },

        $pushName_Search: function () { return $("#PushName_Search") }



    }; //selector end



    //所有事件
    function addEvent() {

        //加载列表数据
        LoadTable();

        //
        selector.$btnSearch().on('click', function () {
            LoadTable();
        });

        //重置按钮事件
        selector.$btnReset().on('click', function () {
            selector.$pushName_Search().val("");
        });

        //导出按钮事件
        selector.$btnExport().on('click', function () {
            var title = selector.$pushName_Search().val();
            window.location.href = "Export?title=" + title;
        });

    }; //addEvent end


    var tool = {

    }; // tool end

    function LoadTable() {
        var roleTypeSource =
            {
                datafields:
                [
                    { name: 'Title', type: 'string' },
                    { name: 'SumQTY', type: 'string' },
                    { name: 'NoRead', type: 'string' },
                    { name: 'Reads', type: 'string' },
                    { name: 'VGUID', type: 'string' }
                ],
                datatype: "json",
                id: "VGUID",
                async: true,
                data: { "pushMsgName": selector.$pushName_Search().val() },
                url: "/ReportManagement/PushMsgReport/GetPushMsgReportList"   //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(roleTypeSource, {
            downloadComplete: function (data) {
                roleTypeSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: true,
                width: "100%",
                height: 400,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columnsHeight: 40,
                columns: [
                  { text: '推送名称', datafield: 'Title', width: 150, align: 'center', cellsAlign: 'center' },
                  { text: '推送总人数', datafield: 'SumQTY', align: 'center', cellsAlign: 'center', width: 200 },
                  { text: '未读推送比例', datafield: 'NoRead', align: 'center', cellsAlign: 'center', cellsRenderer: noReadFunc },
                  { text: '已读推送比例', datafield: 'Reads', align: 'center', cellsAlign: 'center', cellsRenderer: readFunc },
                  { text: 'Vguid', datafield: 'VGUID', hidden: true }
                ]
            });
    }
    //未读比例
    function noReadFunc(row, column, value, rowData) {
        var container = rowData.NoRead;
        container = container * 100 + "%";
        return container;
    }

    //已读比例
    function readFunc(row, column, value, rowData) {
        var container = rowData.Reads;
        container = container * 100 + "%";
        return container;
    }
};

$(function () {
    var page = new $page();
    page.init();
})


