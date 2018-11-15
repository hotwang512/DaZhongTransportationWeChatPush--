var selector = {

    $dataItem: function () { return $(".dataItem") },
    $txtVguid: function () { return $("#txtVguid") },
    $txtAgreementType: function () { return $("#txtAgreementType") },
    $txtPersonVguid: function () { return $("#txtPersonVguid") },

    //按钮
    $btnAgree: function () { return $("#btnAgree") },
    $btnNotAgree: function () { return $("#btnNotAgree") },
    $btnKnow: function () { return $("#btnKnow") }
};

var $page = function () {
    this.init = function () {
        addEvent();
    }

    function addEvent() {
        var width = $(window).width;
        //初始化页面
        initDataItem();


        //初始化页面
        function initDataItem() {
            $.ajax({
                url: "/WeChatPush/PushDetail/GetWeChatPushDetail",
                type: "post",
                dataType: "json",
                data: { vguid: selector.$txtVguid().val() },
                async: false, //同步
                success: function (data) {
                    selector.$dataItem().html(data.Message); //填充到div中
                    selector.$dataItem().css("max-width", width);
                    isExistAgreement();
                }
            });
        }

        //判断是否点击过按钮
        function isExistAgreement() {
            $.ajax({
                url: "/WeChatPush/PushDetail/IsExistAgreementOperationInfo",
                type: "post",
                dataType: "json",
                data: { WeChatPushVGUID: selector.$txtVguid().val(), PersonnelVGUID: selector.$txtPersonVguid().val() },
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            //初始化显示的按钮
                            showButton();
                            break;
                        case "1":
                            break;
                    }
                }
            });
        }
        //初始化显示的按钮
        function showButton() {
            switch (selector.$txtAgreementType().val()) {
                case "1":
                    selector.$btnAgree().show();
                    break;
                case "2":
                    selector.$btnAgree().show();
                    selector.$btnNotAgree().show();
                    break;
                case "3":
                    selector.$btnKnow().show();
                    break;
                case "4":
                    selector.$btnAgree().show();
                    selector.$btnKnow().show();
                    break;
                case "5":
                    selector.$btnAgree().show();
                    selector.$btnNotAgree().show();
                    selector.$btnKnow().show();
                    break;
            }
        }
        //点击按钮
        $(".btn").on("click", function () {
            var txt = "确定" + $(this).children("span").text() + "?";
            var btnText = $(this).children("span").text();
            popTipShow.confirm("确认框", txt, ["确定", "取消"], function (e) {
                //callback 处理按钮事件
                var button = $(e.target).attr('class');
                if (button == 'ok') {
                    createAgreement(btnText);
                }
                if (button == 'cancel') {
                    this.hide();
                }
            });
        });

    }
};

function createAgreement(infos) {
    $.ajax({
        url: "/WeChatPush/PushDetail/CreateAgreementOperationInfo",
        data: { WeChatPushVGUID: selector.$txtVguid().val(), PersonnelVGUID: selector.$txtPersonVguid().val(), Result: infos },
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    popTipShow.alert("提示框", "操作失败！请重新操作！", ["确认"], function (e) {
                        var button = $(e.target).attr('class');
                        if (button == 'ok') {
                            this.hide();
                        }
                    });
                    break;
                case "1":
                    popTipShow.alert("提示框", "操作成功！", ["确认"], function (e) {
                        var button = $(e.target).attr('class');
                        if (button == 'ok') {
                            this.hide();
                            wx.closeWindow();
                        }
                    });

                    break;
            }
        }
    });
}
$(function () {
    var page = new $page();
    page.init();

});