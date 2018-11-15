var selector = {
    //$grid: function () { return $("#DraftInfoList") },
    $pushForm: function () { return $("#pushForm") },

    //按钮
    $btnBack: function () { return $("#btnBack") },
    $btnSend: function () { return $("#btnSend") },

    //界面元素
    $txtTitle: function () { return $("#txtTitle") },
    $pushType: function () { return $("#pushType") },
    $isSendTime: function () { return $("#isSendTime") },
    $sendTime: function () { return $("#sendTime") },
    $effectiveDate: function () { return $("#effectiveDate") },
    $isImportant: function () { return $("#isImportant") },
    $pushPeopleDropDownButton: function () { return $("#pushPeopleDropDownButton") },
    $pushPeopleTree: function () { return $("#pushPeopleTree") },
    $tdMoney: function () { return $(".tdMoney") },
    $revenueType: function () { return $("#revenueType") },
    $uEditor: function () { return $(".UEditor") },//富文本框
    //$edui1_iframeholder: function () { return $(".view") },
    $pushContent: function () { return $("#pushContent") },
    $textEdit: function () { return $(".textEdit") },
    $isEdit: function () { return $("#isEdit") },//编辑/新增
    $vguid: function () { return $("#vguid") },
    $txtMessageType: function () { return $("#txtMessageType") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        var um = UE.getEditor('myEditor');
        //um.setDisabled();
        if (selector.$revenueType().val() == "1") {
            selector.$tdMoney().show();
        }
        //返回按钮事件
        selector.$btnBack().on('click', function () {
            window.location.href = "/WeChatPush/PushedList/PushedList";
        });
        if (selector.$txtMessageType().val() == "3") {
            $(".mod-sender__slider").show();
            $("#coverImgPath").val($(".coverHid").val());
            var imgObj = '<div class="video_image_wrap"><img style="width:100%;vertical-align: middle;display:inline-block;" src="' + $(".coverHid").val() + '"></div>';
            $(".image_edit_placeholder").children().remove();
            $(".image_edit_placeholder").append(imgObj);
            //创建侧边预览框
            createModSender();
        }
    }; //addEvent end


};
//创建侧边预览框
function createModSender() {
    $.ajax({
        url: "/WeChatPush/DraftList/GetMoreGraphicList",
        type: "post",
        data: { vguid: selector.$vguid().val() },
        dataType: "json",
        success: function (msg) {
            for (var i = 0; i < msg.length; i++) {
                onCreate(msg[i]);
            }
            $(".video_artical_add").remove();
        }
    });
}

