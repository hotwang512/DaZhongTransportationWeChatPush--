var selector = {
    $grid: function () { return $("#rhs") },
    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnExport: function () { return $("#btnExport") },
    $jqxDepartmentDropDownButton: function () { return $("#jqxDepartmentDropDownButton") },
    $jqxTree: function () { return $("#jqxTree") },
    $OwnedFleet: function () { return $("#OwnedFleet") },
    $startDate: function () { return $("#StartDate") },
    $endDate: function () { return $("#EndDate") }
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
    //生效时间控件
    selector.$startDate().datepicker({
        format: 'yyyy-mm',
        autoclose: true,//自动关闭
        maxViewMode: 'years',
        minViewMode: "months",
        startView: "months",
        language: 'zh-CN',
        orientation: "bottom right"
    });
    //生效时间控件
    selector.$endDate().datepicker({
        format: 'yyyy-mm',
        autoclose: true,//自动关闭
        maxViewMode: 'years',
        minViewMode: "months",
        startView: "months",
        language: 'zh-CN',
        orientation: "bottom right"
    });
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
        var dept = selector.$OwnedFleet().val() == "" ? "fc17a729-28e4-483f-9fbf-179018b67224" : selector.$OwnedFleet().val();
        var startDate = selector.$startDate().val();
        var endDate = selector.$endDate().val();
        window.location.href = "/ReportManagement/QuestionReport/ExportExerciseTotalReport?startDate=" + startDate + "&endDate=" + endDate + "&dept=" + dept;
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

        var fieldArray = new Array();
        var colArray = new Array();
        var columnGroups = new Array();
        var datasource =
        {
            datatype: "json",
            id: "IDNumber",
            async: true,
            data: {
                dept: selector.$OwnedFleet().val(),
                startDate: selector.$startDate().val(),
                endDate: selector.$endDate().val()
            },
            url: "/ReportManagement/QuestionReport/ExerciseTotalReportSource"
        };
        var typeAdapter = new $.jqx.dataAdapter(datasource, {
            downloadComplete: function (data) {
                datasource = data;
                for (var obj in datasource[0]) {
                    fieldArray.push({ name: obj, type: 'string' });
                    if (obj == "OrganizationName") {
                        colArray.push({ text: '公司/部门', width: 200, datafield: 'OrganizationName', align: 'center', cellsAlign: 'center' });
                    }
                    else if (obj == "Name") {
                        colArray.push({ text: '姓名', width: 150, datafield: 'Name', align: 'center', cellsAlign: 'center' });
                    }
                    else if (obj == "IDNumber") {
                        colArray.push({ text: '身份证号码', width: 150, datafield: 'IDNumber', align: 'center', cellsAlign: 'center' });
                    } else {
                        if (obj.indexOf("_1") < 0) {
                            columnGroups.push({ text: obj, align: 'center', name: obj });
                            colArray.push({ text: "培训", columngroup: obj, width: 80, datafield: obj + "_1", align: 'center', cellsAlign: 'center' });
                            colArray.push({ text: "习题", columngroup: obj, width: 80, datafield: obj, align: 'center', cellsAlign: 'center' });

                        }
                    }
                }
                datasource.datafields = fieldArray;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                width: "100%",
                height: 480,
                pageable: true,
                source: typeAdapter,
                theme: "office",
                columnGroups: columnGroups,
                columns: colArray

            });
    }
}

$(function () {
    var page = new $page();
    page.init();
})