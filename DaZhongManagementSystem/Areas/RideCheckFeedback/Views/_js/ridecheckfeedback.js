//5.给后台传递数据	
//给简答题生成一个文本框
//获取所有题的长度，在页面显示页面

var dataDetailRow;
var dataLen;

var MainRowID;
var count = 0;
var number1 = 0;
dataLen = 7;
var answerCount = 0;
var userAbleAnswerCount = $("#AnswerCount").val();
var isPushed = 0;  //是否推送过
//获取数据
$(function () {
    if ($("#IsOpen").val() == "True") {
        init();
        addEvent()
        basicInformation();
        loadridecheckfeedback()
    }
});




function init() {
    $.selectYY_MM_DD("#txt_checkdate");
    $.select_HH_MM("#txt_checktime");
}

function addEvent() {
    $("#image_invoice").click(function () {
        $("#upload_Invoice").val('');
        $("#upload_Invoice").click();
    });

    $("[name='image_attachment']").click(function () {
        $("#upload_Attachment").val('');
        $("#upload_Attachment").click();
    });

    $("input[type='checkbox']").change(function () {
        var inputs = $(".swiper-slide-active").find("input");
        var chekedInputs = $(".swiper-slide-active").find("input:checked");
        var kk = $(".swiper-slide-active").find(".kk");
        if (chekedInputs.length > 0) {
            $(kk[0]).removeClass("active")
            $(kk[1]).addClass("active")
        } else {
            $(kk[1]).removeClass("active")
            $(kk[0]).addClass("active")
        }
        if ($(".swiper-slide-active").find(".kk").hasClass("active") == true) {
            $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
        }
        else {
            $(".ss").eq(count).removeClass("active").css("background", "#999");
        }
        saveChoiceQuestion(inputs);
    });

}

function getVGUID() {
    return $("#ExerciseVguid").val();
}

function getUser() {
    return $("#PersonVguid").val();
}

function uploadInvoice(ele) {
    layer.open({ type: 2 });
    var id = Number($(".swiper-slide-active").attr("data-swiper-slide-index"));
    $("form").ajaxSubmit({
        url: " /RideCheckFeedback/RideCheckFeedback/uploadFile?user=" + getUser() + "&vguid=" + getVGUID() + "&type=invoice&number=" + (id + 1),
        type: "post",
        dataType: "json",
        success: function (msg) {
            $($("[name='image_invoice']")[1]).attr("src", msg.Data.FilePath)
            $(".ss").eq(6).addClass("active").css("background", "rgb(20, 108, 127)");
            layer.closeAll();
            activeSubmit();
        }
    });
}

function uploadAttachment(ele) {

    layer.open({ type: 2 });
    var id = Number($(".swiper-slide-active").attr("data-swiper-slide-index"));
    $("form").ajaxSubmit({
        url: " /RideCheckFeedback/RideCheckFeedback/uploadFile?user=" + getUser() + "&vguid=" + getVGUID() + "&type=attachment&number=" + (id + 1),
        type: "post",
        dataType: "json",
        success: function (msg) {
            $($("[name='attachmentul']")[1]).append("<li filePath='" + msg.Data.FilePath + "'><span onclick='deleteAttachment(this)' style='color: red;font-size: 50px;'>×</span>" + msg.Data.FileName + "</li>");
            layer.closeAll();
        }
    });
}

function deleteAttachment(ele) {
    if (confirm("确认要删除附件？")) {
        $(ele).parent().remove();
    }
}

function basicInformation() {
    var txt_checkname = $("#txt_checkname").val();
    var txt_checkdate = $("#txt_checkdate").val();
    var txt_checktime = $("#txt_checktime").val();
    var txt_checkcarnumber = $("#txt_checkcarnumber").val();
    var txt_boardinglocation = $("#txt_boardinglocation").val();
    var txt_getoffposition = $("#txt_getoffposition").val();
    var txt_servicecardnumber = $("#txt_servicecardnumber").val();
    if (txt_checkname != "" && txt_checkdate != "" && txt_checktime != "" && txt_checkcarnumber != "" && txt_boardinglocation != "" && txt_getoffposition != "" && txt_servicecardnumber != "") {
        $(".ss").eq(0).addClass("active").css("background", "rgb(20, 108, 127)");

    } else {
        $(".ss").eq(0).removeClass("active").css("background", "#999");
    }
    //保存基本信息
    $.ajax({
        url: "/RideCheckFeedback/RideCheckFeedback/SaveRideCheckFeedbackItemInfor",
        data: {
            user: getUser(),
            vguid: getVGUID(),
            number: 1,
            answer1: txt_checkdate,
            answer2: txt_checktime,
            answer3: txt_checkcarnumber,
            answer4: txt_boardinglocation,
            answer5: txt_getoffposition,
            answer6: txt_servicecardnumber,
            answer7: ""
        },
        type: "POST",
        success: function (data) {
            console.log(data);
        }
    });
    activeSubmit();
}

