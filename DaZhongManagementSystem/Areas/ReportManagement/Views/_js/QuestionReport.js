var selector = {
    $grid: function () { return $("#QuestionReportList") },
    $detailGrid: function () { return $("#ExerciseDetailList") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnExport: function () { return $("#btnExport") },

    //查询
    $exerciseName_Search: function () { return $("#ExercisesName_Search") },
    $exportType_Search: function () { return $("#ExportType_Search") },
    $txtDeparment: function () { return $("#txtDeparment") },
    //饼图
    $answerPersonCountPie: function () { return $("#answerPersonCountPie") },
    $passFineRatePie: function () { return $("#passFineRatePie") },
    $answerCountPie: function () { return $("#answerCountPie") },
    $pushPeopleDropDownButton: function () { return $("#pushPeopleDropDownButton") },
    $pushTree: function () { return $("#pushTree") },
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
        testFunc();
        LoadDetailTable();
        // initDropdownList();
    }; //addEvent end
    $.ajax({
        url: "/BasicDataManagement/UserInfo/GetOrganizationTreeList",
        data: {},
        traditional: true,
        type: "post",
        success: function (msg) {
            //推送接收人下拉框
            selector.$pushPeopleDropDownButton().jqxDropDownButton({
                width: 185,
                height: 25
            });
            //推送接收人下拉框(树形结构)
            selector.$pushTree().on('select', function (event) {
                var args = event.args;
                var item = selector.$pushTree().jqxTree('getItem', args.element);

                selector.$txtDeparment().val(item.id);
                var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                selector.$pushPeopleDropDownButton().jqxDropDownButton('setContent', dropDownContent);
                LoadAnswerPersonCountPie();
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
            selector.$pushTree().jqxTree({ source: records, width: '207px', height: '250px', incrementalSearch: true });//, checkboxes: true
            // var str = selector.$departmentVguid().val();

            //  selector.$pushTree().jqxTree('selectItem', $("#" + str)[0]);// $("#home")[0]+ str
            selector.$pushTree().jqxTree('expandAll');

        }
    });


    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$exerciseName_Search().val("");
        $(".chosen-single span").text("");
        selector.$txtDeparment().val("");
        selector.$pushPeopleDropDownButton().jqxDropDownButton('setContent', "");
    });

    selector.$exerciseName_Search().chosen();
    //if ($("#txtCurrentLoginName").val() != "sysAdmin") {
    //    $("#searchTable").css("width", "610px");
    //}

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        if (selector.$exerciseName_Search().val() == "") {
            jqxNotification("请选择问卷名称！", null, "error");
            return;
        }
        //if (selector.$exerciseName_Search().val() == "") {
        //    jqxNotification("请选择习题名称！", null, "error");
        //    return;
        //}
        LoadAnswerPersonCountPie();
        LoadDetailTable();
        $("#exerciseRateDt").removeAttr("style");
    });

    //导出按钮事件
    selector.$btnExport().on('click', function () {
        if (selector.$exerciseName_Search().val() == "") {
            jqxNotification("请选择习题名称！", null, "error");
            return;
        }
        if (selector.$exportType_Search().val() == "") {
            jqxNotification("请选择导出类型！", null, "error");
            return;
        }
        var exerciseName = selector.$exerciseName_Search().val();
        var exportType = selector.$exportType_Search().val();
        var departmentVguid = selector.$txtDeparment().val();
        window.location.href = "Export?exerciseVguid=" + exerciseName + "&exportType=" + exportType + "&departmentVguid=" + departmentVguid;
    });

    //取消按钮事件
    $("#btnCancel").on('click', function () {
        $("#ScoreReportDialog").modal('hide');
    });


    //答题人数饼状图（答题人数、总人数）
    function LoadAnswerPersonCountPie() {
        $.ajax({
            url: "/ReportManagement/QuestionReport/GetExerciseRateDetail",
            data: { vguid: $("#ExercisesName_Search").val(), "departmentVguid": selector.$txtDeparment().val() },
            traditional: true,
            type: "post",
            success: function (data) {
                if ($("#ExercisesName_Search").val() != "") {
                    $("#chartDesc1").text("答题人数：" + data[2].Share + "  未答题人数：" + data[3].Share);
                    //$("#chartDesc2").text("优秀人数：" + data[11].Share + "  及格人数：" + data[12].Share + " 未及格人数：" + data[13].Share);
                    //$("#chartDesc3").text("未答题人数：" + data[14].Share + "  答题一次人数：" + data[15].Share + " 答题二次人数：" + data[16].Share + " 答题三次人数：" + data[17].Share);
                    var dataStatCounter =
                    [
                        data[0],
                        data[1]
                    ];
                   
                    var charts = [
                        { title: '答题人数比', label: 'Stat', dataSource: dataStatCounter },
                       
                    ];
                    for (var i = 0; i < charts.length; i++) {
                        var chartSettings = {
                            source: charts[i].dataSource,
                            title: '',
                            description: charts[i].title,
                            enableAnimations: false,
                            showLegend: true,
                            showBorderLine: true,
                            padding: { left: 5, top: 5, right: 5, bottom: 5 },
                            titlePadding: { left: 0, top: 0, right: 0, bottom: 10 },
                            colorScheme: 'scheme06',
                            seriesGroups: [
                                {
                                    type: 'pie',
                                    showLegend: true,
                                    enableSeriesToggle: true,
                                    series:
                                        [
                                            {
                                                dataField: 'Share',
                                                displayText: 'Browser',
                                                showLabels: true,
                                                labelRadius: 160,
                                                labelLinesEnabled: true,
                                                labelLinesAngles: true,
                                                labelsAutoRotate: false,
                                                initialAngle: 0,
                                                radius: 115,
                                                minAngle: 0,
                                                maxAngle: 180,
                                                centerOffset: 0,
                                                offsetY: 130,
                                                formatFunction: function (value, itemIdx, serieIndex, groupIndex) {
                                                    if (isNaN(value))
                                                        return value;
                                                    return value;// * 100 + '%';
                                                }
                                            }
                                        ]
                                }
                            ]
                        };
                        // select container and apply settings
                        var selector = '#chartContainer' + (i + 1);
                        $(selector).jqxChart(chartSettings);
                        //去除水印
                        $(".jqx-chart-legend-text").each(function () {
                            if ($(this).text() == "www.jqwidgets.com")
                                $(this).text('');
                        });

                    }
                }
            }
        });
    }

    //默认显示
    function testFunc() {
        var dataStatCounter =
        [
            { Browser: 'Chrome', Share: 50 },
            { Browser: 'IE', Share: 50 },
        ];
       
        var charts = [
            { title: '答题人数比', label: 'Stat', dataSource: dataStatCounter },
           
        ];
        for (var i = 0; i < charts.length; i++) {
            var chartSettings = {
                source: charts[i].dataSource,
                title: '',
                description: charts[i].title,
                enableAnimations: false,
                showLegend: true,
                showBorderLine: true,
                padding: { left: 5, top: 5, right: 5, bottom: 5 },
                titlePadding: { left: 0, top: 0, right: 0, bottom: 10 },
                colorScheme: 'scheme06',
                seriesGroups: [
                    {
                        type: 'pie',
                        showLegend: true,
                        enableSeriesToggle: true,
                        series:
                            [
                                {
                                    dataField: 'Share',
                                    displayText: 'Browser',
                                    showLabels: true,
                                    labelRadius: 160,
                                    labelLinesEnabled: true,
                                    labelLinesAngles: true,
                                    labelsAutoRotate: false,
                                    initialAngle: 0,
                                    radius: 115,
                                    minAngle: 0,
                                    maxAngle: 180,
                                    centerOffset: 0,
                                    offsetY: 130,
                                    formatFunction: function (value, itemIdx, serieIndex, groupIndex) {
                                        if (isNaN(value))
                                            return value;
                                        return value * 100 + '%';
                                    }
                                }
                            ]
                    }
                ]
            };
            // select container and apply settings
            var selector = '#chartContainer' + (i + 1);
            $(selector).jqxChart(chartSettings);
        } // for
    }


    //加载习题详情表
    function LoadDetailTable() {
        var vguid = $("#ExercisesName_Search").val();
        if (vguid == "") {
            vguid = "00000000-0000-0000-0000-000000000000";
        }
        var UserListSource =
            {
                datafields:
                [
                    { name: 'BusinessQuestionDetailVguid', type: 'string' },
                    { name: 'Correct', type: 'string' },
                    { name: 'QuestionName', type: 'string' },
                    { name: 'QuestionNameDetail', type: 'string' }
                ],
                datatype: "json",
                id: "BusinessExersDetailVguid",//主键
                async: true,
                data: { "vguid": vguid, "departmentVguid": selector.$txtDeparment().val() },
                url: "/ReportManagement/QuestionReport/GetQuestionDetail"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(UserListSource, {
            downloadComplete: function (data) {
                UserListSource.totalrecords = data.TotalRows;
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
                columns: [
                  { text: '题目名称', datafield: 'QuestionName', align: 'center', cellsAlign: 'center' },
                  { text: '选项', datafield: 'QuestionNameDetail', align: 'center', cellsAlign: 'center' },
                  { text: '百分比', datafield: 'Correct', align: 'center', cellsAlign: 'center', cellsRenderer: addPercent },
                  { text: 'VGUID', datafield: 'BusinessQuestionDetailVguid', hidden: true }
                ]
            });
    }

    function addPercent(row, column, value, rowData) {
        var container = rowData.Correct;
        container = container*100 + "%";
        return container;
    }

};



$(function () {
    var page = new $page();
    page.init();

})


