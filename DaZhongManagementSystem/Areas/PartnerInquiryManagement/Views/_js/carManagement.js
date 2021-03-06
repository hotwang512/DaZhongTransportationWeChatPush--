﻿var code = getQueryString("code");
var fleet = decodeURI(getQueryString("fleet"));
var fleetOne = decodeURI(getQueryString("fleetOne"));
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        $("#newAccidentData2").hide();
        $("#newOperationData2").hide();
        //var height = window.innerHeight - $("#carDiv").height();
        //$("#newViolationData2").css("height", height);
        //加载车队
        var html = "";
        var fleetList = fleet.split(",");
        if (fleetList.length > 1) {
            html = '<option class="u416_input_option" value="0" selected>全部</option>';
            for (var i = 0; i < fleetList.length; i++) {
                html += '<option class="u416_input_option" value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
            }
        } else {
            html = '<option class="u416_input_option" value="' + fleet + '" selected>' + fleet + '</option>';
        }
        $("#u416_input").append(html);
        $("#u416_input").val(fleetOne);
        //未处理违章
        $("#u340").on("click", function () {
            $("#u340").addClass("selected");
            $("#u341").removeClass("selected");
            $("#u342").removeClass("selected");
            $("#newViolationData2").show();
            $("#newAccidentData2").hide();
            $("#newOperationData2").hide();
            $("#DateDiv").show();//日期标签
        });
        //处理中事故
        $("#u341").on("click", function () {
            $("#u340").removeClass("selected");
            $("#u341").addClass("selected");
            $("#u342").removeClass("selected");
            $("#newViolationData2").hide();
            $("#newAccidentData2").show();
            $("#newOperationData2").hide();
            $("#DateDiv").show();//日期标签
        });
        //搁车明细
        $("#u342").on("click", function () {
            $("#u340").removeClass("selected");
            $("#u341").removeClass("selected");
            $("#u342").addClass("selected");
            $("#newViolationData2").hide();
            $("#newAccidentData2").hide();
            $("#newOperationData2").show();
            $("#DateDiv").hide();//日期标签
        });
        //加载未处理违章数据
        loadRegulationsInfo();
        //加载处理中事故数据
        loadAccidentInfo();
        //加载搁车明细数据
        loadOperationInfo();
        $("#SearchByCarID").on("change", function () {
            var carID = $("#SearchByCarID").val();
            $(".violCheck").remove();
            loadRegulationsInfo(carID);
            loadAccidentInfo(carID);
            loadOperationInfo(carID);
        })
        $("#u416_input").on("change", function () {
            $("#SearchByCarID").val("");
            var fleet = $("#u164_input").val();
            $(".violCheck").remove();
            loadRegulationsInfo();
            loadAccidentInfo();
            loadOperationInfo();
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
function loadInfo() {
    var carID = $("#SearchByCarID").val();
    $(".violCheck").remove();
    loadRegulationsInfo(carID);
    loadAccidentInfo(carID);
    loadOperationInfo(carID);
}
function loadRegulationsInfo(carID) {
    //加载未处理违章数据
    $.ajax({
        url: "/PartnerInquiryManagement/CarManagement/GetElectronicInfo",
        type: "post",
        data: { fleet: $("#u416_input").val(), date: $("#DateSearch").val(), carID: carID, code: code+"K" },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#ElectronicCount").text(msg.length);
                createElectronicDiv(msg);
            } else {
                $("#ElectronicCount").text(msg.length);
            }
        }
    });
}
function loadAccidentInfo(carID) {
    //加载处理中事故数据
    $.ajax({
        url: "/PartnerInquiryManagement/CarManagement/GetAccidentInfo",
        type: "post",
        data: { fleet: $("#u416_input").val(), date: $("#DateSearch").val(), carID: carID, code: code+"K" },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#AccidentCount").text(msg.length);
                createAccidentDiv(msg);
            } else {
                $("#AccidentCount").text(msg.length);
            }
        }
    });
}
function loadOperationInfo(carID) {
    //加载搁车明细数据
    $.ajax({
        url: "/PartnerInquiryManagement/CarManagement/GetOperationInfo",
        type: "post",
        data: { fleet: $("#u416_input").val(), date: $("#DateSearch").val(), carID: carID, code: code+"K" },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                $("#OperationCount").text(msg.length);
                createOperationDiv(msg);
            } else {
                $("#OperationCount").text(msg.length);
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
                    '<div class="viollabel" style="font-weight: bold;">扣分：' + deductPoints + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;罚款：' + fine + '</div>' +
                    '<div class="viollabel">' + carId + '</div>' +
                    '<div class="viollabel">违章日期：' + violationDate + '</div>' +
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
            '<div class="viollabel" style="font-weight: bold;font-size: 14px;">' + accidentLevel + '—' + accidentType + '</div>' +
            '<div class="viollabel">' + driverName + '&nbsp;&nbsp;' + carId + '</div>' +
            '<div class="viollabel">事故日期：' + accidentDate + '</div>' +
            '<div class="viollabel">事故编号：' + accidentNo + '</div>' +
            //'<div class="viollabel">事故等级：' + accidentLevel + '&nbsp;&nbsp;事故类型：' + accidentType + '</div>' +
            '<div class="viollabel">事故地点：' + violationAddress + '</div>' +
            //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
            '<hr style="width: 100%;"/>'
   + '</div>' + '</div>'
    }
    $("#newAccidentData2").append(html);
}
function createOperationDiv(data) {
    var html = "";
    var days = "";
    var divHide = '<div class="violCheck">';
    for (var i = 0; i < data.length; i++) {
        var carId = data[i].CabLicense;
        var fleet = data[i].Motorcade;
        html += divHide +
            //'<div class="viollabel">' + carId + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;搁车时长：' + days + '天</div>' +
            '<div class="viollabel">车牌：' + carId + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车队：' + fleet + '</div>' +
            '<hr style="width: 100%;"/>'
   + '</div>'
    }
    $("#newOperationData2").append(html);
}