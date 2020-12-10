var code = getQueryString("code");
var fleet = decodeURI(getQueryString("fleet"));
var fleetOne = decodeURI(getQueryString("fleetOne"));
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        //首页跳转
        $("#Page1").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/DriverPointsDetails/Index?code=" + code + "&fleet=" + fleet;//司机计分
        });
        $("#Page2").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/DriverJobTraning/Index?code=" + code + "&fleet=" + fleet;//岗中培训司机数
        });
        //加载车队
        var html = "";
        var fleetList = fleet.split(",");
        if (fleetList.length > 1) {
            html = '<option class="u235_input_option" value="0" selected>全部</option>';
            for (var i = 0; i < fleetList.length; i++) {
                html += '<option class="u235_input_option" value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
            }
        } else {
            html = '<option class="u235_input_option" value="' + fleet + '" selected>' + fleet + '</option>';
        }
        $("#u235_input").append(html);
        $("#u235_input").val(fleetOne);
        //加载司机计分数据
        loadDriverScore();
        //加载岗中培训数据
        loadAnswerStatus();
        $("#u235_input").on("change", function () {
            loadDriverScore();
            loadAnswerStatus();
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
function loadDriverScore() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverManagement/GetDriverScore",
        type: "post",
        data: { fleet: $("#u235_input").val(), code: code },
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg != null) {
                $("#value1").text(msg[0]);
                $("#value2").text(msg[1]);
                $("#value3").text(msg[2]);
            }
        }
    });
}
function loadAnswerStatus() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverManagement/getAnswerStatus",
        type: "post",
        data: { fleet: $("#u235_input").val(), code: code },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#value4").text(msg[0]);
                $("#value5").text(msg[1]);
                $("#value6").text(msg[2]);
            }
        }
    });
}