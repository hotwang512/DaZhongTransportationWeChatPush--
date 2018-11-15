// 通过ajax获取数据
var width = $(window).width;
$.ajax({
    url: "/KnowledgeBaseManagement/WeChatKnowledge/GetKnowledgeDetail",
    type: "post",
    data: { vguid: $("#txtVguid").val(), personVguid: $("#txtpersonVguid").val() },
    success: function (data) {
        $(".dataItem").html(data.Content); //填充到div中
        $(".dataItem").css("max-width", width);
        //点击图片拿到当前点击图片的img  赋给放大图片src路径
        $(".dataItem img").on("click", function () {
            $("#Bigimg").show();
            var imgSrc = $(this).attr("src"); //拿到当前点击图片的src;
            $("#img").attr("src", imgSrc);
        });
        //点击放大图片的div  ,div隐藏
        $("#Bigimg").on("click", function () {
            $("#Bigimg").hide();
        });
    }
});
