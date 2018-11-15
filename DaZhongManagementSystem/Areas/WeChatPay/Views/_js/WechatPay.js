
//所有元素选择器
var selector = {
    $btnWeChatPay: function () { return $("#btnWeChatPay"); },
    $txtOpenId: function () { return $("#txtOpenId"); }
}; //selector end




var $page = function () {

    this.init = function () {
        addEvent();
    }



    //所有事件
    function addEvent() {
        var openid = selector.$txtOpenId().val();

        //点击微信付款
        selector.$btnWeChatPay().on('click', function () {
            startWxPay();
        });
        function startWxPay() {
            $.ajax({
                type: "POST",
                url: "/WeChatPay/WechatPay/GetPaySign",
                data: { openid: openid }, 
                beforeSend: function () {
                    selector.$btnWeChatPay().attr({ "disabled": "disabled" });
                },
                success: function (res) {
                    selector.$btnWeChatPay().removeAttr("disabled");
                    if (res.openid != null && res.openid != undefined && res.openid != "") {
                        window.localStorage.setItem("openid", res.openid);
                    }
                    //alert(res.err_code+res.err_desc+res.err_msg)
                    callpay(res);
                }
            });
        }

        //jsapi付款
        function jsApiCall(res) {
            WeixinJSBridge.invoke(
                'getBrandWCPayRequest', {
                    "appId": res.data.appId,     //公众号名称，由商户传入     
                    "timeStamp": res.data.timeStamp,         //时间戳，自1970年以来的秒数     
                    "nonceStr": res.data.nonceStr, //随机串     
                    "package": res.data.package,
                    "signType": "MD5",         //微信签名方式：     
                    "paySign": res.data.paySign //微信签名 
                },
                function (res) {
                    if (res.err_msg == "get_brand_wcpay_request:ok") {
                        WeixinJSBridge.call('closeWindow');
                    }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
                }
                          );
        }



        //初始化微信支付环境
        function callpay(res) {
            if (typeof WeixinJSBridge == "undefined") {
                if (document.addEventListener) {
                    document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                }
                else if (document.attachEvent) {
                    document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                    document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                }
            }
            else {
                jsApiCall(res);
            }
        }

    }; //addEvent end

    var tool = {


    }; // tool end


};


$(function () {

    var page = new $page();
    page.init();
});


