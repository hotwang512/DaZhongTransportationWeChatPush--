var selector = {
    //Grid
    $exerciseDetailGrid: function () { return $("#ExerciseDetailGrid") },
    $exerciseDialog: function () { return $("#ExerciseDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnAdd: function () { return $("#btnAdd") },//新增习题
    $btnDelete: function () { return $("#btnDelete") },//删除习题
    $btnSaveExercise: function () { return $("#btnSaveExercise") },//保存（新增/编辑）习题详情
    $btnCancelExercise: function () { return $("#btnCancelExercise") },//取消保存（新增/编辑）习题详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项

    //习题基本信息
    $ExercisesName: function () { return $("#ExercisesName") },
    $VCRTTIME: function () { return $("#VCRTTIME") },
    $EffectiveDate: function () { return $("#EffectiveDate") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $exerciseTypeRadio: function () { return $(".ExerciseTypeRadio") },

    //单选题
    $singleChoiceName: function () { return $("#singleChoiceContent") },
    $singleScore: function () { return $("#singleScore") },
    $singleAnswer: function () { return $("#singleAnswer") },
    $singleChecked: function () { return $(".singleChecked") },
    //多选题
    $multipleChoiceName: function () { return $("#multipleChoiceContent") },
    $multipleScore: function () { return $("#multipleScore") },
    $multipleAnswer: function () { return $("#multipleAnswer") },
    $multipleChecked: function () { return $(".multipleChecked") },
    //判断题
    $judgeExerciseName: function () { return $("#judgeExerciseContent") },
    $judgeScore: function () { return $("#judgeScore") },
    $judgeAnswer: function () { return $("#judgeAnswer") },
    //简答题
    $shortAnswerName: function () { return $("#shortAnswerContent") },
    $shortScore: function () { return $("#shortScore") },
    $shortQuestionAnswer: function () { return $("#shortQuestionAnswer") },

    //隐藏控件
    $isEdit: function () { return $("#isEdit") },//习题总体信息新增/编辑标示
    $vguid: function () { return $("#editVguid") },
    $isEditDetail: function () { return $("#isEditDetail") },//习题详情新增/编辑标示
    $editVguidDetail: function () { return $("#editVguidDetail") },//每个题的Vguid
    $exerciseTitleID: function () { return $("#exerciseTitleID") },//题号

    //form提交
    $exerciseMainModelForm: function () { return $("#ExerciseDetailForm") },
    $singleChoiceForm: function () { return $("#singleChoice") },
    $multipleChoiceForm: function () { return $("#multipleChoice") },
    $judgeExerciseForm: function () { return $("#judgeExercise") },
    $shortAnswerForm: function () { return $("#shortAnswer") },

    //页面-角色权限
    $ReadsPermission: function () { return $("#ReadsPermission") },
    $EditPermission: function () { return $("#EditPermission") },
    $DeletesPermission: function () { return $("#DeletesPermission") },
    $AddsPermission: function () { return $("#AddsPermission") },
    $SubmitPermission: function () { return $("#SubmitPermission") },
    $ApprovedPermission: function () { return $("#ApprovedPermission") },
    $ImportPermission: function () { return $("#ImportPermission") },
    $ExportPermission: function () { return $("#ExportPermission") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        loadExerciseData();
        //LoadExerciseList();
    }; //addEvent end

    //有效时间控件
    selector.$EffectiveDate().datepicker({
        format: "yyyy-mm-dd",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        startDate: selector.$VCRTTIME().val()
    });

    //保存习题主信息按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$ExercisesName())) {
            validateError++;
        }
        if (!Validate(selector.$EffectiveDate())) {
            validateError++;
        }
        if (validateError <= 0) {
            var totalScore = 0;
            for (var i = 0; i < dataExerciseArray.length; i++) {
                totalScore += parseInt(dataExerciseArray[i].Score);
            }
            if (totalScore == 100) {
                selector.$exerciseMainModelForm().ajaxSubmit({
                    url: '/BasicDataManagement/ExerciseManagement/SaveExerciseMain',
                    type: "post",
                    data: { isEdit: selector.$isEdit().val(), exerciseData: JSON.stringify(dataExerciseArray) },
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                jqxNotification("保存失败！", null, "error");
                                break;
                            case "1":
                                jqxNotification("保存成功！", null, "success");
                                window.location.href = "/BasicDataManagement/ExerciseManagement/ExerciseList";
                        }
                    }
                });
            } else if (totalScore > 100) {
                jqxNotification("总分大于100分！", null, "error");
            } else {
                jqxNotification("总分小于100分！", null, "error");
            }
        }
    });

    //取消习题主信息按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/BasicDataManagement/ExerciseManagement/ExerciseList";
    });

    //新增习题按钮事件
    selector.$btnAdd().on('click', function () {
        newExerciseDetail();
    });

    //删除习题按钮事件
    selector.$btnDelete().on('click', function () {
        selection = [];
        var selectExerciseID = "";
        var grid = selector.$exerciseDetailGrid();
        var checkBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checkBoxs.each(function () {
            if ($(this).is(":checked")) {
                var index = $(this).attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.ExericseTitleID);
            }
        })
        if (selection.length < 1) {
            alert("请选择要删除的数据行！");
        } else {
            if (confirm("您确定要删除这条数据？")) {
                for (var i = 0; i < selection.length; i++) {
                    deleteExerciseData(selection[i]);
                }
            }
        }
    });

    //习题详情弹出框确定按钮事件
    selector.$btnSaveExercise().on('click', function () {
        var page = 1;
        var exerciseType;
        var singleStr = "";
        var multipleStr = "";
        selector.$exerciseTypeRadio().each(function () {
            if ($(this).is(":checked")) {
                exerciseType = $(this).val();
            }
        });
        if (selector.$isEditDetail().val() == "0") {//新增
            if (exerciseType == "1")//提交单选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$singleChoiceName())) {
                    validateError++;
                }
                if (!Validate(selector.$singleScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var singleCountOption = 0;
                    var singleCountOptionText = "";
                    $("input[name='singleOption']").each(function () {
                        singleCountOption++;
                        //将数字准换成相对应的字母
                        singleCountOptionText = letter[singleCountOption - 1];
                        singleStr += singleCountOptionText + "." + $(this).val() + ",|";
                    })
                    singleStr = singleStr.substring(0, singleStr.length - 2);//获得选项字符串
                    //题号
                    if (dataExerciseArray.length > 0) {
                        var i = dataExerciseArray.length;
                        page = i + 1;
                    }
                    //获取答案字符串（并将数字转换成字母）
                    var answer = "";
                    var countSingle = 0;
                    selector.$singleChecked().each(function () {
                        if ($(this).is(":checked")) {
                            answer += letter[$(this).val() - 1] + ",";
                            countSingle++;
                        }
                    });
                    if (countSingle == 1) {
                        answer = answer.substring(0, answer.length - 1);
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 1, singleStr, selector.$singleChoiceName().val(), answer, selector.$singleScore().val());
                        dataExerciseArray.push(dataExerciseModel);
                        //重新加载题号
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            dataExerciseArray[i].ExericseTitleID = i + 1;
                        }
                        appendExercise(JSON.stringify(dataExerciseArray));
                        selector.$exerciseDialog().modal('hide');
                    } else if (countSingle < 1) {
                        jqxNotification("单选题必须有一个正确答案！", null, "error");
                    } else {
                        jqxNotification("单选题只有一个正确答案！", null, "error");
                    }
                }
            } else if (exerciseType == "2")//提交多选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$multipleChoiceName())) {
                    validateError++;
                }
                if (!Validate(selector.$multipleScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var multipleCountOption = 0;
                    var multipleCountOptionText = "";
                    $("input[name='multipleOption']").each(function () {
                        multipleCountOption++;
                        //将数字准换成相对应的字母
                        multipleCountOptionText = letter[multipleCountOption - 1];
                        multipleStr += multipleCountOptionText + "." + $(this).val() + ",|";
                    })
                    multipleStr = multipleStr.substring(0, multipleStr.length - 2);//获得选项字符串
                    if (dataExerciseArray.length > 0) {
                        var i = dataExerciseArray.length;
                        page = i + 1;
                    }
                    var answer = "";
                    var multipleCount = 0;
                    selector.$multipleChecked().each(function () {
                        if ($(this).is(":checked")) {
                            answer += letter[$(this).val() - 1] + ",";
                        }
                    });
                    if (multipleCount < 1) {
                        jqxNotification("多选题必须有一个正确答案！", null, "error");
                    } else {
                        answer = answer.substring(0, answer.length - 1);
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 2, multipleStr, selector.$multipleChoiceName().val(), answer, selector.$multipleScore().val());
                        dataExerciseArray.push(dataExerciseModel);
                        //重新加载题号
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            dataExerciseArray[i].ExericseTitleID = i + 1;
                        }
                        appendExercise(JSON.stringify(dataExerciseArray));
                        selector.$exerciseDialog().modal('hide');
                    }
                }
            } else if (exerciseType == "3")//提交判断题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$judgeExerciseName())) {
                    validateError++;
                }
                if (!Validate(selector.$judgeScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var judgeAnswer = selector.$judgeAnswer().val();
                    if (judgeAnswer == "1") {
                        judgeAnswer = "正确";
                    } else {
                        judgeAnswer = "错误";
                    }
                    if (dataExerciseArray.length > 0) {
                        var i = dataExerciseArray.length;
                        page = i + 1;
                    }
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 3, judgeOption, selector.$judgeExerciseName().val(), judgeAnswer, selector.$judgeScore().val());
                    dataExerciseArray.push(dataExerciseModel);
                    //重新加载题号
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        dataExerciseArray[i].ExericseTitleID = i + 1;
                    }
                    appendExercise(JSON.stringify(dataExerciseArray));
                    selector.$exerciseDialog().modal('hide');
                }
            } else if (exerciseType == "4")//提交简答题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$shortAnswerName())) {
                    validateError++;
                }
                if (!Validate(selector.$shortScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    if (dataExerciseArray.length > 0) {
                        var i = dataExerciseArray.length;
                        page = i + 1;
                    }
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 4, "", selector.$shortAnswerName().val(), selector.$shortQuestionAnswer().val(), selector.$shortScore().val());
                    dataExerciseArray.push(dataExerciseModel);
                    //重新加载题号
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        dataExerciseArray[i].ExericseTitleID = i + 1;
                    }
                    appendExercise(JSON.stringify(dataExerciseArray));
                    selector.$exerciseDialog().modal('hide');
                }
            }
        } else {//编辑
            if (exerciseType == "1")//提交单选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$singleChoiceName())) {
                    validateError++;
                }
                if (!Validate(selector.$singleScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var singleCountOption = 0;
                    var singleCountOptionText = "";
                    $("input[name='singleOption']").each(function () {
                        singleCountOption++;
                        //将数字准换成相对应的字母
                        singleCountOptionText = letter[singleCountOption - 1];
                        singleStr += singleCountOptionText + "." + $(this).val() + ",|";
                    })
                    singleStr = singleStr.substring(0, singleStr.length - 2);//获得选项字符串
                    //题号
                    page = selector.$exerciseTitleID().val();
                    //获取答案字符串（并将数字转换成字母）
                    var answer = "";
                    var countSingle = 0;
                    selector.$singleChecked().each(function () {
                        if ($(this).is(":checked")) {
                            answer += letter[$(this).val() - 1] + ",";
                            countSingle++;
                        }
                    });
                    if (countSingle == 1) {
                        answer = answer.substring(0, answer.length - 1);
                        //查询数组的匹配数据
                        var exerciseModel = new dataExerciseModel_Temp();
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            if (dataExerciseArray[i].ExericseTitleID == selector.$exerciseTitleID().val()) {
                                exerciseModel = dataExerciseArray[i];
                            }
                        }
                        exerciseModel.ExerciseOption = singleStr;
                        exerciseModel.ExerciseName = selector.$singleChoiceName().val();
                        exerciseModel.Answer = answer;
                        exerciseModel.Score = selector.$singleScore().val();
                        //var dataExerciseModel = new dataExerciseModel_Temp(page, 1, singleStr, selector.$singleChoiceName().val(), answer, selector.$singleScore().val());
                        //dataExerciseArray.push(dataExerciseModel);
                        ////重新加载题号
                        //for (var i = 0; i < dataExerciseArray.length; i++) {
                        //    dataExerciseArray[i].ExericseTitleID = i + 1;
                        //}
                        appendExercise(JSON.stringify(dataExerciseArray));
                        selector.$exerciseDialog().modal('hide');
                    } else if (countSingle < 1) {
                        jqxNotification("单选题必须有一个正确答案！", null, "error");
                    } else {
                        jqxNotification("单选题只有一个正确答案！", null, "error");
                    }
                }
            } else if (exerciseType == "2")//提交多选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$multipleChoiceName())) {
                    validateError++;
                }
                if (!Validate(selector.$multipleScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var multipleCountOption = 0;
                    var multipleCountOptionText = "";
                    $("input[name='multipleOption']").each(function () {
                        multipleCountOption++;
                        //将数字准换成相对应的字母
                        multipleCountOptionText = letter[multipleCountOption - 1];
                        multipleStr += multipleCountOptionText + "." + $(this).val() + ",|";
                    })
                    multipleStr = multipleStr.substring(0, multipleStr.length - 2);//获得选项字符串
                    //题号
                    page = selector.$exerciseTitleID().val();
                    var answer = "";
                    var multipleCount = 0;
                    selector.$multipleChecked().each(function () {
                        if ($(this).is(":checked")) {
                            answer += letter[$(this).val() - 1] + ",";
                            multipleCount++;
                        }
                    });
                    if (multipleCount < 1) {
                        jqxNotification("多选题必须有一个正确答案！", null, "error");
                    } else {
                        answer = answer.substring(0, answer.length - 1);
                        //查询数组的匹配数据
                        var exerciseModel = new dataExerciseModel_Temp();
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            if (dataExerciseArray[i].ExericseTitleID == selector.$exerciseTitleID().val()) {
                                exerciseModel = dataExerciseArray[i];
                            }
                        }
                        exerciseModel.ExerciseOption = multipleStr;
                        exerciseModel.ExerciseName = selector.$multipleChoiceName().val();
                        exerciseModel.Answer = answer;
                        exerciseModel.Score = selector.$multipleScore().val();
                        appendExercise(JSON.stringify(dataExerciseArray));
                        selector.$exerciseDialog().modal('hide');
                    }
                }
            } else if (exerciseType == "3")//提交判断题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$judgeExerciseName())) {
                    validateError++;
                }
                if (!Validate(selector.$judgeScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    var judgeAnswer = selector.$judgeAnswer().val();
                    if (judgeAnswer == "1") {
                        judgeAnswer = "正确";
                    } else {
                        judgeAnswer = "错误";
                    }
                    //题号
                    page = selector.$exerciseTitleID().val();
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    //查询数组的匹配数据
                    var exerciseModel = new dataExerciseModel_Temp();
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        if (dataExerciseArray[i].ExericseTitleID == selector.$exerciseTitleID().val()) {
                            exerciseModel = dataExerciseArray[i];
                        }
                    }
                    exerciseModel.ExerciseOption = judgeOption;
                    exerciseModel.ExerciseName = selector.$judgeExerciseName().val();
                    exerciseModel.Answer = judgeAnswer;
                    exerciseModel.Score = selector.$judgeScore().val();
                    appendExercise(JSON.stringify(dataExerciseArray));
                    selector.$exerciseDialog().modal('hide');
                }
            } else if (exerciseType == "4")//提交简答题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$shortAnswerName())) {
                    validateError++;
                }
                if (!Validate(selector.$shortScore())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    //题号
                    page = selector.$exerciseTitleID().val();
                    //查询数组的匹配数据
                    var exerciseModel = new dataExerciseModel_Temp();
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        if (dataExerciseArray[i].ExericseTitleID == selector.$exerciseTitleID().val()) {
                            exerciseModel = dataExerciseArray[i];
                        }
                    }
                    exerciseModel.ExerciseName = selector.$shortAnswerName().val();
                    exerciseModel.Answer = selector.$shortQuestionAnswer().val();
                    exerciseModel.Score = selector.$shortScore().val();
                    appendExercise(JSON.stringify(dataExerciseArray));
                    selector.$exerciseDialog().modal('hide');
                }
            }
        }
    });

    //习题详情弹出框取消按钮事件
    selector.$btnCancelExercise().on('click', function () {
        selector.$exerciseDialog().modal('hide');

    });

    //选择不同习题类型加载不同习题格式
    selector.$exerciseTypeRadio().on('click', function () {
        var id = $(this).val();
        if (id == 1) {
            $(".SingleChoice").removeAttr("style");
            $(".MultipleChoice").attr("style", "display:none");
            $(".JudgeExercise").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else if (id == 2) {
            $(".MultipleChoice").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".JudgeExercise").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else if (id == 3) {
            $(".JudgeExercise").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".MultipleChoice").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else {
            $(".ShortAnswer").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".MultipleChoice").attr("style", "display:none");
            $(".JudgeExercise").attr("style", "display:none");
        }
    });

    var tool = {

    }; // tool end
};

