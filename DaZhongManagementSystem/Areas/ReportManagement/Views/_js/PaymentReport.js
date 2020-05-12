var selector = {
    //表格
    $grid: function () { return $("#datatable") },
    $chartContainer: function () { return $("#chartContainer") },
    $chartContainerMonth: function () { return $("#chartContainerMonth") },
    //查询条件
    $txtName: function () { return $("#txtName") },
    $txtMobilePhone: function () { return $("#txtMobilePhone") },
    $txtPaymentForm: function () { return $("#txtPaymentForm") },
    $txtPaymentTo: function () { return $("#txtPaymentTo") },
    $pushPeopleDropDownButton: function () { return $("#pushPeopleDropDownButton") },
    $pushTree: function () { return $("#pushTree") },
    $selPaymentStatus: function () { return $("#selPaymentStatus") },
    //隐藏域
    $txtDeparment: function () { return $("#txtDeparment") },
    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnExport: function () { return $("#btnExport") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {

        initAggregates();
        initDepartment();
        initChartMonth();
        //查询
        selector.$btnSearch().on("click", function () {
            initAggregates();
           // initChart();
            initChartMonth();
        });

        //重置
        selector.$btnReset().on("click", function () {
            selector.$txtName().val("");
            selector.$txtMobilePhone().val("");
            selector.$txtPaymentForm().val("");
            selector.$txtPaymentTo().val("");
            selector.$txtDeparment().val("");
            selector.$pushPeopleDropDownButton().jqxDropDownButton('setContent', "");
            selector.$selPaymentStatus().val("");
        });

        //生效时间控件
        selector.$txtPaymentForm().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            language: 'zh-CN',
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });

        selector.$txtPaymentForm().on('changeDate', function (e) {
            selector.$txtPaymentTo().datetimepicker('setStartDate', e.date);
        });
        //生效时间控件
        selector.$txtPaymentTo().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            language: 'zh-CN',
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true,
        });
        $(".glyphicon-arrow-left").text("<<");
        $(".glyphicon-arrow-right").text(">>");
        //初始化部门下拉框
        function initDepartment() {
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
                        selector.$txtDeparment().val(item.id); //隐藏域中存放部门vguid
                        var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                        selector.$pushPeopleDropDownButton().jqxDropDownButton('setContent', dropDownContent);
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
                    selector.$pushTree().jqxTree('expandAll');

                }
            });
        }

        //导出
        selector.$btnExport().on("click", function () {
            var data = {
                "Name": selector.$txtName().val(),
                "PhoneNumber": selector.$txtMobilePhone().val(),
                "PayDateFrom": selector.$txtPaymentForm().val(),
                "PayDateTo": selector.$txtPaymentTo().val(),
                "Department": selector.$txtDeparment().val(),
                "PaymentStatus": selector.$selPaymentStatus().val()
            };
            window.location.href = "/ReportManagement/PaymentReport/Export?searchParas=" + JSON.stringify(data);
        });
        //初始化聚合函数
        function initAggregates() {
            $.ajax({
                url: "/ReportManagement/PaymentReport/GetPaymentCount",
                data: {
                    "Name": selector.$txtName().val().trim(),
                    "PhoneNumber": selector.$txtMobilePhone().val().trim(),
                    "PayDateFrom": selector.$txtPaymentForm().val(),
                    "PayDateTo": selector.$txtPaymentTo().val(),
                    "Department": selector.$txtDeparment().val(),
                    "PaymentStatus": selector.$selPaymentStatus().val()
                },
                type: "post",
                dataType: "json",
                success: function (msg) {
                    initTable(msg);
                    initChart(msg);
                  
                }
            });
        }

        //初始化表格
        function initTable(msg) {
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
                        { name: 'RevenueType', type: 'string' },
                        { name: 'RevenueStatus', type: 'string' },
                        { name: 'VGUID', type: 'string' },
                    ],
                    datatype: "json",
                    id: "VGUID",//主键
                    async: true,
                    data: {
                        "Name": selector.$txtName().val(),
                        "PhoneNumber": selector.$txtMobilePhone().val(),
                        "PayDateFrom": selector.$txtPaymentForm().val(),
                        "PayDateTo": selector.$txtPaymentTo().val(),
                        "Department": selector.$txtDeparment().val(),
                        "PaymentStatus": selector.$selPaymentStatus().val()
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
                    height: 470,
                    pageSize: 10,
                    serverProcessing: true,
                    pagerButtonsCount: 10,
                    source: typeAdapter,
                    theme: "office",
                    showAggregates: true,
                    aggregatesHeight: 30,
                    columns: [
                      { text: '人员姓名', datafield: 'Name', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '部门', datafield: 'OrganizationName', width: 150, align: 'center', cellsAlign: 'center' },
                      { text: '工号', datafield: 'JobNumber', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '手机号', datafield: 'PhoneNumber', width: 100, align: 'center', cellsAlign: 'center' },
                      {
                          text: '应付款(:元)', cellsFormat: "d2", width: 150, datafield: 'PaymentAmount', align: 'center', cellsAlign: 'center',
                          aggregatesRenderer: function (aggregates, column, element) {
                              var renderString = "<div style='padding-top: 4px; height: 100%;background-color:#2F74B5;color:white'>";
                              renderString += "<strong>Total: </strong>" + msg[0].PaymentAmount + "</div>";
                              return renderString;
                          }

                      },
                      {
                          text: '实际付款(:元)', width: 150, cellsFormat: "d2", datafield: 'ActualAmount', align: 'center', cellsAlign: 'center',
                          aggregatesRenderer: function (aggregates, column, element) {
                              var renderString = "<div style='padding-top: 4px;height: 100%;background-color:#2F74B5;color:white'>";
                              renderString += "<strong>Total: </strong>" + msg[0].ActualAmount + "</div>";
                              return renderString;
                          }
                      },
                      {
                          text: '公司到账(:元)', cellsFormat: "d2", width: 150, datafield: 'CompanyAccount', align: 'center', cellsAlign: 'center',
                          aggregatesRenderer: function (aggregates, column, element) {
                              var renderString = "<div style='padding-top: 4px; height: 100%;background-color:#2F74B5;color:white'>";
                              renderString += "<strong>Total: </strong>" + msg[0].CompanyAccount + "</div>";
                              return renderString;
                          }
                      },
                      { text: '支付中间商', datafield: 'PaymentBrokers', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '微信支付单号', datafield: 'TransactionID', width: 220, align: 'center', cellsAlign: 'center' },
                      { text: '付款时间', datafield: 'PayDate', width: 150, align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                      { text: '营收类型', datafield: 'RevenueType', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '支付状态', datafield: 'PaymentStatus', width: 100, align: 'center', cellsAlign: 'center' },
                      { text: '营收状态', datafield: 'RevenueStatus', align: 'center', cellsAlign: 'center' },
                      { text: 'VGUID', datafield: 'VGUID', hidden: true }
                    ]
                });
        }

        function initChart(msg) {
            //var source =
            //{
            //    datatype: "json",
            //    datafields: [
            //        { name: 'PaymentAmount' },
            //        { name: 'ActualAmount' },
            //        { name: 'CompanyAccount' },
            //    ],
            //    url: '/ReportManagement/PaymentReport/GetPaymentCount',
            //    data: {
            //        "Name": selector.$txtName().val(),
            //        "PhoneNumber": selector.$txtMobilePhone().val(),
            //        "PayDateFrom": selector.$txtPaymentForm().val(),
            //        "PayDateTo": selector.$txtPaymentTo().val(),
            //        "Department": selector.$txtDeparment().val(),
            //        "PaymentStatus": selector.$selPaymentStatus().val()
            //    },
            //};
           // var dataAdapter = new $.jqx.dataAdapter(source, { async: false, autoBind: true, loadError: function (xhr, status, error) { alert('Error loading "' + source.url + '" : ' + error); } });

            var toolTipCustomFormatFn = function (value, itemIndex, serie, group) {
                return '<div style="text-align:left;width:100px;">' + serie.displayText + ':' + value + '元' + '</div>';
            };
            var settings = {
                title: "支付报表(总)",
                description: "",
                showLegend: true,
                enableAnimations: true,
                padding: { left: 5, top: 5, right: 5, bottom: 5 },
                titlePadding: { left: 90, top: 0, right: 0, bottom: 10 },
                source: msg,
                xAxis:
                    {
                        visible: false,
                        dataField: 'Total',
                        gridLines: { visible: false },
                        valuesOnTicks: true
                    },
                colorScheme: 'scheme01',
                columnSeriesOverlap: false,
                toolTipFormatFunction: toolTipCustomFormatFn,
                seriesGroups:
                    [
                        {
                            type: 'column',
                            valueAxis:
                            {
                                minValue: 0,
                                visible: true,
                                //unitInterval: 1,
                                title: { text: '付款金额(元)<br>' }
                            },
                            series: [
                                    { dataField: 'PaymentAmount', displayText: '应付款' },
                                    { dataField: 'ActualAmount', displayText: '实际付款' },
                                    { dataField: 'CompanyAccount', displayText: '公司到账' }
                            ]
                        }
                    ]
            };
            selector.$chartContainer().jqxChart(settings);

        }


        function initChartMonth() {
            var source =
          {
              datatype: "json",
              datafields: [
                  { name: 'PaymentAmount' },
                  { name: 'ActualAmount' },
                  { name: 'CompanyAccount' },
                  { name: 'PayMonth' },
              ],
              url: '/ReportManagement/PaymentReport/GetMonthlyPayment',
              data: {
                  "Name": selector.$txtName().val(),
                  "PhoneNumber": selector.$txtMobilePhone().val(),
                  "PayDateFrom": selector.$txtPaymentForm().val(),
                  "PayDateTo": selector.$txtPaymentTo().val(),
                  "Department": selector.$txtDeparment().val(),
                  "PaymentStatus": selector.$selPaymentStatus().val()
              },
          };
            var dataAdapter = new $.jqx.dataAdapter(source, { async: false, autoBind: true, loadError: function (xhr, status, error) { alert('Error loading "' + source.url + '" : ' + error); } });
            var toolTipCustomFormatFn = function (value, itemIndex, serie, group) {
                return '<div style="text-align:left;width:100px;">' + serie.displayText + ':' + value + '元' + '</div>';
            };
            var settings = {
                title: "支付报表(月)",
                description: "",
                showLegend: true,
                enableAnimations: true,
                padding: { left: 5, top: 5, right: 5, bottom: 5 },
                titlePadding: { left: 90, top: 0, right: 0, bottom: 10 },
                source: dataAdapter,
                xAxis:
                    {
                        dataField: 'PayMonth',
                        gridLines: { visible: false },
                        valuesOnTicks: true,
                        title: { text: '月份' }
                    },
                colorScheme: 'scheme01',
                columnSeriesOverlap: false,
                toolTipFormatFunction: toolTipCustomFormatFn,
                seriesGroups:
                    [
                        {
                            type: 'column',
                            valueAxis:
                            {
                                visible: true,
                                //unitInterval: 1,
                                minValue: 0,
                                title: { text: '付款金额(元)<br>' }
                            },
                            series: [
                                    { dataField: 'PaymentAmount', displayText: '应付款' },
                                    { dataField: 'ActualAmount', displayText: '实际付款' },
                                     { dataField: 'CompanyAccount', displayText: '公司到账' }
                            ]
                        }
                    ]
            };
            selector.$chartContainerMonth().jqxChart(settings);
        }
    }; //addEvent end


};


$(function () {
    var page = new $page();
    page.init();

});
