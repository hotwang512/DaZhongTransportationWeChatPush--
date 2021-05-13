var code = $("#UserVGUID").val();
var vguid = getQueryString("VGUID");//公司VGUID
    vguid = "153dd969-ab80-441c-b881-4dba7120748e";//测试
var phoneNumber = $("#PhoneNumber").val();
var isCleaning = false;
var type = "";//清洗类型
var description = "";//清洗类型描述
var remark = "";
var cleaningDate = "";
var $page = function () {

    this.init = function () {
        addLocation();
        addEvent();
    };
    var selector = this.selector = {};

    function addLocation() {
        loadCompanyLocation();
        loadWXAPPIDInfo();
    }
    function addEvent() {
        loadIsCleaning();
        //二级清洗
        $("#Page1").on("click", function () {
            if (isCleaning == true) {
                alert("您当月免费清洗次数已用完,请与老板联系自主清洗!");
            } else {
                var con = confirm("是否使用免费次数进行清洗!");
                if (con == true) {
                    type = "1";
                    description = "二级清洗";
                    remark = "清洗";
                    loadCleaning(type, description);
                }
            }
        });
        //座位套更换
        $("#Page2").on("click", function () {
            type = "2";
            description = "座位套更换(部份)";
            remark = "更换";
            loadCleaning(type, description);
        });
        //座位套全套更换
        $("#Page3").on("click", function () {
            type = "3";
            description = "座位套更换(全套)";
            remark = "更换";
            loadCleaning(type, description);
        });
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

function loadCompanyLocation() {
    $.ajax({
        url: "/SecondaryCleaningManagement/CleaningTypePage/GetCompanyLocation",
        data: { vguid: vguid },
        //traditional: true,
        type: "post",
        success: function (msg) {
            if (msg != null) {
                $("#CompanyName").val(msg.CompanyName);
                $("#Location").val(msg.Location);
                $("#Radius").val(msg.Radius);
            }
        }
    })
}
function loadWXAPPIDInfo() {
    $.ajax({
        url: "/SecondaryCleaningManagement/CleaningTypePage/GetWXAPPIDInfo",
        data: { url: location.href.split("#")[0] },
        //traditional: true,
        type: "post",
        success: function (msg) {
            if (msg != null) {
                loadLocation(msg);
            }
        }
    })
}
function loadLocation(wxData) {
    var latitude = "";
    var longitude = "";
    wx.config({
        beta: true,// 必须这么写，否则wx.invoke调用形式的jsapi会有问题
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: wxData.appid, // 必填，企业微信的corpID
        timestamp: wxData.timestamp, // 必填，生成签名的时间戳
        nonceStr: wxData.nonce_str, // 必填，生成签名的随机串
        signature: wxData.sign2,// 必填，签名
        jsApiList: ['getLocation'] // 必填，需要使用的JS接口列表
    });
    //wx.agentConfig({
    //    corpid: wxData.appid, // 必填，企业微信的corpid，必须与当前登录的企业一致
    //    agentid: '10', // 必填，企业微信的应用id （e.g. 1000247）
    //    timestamp: wxData.timestamp, // 必填，生成签名的时间戳
    //    nonceStr: wxData.nonce_str, // 必填，生成签名的随机串
    //    signature: wxData.sign,// 必填，签名，见附录-JS-SDK使用权限签名算法
    //    jsApiList: ['getNetworkType'], //必填
    //    success: function (res) {
    //        var v = res;
    //        // 回调
    //    },
    //    fail: function (res) {
    //        if (res.errMsg.indexOf('function not exist') > -1) {
    //            alert('版本过低请升级')
    //        }
    //    }
    //});
    wx.checkJsApi({
        jsApiList: ['getLocation'],
        success: function (res) {
            if (res.checkResult.getLocation == false) {
                //alert('你的微信版本太低，不支持微信JS接口，请升级到最新的微信版本！');
                $("#ClearHideDiv").show();
                $("#ClearShowDiv").hide();
                $("#ClearHideDiv").html("你的微信版本太低，不支持微信JS接口，请升级到最新的微信版本！");
                return;
            } else {
                //alert("验证成功");
            }
        }
    });
    wx.ready(function () {
        //获取微信定位
        getLocation();
    });
    //wx.error(function (res) {
    //    // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
    //    var err = res;
    //    alert("config错误信息：" + res);
    //});
}
function getLocation() {
    //alert("开始获取定位");
    wx.getLocation({
        type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
        success: function (res) {
            var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
            var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
            //var speed = res.speed; // 速度，以米/每秒计
            //var accuracy = res.accuracy; // 位置精度
            //alert(latitude + "," + longitude);
            console.log(latitude + "," + longitude);
            //获取到经纬度之后,与百度地图上所设定的范围比较
            $("#Hidelat").val(latitude);
            $("#Hidelon").val(longitude);
            var isCheck = checkIsPointInCircle(latitude, longitude);
            //-=匹配          alert("isCheck:" + isCheck);
            if (!isCheck) {
                $("#ClearHideDiv").show();
                $("#ClearShowDiv").hide();
                $("#ClearHideDiv").html("定位超出所配置范围");
            } else {
                $("#ClearHideDiv").hide();
                $("#ClearShowDiv").show();
            }
        },
        cancel: function (res) {
            $("#ClearHideDiv").show();
            $("#ClearShowDiv").hide();
            $("#ClearHideDiv").html("用户拒绝授权获取地理位置");
            //alert("获取定位失败");
        },
        fail: function (res) {
            alert(JSON.stringify(res));//跳转的话会进入到这里
        }
    });
}
function checkIsPointInCircle(latitude, longitude) {
    var isIn = false;
    $.ajax({
        url: "/SecondaryCleaningManagement/CleaningTypePage/IsPointInCircle",
        data: { companyVGUID: vguid, lat: latitude, lon: longitude },
        //data: { companyVGUID: "0146dc3a-6752-436f-9c03-a61d4a4ebd7f", lat: "31.23651123046875", lon: "121.38680267333984" },
        type: "post",
        async: false,
        success: function (msg) {
            isIn = msg
        },
        error: function (xhr, textStatus, errorThrown) {
            /*错误信息处理*/
            //当前状态,0-未初始化，1-正在载入，2-已经载入，3-数据进行交互，4-完成。
            //alert("进入error---" + "状态码：" + xhr.status + "状态:" + xhr.readyState + "错误信息:" + xhr.statusText + "请求状态：" + textStatus + errorThrown);
            //alert("返回响应信息：" + xhr.responseText);//这里是详细的信息
        }
    })
    return isIn;
}
function loadIsCleaning() {
    $.ajax({
        url: "/SecondaryCleaningManagement/CleaningTypePage/GetIsClearing",
        data: { code: code },
        type: "post",
        success: function (msg) {
            if (msg.isSuccess == true) {
                //当月存在清洗记录
                $("#SClean").hide();
                isCleaning = true;
            } else {
                $("#SClean").show();
                //alert("当月不存在清洗记录");
            }
            //返回当前车牌号
            //alert(msg.respnseInfo);
        }
    })
}
function loadCleaning(type, description) {
    var lat = $("#Hidelat").val();
    var lon = $("#Hidelon").val();
    var WXlocation = lat + "," + lon;
    $.ajax({
        url: "/SecondaryCleaningManagement/CleaningTypePage/SaveCleaningInfo",
        data: { companyVGUID: vguid, location: WXlocation, type: type, description: description, code: code },
        type: "post",
        success: function (msg) {
            if (msg.isSuccess == true) {
                //WindowConfirmDialog(funcOK, "已核销,请前往清洗", "确认框", "确定", "取消");
                cleaningDate = msg.respnseInfo;
                show_confirm(type);
            } else {
                alert("操作失败,请重新扫码!");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            /*错误信息处理*/
            //当前状态,0-未初始化，1-正在载入，2-已经载入，3-数据进行交互，4-完成。
            alert("进入error---" + "状态码：" + xhr.status + "状态:" + xhr.readyState + "错误信息:" + xhr.statusText + "请求状态：" + textStatus + errorThrown);
            alert("返回响应信息：" + xhr.responseText);//这里是详细的信息
        }
    })
}
function show_confirm(type) {
    var r = "";
    if (type == "1") {
        r = confirm("已核销,凭推送信息前往清洗");
    } else {
        r = confirm("已核销,凭推送信息前往更换");
    }
    if (r == true) {
        wx.closeWindow();
    }
    else {
        wx.closeWindow();
    }
    sendWXMessage();
    //sendMessage();
}
function sendWXMessage() {
    var key = $("#Key").val();
    var userId = $("#UserId").val();
    var companyName = $("#CompanyName").val();
    var param = {
        Title: "二级清洗",
        Message: "您已于" + cleaningDate + "，在" + companyName + "进行" + description + "扫码操作，请前往进行" + remark + "",
        Url: "",
        Image: "",
        PushPeople: null,
        PushPeoples: userId,
        founder: userId
    };
    var jsonStr = JSON.stringify(param);
    $.ajax({
        url: "/API/WXTextPush",
        type: "post",
        data: { SECURITYKEY: key, pushparam: jsonStr },
        dataType: "json",
        success: function (msg) {
            //if (msg.Success == true) {
            //    alert("发送成功");
            //} else {
            //    alert("发送失败");
            //}
        }
    });
}
function sendMessage(event) {
    var key = $("#hideKey").val();
    var param = {
        mobile: phoneNumber,
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
            //if (msg.Success == true) {
            //    alert("发送成功");
            //} else {
            //    alert("发送失败");
            //}
        }
    });
}