//全局字典
var letter = ["A", "B", "C", "D", "E", "F", "G", "H"];
//新增习题
function newExerciseDetail() {
    selector.$exerciseTypeRadio().each(function () {
        $(this).removeAttr("checked");
        $(this).removeAttr("disabled");
    });
    $(".SingleChoice").hide();
    $(".MultipleChoice").hide();
    $(".JudgeExercise").hide();
    $(".ShortAnswer").hide();
    //清空单选题内容
    selector.$singleChoiceName().val("");
    selector.$singleScore().val("");
    selector.$singleInputsWrapper().find('div').remove();
    selector.$singleInputsWrapper().append("<div style='height: 40px;'>" +
          "<div class='option' style='position: absolute; margin-top: 10px;'>" +
              "<span style='font-size: 14px;'>选项1：</span>" +
          "</div>" +
          "<input id='field_1' type='text' style='width: 75%; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +
          //"<div class='deleteOption'>" +
          //    "<a href='#' class='removeclass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
          //"</div>" +
          "<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              "<input id='firstSingleCheck' type='checkbox' class='singleChecked' style='margin-top: 10px;' value='1'/>" +
          "</div>" +
      "</div>");
    fieldCount = 1;
    singleCount = 1;
    //清空多选题内容
    selector.$multipleChoiceName().val("");
    selector.$multipleScore().val("");
    selector.$multipleInputsWrapper().find('div').remove();
    selector.$multipleInputsWrapper().append("<div style='height: 40px;'>" +
          "<div class='option' style='position: absolute; margin-top: 10px;'>" +
              "<span style='font-size: 14px;'>选项1：</span>" +
          "</div>" +
          "<input id='multipleField_1' type='text' style='width: 75%; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
          //"<div class='deleteOption'>" +
          //    "<a href='#' class='removeclass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
          //"</div>" +
          "<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              "<input id='firstMultipleCheck' type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='1'/>" +
          "</div>" +
      "</div>");
    multipleCount = 1;
    multipleFieldCount = 1;
    //清空判断题内容
    selector.$judgeExerciseName().val("");
    selector.$judgeAnswer().val("1");
    selector.$judgeScore().val("");
    //清空简答题内容
    selector.$shortAnswerName().val("");
    selector.$shortScore().val("");
    selector.$shortQuestionAnswer().val("");

    selector.$exerciseDialog().find("input:text,select,input:hidden,input:password,textarea").each(function () {
        if ($(this).hasClass("input_Validate")) {
            $(this).removeClass("input_Validate");
            $(this).next(".msg").remove();
        }
    });

    //弹出新增框
    selector.$isEditDetail().val("0");
    selector.$editVguidDetail().val("");
    selector.$myModalLabel_title().text("新增习题");
    selector.$exerciseDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$exerciseDialog().modal('show');
}

