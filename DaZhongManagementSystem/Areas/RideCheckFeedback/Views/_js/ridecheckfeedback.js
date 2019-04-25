//5.给后台传递数据	
//给简答题生成一个文本框
//获取所有题的长度，在页面显示页面

var dataDetailRow;
var dataLen;

var MainRowID;
var count = 0;
var number1 = 0;
dataLen = 14;
var answerCount = 0;
var userAbleAnswerCount = $("#AnswerCount").val();
var isPushed = 0;  //是否推送过
//获取数据
$(function () {
    $.ajax({
        url: "/RideCheckFeedback/RideCheckFeedback/getRideCheckFeedback",
        data: {
            vguid: $("#ExerciseVguid").val(),//"11F62166-64A8-4B05-9B97-F534C804181F",
            personVguid: $("#PersonVguid").val() //"575808EF-B574-48A3-B5BB-4D0EBB1B3592" 
        },
        type: "get",
        dataType: 'json',
        success: function (data) {
            //if ($("#isHistory").val() == "1") {
            //    $("#dvTrans").css("display", "block");
            //    $.ajax({
            //        url: "/BasicDataManagement/WeChatExercise/IsPushed",
            //        data: {
            //            BusinessPersonnelVguid: $("#PersonVguid").val(),
            //            wechatMainVguid: $("#txtWechatMain").val()
            //        },
            //        type: "POST",
            //        success: function (msg) {
            //            switch (msg.respnseInfo) {
            //                case "0":
            //                    $(".f_center2").text("获取习题");
            //                    $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
            //                    break;
            //                case "1":
            //                    isPushed = 1;
            //                    $(".f_center").css('display', 'none');
            //                    break;
            //            }
            //        }
            //    });

            //}
            var ans = $(".answer").children();
            var ansLen = ans.length;
            var number2 = 0;
            for (var t = 0; t < ansLen; t++) {
                if (jQuery(ans[t]).hasClass("active")) {
                    number2++;
                }
            }
            //判断当前试题是否已经完成
            if (number2 == dataLen) {
                $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
            }

            //swpeir控制器
            var mySwiper = new Swiper('.kj1', {
                loop: true,
                pagination: '.swiper-pagination',
                paginationType: 'fraction',
                observer: true,
                observeParents: true
            });
            $(".kj1").show();
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
                    // 第二种方法
                    // $(this).parent().siblings().children().find(".kk").removeClass("active");
                    // $(this).addClass("active");
                    $(this).addClass("active").parent().siblings().find(".kk").removeClass("active")
                    var arr = [];
                    var goods = $.makeArray($(".swiper-slide-active").find(".kk"));
                    console.log(goods);
                    //var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    //console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串
                    console.log(Array);
                    ////给后台上传数据
                    //$.ajax({
                    //    url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                    //    data: {
                    //        BusinessPersonnelVguid: $("#PersonVguid").val(),
                    //        Answer: Array,
                    //        BusinessExercisesVguid: MainRowID,
                    //        BusinessExercisesDetailVguid: id
                    //    },
                    //    type: "POST",
                    //    success: function (data) {
                    //        console.log(data);
                    //    }
                    //})
                    //在答题卡中判断某一题是否已经完成提交
                    if (arr != "") {
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
                    var arr = []; //把答案保存在数组中
                    var goods = $.makeArray($(".swiper-slide-active").find(".kk"));
                    console.log(goods);
                    //var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    //console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串

                    //$.ajax({
                    //    url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                    //    data: {
                    //        BusinessPersonnelVguid: $("#PersonVguid").val(),
                    //        Answer: Array,
                    //        BusinessExercisesVguid: MainRowID,
                    //        BusinessExercisesDetailVguid: id
                    //    },
                    //    type: "POST",
                    //    success: function (data) {
                    //        console.log(data);
                    //    }
                    //});
                    //在答题卡中判断某一题是否已经完成提交
                    if (arr != "") {
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

            });

            //点击答案追加样式
            $(".title").click(function () {
                //点击题目通过索引找到对应的选项
                if ($(this).attr("nameSub") == 1) {
                    $(this).parent().siblings().find(".kk").removeClass("active");
                    $(this).siblings().addClass("active");
                    var arr = []; //把答案保存在数组中
                    var goods = $.makeArray($(".swiper-slide-active").find(".kk"));
                    console.log(goods);
                    //var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    //console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串

                    ////给后台上传数据
                    //$.ajax({
                    //    url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                    //    data: {
                    //        BusinessPersonnelVguid: $("#PersonVguid").val(),
                    //        Answer: Array,
                    //        BusinessExercisesVguid: MainRowID,
                    //        BusinessExercisesDetailVguid: id
                    //    },
                    //    type: "POST",
                    //    success: function (data) {
                    //        console.log(data);
                    //    }
                    //});
                    //在答题卡中判断某一题是否已经完成提交
                    if (arr != "") {
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
                    var arr = []; //把答案保存在数组中
                    var goods = $.makeArray($(".swiper-slide-active").find(".kk"));
                    console.log(goods);
                    //var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID

                    //console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串
                    console.log(Array);
                    //$.ajax({
                    //    url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                    //    data: {
                    //        BusinessPersonnelVguid: $("#PersonVguid").val(),
                    //        Answer: Array,
                    //        BusinessExercisesVguid: MainRowID,
                    //        BusinessExercisesDetailVguid: id
                    //    },
                    //    type: "POST",
                    //    success: function (data) {
                    //        console.log(data);
                    //    }
                    //});
                    //在答题卡中判断某一题是否已经完成提交
                    if (arr != "") {
                        $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
                    }
                    else {
                        $(".ss").eq(count).removeClass("active").css("background", "#999");
                    }
                }

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
                    console.log(dataLen)
                    $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
                }
                else {
                    $(".f_center1").removeClass("active").css("background", "#999");
                }
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
    });
});


//简答题
function LeaveSubmit(e) {
    var test = $(e).val();

    if (test != "") {
        var id = dataDetailRow[count].ExercisesDetailVGUID;
        $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
        //判断那个答案选项被追加上样式
        number1++;
        //判断答题是否已经完成
        if (number1 == dataLen) {
            console.log(number1);
            console.log(dataLen)
            $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
        } else if (number1 > dataLen) {
            number1--;
        }
        else {
            $(".f_center1").removeClass("active").css("background", "#999");
        }
        //console.log(id);
        //套题ID MainRowID
        $.ajax({
            url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
            data: {
                BusinessPersonnelVguid: $("#PersonVguid").val(),
                Answer: test,
                BusinessExercisesVguid: MainRowID,
                BusinessExercisesDetailVguid: id
            },
            type: "POST",
            success: function (data) {
                console.log(data);
            }
        })
    } else {
        number1--;
        $(".f_center1").removeClass("active").css("background", "#999");
    }
}

//点击交卷
$(".f_center").click(function () {
    var url = $("#OpenHttpAddress").val() + "/BasicDataManagement/WeChatExercise/Index?exerciseVguid=" + MainRowID;
    if ($("#isHistory").val() == "1") {
        $.ajax({
            url: "/BasicDataManagement/WeChatExercise/ReWechatPushExercise",
            data: {
                BusinessPersonnelVguid: $("#PersonVguid").val(),
                wechatMainVguid: $("#txtWechatMain").val()
            },
            type: "POST",
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "0":
                        alert("获取失败");
                        break;
                    case "1":
                        alert("获取成功");
                        $(".f_center").css('display', 'none');
                        break;
                }
            }
        });
    }
    else if ($(".f_center1").hasClass("active")) {
        $.ajax({
            url: "/BasicDataManagement/WeChatExercise/SubmitAllExercise",
            data: {
                BusinessPersonnelVguid: $("#PersonVguid").val(),
                businessExercisesVguid: MainRowID
            },
            type: "POST",
            success: function (data) {
                if (data.isSuccess == true) {
                    if (Number(data.respnseInfo) >= 100) {
                        alert("本次答题已完成,您此次答题的分数为" + data.respnseInfo + "！");
                        $("#success").show();
                    }
                    else if (data.respnseInfo == "-1") {
                        alert("本次答题已完成,等待阅卷！");
                        $("#marking").show();
                    }
                    else {
                        var count = (userAbleAnswerCount - 1) - answerCount;//剩余答题机会
                        alert("您此次答题的分数为" + data.respnseInfo + "，此次答题未达标，您还有" + count + "次答题机会！");
                        if (count == 0) {
                            $("#error").show();
                            return;
                        }
                        //若是习题推送(null)，答题完后刷新当前习题界面即可；若是培训推送，答题完后跳转回培训界面
                        if ($("#PushContentVguid").val() == "") {
                            window.location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + $("#CorpID").val() + "&redirect_uri=" + url + "&response_type=code&scope=snsapi_base#wechat_redirect";
                        } else {
                            window.location.href = $("#OpenHttpAddress").val() + "/WeChatPush/PushDetail/PushDetail?Vguid=" + $("#PushContentVguid").val();
                        }
                    }
                } else {
                    alert("提交失败");
                }
            }
        });
    } else {
        alert("当前共" + dataLen + "道题，已完成" + number1 + "道题，未完成" + (dataLen - number1) + "道题");
    }
});

