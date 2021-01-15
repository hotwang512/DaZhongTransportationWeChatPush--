var selector = {
    //表格
    $grid: function () { return $("#datatable") },
    $txtMonth: function () { return $("#txtMonth") },
    $txtChannel: function () { return $("#txtChannel") },
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

        initTable();
        //查询
        selector.$btnSearch().on("click", function () {
            initTable();
        });

        //导出
        selector.$btnExport().on("click", function () {
            var cabOrgName = $("#CabOrgName").val();
            var operationDate = $("#OperationDate").val();
            var couponType = $("#CouponType").val();
            window.location.href = "/ReportManagement/CleaningReport/ExportCleaningInfo?cabOrgName=" + cabOrgName + "&operationDate=" + operationDate + "&couponType=" + couponType;
        })

        function initTable() {
            $.ajax({
                url: "/ReportManagement/CleaningReport/GetCleaningInfo",
                data: { "cabOrgName": $("#CabOrgName").val(), "operationDate": $("#OperationDate").val(), "couponType": $("#CouponType").val() },
                datatype: "json",
                type: "post",
                success: function (mps) {
                    if (mps.length > 0) {
                        var utils = $.pivotUtilities;
                        var heatmap = utils.renderers["Heatmap"];
                        var sumOverSum = utils.aggregators["Count"];
                        $("#datatable").pivot(mps, {
                            rows: ["Description"],
                            cols: ["CabOrgName", "CouponType"],
                            //aggregatorName: "Sum",
                            aggregator: sumOverSum(["Account"]),
                            //vals: ["Money"],
                        });

                        $(".pvtAxisLabel").eq(0).text("所属公司");
                        $(".pvtAxisLabel").eq(0).css("text-align", "center")
                        $(".pvtAxisLabel").eq(1).text("清洗类型");
                        $(".pvtAxisLabel").eq(1).css("text-align", "center")
                        $(".pvtAxisLabel").eq(2).text("月份");
                        $(".pvtAxisLabel").eq(2).css("text-align", "center")
                        $(".pvtRowLabel").css("text-align", "center")
                        $("#datatable").show();

                    } else {
                        jqxNotification("当前公司没有数据！", null, "error");
                    }
                }
            });
        }

    }; //addEvent end

};

$(function () {
    var page = new $page();
    page.init();

});