//5.给后台传递数据	
//给简答题生成一个文本框
//获取所有题的长度，在页面显示页面

var dataDetailRow;
var dataLen;

var MainRowID;
var count = 0;
var number1 = 0;
dataLen = 0;
var answerCount = 0;
var userAbleAnswerCount = $("#AnswerCount").val();
var isPushed = 0;  //是否推送过
//获取数据
$(function () {
    $.ajax({
        url: "/BasicDataManagement/WeChatExercise/GetExerciseAll",
        data: {
            vguid: $("#ExerciseVguid").val(),//"11F62166-64A8-4B05-9B97-F534C804181F",
            personVguid: $("#PersonVguid").val() //"575808EF-B574-48A3-B5BB-4D0EBB1B3592" 
        },
        type: "get",
        dataType: 'json',
        success: function (data) {
            dataDetailRow = data.DetailRow;
            dataLen = dataDetailRow.length;
            answerCount = data.MainRow[0].SolveNumber;//答题次数
            if (dataDetailRow[0].Status == 0 && $("#isHistory").val() == "0") {
                if ($("#txtIsValid").val() == "True") {
                    $("#errorExprice").css("display", "block");
                    return false;
                }
            }
            if (dataDetailRow[0].Status == 1 && $("#isHistory").val() == "0") {
                if ($("#txtIsValid").val() == "True") {
                    $("#errorExprice").css("display", "block");
                    return false;
                }
            }
            //答过题的不允许再次答题
            if (dataDetailRow[0].Status == 2 && $("#isHistory").val() == "0") {
                //判断是否有简答题
                for (var i = 0; i < dataLen; i++) {
                    //判断是否有简答题（ExerciseType等于4代表有简答题）
                    if (dataDetailRow[i].ExerciseType == 4) {
                        //判断简答题是否阅卷（1为没阅过,2为已阅）
                        if (data.MainRow[0].Marking == 1) {
                            //alert("本次答题已完成,等待阅卷！");
                            $("#marking").show();
                        }
                            //已阅卷
                        else if (data.MainRow[0].Marking == 2) {
                            if (data.MainRow[0].TotalScore < 60) {
                                //   alert("您此次答题的分数为" + data.MainRow[0].TotalScore + "，此次答题未达标，请与车队联系进行线下培训！");
                                //不及格的话判断答题次数，超过3(userAbleAnswerCount)次不允许再次答题
                                var ansCount = userAbleAnswerCount - answerCount;//剩余答题机会
                                if (ansCount == 0) {
                                    $("#error").show();
                                    return false;
                                }
                                alert("您此次答题的分数为" + data.MainRow[0].TotalScore + "，此次答题未达标，您还有" + ansCount + "次答题机会！");
                            } else {
                                $("#success").show();
                                return false;
                            }
                        }
                    }
                }
                //判断是否及格（60分及格）
                if (data.MainRow[0].TotalScore >= 60) {
                    $("#success").show();
                    return false;
                } else {
                    //不及格的话判断答题次数，超过3(userAbleAnswerCount)次不允许再次答题
                    if (data.MainRow[0].SolveNumber >= userAbleAnswerCount) {
                        $("#error").show();
                        return false;
                    } else if (data.MainRow[0].SolveNumber < userAbleAnswerCount && $("#txtIsValid").val() == "True") {
                        $("#errorExprice").css("display", "block");
                        return false;
                    }
                }
            }
            var len = dataDetailRow.length;
            MainRowID = data.MainRow[0].BusinessExercisesVGUID; //当前习题的套题ID
            //var PicturePath = data.MainRow[0].PicturePath;
            //Add User:刘洋
            //Add Date:2017.4.17
            //Start
            //if ($("#isHistory").val() == "1") {
            //    $("#photo").hide();
            //}
            //else if ($("#txtIsValid").val() == "True") {
            //    $("#photo").hide();
            //}   //End
            //else if (PicturePath == "" || PicturePath == null) {
            //    $("#photo").show();
            //} else {
            //    $("#photo").hide();
            //}
            for (var i = 0; i < len; i++) {
                var htmler1 = ""; //打印题目信息（选项和答案）
                var exercises = ''; //显示在每个页面的内容
                var oldAnswer = dataDetailRow[i].ExercisesAnswerDetailAnswer;
                if (oldAnswer == null) {
                    oldAnswer = "";
                }

                if (oldAnswer != "" && oldAnswer != null) {
                    var answers = "<div  class='ss active' style='background: rgb(20, 108, 127)'>" + (i + 1) + "</div>";
                } else {
                    var answers = "<div  class='ss'>" + (i + 1) + "</div>";
                }

                $(".answer").append(answers);
                var test = dataDetailRow[i].ExerciseType; //判断当前题型
                //console.log(test);
                //通过题型判断生成指定的页面
                if (test == 1) {
                    var itemstr = dataDetailRow[i].ExerciseOption.split(",|");
                    for (var j = 0; j < itemstr.length; j++) {
                        htmler1 += '<div class="so_p">';
                        var index = oldAnswer.indexOf(j);
                        if (index > -1) {
                            htmler1 += '<div class="option kk active"  nameSub="1">' + itemstr[j].split(".")[0];
                        } else {
                            htmler1 += '<div class="option kk"  nameSub="1">' + itemstr[j].split(".")[0];
                        }
                        htmler1 += '</div>' +
							'<div class="title"  nameSub="1">' + itemstr[j].split(".")[1] +
							'</div>' +
							'</div>';
                    }

                    exercises = '<div class="swiper-slide swiper-no-swiping">' +
						'<div class="s_title">' +
						'<div class="st_title">' + '<span>[单选题]&nbsp&nbsp</span>' + dataDetailRow[i].ExerciseName + '</div>' +
						'</div>' +
						'<div class="s_option">' + htmler1 + '</div>' +
						'</div>';
                } else if (test == 2) {
                    var itemstr = dataDetailRow[i].ExerciseOption.split(",|");
                    for (var j = 0; j < itemstr.length; j++) {
                        htmler1 += '<div class="so_p">';
                        var index = oldAnswer.indexOf(j);
                        if (index > -1) {
                            htmler1 += '<div class="option kk active"  nameSub="2">' + itemstr[j].split(".")[0];
                        } else {
                            htmler1 += '<div class="option kk"  nameSub="2">' + itemstr[j].split(".")[0];
                        }
                        htmler1 += '</div>' +
							'<div class="title"  nameSub="2">' + itemstr[j].split(".")[1] +
							'</div>' +
							'</div>';
                    }
                    exercises = '<div class="swiper-slide swiper-no-swiping">' +
						'<div class="s_title">' +
						'<div class="st_title">' + '<span>[多选题]&nbsp&nbsp</span>' + dataDetailRow[i].ExerciseName + '</div>' +
						'</div>' +
						'<div class="s_option">' + htmler1 + '</div>' +
						'</div>';
                } else if (test == 3) {
                    var itemstr = dataDetailRow[i].ExerciseOption.split(",|");
                    for (var j = 0; j < itemstr.length; j++) {
                        htmler1 += '<div class="so_p">';
                        var index = oldAnswer.indexOf(j);
                        if (index > -1) {
                            htmler1 += '<div class="option kk active"  nameSub="1">' + itemstr[j].split(".")[0];
                        } else {
                            htmler1 += '<div class="option kk"  nameSub="1">' + itemstr[j].split(".")[0];
                        }
                        htmler1 += '</div>' +
							'<div class="title"  nameSub="1">' + itemstr[j].split(".")[1] +
							'</div>' +
							'</div>';
                    }
                    exercises = '<div class="swiper-slide swiper-no-swiping">' +
						'<div class="s_title">' +
						'<div class="st_title">' + '<span>[判断题]&nbsp&nbsp</span>' + dataDetailRow[i].ExerciseName + '</div>' +
						'</div>' +
						'<div class="s_option">' + htmler1 + '</div>' +
						'</div>';
                } else if (test == 4) {
                    exercises = '<div class="swiper-slide swiper-no-swiping">' +
						'<div class="s_title">' +
						'<div class="st_title">' + '<span>[简答题]&nbsp&nbsp</span>' + dataDetailRow[i].ExerciseName + '</div>' +
						'</div>' +
						'<div class="s_option" nameSub="4">';
                    if (oldAnswer != "") {
                        htmler1 += '<textarea  style="width: 75%;height: 5rem;margin:5% 10%" onBlur="LeaveSubmit(this)">' + oldAnswer + '</textarea>';
                    } else {
                        htmler1 += '<textarea  style="width: 75%;height: 5rem;margin:5% 10%" onBlur="LeaveSubmit(this)"></textarea>';
                    }
                    exercises += htmler1;
                    exercises += '</div>' +
						'</div>';
                }
                $(".swiper-wrapper").append(exercises);

            }
            //Add User:刘洋
            //Add Date:2017.4.17
            if ($("#isHistory").val() == "1") {
                $("#dvTrans").css("display", "block");
                $.ajax({
                    url: "/BasicDataManagement/WeChatExercise/IsPushed",
                    data: {
                        BusinessPersonnelVguid: $("#PersonVguid").val(),
                        wechatMainVguid: $("#txtWechatMain").val()
                    },
                    type: "POST",
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                $(".f_center2").text("获取习题");
                                $(".f_center1").addClass("active").css("background", "rgb(164, 191, 17)");
                                break;
                            case "1":
                                isPushed = 1;
                                $(".f_center").css('display', 'none');
                                break;
                        }
                    }
                });

            }
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
                if (count > dataDetailRow.length - 1) {
                    count = dataDetailRow.length - 1;
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
                    var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串
                    console.log(Array);
                    //给后台上传数据
                    $.ajax({
                        url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                        data: {
                            BusinessPersonnelVguid: $("#PersonVguid").val(),//"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                            Answer: Array,
                            BusinessExercisesVguid: MainRowID,
                            BusinessExercisesDetailVguid: id
                        },
                        type: "POST",
                        success: function (data) {
                            console.log(data);
                        }
                    })
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
                    var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串

                    $.ajax({
                        url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                        data: {
                            BusinessPersonnelVguid: $("#PersonVguid").val(),//"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                            Answer: Array,
                            BusinessExercisesVguid: MainRowID,
                            BusinessExercisesDetailVguid: id
                        },
                        type: "POST",
                        success: function (data) {
                            console.log(data);
                        }
                    });
                    //在答题卡中判断某一题是否已经完成提交
                    if (arr != "") {
                        $(".ss").eq(count).addClass("active").css("background", "rgb(20, 108, 127)");
                    }
                    else {
                        $(".ss").eq(count).removeClass("active").css("background", "#999");
                    }
                } else if ($(this).attr("nameSub") == 4) {
                    var text = $(this).text();
                    $.ajax({
                        url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                        data: {
                            BusinessPersonnelVguid: $("#PersonVguid").val(),//"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                            Answer: text,
                            BusinessExercisesVguid: MainRowID,
                            BusinessExercisesDetailVguid: id
                        },
                        type: "POST",
                        success: function (data) {
                            console.log(data);
                        }
                    });
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
                    var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID
                    console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串

                    //给后台上传数据
                    $.ajax({
                        url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                        data: {
                            BusinessPersonnelVguid: $("#PersonVguid").val(), //"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                            Answer: Array,
                            BusinessExercisesVguid: MainRowID,
                            BusinessExercisesDetailVguid: id
                        },
                        type: "POST",
                        success: function(data) {
                            console.log(data);
                        }
                    });
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
                    var id = dataDetailRow[count].ExercisesDetailVGUID; // 每一道题的ID

                    console.log(id);
                    var len = goods.length;
                    for (var k = 0; k < len; k++) {
                        if (jQuery(goods[k]).hasClass("active")) {
                            arr.push(k);
                        }
                    }
                    var Array = arr.join(","); //上传给服务器答案,转化成字符串
                    console.log(Array);
                    $.ajax({
                        url: "/BasicDataManagement/WeChatExercise/SubmitUserAnswer",
                        data: {
                            BusinessPersonnelVguid: $("#PersonVguid").val(),//"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                            Answer: Array,
                            BusinessExercisesVguid: MainRowID,
                            BusinessExercisesDetailVguid: id
                        },
                        type: "POST",
                        success: function (data) {
                            console.log(data);
                        }
                    });
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
            $(".fc_right").html(dataDetailRow.length);
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
        console.log(id);
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

//上传图片功能
$(function () {
    var dataURL; //照片路径
    $("#file").change(function (event) {
        var $file = $(this);
        var fileObj = $file[0];
        var windowURL = window.URL || window.webkitURL;
        var $img = $("#preview");
        if (fileObj && fileObj.files && fileObj.files[0]) {
            dataURL = windowURL.createObjectURL(fileObj.files[0]);
            $img.attr('src', dataURL);
        } else {
            dataURL = $file.val();
            var imgObj = document.getElementById("preview");
            // 两个坑:
            // 1、在设置filter属性时，元素必须已经存在在DOM树中，动态创建的Node，也需要在设置属性前加入到DOM中，先设置属性在加入，无效；
            // 2、src属性需要像下面的方式添加，上面的两种方式添加，无效；
            imgObj.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)";
            imgObj.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = dataURL;
            //  console.log(dataURL);
        }
    });
    $(".upload").click(function () {
        if ($("#file").val() != "" && $("#file").val() != null) {
            $("#Uploading").css("display", "block");
            $("#photo").ajaxSubmit({
                url: "/BasicDataManagement/WeChatExercise/LoadUserPic",
                data: {
                    personVguid: $("#PersonVguid").val(),//"B3A5A687-2CB8-4693-B659-1C1EDCE13B14",
                    exerciseVguid: MainRowID
                },
                type: "post",
                dataType: 'json',
                success: function (data) {
                    if (data.IsError == true) {
                        alert("上传失败！");
                        $("#photo").hide();
                    } else {
                        alert("上传成功！");
                        $("#photo").hide();
                    }
                    $("#Uploading").hide();
                }
            });
        } else {
            alert("请选择图片！");
        }
    });
});
