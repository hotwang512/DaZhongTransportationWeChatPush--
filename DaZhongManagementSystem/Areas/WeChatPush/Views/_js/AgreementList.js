var selector = {
    //表格
    $grid: function () { return $("#jqxTable") },
    //按钮
    $btnExport: function () { return $("#btnExport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $jqxWidget: function () { return $("#jqxWidget") },
    //查询条件
    $txtTitle: function () { return $("#txtTitle") },
    $txtResult: function () { return $("#txtResult") },
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

        initDropdownList();
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
        selector.$txtOperationTimeFrom().on('changeDate', function (e) {
            selector.$txtOperationTimeTo().datetimepicker('setStartDate', e.date);
        });
        $(".glyphicon-arrow-left").text("<<");
        $(".glyphicon-arrow-right").text(">>");
        //点击搜索按钮
        selector.$btnSearch().on("click", function () {
            initTable();
        });
        //点击重置按钮
        selector.$btnReset().on("click", function () {
            selector.$txtTitle().val("");
            //  selector.$txtResult().val("");
            selector.$txtOperationTimeFrom().val("");
            selector.$txtOperationTimeTo().val("");
            selector.$txtName().val("");
            selector.$txtMobilePhone().val("");
            selector.$jqxWidget().jqxDropDownList('uncheckAll');
            selector.$jqxWidget().jqxDropDownList('setContent', '===请选择===');
        });
        //点击导出按钮
        selector.$btnExport().on("click", function () {
            var items = selector.$jqxWidget().jqxDropDownList('getCheckedItems');
            var checkedItems = "";
            if (items) {
                $.each(items, function (index) {
                    checkedItems += this.label + ",";
                });
                checkedItems = checkedItems.substr(0, checkedItems.length - 1);
            }
            var data = {
                "Title": selector.$txtTitle().val(),
                "Result": checkedItems,
                "OperationTimeFrom": selector.$txtOperationTimeFrom().val(),
                "OperationTimeTo": selector.$txtOperationTimeTo().val(),
                "Name": selector.$txtName().val(),
                "PhoneNumber": selector.$txtMobilePhone().val()
            };
            window.location.href = "/WeChatPush/AgreementOperation/Export?para=" + JSON.stringify(data);
        });

        //修改下拉框的样式
        $("#jqxWidget .jqx-icon").css('left', '10%');
        $("#jqxWidget").children('div').css('background-color', 'white');
    }
    //初始化表格
    function initTable() {
        var items = selector.$jqxWidget().jqxDropDownList('getCheckedItems');
        var checkedItems = "";
        if (items) {
            $.each(items, function (index) {
                checkedItems += this.label + ",";
            });
            checkedItems = checkedItems.substr(0, checkedItems.length - 1);
        }
        var source =
            {
                datafields:
                [
                    //{ name: "checkbox", type: null },
                    { name: 'Title', type: 'string' },
                    { name: 'Result', type: 'string' },
                    { name: 'OperationTime', type: 'date' },
                    { name: 'Name', type: 'string' },
                    { name: 'PhoneNumber', type: 'string' },
                    { name: 'Vguid', type: 'string' },
                ],
                datatype: "json",
                id: "Vguid",//主键
                async: true,
                data: {
                    "Title": selector.$txtTitle().val(),
                    "Result": checkedItems,
                    "OperationTimeFrom": selector.$txtOperationTimeFrom().val(),
                    "OperationTimeTo": selector.$txtOperationTimeTo().val(),
                    "Name": selector.$txtName().val(),
                    "PhoneNumber": selector.$txtMobilePhone().val()
                },
                url: "/WeChatPush/AgreementOperation/GetAgreementOpertaionList"    //获取数据源的路径
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
               //   { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '协议标题', width: '20%', datafield: 'Title', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  { text: '操作人', width: '20%', datafield: 'Name', align: 'center', cellsAlign: 'center' },
                  { text: '手机号', width: '20%', datafield: 'PhoneNumber', align: 'center', cellsAlign: 'center' },
                  { text: '操作结果', width: '20%', datafield: 'Result', align: 'center', cellsAlign: 'center' },
                  { text: '操作时间', width: '20%', datafield: 'OperationTime', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm" },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });

    }

    function detailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='AgreementOperationDetail?vguid=" + rowData.Vguid + "' style=\"text-decoration: underline;color: #333;\">" + rowData.Title + "</a>";
        return container;
    }

    //初始化标签下拉框
    function initDropdownList() {
        var source =
            {
                datatype: "json",
                datafields: [
                    { name: 'Result' },
                 //   { name: 'VGUID' }
                ],
                id: 'LabelName',
                url: '/WeChatPush/AgreementOperation/GetAgreementTypeList',
                //async: false,
            };
        var dataAdapter = new $.jqx.dataAdapter(source);
        selector.$jqxWidget().jqxDropDownList({ checkboxes: true, source: dataAdapter, placeHolder: "===请选择===", displayMember: "Result", valueMember: "Result", width: 210, height: 35 });
        selector.$jqxWidget().jqxDropDownList('checkIndex', -1);
    }


};



$(function () {
    var page = new $page();
    page.init();
});