var div_artical = 0;
//创建预览框
function onCreate(msg) {
    div_artical++;
    var title = msg == null ? "" : msg.Title;
    var coverImg = msg == null ? "" : msg.CoverImg;
    var coverDesc = msg == null ? "" : msg.CoverDescption;
    var message = msg == null ? "" : msg.Message;
    var div = '<div class="video_artical" index="' + div_artical + '"  onclick=onSelect(this)>' +
          '<div class="choose_mode_style"></div>' +
          '<p class="image_edit_placeholder_1"></p>' +
          '<p class="js_article_title video_artical_title">' + title + '</p>' +
          '</div>';

    $(".video_artical_add").before(div);
    if (msg != null) {
        var imgObj = '<div class="video_image_wrap"><img style="width:80px;vertical-align: middle;display:inline-block;" src="' + coverImg + '"></div>';
        $(".video_artical[index=" + div_artical + "]").children("p[class=image_edit_placeholder_1]").append(imgObj);
    }
    $(".video_artical[index=" + div_artical + "]").click();
    var tableObj = '<table style="margin-top: 20px; margin-left: 20px;"  tabType="' + div_artical + '"  class="tableClass_' + div_artical + '">' +
                     '<tr style="height:50px">' +
                         '<td align="right" style="width: 90px;">标题：</td>' +
                         '<td colspan="5" style="width: 210px;">' +
                         '<input id="txtTitle' + div_artical + '" type="text" disabled="disabled" name="Title" validatetype="required" style="width: 100%!important;background-color:#f5f5f5!important;" class="input_text form-control"  value="' + title + '">' +
                         '</td><td></td><td></td>' +
                    '</tr>' +
                   '<tr style="height:50px">' +
                     '<td align="right">推送方式：</td>' +
                     '<td>' +
                           ' <select id="pushType" name="PushType" validatetype="required" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                    '<option value="1" selected="selected">微信推送</option>' +
                                    '<option value="2">短信推送</option>' +
                            '</select>' +
                   ' </td>' +
                   '<td align="right" style="width: 105px;">是否定时发送：</td>' +
                   '<td>' +
                        '<select id="isSendTime" name="Timed" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                '<option value="True">是</option>' +
                                '<option value="False" selected="selected">否</option>' +
                        '</select>' +
                    '</td>' +
                    '<td align="right" style="width: 105px;">发送时间：</td>' +
                    '<td><input id="sendTime" name="TimedSendTime" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control" value="">  </td>' +
                     '<tr style="height:50px">' +
                     '<td align="right">推送接收者：</td>' +
                     '<td><input type="text" id="pushPeople" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control"> </td>' +
                    '<td align="right">有效日期：</td>' +
                    '<td>' +
                    '<input id="effectiveDate" name="PeriodOfValidity" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control" >' +
                    '</td>' +
                    '<td align="right">是否永久：</td>' +
                     '<td>' +
                        '<select id="isImportant" name="Important" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                '<option value="True">是</option>' +
                                '<option value="False" selected="selected">否</option>' +
                        '</select>' +
                    '</td>' +
                    '</tr>' +
                    '<tr id="weChatTypeRow">' +
                        '<td align="right">微信推送类型：</td>' +
                        '<td>' +
                            '<select id="weChatMessageType" name="MessageType" disabled=disabled style="background-color:#f5f5f5!important;"   class="input_text form-control">' +
                                '<option value="">===请选择===</option>' +
                                    '<option value="3" selected="selected">图文推送</option>' +
                            '</select>' +
                        '</td>' +
                        '<td class="weChatCoverMsg" align="right">封面图片：</td>' +
                        '<td class="weChatCoverMsg">' +
                            '<input type="file" id="coverImg_input' + div_artical + '" name="CoverImgs' + div_artical + '"  style="background-color: #f5f5f5!important;" disabled="disabled" class="input_text form-control" accept="image/gif,image/jpeg,image/jpg,image/png">' +
                        '</td>' +
                        '<td class="weChatCoverMsg" align="right">封面描述：</td>' +
                        '<td class="weChatCoverMsg">' +
                            '<input id="txtCoverDescption' + div_artical + '" type="text" name="CoverDescption" style="background-color: #f5f5f5!important;" disabled="disabled" class="input_text form-control" value="' + coverDesc + '">' +
                       ' </td>' +
                    '</tr>' +
                    '<tr style="height:45px;">' +
                        '<td class="personLabel1" align="right">人员标签：</td>' +
                        '<td class="personLabel1">' +
                            '<input type="text" disabled="disabled" class="input_text form-control" id="txtLabel" name="Label" style="background-color: #f5f5f5!important;">' +
                        '</td>' +
                        '<td align="right" class="pushHistory">记录推送历史：</td>' +
                        '<td class="pushHistory">' +
                            '<input id="chkHistory" type="checkbox" disabled="disabled"  style="width: 17px; height: 17px;background-color:#f5f5f5!important;">' +
                       ' </td>' +
                       '</tr>' +
                       '<tr style="height: 45px;">' +
                       '<td align="right" id="pushContentText">推送内容：</td>' +
                        '<td colspan="5"> <div class="UEditor" style="display: block;width: 815px;">' +
                                '<textarea id="myEditor' + div_artical + '" name="Message" runat="server" onblur="setUeditor()">' + message +
                             '</textarea>' +
                            '</div>' +
                 '</td></tr>' +
               '</table>';
    $(".tableClass").hide();
    $(".tableClass").after(tableObj);
    $("#pushForm table").each(function () {
        if ($(this).attr("tabType")) {
            $(this).css("display", "none");
        }
    });
    $(".tableClass_" + div_artical).show();
    var um = UE.getEditor('myEditor' + div_artical, { readonly: true });
    $("#myEditor" + div_artical).css("height", "200px");
    if (msg != null) {
        onSelect_M();
    }
}

//选中主预览框
function onSelect_M() {
    if ($(".video_artical").length >= 1) {
        $(".choose_mode_style").css("opacity", "0");
        $(".video-unit").css("border", "2px solid #4a90e2");
        $(".tableClass").show();
        $("#pushForm table").each(function () {
            if ($(this).attr("tabType")) {
                $(this).css("display", "none");
            }
        });
    }
}
//选中子预览框
function onSelect(obj) {
    $(".tableClass").hide();
    $(".video-unit").css("border", "0 solid #ccc");
    $("#pushForm table").each(function () {
        if ($(this).attr("tabType")) {
            $(this).css("display", "none");
        }
    });
    $(".choose_mode_style").css("opacity", "0");
    $(obj).children("div").css("opacity", "1");
    var id = $(obj).attr("index");
    $(".tableClass_" + id).css("display", "block");
}


$(function () {
    var page = new $page();
    page.init();

})
