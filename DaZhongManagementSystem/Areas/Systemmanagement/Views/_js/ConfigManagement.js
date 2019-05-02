var selector = {
    $form: function () { return $("#configForm") },

    //按钮
    $btnEdit: function () { return $("#btnEdit") },
    $btnSave: function () { return $("#btnSave") },

    //界面元素
    $SMSCheckBox: function () { return $(".SMSCheckBox") },
    $WeChatCheckBox: function () { return $(".WeChatCheckBox") },
    $APICheckBox: function () { return $(".APICheckBox") },
    $RevenueCheckBox: function () { return $(".RevenueCheckBox") },

    $QueryTimes: function () { return $("#QueryTimes") },
    $AnswerCount: function () { return $("#AnswerCount") },
    $CountDown: function () { return $("#CountDown") },
    $RevenueQueryReply: function () { return $("#RevenueQueryReply") },//营收数据查询成功回复
    $RevenueQueryRefuse: function () { return $("#RevenueQueryRefuse") },//非司机人员营收查询回复
    $RevenueQueryTimesReply: function () { return $("#RevenueQueryTimesReply") },//当月查询营收次数已够回复
    $RevenueQueryExceptionReply: function () { return $("#RevenueQueryExceptionReply") },//营收数据异常回复
    $txtNum: function () { return $("#txtNum") },
    $txtSingleChoiceNum: function () { return $("#txtSingleChoiceNum") },
    $txtMultipelChoiceNum: function () { return $("#txtMultipelChoiceNum") },
    $txtJudgeNum: function () { return $("#txtJudgeNum") },
    $txtDriverPay: function () { return $("#txtDriverPay") },
    $txtCompanyPay: function () { return $("#txtCompanyPay") },
    $QueryReply: function () { return $("#QueryReply") },
    $txtRideCheckFeedback: function () { return $("#txtRideCheckFeedback") }
};


