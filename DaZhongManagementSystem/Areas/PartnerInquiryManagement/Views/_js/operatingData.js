var code = getQueryString("code");
var fleet = decodeURI(getQueryString("fleet"));
var fleetOne = decodeURI(getQueryString("fleetOne"));
var $page = function () {

    this.init = function () {
        newAddEvent();
        //addEvent();
    };
    var selector = this.selector = {};
    function newAddEvent() {
        //加载日期
        loadDate();
        //加载车队
        loadFleet(fleet);
        //加载营运日报数据
        loadTaxiSummaryInfo();
        $("#u164_input").on("change", function () {
            var fleet = $("#u164_input").val();
            loadTaxiSummaryInfo(fleet);
        })
    }
};

$(function () {
    var page = new $page();
    page.init();
});
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}
function loadDate() {
    var day1 = new Date();
    day1.setTime(day1.getTime() - 24 * 60 * 60 * 1000);
    var day = day1.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var s1 = day1.getFullYear() + "-" + (day1.getMonth() + 1) + "-" + day;
    $("#DateSearch").val(s1);
}
function loadFleet(fleet) {
    var html = "";
    var fleetList = fleet.split(",");
    if (fleetList.length > 1) {
        html = '<option class="u164_input_option" value="0" selected>全部</option>';
        for (var i = 0; i < fleetList.length; i++) {
            html += '<option class="u164_input_option" value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
        }
    } else {
        html = '<option class="u164_input_option" value="' + fleet + '" selected>' + fleet + '</option>';
    }
    $("#u164_input").append(html);
    $("#u164_input").val(fleetOne);
}
function loadTaxiSummaryInfo() {
    $.ajax({
        url: "/PartnerInquiryManagement/OperatingData/GetTaxiSummaryInfo",
        type: "post",
        data: { fleet: $("#u164_input").val(), dateSearch: $("#DateSearch").val(), code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg.length > 2) {
                var data = JSON.parse(msg);
                setValueLabel(data);
            } else {
                alert("暂无数据");
            }
        }
    });
}
function setValueLabel(data) {
    //总营收
    $("#value1").text(data.总营收);
    $("#value2").text(data.总营收率);//↑xxx%
    //总差次
    $("#value3").text(data.总差次);
    $("#value4").text(data.总差次率);//↑xxx%
    //营运车辆总数
    $("#value5").text(data.营运车辆总数);
    $("#value6").text(data.营运车辆总数率);//↑xxx%
    //总线上营收
    $("#value7").text(data.总线上营收);
    $("#value8").text(data.总线上营收率);//↑xxx%
    //总线上差次
    $("#value9").text(data.总线上差次);
    $("#value10").text(data.总线上差次率);//↑xxx%
    //车均营收
    $("#value11").text(data.车均营收);
    $("#value12").text(data.车均营收率);//↑xxx%
    //车均差次
    $("#value13").text(data.车均差次);
    $("#value14").text(data.车均差次率);//↑xxx%
    //车均线上营收
    $("#value15").text(data.车均线上营收);
    $("#value16").text(data.车均线上营收率);//↑xxx%
    //车均线上差次
    $("#value17").text(data.车均线上差次);
    $("#value18").text(data.车均线上差次率);//↑xxx%
    //车均行驶里程
    $("#value19").text(data.车均行驶里程);
    $("#value20").text(data.车均行驶里程率);//↑xxx%
    //车均营运里程
    $("#value21").text(data.车均营运里程);
    $("#value22").text(data.车均营运里程率);//↑xxx%
    //车均空驶里程
    $("#value23").text(data.车均空驶里程);
    $("#value24").text(data.车均空驶里程率);//↑xxx%
}