var page = 1;   //当前的页码数
getAjaxNews();//加载第一次数据
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
        url: "/KnowledgeBaseManagement/WeChatKnowledge/GetKnowledgeList",
        data: { pageIndex: page, personVguid: $("#PersonVguid").val() },
        success: function (list) {
            $(".cover").hide();
            if (list != "") {
                var appendStr = "";
                for (var i = 0; i < list.length; i++) {
                    appendStr += '<a href="#" class="list-group-item" onclick = "leaveItemClick(this)">'
                                    + '<div class="title">' + list[i].Title + '</div>'
                                    + '<div class="timer">日期：' + getLocalTime(list[i].CreatedDate) + '</div>'
                                    + '<div style="display: none;" class="VGUID">' + list[i].Vguid + '</div>'
                                + '</a>';
                }
                $("#swiper-wrapper").append(appendStr);
            }
        }
    });
};

//点击跳转到详情页面
function leaveItemClick(event) {
    $(".cover").show();
    var VGUID = $(event).find(".VGUID").html();  //获取到点击list的VGUID
    window.location.href = "/KnowledgeBaseManagement/WeChatKnowledge/KnowledgeBaseDetail?Vguid=" + VGUID + "&personVguid=" + $("#PersonVguid").val();
    $(".cover").hide();
}
//转换时间戳
function getLocalTime(nS) {
    return new Date(parseInt(nS.substr(6, 13))).toLocaleDateString();
}