var $page = function () {
    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        //LoadTable();

    }; //addEvent end

    //编辑按钮事件
    selector.$btnEdit().on('click', function () {
        enableElement();
    });

    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var weChatCheck = 0;
        selector.$WeChatCheckBox().each(function () {
            if ($(this).is(":checked")) {
                weChatCheck = 1;
            }
        });
        var weChatModel = new configModel_Temp(1, weChatCheck);
        dataConfigArray.push(weChatModel);

        var smsCheck = 0;
        selector.$SMSCheckBox().each(function () {
            if ($(this).is(":checked")) {
                smsCheck = 1;
            }
        });
        var smsModel = new configModel_Temp(2, smsCheck);
        dataConfigArray.push(smsModel);

        var apiCheck = 0;
        selector.$APICheckBox().each(function () {
            if ($(this).is(":checked")) {
                apiCheck = 1;
            }
        });
        var apiModel = new configModel_Temp(17, apiCheck);
        dataConfigArray.push(apiModel);

        var revenueCheck = 0;
        selector.$RevenueCheckBox().each(function () {
            if ($(this).is(":checked")) {
                revenueCheck = 1;
            }
        });
        var revenueModel = new configModel_Temp(71, revenueCheck);
        dataConfigArray.push(revenueModel);




        var queryTimeModel = new configModel_Temp(6, selector.$QueryTimes().val());
        dataConfigArray.push(queryTimeModel);

        var queryReply = new configModel_Temp(16, selector.$QueryReply().val());
        dataConfigArray.push(queryReply);
        var answerCountModel = new configModel_Temp(8, selector.$AnswerCount().val());
        dataConfigArray.push(answerCountModel);
        var countDownModel = new configModel_Temp(7, selector.$CountDown().val());
        dataConfigArray.push(countDownModel);
        var revenueQueryReply = new configModel_Temp(5, selector.$RevenueQueryReply().val());
        dataConfigArray.push(revenueQueryReply);
        var revenueQueryRefuse = new configModel_Temp(3, selector.$RevenueQueryRefuse().val());
        dataConfigArray.push(revenueQueryRefuse);
        var revenueQueryTimesReply = new configModel_Temp(4, selector.$RevenueQueryTimesReply().val());
        dataConfigArray.push(revenueQueryTimesReply);
        var num = new configModel_Temp(9, selector.$txtNum().val());
        dataConfigArray.push(num);
        var singleChoiceNum = new configModel_Temp(10, selector.$txtSingleChoiceNum().val());
        dataConfigArray.push(singleChoiceNum);
        var multipelChoiceNum = new configModel_Temp(11, selector.$txtMultipelChoiceNum().val());
        dataConfigArray.push(multipelChoiceNum);
        var judgeNum = new configModel_Temp(12, selector.$txtJudgeNum().val());
        dataConfigArray.push(judgeNum);
        var revenueQueryExceptionsReply = new configModel_Temp(13, selector.$RevenueQueryExceptionReply().val());
        dataConfigArray.push(revenueQueryExceptionsReply);
        var driverPAy = new configModel_Temp(14, selector.$txtDriverPay().val());
        dataConfigArray.push(driverPAy);
        var companyPay = new configModel_Temp(15, selector.$txtCompanyPay().val());
        dataConfigArray.push(companyPay);
        var rideCheckFeedback = new configModel_Temp(72, selector.$txtRideCheckFeedback().val());
        dataConfigArray.push(rideCheckFeedback);

        if (parseInt(selector.$txtNum().val()) != parseInt(selector.$txtSingleChoiceNum().val()) + parseInt(selector.$txtMultipelChoiceNum().val()) + parseInt(selector.$txtJudgeNum().val())) {
            if (selector.$txtSingleChoiceNum().val() != "0" || selector.$txtMultipelChoiceNum().val() != "0" || selector.$txtJudgeNum().val() != "0") {
                jqxNotification("习题总数不等于单选题、多选题以及判断题数量之和！", null, "error");
                dataConfigArray.length = 0;
                return false;
            }
        }
        selector.$form().ajaxSubmit({
            url: '/Systemmanagement/ConfigManagement/SaveConfig',
            type: "post",
            data: { configData: JSON.stringify(dataConfigArray) },
            traditional: true,
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "0":
                        jqxNotification("保存失败！", null, "error");
                        break;
                    case "1":
                        jqxNotification("保存成功！", null, "success");
                        disableElement();
                        break;
                }
            }
        });
    });
};

//禁用界面元素
function disableElement() {
    selector.$SMSCheckBox().attr("disabled", "disabled");
    selector.$SMSCheckBox().attr("style", "background-color: #f5f5f5!important");
    selector.$WeChatCheckBox().attr("disabled", "disabled");
    selector.$WeChatCheckBox().attr("style", "background-color: #f5f5f5!important");
    selector.$APICheckBox().attr("disabled", "disabled");
    selector.$APICheckBox().attr("style", "background-color: #f5f5f5!important");
    selector.$RevenueCheckBox().attr("disabled", "disabled");
    selector.$RevenueCheckBox().attr("style", "background-color: #f5f5f5!important");
    selector.$QueryTimes().attr("disabled", "disabled");
    selector.$QueryTimes().attr("style", "background-color: #f5f5f5!important");
    selector.$AnswerCount().attr("disabled", "disabled");
    selector.$AnswerCount().attr("style", "background-color: #f5f5f5!important");
    selector.$CountDown().attr("disabled", "disabled");
    selector.$CountDown().attr("style", "background-color: #f5f5f5!important");
    selector.$RevenueQueryReply().attr("disabled", "disabled");
    selector.$RevenueQueryReply().attr("style", "background-color: #f5f5f5!important");
    selector.$RevenueQueryRefuse().attr("disabled", "disabled");
    selector.$RevenueQueryRefuse().attr("style", "background-color: #f5f5f5!important");
    selector.$RevenueQueryTimesReply().attr("disabled", "disabled");
    selector.$RevenueQueryTimesReply().attr("style", "background-color: #f5f5f5!important");
    selector.$RevenueQueryExceptionReply().attr("disabled", "disabled");
    selector.$RevenueQueryExceptionReply().attr("style", "background-color: #f5f5f5!important");
    selector.$txtNum().attr("disabled", "disabled");
    selector.$txtNum().attr("style", "background-color: #f5f5f5!important");
    selector.$txtSingleChoiceNum().attr("disabled", "disabled");
    selector.$txtSingleChoiceNum().attr("style", "background-color: #f5f5f5!important");
    selector.$txtMultipelChoiceNum().attr("disabled", "disabled");
    selector.$txtMultipelChoiceNum().attr("style", "background-color: #f5f5f5!important");
    selector.$txtJudgeNum().attr("disabled", "disabled");
    selector.$txtJudgeNum().attr("style", "background-color: #f5f5f5!important");
    selector.$txtDriverPay().attr("disabled", "disabled");
    selector.$txtDriverPay().attr("style", "background-color: #f5f5f5!important");
    selector.$txtCompanyPay().attr("disabled", "disabled");
    selector.$txtCompanyPay().attr("style", "background-color: #f5f5f5!important");
    selector.$QueryReply().attr("disabled", "disabled");
    selector.$QueryReply().attr("style", "background-color: #f5f5f5!important");
    selector.$txtRideCheckFeedback().attr("disabled", "disabled");
    selector.$txtRideCheckFeedback().attr("style", "background-color: #f5f5f5!important");
}