//编辑时加载习题详细信息列表
function loadExerciseData() {
    var exerciseMainVguid = selector.$vguid().val();//习题主信息Vguid
    $.ajax({
        url: "/ExerciseManagement/GetExerciseDetailListByMainVguid",
        data: { "Vguid": exerciseMainVguid },
        type: "post",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var id = data[i].ExericseTitleID;
                var exerciseType = data[i].ExerciseType;
                var exerciseOption = data[i].ExerciseOption;
                var exerciseName = data[i].ExerciseName;
                var answer = data[i].Answer;
                var score = data[i].Score;
                var exerciseDataModel = new dataExerciseModel_Temp(id, exerciseType, exerciseOption, exerciseName, answer, score);
                dataExerciseArray.push(exerciseDataModel);
            }
            appendExercise(data);
        }
    });
}

//绘制习题详细信息表格
function appendExercise(data) {
    var editedRows = new Array();
    var source =
        {
            datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'ExericseTitleID', type: 'string' },
                    { name: 'ExerciseType', type: 'string' },
                    { name: 'ExerciseName', type: 'string' },
                    { name: 'ExerciseOption', type: 'string' },
                    { name: 'Answer', type: 'string' },
                    { name: 'Score', type: 'string' },
                    { name: 'Vguid', type: 'string' },
                    { name: 'ExercisesInformationVguid', type: 'string' }
                ],
            datatype: "json",
            updaterow: function (rowid, rowdata, commit) {
                // that function is called after each edit.
                //var rowindex = selector.$exerciseDetailGrid().jqxGrid('getrowboundindexbyid', rowid);
                //editedRows.push({ index: rowindex, data: rowdata });
                commit(true);
            },
            id: "Vguid",//主键
            async: true,
            localdata: data,
            //data: { "Vguid": exerciseMainVguid },//参数
            //url: "/ExerciseManagement/GetExerciseDetailListByMainVguid"   //获取数据源的路径
        };
    var dataAdapter = new $.jqx.dataAdapter(source);
    selector.$exerciseDetailGrid().jqxDataTable(
    {
        width: "100%",
        height: 320,
        source: dataAdapter,
        pageable: true,
        //editable: true,
        altRows: true,
        editSettings: { saveOnPageChange: true, saveOnBlur: true, saveOnSelectionChange: true, cancelOnEsc: true, saveOnEnter: true, editSingleCell: true, editOnDoubleClick: true, editOnF2: true },
        pagerButtonsCount: 10,
        theme: "office",
        columns: [
          { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
          { text: '题号', datafield: 'ExericseTitleID', width: 50, align: 'center', cellsalign: 'center' },
          { text: '习题类型', datafield: 'ExerciseType', width: 200, align: 'center', cellsalign: 'center', cellsRenderer: explainExerciseType },
          { text: '习题内容', datafield: 'ExerciseName', align: 'center', cellsalign: 'center' },
          { text: '答案', datafield: 'Answer', align: 'center', cellsalign: 'center', cellsRenderer: explainJudgeAnswer },
          { text: '分值', datafield: 'Score', width: 50, align: 'center', cellsalign: 'center' },
        ]
    });
    //翻译判断题答案
    function explainJudgeAnswer(row, column, value, rowData) {
        var container = rowData.Answer;
        if (rowData.Answer == 0) {
            container = "正确";
        } else if (rowData.Answer == 1) {
            container = "错误";
        }
        return container;
    }

    //翻译习题类型
    function explainExerciseType(row, column, value, rowData) {
        var container = "";
        switch (rowData.ExerciseType) {
            case 1:
                container = "单选题";
                break;
            case 2:
                container = "多选题";
                break;
            case 3:
                container = "判断题";
                break;
            case 4:
                container = "简答题";
                break;
        }
        return container;
    };

    function cellsRendererFunc(row, column, value, rowData) {
        return "<input class=\"jqx_datatable_checkbox\" index=\"" + row + "\" type=\"checkbox\"  style=\"margin:auto;width: 17px;height: 17px;\" />";
    };

    function rendererFunc() {
        var checkBox = "<div id='jqx_datatable_checkbox_all' class='jqx_datatable_checkbox_all' style='z-index: 999; margin-left:7px ;margin-top: 7px;'>";
        checkBox += "</div>";
        return checkBox;
    };

    function renderedFunc(element) {
        var grid = selector.$exerciseDetailGrid();
        element.jqxCheckBox();
        element.on('change', function (event) {
            var checked = element.jqxCheckBox('checked');

            if (checked) {
                var rows = grid.jqxDataTable('getRows');
                for (var i = 0; i < rows.length; i++) {
                    grid.jqxDataTable('selectRow', i);
                    grid.find(".jqx_datatable_checkbox").attr("checked", "checked")
                }
            } else {
                grid.jqxDataTable('clearSelection');
                grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked")
            }
        });
        return true;
    };
}