//保存单选题
function saveChoiceQuestion(inputs) {
    if (inputs == undefined) {
        inputs = $(".swiper-slide-active").find("input");
    }
    var id = Number($(".swiper-slide-active").attr("data-swiper-slide-index"));
    var data = {};
    data.user = getUser();
    data.vguid = getVGUID();
    data.number = id + 1;
    //if (id == 9) {
    //    for (var i = 0; i < kk.length; i++) {
    //        data["answer" + (i + 1)] = $(kk[i]).hasClass("active") == true ? "1" : "";
    //    }
    //}
    //else {
    var kk = $(".swiper-slide-active").find(".kk");
    data.answer1 = $(kk[0]).hasClass("active") == true ? "A" : "B";
    for (var i = 0; i < inputs.length; i++) {
        data["answer" + (i + 2)] = $(inputs[i]).is(':checked') == true ? "1" : "";
    }
    //}

    $.ajax({
        url: "/RideCheckFeedback/RideCheckFeedback/SaveRideCheckFeedbackItemInfor",
        data: data,
        type: "POST",
        success: function (data) {
            //console.log(data);
        }
    })
}

//简答题
function LeaveSubmit(e) {

    var id = Number($(".swiper-slide-active").attr("data-swiper-slide-index"));

    var data = {};
    data.user = getUser();
    data.vguid = getVGUID();
    data.number = id + 1;
    data.answer1 = $(e).val();
    $.ajax({
        url: "/RideCheckFeedback/RideCheckFeedback/SaveRideCheckFeedbackItemInfor",
        data: data,
        type: "POST",
        success: function (data) {
            //console.log(data);
        }
    })
}