//启用界面元素
function enableElement() {
    selector.$SMSCheckBox().removeAttr("disabled");
    selector.$SMSCheckBox().removeAttr("style");
    selector.$WeChatCheckBox().removeAttr("disabled");
    selector.$WeChatCheckBox().removeAttr("style");
    selector.$APICheckBox().removeAttr("disabled");
    selector.$APICheckBox().removeAttr("style");
    selector.$RevenueCheckBox().removeAttr("disabled");
    selector.$RevenueCheckBox().removeAttr("style");
    selector.$QueryTimes().removeAttr("disabled");
    selector.$QueryTimes().removeAttr("style");
    selector.$AnswerCount().removeAttr("disabled");
    selector.$AnswerCount().removeAttr("style");
    selector.$CountDown().removeAttr("disabled");
    selector.$CountDown().removeAttr("style");
    selector.$RevenueQueryReply().removeAttr("disabled");
    selector.$RevenueQueryReply().removeAttr("style");
    selector.$RevenueQueryRefuse().removeAttr("disabled");
    selector.$RevenueQueryRefuse().removeAttr("style");
    selector.$RevenueQueryTimesReply().removeAttr("disabled");
    selector.$RevenueQueryTimesReply().removeAttr("style");
    selector.$RevenueQueryExceptionReply().removeAttr("disabled");
    selector.$RevenueQueryExceptionReply().removeAttr("style");
    selector.$txtNum().removeAttr("disabled");
    selector.$txtNum().removeAttr("style");
    selector.$txtSingleChoiceNum().removeAttr("disabled");
    selector.$txtSingleChoiceNum().removeAttr("style");
    selector.$txtMultipelChoiceNum().removeAttr("disabled");
    selector.$txtMultipelChoiceNum().removeAttr("style");
    selector.$txtJudgeNum().removeAttr("disabled");
    selector.$txtJudgeNum().removeAttr("style");
    selector.$txtDriverPay().removeAttr("disabled");
    selector.$txtDriverPay().removeAttr("style");
    selector.$txtCompanyPay().removeAttr("disabled");
    selector.$txtCompanyPay().removeAttr("style");
    selector.$QueryReply().removeAttr("disabled");
    selector.$QueryReply().removeAttr("style");
    selector.$txtRideCheckFeedback().removeAttr("disabled");
    selector.$txtRideCheckFeedback().removeAttr("style");
}

var configModel_Temp = function (id, configValue) {
    this.ID = id;
    this.ConfigValue = configValue;
};
var dataConfigArray = new Array();


$(function () {
    var page = new $page();
    page.init();

})


