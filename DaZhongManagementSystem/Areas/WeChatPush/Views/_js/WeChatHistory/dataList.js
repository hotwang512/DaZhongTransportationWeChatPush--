var page = 1;   //当前的页码数

$(function () {
    getAjaxNews();
});


//判断滚动条是否到达底部
$(".wrapper").scroll(function () {
    var $this = $(this);
    var viewH = $(this).height(), //可见高度  
         contentH = $(this).get(0).scrollHeight, //内容高度  
         scrollTop = $(this).scrollTop() + 1; //滚动高度  
    if (contentH - viewH - scrollTop <= 0) { //判断滚动条是否到达底部,加载新内容  
        page++; //每次下拉页码加1
        getAjaxNews();

    }
});
//加载数据
function getAjaxNews() {
    //在deom中加一个一秒的延迟，判断加载动画是否显示
    $(".cover").show();
    $.ajax({
        type: 'post',
        url: "/WeChatPush/WeChatHistory/GetWeChatPushList",
        data: { pageIndex: page, personVguid: $("#PersonVguid").val() },
        success: function (list) {
            $(".cover").hide();
            if (list.length > 0) {

                for (var i = 0; i < list.length; i++) {
                    var appendStr = '<a href="#" class="list-group-item" onclick = "leaveItemClick(this)">'
                                       + '<div class="title">' + list[i].Title + '</div>'
                                       + '<div class="timer">日期：' + getLocalTime(list[i].PushDate) + '</div>'
                                       + '<div style="display: none;" class="VGUID">' + list[i].VGUID + '</div>'
                                       + '<div style="display: none;" class="messageType">' + list[i].MessageType + '</div>'
                                       + '<div style="display: none;" class="ExercisesVGUID">' + list[i].ExercisesVGUID + '</div>'
                                   + '</a>';
                    $("#swiper-wrapper").append(appendStr);
                }

            }
        }
    });
};

//点击跳转到详情页面
function leaveItemClick(event) {
    $(".cover").show();
    var VGUID = $(event).find(".VGUID").html();  //获取到点击list的VGUID
    var messageType = $(event).find(".messageType").html();  //获取到点击list的消息类型
    if (messageType == "4") { //说明是习题类型，则跳转到习题详情页
        var exerciseVguid = $(event).find(".ExercisesVGUID").html();
        var exerciseUrl = $("#openHttpAddress").val() + "/BasicDataManagement/WeChatExercise/Index?exerciseVguid=" + exerciseVguid + "," + "00000000-0000-0000-0000-000000000000" + "," + VGUID;
        var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + $("#corpId").val() + "&redirect_uri=" + exerciseUrl + "&response_type=code&scope=snsapi_base#wechat_redirect";
        window.location.href = url;
    } else {
        window.location.href = "/WeChatPush/WeChatHistory/HistoryDetail?Vguid=" + VGUID + "&personVguid=" + $("#PersonVguid").val();
    }


    $(".cover").hide();
}
//转换时间戳
function getLocalTime(nS) {
    return new Date(parseInt(nS.substr(6, 13))).toLocaleDateString();
}

