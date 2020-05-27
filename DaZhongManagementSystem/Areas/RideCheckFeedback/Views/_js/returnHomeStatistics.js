var selector = {
    $grid: function () { return $("#rhs") },
    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnExport: function () { return $("#btnExport") },
    $jqxDepartmentDropDownButton: function () { return $("#jqxDepartmentDropDownButton") },
    $jqxTree: function () { return $("#jqxTree") },
    $OwnedFleet: function () { return $("#OwnedFleet") },
    $year: function () { return $("#year") }
}
var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        LoadTable();
        initDepartment();

    }; //addEvent end

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        LoadTable();
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$jqxDepartmentDropDownButton().jqxDropDownButton('setContent', "");
        selector.$jqxTree().jqxTree('clearSelection');
        selector.$OwnedFleet().val("");
        selector.$year().val(new Date().getFullYear());
        LoadTable();
    });
    selector.$btnExport().click(function () {
        var year = selector.$year().val();
        var dept = selector.$OwnedFleet().val()
        window.location.href = "/RideCheckFeedback/HomecomingSurvey/ReturnHomeStatisticsExport?year=" + year + "&dept=" + dept;
    });
    //初始化下拉框
    function initDepartment() {
        //推送接收人下拉框
        $.ajax({
            url: "/BasicDataManagement/OrganizationManagement/GetUserOrganizationTreeList",
            data: {},
            traditional: true,
            type: "post",
            success: function (msg) {
                //推送接收人下拉框
                selector.$jqxDepartmentDropDownButton().jqxDropDownButton({
                    width: 185,
                    height: 25
                });
                selector.$jqxDepartmentDropDownButton().on("open", function () {
                    //selector.$jqxTree().show();
                });
                //推送接收人下拉框(树形结构)
                selector.$jqxTree().on('select', function (event) {
                    var args = event.args;
                    var item = selector.$jqxTree().jqxTree('getItem', args.element);
                    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                    selector.$jqxDepartmentDropDownButton().jqxDropDownButton('setContent', dropDownContent);
                    var items = selector.$jqxTree().jqxTree('getSelectedItem');
                    selector.$OwnedFleet().val("");
                    // selector.$TranslationOwnedFleet_Search().val("");
                    selector.$OwnedFleet().val(items.id);
                    selector.$jqxDepartmentDropDownButton().jqxDropDownButton('close');
                    // selector.$TranslationOwnedFleet_Search().val(items.label);
                    initTable();

                });
                var source =
                        {
                            datatype: "json",
                            datafields: [
                                { name: 'OrganizationName' },
                                { name: 'ParentVguid' },
                                { name: 'Vguid' }
                            ],
                            id: 'Vguid',
                            localdata: msg
                        };
                var dataAdapter = new $.jqx.dataAdapter(source);
                // perform Data Binding.
                dataAdapter.dataBind();
                var records = dataAdapter.getRecordsHierarchy('Vguid', 'ParentVguid', 'items',
                    [
                        {
                            name: 'OrganizationName',
                            map: 'label'
                        },
                        {
                            name: 'Vguid',
                            map: 'id'
                        },
                        {
                            name: 'ParentVguid',
                            map: 'parentId'
                        }
                    ]);
                selector.$jqxTree().jqxTree({ source: records, width: '285px', height: '300px', incrementalSearch: true });//, checkboxes: true

                selector.$jqxTree().jqxTree('expandAll');

            }
        });
    }


    //加载团队表
    function LoadTable() {

        var hsSource =
            {
                datafields:
                [
                    { name: "OrganizationName", type: 'string' },
                    { name: 'ReturnHome', type: 'string' },
                    { name: 'NoReturnHome', type: 'string' },
                    { name: 'VGUID', type: 'string' },
                ],
                datatype: "json",
                id: "vguid",//主键
                async: true,
                data: { year: selector.$year().val(), dept: selector.$OwnedFleet().val() },
                url: "/RideCheckFeedback/HomecomingSurvey/ReturnHomeStatisticsSource"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(hsSource, {
            downloadComplete: function (data) {
                hsSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                //pageable: true,
                width: "100%",
                height: 480,
                //pageSize: 10,
                serverProcessing: true,
                //pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [

                  { text: '公司/部门', width: 150, datafield: 'OrganizationName', align: 'center', cellsAlign: 'center' },
                  { text: '返乡人数', width: 150, datafield: 'ReturnHome', align: 'center', cellsAlign: 'center' },
                  { text: '不返乡人数', width: 200, datafield: 'NoReturnHome', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'VGUID', hidden: true }
                ]
            });
    }
}

$(function () {
    var page = new $page();
    page.init();
})