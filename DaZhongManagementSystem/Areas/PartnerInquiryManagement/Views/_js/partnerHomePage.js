var code = "";
var fleet = "";
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        //加载车队
        code = $("#hideCode").val();
        fleet = $("#hideFleet").val();
        var html = "";
        var fleetList = fleet.split(",");
        if (fleetList.length > 1) {
            html = '<option class="u74_input_option" value="0" selected>全部</option>';
            for (var i = 0; i < fleetList.length; i++) {
                html += '<option class="u74_input_option" value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
            }
        } else {
            html = '<option class="u74_input_option" value="' + fleet + '" selected>' + fleet + '</option>';
        }
        $("#u74_input").append(html);
        //首页跳转
        $("#Page1").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/OperatingData/Index?code=" + code + "&fleet=" + fleet;//营运数据
        });
        $("#Page2").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/DriverManagement/Index?code=" + code + "&fleet=" + fleet;//司机管理
        });
        $("#Page3").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CarManagement/Index?code=" + code + "&fleet=" + fleet;//车辆管理
        });
        $("#Page4").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CheckScheduling/Index?code=" + code + "&fleet=" + fleet;//排班检查
        });
        $("#MoreLabel").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/OperatingData/Index?code=" + code + "&fleet=" + fleet;//营运数据,首页"更多"
        });
        $("#u80").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CheckScheduling/Index?code=" + code + "&fleet=" + fleet;//排班检查,首页车辆检查排期"全部"
        });
        $("#u79").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CheckScheduling/Index?code=" + code + "&fleet=" + fleet;//排班检查,车辆检查排期按钮
        });
        $("#u110").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CarManagement/Index?code=" + code + "&fleet=" + fleet;//车辆管理,首页未处理违章"全部"
        });
        $("#u109").on("click", function () {
            window.location.href = "/PartnerInquiryManagement/CarManagement/Index?code=" + code + "&fleet=" + fleet;//车辆管理,未处理违章按钮
        });

        //加载营运日报数据
        loadTaxiSummaryInfo(fleet);
        //加载车辆检车排期
        loadVehicleMaintenanceInfo(fleet);
        //加载未处理违章
        loadCarViolationInfo(fleet);
        $("#u74_input").on("change", function () {
            var fleet = $("#u74_input").val();
            $(".mainCheck").remove();
            $(".violCheck").remove();
            loadTaxiSummaryInfo(fleet);
            loadVehicleMaintenanceInfo(fleet);
            loadCarViolationInfo(fleet);
        })
    }
};

$(function () {
    var page = new $page();
    page.init();
});

function loadTaxiSummaryInfo(fleet) {
    $.ajax({
        url: "/PartnerInquiryManagement/PartnerHomePage/GetTaxiSummaryInfo",
        type: "post",
        data: { fleet: $("#u74_input").val(), code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg != null) {
                var data = JSON.parse(msg);
                //上线司机数
                $("#value1").text(data.上线司机数);
                $("#value2").text(data.上线司机数率);//↑xxx%
                //上线车辆数
                $("#value3").text(data.上线车辆数);
                $("#value4").text(data.上线车辆数率);//↑xxx%
                //完单数--总差次
                $("#value5").text(data.总差次);
                $("#value6").text(data.总差次率);//↑xxx%
                //车均营收
                $("#value7").text(data.车均营收);
                $("#value8").text(data.车均营收率);//↑xxx%
                //车均差次
                $("#value9").text(data.车均差次);
                $("#value10").text(data.车均差次率);//↑xxx%
                //车均在线时长
                $("#value11").text(data.车均在线时长);
                $("#value12").text(data.车均在线时长率);//↑xxx%
            }
        }
    });
}
function loadVehicleMaintenanceInfo(fleet) {
    $.ajax({
        url: "/PartnerInquiryManagement/PartnerHomePage/GetVehicleMaintenanceInfo",
        type: "post",
        data: { fleet: $("#u74_input").val(), code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg.length > 0) {
                createCarCheckDiv(msg);
                $("#u79").show();
            } else {
                $("#u79").hide();
            }
        }
    });
}
function loadCarViolationInfo(fleet) {
    $.ajax({
        url: "/PartnerInquiryManagement/PartnerHomePage/GetCarViolationInfo",
        type: "post",
        data: { fleet: $("#u74_input").val(), code: code },
        dataType: "json",
        //traditional: true,
        success: function (msg) {
            if (msg.length > 0) {
                createCarViolationDiv(msg);
                $("#u109").show();
            } else {
                $("#u109").hide();
            }
        }
    });
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
        if (isStatus == "0") {
            //status = '<div id="" class="u474_div" phone="' + mobilePhone + '" name="' + driverName + '" date="' + maintainDate + '" time="' + maintainTime + '" place="' + maintainAddress + '" lb="' + maintainLevel + '" ' + '" isYanche="' + isYanche + '" ' +
            //'onclick="sendMessage(this)" ' +
            //'style="line-height: 20px;text-align: center;"><span style="">保养提醒</span></div>';
            html += divHide +
                        '<div class="mainlabel">车队：' + fleet + '</div>' + yanche + '<div class="ax_default primary_button2" style="background-color: ' + color + ';"><span style="margin-left: 6px;color: #FFFFFF;">' + maintainLevel + '级保养</span></div>' +
                        '<div class="mainlabel">' + driverName + '&nbsp;&nbsp;&nbsp;' + carId + '</div>' +
                        '<div class="mainlabel"><span>保养时间：</span>' + maintainDate + '&nbsp;' + maintainTime + '</div>' +
                        '<div class="mainlabel"><span>保养地点：</span>' + maintainAddress + '</div>' +
                        //' <img id="imgLine" class="img " src="/_theme/images/hengxian.png">'
                        '<hr />'
                   + '</div>'
        }
    }
    $("#newMaintainData").append(html);
}
function createCarViolationDiv(data) {
    var html = "";
    var divHide = '<div class="violCheck">';
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
                    '<div class="viollabel">' + carId + '&nbsp;&nbsp;&nbsp;&nbsp;违章日期：' + violationDate + '</div>'+
                    '<div class="viollabel">扣分：' + deductPoints + '&nbsp;&nbsp;&nbsp;罚款：' + fine + '</div>' +
                    '<div class="viollabel"><span>违章地点：</span>' + violationAddress + '</div>' +
                    '<div class="viollabel"><span>违章行为：</span>' + violationInfo + '</div>' +
                    //' <img id="imgLine2" class="img " src="/_theme/images/hengxian.png">'
                    '<hr />'
               + '</div>'
    }
    $("#newViolationData").append(html);
}