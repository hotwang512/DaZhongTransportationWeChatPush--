var code = "";
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        code = $("#hideCode").val();
        $("#u543").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/DriverCheckDetails/Index?code=" + code + "&type=1"//营运数据
        });
        $("#u563").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/DriverCheckDetails/Index?code=" + code + "&type=2"//营运数据
        });
        //获取前一天日期
        var day1 = new Date();
        day1.setTime(day1.getTime() - 24 * 60 * 60 * 1000);
        var day = day1.getDate();
        if (day < 10) {
            day = "0" + day;
        }
        var s1 = day1.getFullYear() + "-" + (day1.getMonth() + 1) + "-" + day;
        $("#DateTime").text(s1);
        //加载基础服务数据
        loadBaseData();
        //加载营运日报数据
        loadTaxiSummaryInfo();
        //加载未处理违章
        loadCarViolationInfo();
        //加载处理中事故数据
        loadAccidentInfo();
        //加载保养数据
        loadVehicleMaintenanceInfo();
    }
};

$(function () {
    var page = new $page();
    page.init();
});

function loadTaxiSummaryInfo() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheck/GetTaxiSummaryInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg != null) {
                var data = JSON.parse(msg);
                setValueLabel(data);
            } else {
                //jqxNotification("未查询到数据！", null, "error");
            }
        }
    });
}
function loadCarViolationInfo() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheck/GetCarViolationInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            createCarViolationDiv(msg);
        }
    });
}
function loadAccidentInfo() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheck/GetAccidentInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        success: function (msg) {
            createAccidentDiv(msg);
        }
    });
}
function loadBaseData() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheck/GetBaseData",
        type: "post",
        data: { code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg != null) {
                var score = parseInt(msg[0]);
                if (score >= 8) {
                    $("#Score").css("color", "red");
                }
                $("#Score").text(score);
                var value = msg[1];
                var value1 = "";
                if (value != "合格" && value != "不合格") {
                    value1 = "未阅";
                } else {
                    if (value == "合格") {
                        value1 = "已答题";
                    }
                    if (value == "不合格") {
                        value1 = "未答题";
                    }
                }
                $("#AnswerStatus").text(value1);
                $("#month1").text(msg[2]);
                $("#month2").text(msg[2]);
                $("#Name").text(msg[3]);
                $("#CabLicense").text(msg[4]);
                $("#MotorcadeName").text(msg[5]);
            } else {
                //jqxNotification("未查询到数据！", null, "error");
            }
        }
    });
}
function loadVehicleMaintenanceInfo() {
    $.ajax({
        url: "/PartnerInquiryManagement/DriverCheck/GetVehicleMaintenanceInfo",
        type: "post",
        data: { code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg.length > 0) {
                createCarCheckDiv(msg);
            } 
        }
    });
}