function loadridecheckfeedback() {


    //var ans = $(".answer").children();
    //var ansLen = ans.length;
    //var number2 = 0;
    //for (var t = 0; t < ansLen; t++) {
    //    if (jQuery(ans[t]).hasClass("active")) {
    //        number2++;
    //    }
    //}
    //判断当前试题是否已经完成
    //if (number2 == dataLen) {
    //    $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
    //}
    activeSubmit();
    //swpeir控制器
    var mySwiper = new Swiper('.kj1', {
        loop: true,
        pagination: '.swiper-pagination',
        paginationType: 'fraction',
        observer: true,
        observeParents: true
    });
    $(".kj1").show();
    $(".ss").eq(8).addClass("active").css("background", "rgb(20, 108, 127)");
    var countNum = 5;
    var countup;
    //切换上个页面
    $(".itemLast").click(function () {
        count--;
        console.log(count);

        if (count < 0) {
            count = 0;
        } else {
            $(".f_lastHide").show();
            countup = setInterval(CountUp, 100);
            CountUp();
        }
        $(".fc_left").html(count + 1);
    });
    function CountUp() {
        countNum--;
        console.log(countNum);
        if (countNum == 0) {
            $(".f_lastHide").hide();
            mySwiper.slidePrev();
            clearInterval(countup);
            countNum = 5;
        }
    }
    //切换下个页面
    var countdown;
    $(".itemNext").click(function () {
        count++;
        if (count > dataLen - 1) {
            count = dataLen - 1;
        } else {
            $(".f_nextHide").show();
            countdown = setInterval(CountDown, 100);
            CountDown();
        }
        $(".fc_left").html(count + 1);
    });
    function CountDown() {
        countNum--;
        console.log(countNum);
        if (countNum == 0) {
            $(".f_nextHide").hide();
            mySwiper.slideNext();
            clearInterval(countdown);
            countNum = 5;
        }

    }
    //设置单选多选样式
    //点击选项追加样式
    $(".option").click(function () {
        var index = $(this).index();
        if ($(this).attr("nameSub") == 1) {
            $(this).addClass("active").parent().siblings().find(".kk").removeClass("active")

            var inputs = $(this).parent().parent().find(".infor input");
            if ($(this).text() == "A") {
                inputs.removeAttr("checked")
                //inputs.attr("disabled", "disabled");
            } else {
                //inputs.removeAttr("disabled");
            }
            saveChoiceQuestion(inputs);

            //在答题卡中判断某一题是否已经完成提交
            if ($(".swiper-slide-active").find(".kk").hasClass("active") == true) {
                $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
            }
            else {
                $(".ss").eq(count).removeClass("active").css("background", "#999");
            }
        }
        else if ($(this).attr("nameSub") == 2) {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
            } else {
                $(this).addClass('active');
            }
            saveChoiceQuestion();
            //在答题卡中判断某一题是否已经完成提交
            if ($(".swiper-slide-active").find(".kk").hasClass("active") == true) {
                $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
            }
            else {
                $(".ss").eq(count).removeClass("active").css("background", "#999");
            }
        } else if ($(this).attr("nameSub") == 4) {
            var text = $(this).text();
            //$.ajax({
            //    url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
            //    data: {
            //        BusinessPersonnelVguid: $("#PersonVguid").val(),
            //        Answer: text,
            //        BusinessExercisesVguid: MainRowID,
            //        BusinessExercisesDetailVguid: id
            //    },
            //    type: "POST",
            //    success: function (data) {
            //        console.log(data);
            //    }
            //});
        }
        activeSubmit();
    });

    //点击答案追加样式
    $(".title").click(function () {
        //点击题目通过索引找到对应的选项
        if ($(this).attr("nameSub") == 1) {
            $(this).parent().siblings().find(".kk").removeClass("active");
            $(this).siblings().addClass("active");

            var inputs = $(this).parent().parent().find(".infor input");
            if ($(this).text() == "合格(执行)") {
                inputs.removeAttr("checked")
                //inputs.attr("disabled", "disabled");
            } else {
                //inputs.removeAttr("disabled");
            }
            saveChoiceQuestion(inputs);
            //在答题卡中判断某一题是否已经完成提交
            if ($(".swiper-slide-active").find(".kk").hasClass("active") == true) {
                $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
            }
            else {
                $(".ss").eq(count).removeClass("active").css("background", "#999");
            }
        } else if ($(this).attr("nameSub") == 2) {
            if ($(this).siblings().hasClass('active')) {
                $(this).siblings().removeClass('active');
            } else {
                $(this).siblings().addClass('active');
            }
            saveChoiceQuestion();
            //在答题卡中判断某一题是否已经完成提交
            if ($(".swiper-slide-active").find(".kk").hasClass("active") == true) {
                $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
            }
            else {
                $(".ss").eq(count).removeClass("active").css("background", "#999");
            }
        }
        activeSubmit();

    });

    //点击切换到答题卡
    $("#ans").click(function () {
        var div = $(".answer");
        if (div.css("display") === "none") {
            div.show();
            $(".f_next").hide();
            $(".f_center").hide();
            $(".f_last").hide();
            $(".h_back").css("width", "75%");
        } else {
            div.hide();
            $(".f_next").show();
            if (isPushed == 1) {
                $(".f_center").hide();
            } else {
                $(".f_center").show();
            }
            $(".f_last").show();
            $(".h_back").css("width", "25%");
        }
    });
    //点击答题卡切换页面
    $(".answer .ss").on("click", function () {
        count = $(this).index();
        $(".answer").hide();
        $(".f_next").show();
        if (isPushed == 1) {
            $(".f_center").hide();
        } else {
            $(".f_center").show();
        }
        $(".f_last").show();
        $(".h_back").css("width", "25%");
        mySwiper.slideTo(count + 1, 100, false);
        $(".fc_left").html(count + 1);
    });

    $(".fc_left").html(count + 1);
    $(".fc_right").html(dataLen);

}

function activeSubmit() {
    //判断一共做了几道题
    var ans = $(".answer").children();
    var ansLen = ans.length;
    number1 = 0;
    for (var t = 0; t < ansLen; t++) {
        if (jQuery(ans[t]).hasClass("active")) {
            number1++;
        }
    }

    //判断当前试题是否已经完成
    if (number1 == dataLen) {
        console.log(number1);
        console.log(dataLen);
        console.log(dataLen)
        $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
    }
    else {
        $(".f_center1").removeClass("active").css("background", "#999");
    }
}



//点击交卷
$(".f_center").click(function () {

    if ($(".f_center1").hasClass("active")) {
        layer.open({ type: 2 });
        $.ajax({
            url: "/RideCheckFeedback/RideCheckFeedback/Submit",
            data: {
                vguid: getVGUID(),
            },
            type: "POST",
            success: function (data) {
                //alert("本次检查单已完成，谢谢您的参与！！");
                layer.closeAll();
                $("#success").show();
            }
        });
    }
    else {
        alert("当前共" + dataLen + "道题，已完成" + number1 + "道题，未完成" + (dataLen - number1) + "道题");
    }
});

