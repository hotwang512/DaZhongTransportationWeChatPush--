var docEl = document.documentElement,
    resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
    recalc = function () {
        //设置根字体大小
        var myfontSize = 20 * (docEl.clientWidth / 375);
        if (myfontSize > 30) {
            docEl.style.fontSize = "30px";
        } else {
            docEl.style.fontSize = 20 * (docEl.clientWidth / 375) + 'px';
        }
        //myScroll.refresh();
    };
//绑定浏览器缩放与加载时间
window.addEventListener(resizeEvt, recalc, false);
document.addEventListener('DOMContentLoaded', recalc, false);


if (parseFloat($(".n5_num").text()) >= 0) {
    $(".f_nav").hide();
    $("#paymentTitle").hide();
    $("#paymentContent").hide();
    $("#spDriverPay").hide();
} else {
    $(".f_nav").show();
    $("#paymentTitle").show();
    $("#paymentContent").show();
    $("#spDriverPay").show();
}

var fee = $("#txtDriverPay").val();
if (fee == "0%" || fee == "") {
    $("#spDriverPay").hide();
} else {

    var span = '<div style="font-size:32px;float:left;padding:0 5px 0 5px;">' + fee + '</div>';
    $("#spDriverPay").html("<div style='float:left;line-height:45px;'>实际缴费金额包含微信手续费</div>" + span + "<div style='float:left;line-height:45px;'>!<div>");

}

//点击微信支付按钮
//$(".btn_wc").on("click", function () {
//    $(".btn_wc").attr({ "disabled": "disabled" });
//    $.ajax({
//        url: "/WeChatPush/WeChatRevenue/IsValid",
//        data: { pushContentVguid: $("#txtpushContentVguid").val() },
//        type: "post",
//        dataType: "json",
//        success: function (msg) {
//            switch (msg.respnseInfo) {
//                case "1":
//                    alert("消息已过有效期");
//                    break;
//                case "0":
//                    if ($("#txtPayException").val() == "1") {
//                        alert("车牌号为空，支付失败！");
//                    } else {
//                        var revenue = parseFloat($(".n7_num").text());
//                        var total_fee = parseFloat($(".n8_num").text());

//                        startWxPay(revenue, total_fee);
//                    }

//                    break;
//            }
//        }
//    });
//});

//进页面调接口获取支付连接地址
var urlPay = "";
var billNo = "";
//startWxPay2();
//点击其他支付
$(".btn_qt").on("click", function () {
    //alert("功能暂未开通");
    //  window.location.href = "/WeChatPush/WeChatRevenue/AliPay";

    //$(".btn_qt").attr({ "disabled": "disabled" });
    //$("#txtpushContentVguid").val("83F2B171-7C89-477A-B438-0000C249BDFF");
    $(".btn_qt").val("支付调转中...");
    startWxPay2();
    //$.ajax({
    //    url: "/WeChatPush/WeChatRevenue/IsValid",
    //    data: { pushContentVguid: $("#txtpushContentVguid").val(), billNo: billNo },
    //    type: "post",
    //    dataType: "json",
    //    success: function (msg) {
    //        switch (msg.respnseInfo) {
    //            case "1":
    //                alert("消息已过有效期");
    //                break;
    //            case "0":
    //                if ($("#txtPayException").val() == "1") {
    //                    alert("车牌号为空，支付失败！");
    //                } else {
    //                    //var revenue = parseFloat($(".n7_num").text());
    //                    //var total_fee = parseFloat($(".n8_num").text());
    //                    //startWxPay(revenue, total_fee);
    //                    //跳转进入支付界面
    //                    saveAndNavigation();

    //                }

    //                break;
    //        }
    //    }
    //});
});

//function startWxPay(revenue, total_fee) {
//    $.ajax({
//        type: "POST",
//        url: "/WeChatPush/WeChatRevenue/GetPaySign",
//        data: { openid: $("#txtOpenId").val(), revenueFee: revenue, totalFee: total_fee, personVguid: $("#txtVguid").val(), pushContentVguid: $("#txtpushContentVguid").val(), revenueType: 2 },
//        beforeSend: function () {
//            //$(".btn_wc").attr({ "disabled": "disabled" });
//        },
//        success: function (res) {
//            if (res.success == true) {
//                //$(".btn_wc").removeAttr("disabled");
//                callpay(res);
//            } else {
//                alert("写入支付信息失败，请重试!")
//            }
//        }
//    });
//}
function startWxPay2() {
    $.ajax({
        type: "POST",
        url: "/WeChatPush/WeChatRevenue/GetPaySignWX",
        data: { driverId: $("#txtDriverId").val(), organizationId: $("#txtOrganizationId").val(), revenueFee: parseFloat($(".n7_num").text()), personVguid: $("#txtVguid").val(), pushContentVguid: $("#txtpushContentVguid").val(), revenueType: 2 },
        beforeSend: function () {
            //$(".btn_wc").attr({ "disabled": "disabled" });
        },
        success: function (res) {
            if (res.isSuccess == true) {
                //$(".btn_wc").removeAttr("disabled");
                //callpay(res);
                var data = res.respnseInfo.split(",");
                urlPay = data[0];
                billNo = data[1];
                window.location.href = urlPay;
                //saveAndNavigation();
            } else {
                //alert("扫码处理失败，请重试！")
                alert(res.respnseInfo);
            }
        }
    });
}
function saveAndNavigation() {
    $.ajax({
        type: "POST",
        url: "/WeChatPush/WeChatRevenue/SavePaymentHistory",
        data: { driverId: $("#txtDriverId").val(), revenueFee: parseFloat($(".n7_num").text()), personVguid: $("#txtVguid").val(), pushContentVguid: $("#txtpushContentVguid").val(), revenueType: 2, billNo: billNo },
        async: false,
        success: function (res) {
            if (res.success == true) {
                window.location.href = urlPay;
            } else {
                //alert("扫码处理失败，请重试！")
            }
        }
    });
}

//jsapi付款
//function jsApiCall(res) {
//    WeixinJSBridge.invoke(
//        'getBrandWCPayRequest', {
//        "appId": res.data.appId,     //公众号名称，由商户传入     
//        "timeStamp": res.data.timeStamp,         //时间戳，自1970年以来的秒数     
//        "nonceStr": res.data.nonceStr, //随机串     
//        "package": res.data.package,
//        "signType": "MD5",         //微信签名方式： MD5    
//        "paySign": res.data.paySign //微信签名 
//    },
//        function (res) {
//            if (res.err_msg == "get_brand_wcpay_request:ok") {
//                //  AddPaymentHistory();
//                $(".btn_wc").removeAttr("disabled");
//                WeixinJSBridge.call('closeWindow');  //关闭窗口
//            }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回ok，但并不保证它绝对可靠。 
//            else if (res.err_msg == "get_brand_wcpay_request:fail") {
//                alert(res.err_desc);
//            }
//        }
//    );
//}



////初始化微信支付环境
//function callpay(res) {
//    if (typeof WeixinJSBridge == "undefined") {
//        if (document.addEventListener) {
//            document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
//        }
//        else if (document.attachEvent) {
//            document.attachEvent('WeixinJSBridgeReady', jsApiCall);
//            document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
//        }
//    }
//    else {
//        jsApiCall(res);
//    }
//}