//习题详情临时模型
var dataExerciseModel_Temp = function (id, exerciseType, exerciseOption, exerciseName, answer, score) {
    this.ExericseTitleID = id;
    this.ExerciseType = exerciseType;
    this.ExerciseOption = exerciseOption;
    this.ExerciseName = exerciseName;
    this.Answer = answer;
    this.Score = score;
};
var dataExerciseArray = new Array();//习题详情数组

function clearRadio() {
    selector.$exerciseTypeRadio().each(function () {
        if ($(this).is(":checked")) {
            $(this).removeAttr("checked");
        }
    });
};

//双击行编辑事件
selector.$exerciseDetailGrid().on('rowDoubleClick', function (event) {
    var args = event.args;
    var key = args.key;
    var row = args.row;
    //选中行题号row.ExericseTitleID
    newExerciseDetail();//清空原有记录
    selector.$exerciseTitleID().val(row.ExericseTitleID);
    //clearRadio();
    //加载选中行数据
    $("#ExerciseTypeRadio" + row.ExerciseType).trigger("click");
    selector.$exerciseTypeRadio().each(function () {
        $(this).attr("disabled", "disabled");
    });

    if (row.ExerciseType == "1") {
        $(".SingleChoice").removeAttr("style");
        selector.$singleChoiceName().val(row.ExerciseName);
        selector.$singleScore().val(row.Score);
        singleOption = [];//单选题选项数组
        singleOption = row.ExerciseOption.split(",|");
        //加载单选题选项
        $("#field_1").val(singleOption[0].split(".")[1]);
        if (row.Answer == letter[0]) {
            $("#firstSingleCheck").attr("checked", "checked");
        }
        for (var i = 1; i < singleOption.length; i++) {
            fieldCount++;
            selector.$singleInputsWrapper().append("<div style='height: 40px;'>" +
                  "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                      "<span style='font-size: 14px;'>选项" + fieldCount + "：</span>" +
                  "</div>" +
                  "<input id='field_" + fieldCount + "' type='text' style='width: 75%; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +
                  "<div class='deleteOption'>" +
                      "<a href='#' class='removeclass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
                  "</div>" +
                  "<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                      "<input type='checkbox' class='singleChecked' style='margin-top: 10px;' value='" + fieldCount + "'/>" +
                  "</div>" +
              "</div>");
            singleCount++;
            $("#field_" + fieldCount).val(singleOption[i].split(".")[1]);
            selector.$singleChecked().each(function () {
                if (row.Answer == letter[$(this).val() - 1]) {
                    $(this).attr("checked", "checked");
                }
            });
        }

    } else if (row.ExerciseType == "2") {
        $(".MultipleChoice").removeAttr("style");
        selector.$multipleChoiceName().val(row.ExerciseName);
        selector.$multipleScore().val(row.Score);
        multipleOption = [];//所选题选项数组
        multipleOption = row.ExerciseOption.split(",|");
        $("#multipleField_1").val(multipleOption[0].split(".")[1]);
        var correctAnswer = row.Answer.split(",");
        for (var i = 0; i < correctAnswer.length; i++) {
            if (correctAnswer[i] == letter[0]) {
                $("#firstMultipleCheck").attr("checked", "checked");
            }
        }
        //加载多选题选项
        for (var i = 1; i < multipleOption.length; i++) {
            multipleFieldCount++;
            selector.$multipleInputsWrapper().append("<div style='height: 40px;'>" +
                  "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                      "<span style='font-size: 14px;'>选项" + multipleFieldCount + "：</span>" +
                  "</div>" +
                  "<input id='multipleField_" + multipleFieldCount + "' type='text' style='width: 75%; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
                  "<div class='deleteOption'>" +
                      "<a href='#' class='removeMultipleClass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
                  "</div>" +
                  "<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                      "<input id='firstMultipleCheck' type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='" + multipleFieldCount + "' />" +
                  "</div>" +
              "</div>");
            multipleCount++;
            $("#multipleField_" + multipleFieldCount).val(multipleOption[i].split(".")[1]);
            selector.$multipleChecked().each(function () {
                for (var i = 0; i < correctAnswer.length; i++) {
                    if (correctAnswer[i] == letter[$(this).val() - 1]) {
                        $(this).attr("checked", "checked");
                    }
                }
            });
        }

    } else if (row.ExerciseType == "3") {
        $(".JudgeExercise").removeAttr("style");
        selector.$judgeExerciseName().val(row.ExerciseName);
        selector.$judgeScore().val(row.Score);
        if (row.Answer == "正确") {
            selector.$judgeAnswer().val(1);
        } else {
            selector.$judgeAnswer().val(0);
        }
    } else {
        $(".ShortAnswer").removeAttr("style");
        selector.$shortAnswerName().val(row.ExerciseName);
        selector.$shortQuestionAnswer().val(row.Answer);
        selector.$shortScore().val(row.Score);
    }

    //弹出编辑框
    selector.$isEditDetail().val("1");
    //selector.$editVguidDetail().val("");
    selector.$myModalLabel_title().text("编辑习题");
    selector.$exerciseDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$exerciseDialog().modal('show');
});

