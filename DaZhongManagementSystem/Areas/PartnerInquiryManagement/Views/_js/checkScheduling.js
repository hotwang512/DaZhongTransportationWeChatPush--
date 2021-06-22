var code = getQueryString("code");
var fleet = decodeURI(getQueryString("fleet"));
var fleetOne = decodeURI(getQueryString("fleetOne"));
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        //加载车队
        var html = "";
        var fleetList = fleet.split(",");
        if (fleetList.length > 1) {
            html = '<option  value="0" selected>全部-车队</option>';
            for (var i = 0; i < fleetList.length; i++) {
                html += '<option value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
            }
        } else {
            html = '<option value="' + fleet + '" selected>' + fleet + '</option>';
        }
        $("#FleetSelect").append(html);
        $("#FleetSelect").val(fleetOne);
        //加载保养数据
        loadVehicleMaintenance();
        $("#SearchByCarID").on("change", function () {
            var carID = $("#SearchByCarID").val();
            $(".mainCheck").remove();
            loadVehicleMaintenance(carID);
        });
        $("#MaintainType").on("change", function () {
            $("#SearchByCarID").val("");
            $(".mainCheck").remove();
            loadVehicleMaintenance();
        });
        $("#MaintainStatus").on("change", function () {
            $("#SearchByCarID").val("");
            $(".mainCheck").remove();
            loadVehicleMaintenance();
        });
        $("#FleetSelect").on("change", function () {
            $("#SearchByCarID").val("");
            $(".mainCheck").remove();
            loadVehicleMaintenance();
        });;
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
function loadVehicleMaintenance(carID) {
    //加载处理中事故数据
    $.ajax({
        url: "/PartnerInquiryManagement/CheckScheduling/GetVehicleMaintenanceInfo",
        type: "post",
        data: { fleet: $("#FleetSelect").val(), type: $("#MaintainType").val(), status: $("#MaintainStatus").val(), idCard: carID, code: code + "K" },
        dataType: "json",
        success: function (msg) {
            if (msg != null) {
                createMaintainDiv(msg);
            } else {
                alert("暂无数据");
            }
        }
    });
}

function createMaintainDiv(data) {
    var html = "";
    var divHide = '';
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
        var mobilePhone = $.trim(data[i].MobilePhone);//手机号
        var isMaintenance = $.trim(data[i].IsMaintenance);//是否已保养
        var color = "";
        var yanche = "";
        var baoyang = "";
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
        if (isMaintenance == "1") {
            baoyang = '<div class="ax_default primary_button2" style="background-color: red;margin-left: 2px;">' +
                    '<span style="margin-left: 10px;color: #FFFFFF;">已保养</span></div>'
        }
        if (isStatus == "0") {
            if (isMaintenance == "0") {
                status = '<div id="" class="u474_div" phone="' + mobilePhone + '" name="' + driverName + '" date="' + maintainDate + '" time="' + maintainTime + '" place="' + maintainAddress + '" lb="' + maintainLevel + '" ' + '" isYanche="' + isYanche + '" ' +
                'onclick="sendMessage(this)" ' +
                'style="line-height: 20px;text-align: center;"><span style="">保养提醒</span></div>';
            }

            html += '<div class="mainCheck">' +
                        '<div class="mainlabel">车队：' + fleet + '</div>' + yanche + baoyang + '<div class="ax_default primary_button2" style="background-color: ' + color + ';">' +
                        '<span style="margin-left: 6px;color: #FFFFFF;">' + maintainLevel + '级保养</span></div>' +
                        '<div class="mainlabel">' + driverName + '&nbsp;&nbsp;&nbsp;' + carId + '</div>' +
                        '<div class="mainlabel"><span>保养时间：</span>' + maintainDate + '&nbsp;' + maintainTime + '</div>' + status +
                        '<div class="mainlabel"><span>保养地点：</span>' + maintainAddress + '</div>' +
                        //' <img id="imgLine" class="img " src="/_theme/images/hengxian.png">'
                        '<hr />'
                   + '</div>'
        }
    }
    $("#newMaintainData2").append(html);
}

function sendMessage(event) {
    var mobilePhone = event.getAttribute("phone");
    var Name = event.getAttribute("name");
    var Date = event.getAttribute("date");
    var Time = event.getAttribute("time");
    var Place = event.getAttribute("place");
    var lb = event.getAttribute("lb");
    var isYanche = event.getAttribute("isYanche");
    var lbName = lb + "级";
    if (isYanche == "1") {
        lbName = lb + "级验车";
    }
    //mobilePhone = "18936495119";
    var key = $("#hideKey").val();
    var param = {
        mobile: mobilePhone,
        temp_id: "186192",
        temp_para: { Name: Name, Date: $.trim(Date), Time: $.trim(Time), Place: Place, lb: lbName }
    };
    var jsonStr = JSON.stringify(param);
    $.ajax({
        url: "/API/NotificationSMS",
        type: "post",
        data: { SECURITYKEY: key, pushparam: jsonStr },
        dataType: "json",
        success: function (msg) {
            if (msg.Success == true) {
                alert("发送成功");
            } else {
                alert("发送失败");
            }
        }
    });
}