$(".input_text").attr("autocomplete", "new-password");
var selector = {
    $grid: function () { return $("#UserInfoList") },
    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
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
        selector.$Company_Search().val("");
        selector.$ContactPerson_Search().val("");
        //selector.$Department_Search().val("");
    });


    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'Type', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'Location', type: 'string' },
                    { name: 'Personnel', type: 'string' },
                    { name: 'OperationDate', type: 'date' },
                    { name: 'CouponType', type: 'string' },
                    { name: 'CabLicense', type: 'string' },
                    { name: 'CabOrgName', type: 'string' },
                    { name: 'ManOrgName', type: 'string' },
                    { name: 'CreatedUser', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "Vguid",//主键
                //async: true,
                data: { cabOrgName: $("#CabOrgName").val(), manOrgName: $("#ManOrgName").val(), couponType: $("#CouponType").val(), cabLicense: $("#CabLicense").val() },
                url: "/SecondaryCleaningManagement/CleaningInfo/GetCleaningInfo"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(UserInfoListSource, {
            downloadComplete: function (data) {
                UserInfoListSource.totalrecords = data.TotalRows;
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
                  //{ width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '清洗类型', datafield: 'CouponType', width: 200, align: 'center', cellsAlign: 'center', },
                  { text: '清洗车辆', datafield: 'CabLicense', width: 200, align: 'center', cellsAlign: 'center' },
                  { text: '车辆所属公司', datafield: 'CabOrgName', width: 200, align: 'center', cellsAlign: 'center' },
                  { text: '当前驾驶员', datafield: 'CreatedUser', width: 200, align: 'center', cellsAlign: 'center' },
                  { text: '驾驶员所属公司', datafield: 'ManOrgName', align: 'center', width: 200, cellsAlign: 'center' },
                  { text: '操作时间', datafield: 'OperationDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss", },
                  //{ text: '创建人', datafield: 'CreatedUser', align: 'center', cellsAlign: 'center', hidden: true },
                  { text: '创建时间', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss", hidden: true },
                  { text: 'Vguid', datafield: 'Vguid', hidden: true }
                ]
            });
    }

};

$(function () {
    var page = new $page();
    page.init();

})