function setValueLabel(data) {
    //总营收
    $("#value1").text(data.营收);
    $("#value2").text(data.营收率);//↑xxx%
    //总差次
    $("#value3").text(data.差次);
    $("#value4").text(data.差次率);//↑xxx%
    //总线上营收
    $("#value5").text(data.线上营收);
    $("#value6").text(data.线上营收率);//↑xxx%
    //总线上差次
    $("#value7").text(data.线上差次);
    $("#value8").text(data.线上差次率);//↑xxx%
}
function createCarViolationDiv(data) {
    var html = "";
    var divHide = '<div class="violCheck">';
    if (data.length == 0) {
        html += divHide +
                   '<div class="viollabel">没有违章记录</div>' 
    } else {
        for (var i = 0; i < data.length; i++) {
            //var driverName = data[i].driverName
            var carId = data[i].plate_no;
            var deductPoints = data[i].score;//扣分
            var fine = data[i].amercement;//罚款
            var violationDate = data[i].peccancy_date;
            var violationAddress = data[i].area;
            var violationInfo = data[i].act;
            if (i >= 2) {
                divHide = '<div class="hide">'
            }
            html += divHide +
                        '<div class="viollabel">违章日期：' + violationDate + '</div>' +
                        '<div class="viollabel">扣分：' + deductPoints + '</div>' +
                        '<div class="viollabel">罚款：' + fine + '</div>' +
                        '<div class="viollabel"><span>违章地点：</span>' + violationAddress + '</div>' +
                        '<div class="viollabel"><span>违章行为：</span>' + violationInfo + '</div>' +
                        //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
                        '<hr style="width: 100%;margin-top: 5px;border-top: 1px solid #aaa;"/>'
                   + '</div>'
        }
    }
    $("#newViolationData").append(html);
}
function createAccidentDiv(data) {
    var html = "";
    var divHide = '<div class="violCheck">';
    if (data.length == 0) {
        html += divHide +
                   '<div class="viollabel hide">没有处理中事故</div>'
    } else {
        for (var i = 0; i < data.length; i++) {
            var accidentDate = data[i].occurrenceTime;//事故日期
            var accidentNo = data[i].accidentNo;//事故编号
            var accidentLevel = data[i].accidentGradeName;//事故等级
            var accidentType = data[i].accidentTypeName;//事故类型
            var violationAddress = data[i].accidentLocation;//事故地点
            html += divHide +
                '<div class="viollabel">事故日期：' + accidentDate + '</div>' +
                '<div class="viollabel">事故编号：' + accidentNo + '</div>' +
                '<div class="viollabel">事故等级：' + accidentLevel + '</div>' +
                '<div class="viollabel">事故类型：' + accidentType + '</div>' +
                '<div class="viollabel">事故地点：' + violationAddress + '</div>' +
                //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
                 '<hr style="width: 100%;margin-top: 5px;border-top: 1px solid #aaa;"/>'
       + '</div>'
        }
    }
    $("#newAccidentData").append(html);
}
function createCarCheckDiv(data) {
    var html = "";
    var divHide = '<div class="mainCheck">';
    for (var i = 0; i < data.length; i++) {
        var fleet = data[i].MotorcadeName //车队
        var driverName = data[i].Name
        var carId = data[i].CabLicense;
        var maintainDate = data[i].Date;
        var maintainTime = data[i].Time;
        var maintainAddress = data[i].Address;
        var maintainLevel = $.trim(data[i].MaintenanceType);
        var isYanche = $.trim(data[i].Yanche);//是否验车
        var isStatus = $.trim(data[i].Status);//是否过期
        var color = "";
        var yanche = "";
        var status = "";
        switch (maintainLevel) {
            case "1": color = "rgba(0, 191, 191, 1)"; break;
            case "2": color = "rgb(22 213 139)"; break;
            case "3": color = "rgba(22, 155, 213, 1)"; break;
            default: break;
        }
        if (isYanche == "1") {
            yanche = '<div class="ax_default primary_button2" style="background-color: rgba(184, 116, 26, 1);margin-left: 2px;">' +
                    '<span style="margin-left: 16px;color: #FFFFFF;">验车</span></div>'
        }
        if (isStatus == "1") {
            $("#u571").show();
        }
        html += divHide +
                        //'<div class="mainlabel">' + carId + '</div>' +
                        yanche + '<div class="ax_default primary_button2" style="background-color: ' + color + ';"><span style="margin-left: 6px;color: #FFFFFF;">' + maintainLevel + '级保养</span></div>' +
                        //'<div class="mainlabel">' + driverName + '&nbsp;&nbsp;&nbsp;' + carId + '</div>' +
                        '<div class="mainlabel"><span>保养时间：</span>' + maintainDate + '&nbsp;' + maintainTime + '</div>' +
                        '<div class="mainlabel"><span>保养地点：</span>' + maintainAddress + '</div>' +
                        //'<hr />'+ 
                   '</div>'
    }
    $("#newMaintainData").append(html);
}