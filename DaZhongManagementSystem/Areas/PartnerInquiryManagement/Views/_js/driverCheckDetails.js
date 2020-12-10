var code = getQueryString("code");
var type = getQueryString("type");
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        $("#newAccidentData2").hide();
        if (type == "2") {
            $("#u340").removeClass("selected");
            $("#u341").addClass("selected");
            $("#newViolationData2").hide();
            $("#newAccidentData2").show();
        }
        //未处理违章
        $("#u340").on("click", function () {
            $("#u340").addClass("selected");
            $("#u341").removeClass("selected");
            $("#newViolationData2").show();
            $("#newAccidentData2").hide();
        });
        //处理中事故
        $("#u341").on("click", function () {
            $("#u340").removeClass("selected");
            $("#u341").addClass("selected");
            $("#newViolationData2").hide();
            $("#newAccidentData2").show();
        });
        //加载未处理违章数据
        loadRegulationsInfo();
        //加载处理中事故数据
        loadAccidentInfo();
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
function loadRegulationsInfo(carID) {
    //加载未处理违章数据
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheckDetails/GetElectronicInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#ElectronicCount").text(msg.length);
                createElectronicDiv(msg);
            }
        }
    });
}
function loadAccidentInfo(carID) {
    //加载处理中事故数据
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheckDetails/GetAccidentInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#AccidentCount").text(msg.length);
                createAccidentDiv(msg);
            }
        }
    });
}
function createElectronicDiv(data) {
    var html = "";

    for (var i = 0; i < data.length; i++) {
        var divHide = '<div class="violCheck">';
        //var driverName = data[i].driverName
        var carId = data[i].plate_no;
        var deductPoints = data[i].score;//扣分
        var fine = data[i].amercement;//罚款
        var violationDate = data[i].peccancy_date;
        var violationAddress = data[i].area;
        var violationInfo = data[i].act;
        html += divHide +
                    //'<div class="viollabel">' + carId + '</div>' +
                    '<div class="viollabel">违章日期：' + violationDate + '</div>' +
                    '<div class="viollabel">扣分：' + deductPoints + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;罚款：' + fine + '</div>' +
                    '<div class="viollabel"><span>违章地点：</span>' + violationAddress + '</div>' +
                    '<div class="viollabel"><span>违章行为：</span>' + violationInfo + '</div>' +
                    //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
                    '<hr style="width: 100%;"/>'
               + '</div>' + '</div>'
    }
    $("#newViolationData2").append(html);
}
function createAccidentDiv(data) {
    var html = "";

    for (var i = 0; i < data.length; i++) {
        var driverName = data[i].driverName
        var carId = data[i].carNo;
        var accidentDate = data[i].occurrenceTime;//事故日期
        var accidentNo = data[i].accidentNo;//事故编号
        var accidentLevel = data[i].accidentGradeName;//事故等级
        var accidentType = data[i].accidentTypeName;//事故类型
        var violationAddress = data[i].accidentLocation;//事故地点
        var divHide = '<div class="violCheck">';
        html += divHide +
            //'<div class="viollabel">' + driverName + '&nbsp;&nbsp;' + carId + '</div>' +
            '<div class="viollabel">事故日期：' + accidentDate + '</div>' +
            '<div class="viollabel">事故编号：' + accidentNo + '</div>' +
            '<div class="viollabel">事故等级：' + accidentLevel + '&nbsp;&nbsp;事故类型：' + accidentType + '</div>' +
            '<div class="viollabel">事故地点：' + violationAddress + '</div>' +
            //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
            '<hr style="width: 100%;"/>'
   + '</div>' + '</div>'
    }
    $("#newAccidentData2").append(html);
}