//列表删除操作
function deleteExerciseData(id) {
    var exerciseModel = new dataExerciseModel_Temp();
    for (var i = 0; i < dataExerciseArray.length; i++) {
        if (dataExerciseArray[i].ExericseTitleID == id) {
            exerciseModel = dataExerciseArray[i];
        }
    }
    var index = dataExerciseArray.indexOf(exerciseModel);
    dataExerciseArray.splice(index, 1);
    for (var i = 0; i < dataExerciseArray.length; i++) {
        dataExerciseArray[i].ExericseTitleID = i + 1;
    }
    appendExercise(JSON.stringify(dataExerciseArray));
}

var maxInputs = 8;//最大input框数量
//单选题添加选项
var singleCount = selector.$singleInputsWrapper().length;
var fieldCount = 1;
//单选题添加选择项按钮事件
selector.$btnAddSingleOption().on('click', function () {
    if (singleCount < maxInputs) {
        fieldCount++;
        //selector.$singleInputsWrapper().append('<div style="height: 40px;"><input type="text" style="width: 100%;" name="singleOption" class="input_text form-control" id="field_' + fieldCount + '" value="选项' + fieldCount + '"/><div class="deleteOption"><a href="#" class="removeclass"><i class="iconfont btn_icon" style="color: black !important;">&#xe60a;</i></a></div></div>');
        selector.$singleInputsWrapper().append("<div style='height: 40px;'>" +
              "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                  "<span style='font-size: 14px;'>选项" + fieldCount + "：</span>" +
              "</div>" +
              "<input id='field_" + fieldCount + "' type='text' style='width: 75%; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +
              "<div class='deleteOption'>" +
                  "<a href='#' class='removeclass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
              "</div>" +
              "<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                  "<input type='checkbox' class='singleChecked' style='margin-top: 10px;' value='" + fieldCount + "'/>" +
              "</div>" +
          "</div>");
        singleCount++;
    }
    return false;
});
$("body").on("click", ".removeclass", function (e) { //user click on remove text 
    if (singleCount > 1) {
        $(this).parent('div').parent('div').remove(); //remove text box
        singleCount--; //decrement textbox
        fieldCount--;
    }
    return false;
});

//多选题添加选项
var multipleCount = selector.$multipleInputsWrapper().length;
var multipleFieldCount = 1;
selector.$btnAddmultipleOption().on('click', function () {
    if (multipleCount < maxInputs) {
        multipleFieldCount++;
        //selector.$multipleInputsWrapper().append('<div style="height: 40px;"><input type="text" style="width: 100%;" name="multipleOption" class="input_text form-control" id="multipleField_' + multipleFieldCount + '" value="选项' + multipleFieldCount + '"/><div class="deleteOption"><a href="#" class="removeMultipleClass"><i class="iconfont btn_icon" style="color: black !important;">&#xe60a;</i></a></div></div>');
        selector.$multipleInputsWrapper().append("<div style='height: 40px;'>" +
              "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                  "<span style='font-size: 14px;'>选项" + multipleFieldCount + "：</span>" +
              "</div>" +
              "<input id='multipleField_" + multipleFieldCount + "' type='text' style='width: 75%; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
              "<div class='deleteOption'>" +
                  "<a href='#' class='removeMultipleClass'><i class='iconfont btn_icon' style='color: black !important;'>&#xe60a;</i></a>" +
              "</div>" +
              "<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                  "<input type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='" + multipleFieldCount + "' />" +
              "</div>" +
          "</div>");
        multipleCount++;
    }
    return false;
});
$("body").on("click", ".removeMultipleClass", function (e) { //user click on remove text
    if (multipleCount > 1) {
        $(this).parent('div').parent('div').remove(); //remove text box
        multipleCount--; //decrement textbox
        multipleFieldCount--;
    }
    return false;
});

$(function () {
    var page = new $page();
    page.init();

})


