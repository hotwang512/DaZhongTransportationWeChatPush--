var selector = {
    //Grid
    $exerciseDetailGrid: function () { return $("#ExerciseDetailGrid") },
    $exerciseDialog: function () { return $("#ExerciseDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框
    $ExerciseLibraryList: function () { return $("#ExerciseLibraryList") },

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnAdd: function () { return $("#btnAdd") },//新增习题
    $btnDelete: function () { return $("#btnDelete") },//删除习题
    $btnSaveExercise: function () { return $("#btnSaveExercise") },//保存（新增/编辑）习题详情
    $btnCancelExercise: function () { return $("#btnCancelExercise") },//取消保存（新增/编辑）习题详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项
    $btnSelect: function () { return $("#btnSelect") },   //习题库选择
    $btnRandom: function () { return $("#btnRandom") },   //习题库随机
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnSaveFormalExercise: function () { return $("#btnSaveFormalExercise") },
    $btnCancelFormalExercise: function () { return $("#btnCancelFormalExercise") },
    //习题基本信息
    $ExercisesName: function () { return $("#ExercisesName") },
    $VCRTTIME: function () { return $("#VCRTTIME") },
    $EffectiveDate: function () { return $("#EffectiveDate") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $exerciseTypeRadio: function () { return $(".ExerciseTypeRadio") },
    //$isEntryExerciseLibrary: function () { return $(".isEntryExerciseLibrary") },
    $isEntryExerciseLibrary: function () { return $("#isEntryExerciseLibrary") },
    $ExerciseLibraryDialog: function () { return $("#ExerciseLibraryDialog") },
    //单选题
    $singleChoiceName: function () { return $("#singleChoiceContent") },
    $singleScore: function () { return $("#singleScore") },
    $singleAnswer: function () { return $("#singleAnswer") },
    $singleChecked: function () { return $(".singleChecked") },
    $isEntryExerciseLibrarySingle: function () { return $("#isEntryExerciseLibrarySingle") },

    //多选题
    $multipleChoiceName: function () { return $("#multipleChoiceContent") },
    $multipleScore: function () { return $("#multipleScore") },
    $multipleAnswer: function () { return $("#multipleAnswer") },
    $multipleChecked: function () { return $(".multipleChecked") },
    $isEntryExerciseLibraryMultiple: function () { return $("#isEntryExerciseLibraryMultiple") },
    //判断题
    $judgeExerciseName: function () { return $("#judgeExerciseContent") },
    $judgeScore: function () { return $("#judgeScore") },
    $judgeAnswer: function () { return $("#judgeAnswer") },
    $isEntryExerciseLibraryJudge: function () { return $("#isEntryExerciseLibraryJudge") },
    //简答题
    $shortAnswerName: function () { return $("#shortAnswerContent") },
    $shortScore: function () { return $("#shortScore") },
    $shortQuestionAnswer: function () { return $("#shortQuestionAnswer") },
    $isEntryExerciseLibraryAnswer: function () { return $("#isEntryExerciseLibraryAnswer") },
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
    //查询
    $exerciseName_Search: function () { return $("#ExercisesName_Search") },
    $ExerciseType_Search: function () { return $("#ExerciseType_Search") },
    $inputType_Search: function () { return $("#InputType_Search") },
    $createdTimeStart_Search: function () { return $("#CreatedTimeStart_Search") },
    $createTimeEnd_Search: function () { return $("#CreateTimeEnd_Search") },
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

var deletedExercise = [];  //要删除的习题
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
    selector.$EffectiveDate().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true,
        startDate: selector.$VCRTTIME().val()
    });
    //有效时间控件
    selector.$createdTimeStart_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true,
        //   startDate: selector.$VCRTTIME().val()
    });

    //有效时间控件
    selector.$createTimeEnd_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true,
        //  startDate: selector.$VCRTTIME().val()
    });
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //保存习题主信息按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$ExercisesName())) {
            validateError++;
        }
        if (!Validate(selector.$EffectiveDate())) {
            validateError++;
        }
        if (!checkEndTime()) {
            jqxNotification("有效时间不能小于创建时间", null, "error");
            validateError++;
        }
        if (validateError <= 0) {
            selector.$exerciseMainModelForm().ajaxSubmit({
                url: '/ExerciseManagement/ExerciseManagement/SaveExerciseMain',
                type: "post",
                data: { isEdit: selector.$isEdit().val(), exerciseData: JSON.stringify(dataExerciseArray) },
                traditional: true,
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            jqxNotification("保存成功！", null, "success");
                            window.location.href = "/ExerciseManagement/ExerciseManagement/ExerciseList";
                    }
                }
            });
        }
    });
    //比较时间大小
    function checkEndTime() {
        var startTime = selector.$VCRTTIME().val();
        var start = new Date(startTime.replace("-", "/").replace("-", "/"));
        var endTime = selector.$EffectiveDate().val();
        var end = new Date(endTime.replace("-", "/").replace("-", "/"));
        if (end < start) {
            return false;
        }
        return true;
    }
    //取消习题主信息按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/ExerciseManagement/ExerciseManagement/ExerciseList";
    });

    //新增习题按钮事件
    selector.$btnAdd().on('click', function () {
        newExerciseDetail();
    });

    //删除习题按钮事件
    selector.$btnDelete().on('click', function () {
        var selection = [];
        var grid = selector.$exerciseDetailGrid();
        var checkBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checkBoxs.each(function () {
            if ($(this).is(":checked")) {
                var index = $(this).attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.ExericseTitleID);
            }
        });
        if (selection.length < 1) {
            jqxNotification("请选择要删除的数据行！", null, "error");
        } else {
            deletedExercise = selection;
            WindowConfirmDialog(deleted, "您确定要删除这条数据？", "确认框", "确认", "取消");
        }
    });

    //习题详情弹出框确定按钮事件
    selector.$btnSaveExercise().on('click', function () {
        var page = 1;
        var exerciseType;
        var singleStr = "";
        var multipleStr = "";
        var isEntryLibrary = 0;  //是否录入习题库
        selector.$exerciseTypeRadio().each(function () {
            if ($(this).is(":checked")) {
                exerciseType = $(this).val();
            }
        });
        if (selector.$isEntryExerciseLibrary().is(":checked")) {
            isEntryLibrary = 1;
        }
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
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 1, singleStr, selector.$singleChoiceName().val(), answer, selector.$singleScore().val(), isEntryLibrary);
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
                            multipleCount++
                        }
                    });
                    if (multipleCount < 1) {
                        jqxNotification("多选题必须有一个正确答案！", null, "error");
                    } else {
                        answer = answer.substring(0, answer.length - 1);
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 2, multipleStr, selector.$multipleChoiceName().val(), answer, selector.$multipleScore().val(), isEntryLibrary);
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
                    if (dataExerciseArray.length > 0) {
                        var i = dataExerciseArray.length;
                        page = i + 1;
                    }
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 3, judgeOption, selector.$judgeExerciseName().val(), judgeAnswer, selector.$judgeScore().val(), isEntryLibrary);
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
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 4, "", selector.$shortAnswerName().val(), selector.$shortQuestionAnswer().val(), selector.$shortScore().val(), isEntryLibrary);
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
                        if (selector.$isEntryExerciseLibrary().is(":checked")) {
                            exerciseModel.IsEntry = "1";
                        } else {
                            exerciseModel.IsEntry = "0";
                        }
                        //exerciseModel.isEntryLibrary=selector.
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
                        if (selector.$isEntryExerciseLibrary().is(":checked")) {
                            exerciseModel.IsEntry = "1";
                        } else {
                            exerciseModel.IsEntry = "0";
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
                    if (selector.$isEntryExerciseLibrary().is(":checked")) {
                        exerciseModel.IsEntry = "1";
                    } else {
                        exerciseModel.IsEntry = "0";
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
                    if (selector.$isEntryExerciseLibrary().is(":checked")) {
                        exerciseModel.IsEntry = "1";
                    } else {
                        exerciseModel.IsEntry = "0";
                    }
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
    //点击习题库选择按钮
    selector.$btnSelect().on('click', function () {
        initTable();           //初始化表格
        selector.$ExerciseLibraryDialog().modal({ backdrop: 'static', keyboard: false });
        selector.$ExerciseLibraryDialog().modal('show');
    });
    //点击习题库选择弹出框中的取消按钮
    selector.$btnCancelFormalExercise().on('click', function () {
        selector.$ExerciseLibraryDialog().modal('hide');
    });
    //点击习题库选择弹出框中的保存按钮
    selector.$btnSaveFormalExercise().on('click', function () {

        if (selectionExercise.length < 1) {
            jqxNotification("请选择习题！", null, "error");
        } else {

            //重新加载题号
            for (var i = 0; i < dataExerciseArray.length; i++) {
                dataExerciseArray[i].ExericseTitleID = i + 1;
            }
            appendExercise(dataExerciseArray);
            selector.$ExerciseLibraryDialog().modal('hide');
        }

    });
    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        initTable();
    });

    //点击随机选择按钮
    selector.$btnRandom().on('click', function () {
        //习题库中的习题数量书否大于等于用户配置的数量
        $.ajax({
            url: "/ExerciseLibraryManagement/CheckedExerciseLibrary/GetExerciseScoreInfo",
            type: 'post',
            dateType: "json",
            success: function (msg) {
                $.post("/Systemmanagement/ConfigManagement/GetConfigList", {}, function (result) {
                    if (parseInt(msg[0].TotalCount) < parseInt(result[8].ConfigValue)) {
                        jqxNotification("习题库中的习题数量不足！", null, "error");
                        return false;
                    } else {
                        if (parseInt(msg[0].TotalScore) < 100) {
                            jqxNotification("习题库中的习题总分值不足100分！", null, "error");
                            return false;
                        }
                        if (parseInt(msg[0].ExerciseCount) < parseInt(result[9].ConfigValue)) {
                            jqxNotification("习题库中的单选题数量不足！", null, "error");
                            return false;
                        }
                        if (parseInt(msg[1].ExerciseCount) < parseInt(result[10].ConfigValue)) {
                            jqxNotification("习题库中的多选题数量不足！", null, "error");
                            return false;
                        }
                        if (parseInt(msg[2].ExerciseCount) < parseInt(result[11].ConfigValue)) {
                            jqxNotification("习题库中的判断题数量不足！", null, "error");
                            return false;
                        }
                        var source = selector.$exerciseDetailGrid().jqxDataTable('source');
                        if (source.records.length > 0) {
                            WindowConfirmDialog(random, "是否要清空习题？", "确认框", "确定", "取消");
                        } else {
                            showLoading();//显示加载等待框
                            random();
                            closeLoading();//关闭加载等待框
                        }
                    }
                });
            }
        });
    });
    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$exerciseName_Search().val("");
        selector.$inputType_Search().val("");
        selector.$createdTimeStart_Search().val("");
        selector.$createTimeEnd_Search().val("");
        selector.$ExerciseType_Search().val("");
    });

};
var selectionExercise = [];
//随机生成习题
function random() {
    var randomExercise = [];
    $.ajax({
        url: "/ExerciseLibraryManagement/CheckedExerciseLibrary/RandomExercise",
        type: "post",
        dataType: "json",
        success: function (msg) {
            for (var j = 0; j < msg.length; j++) {
                var model = new dataExerciseModel_Temp(1, msg[j].ExerciseType, msg[j].Option, msg[j].ExerciseName, msg[j].Answer, msg[j].Score, "0");
                randomExercise.push(model);
            }
            //重新加载题号
            for (var i = 0; i < randomExercise.length; i++) {
                randomExercise[i].ExericseTitleID = i + 1;
            }
            appendExercise(randomExercise);
            dataExerciseArray = randomExercise;
        }
    });

}

function initTable() {
    var source =
          {
              datafields:
              [
                  { name: "checkbox", type: null },
                  { name: 'ExerciseName', type: 'string' },
                  { name: 'CreatedDate', type: 'date' },
                  { name: 'Status', type: 'string' },
                  { name: 'TranslateStatusExerciseType', type: 'string' },
                  { name: 'ExerciseType', type: 'string' },
                  { name: 'TranslateExerciseType', type: 'string' },
                  { name: 'InputType', type: 'string' },
                  { name: 'TranslateInputType', type: 'string' },
                  { name: 'Option', type: 'string' },
                  { name: 'Answer', type: 'string' },
                  { name: 'Score', type: 'string' },
                  { name: 'Vguid', type: 'string' }
              ],
              datatype: "json",
              id: "VGUID",//主键
              async: true,
              data: { "ExerciseName": selector.$exerciseName_Search().val(), "ExerciseType": selector.$ExerciseType_Search().val(), "InputType": selector.$inputType_Search().val(), "CreatedTimeStart": selector.$createdTimeStart_Search().val(), "CreatedTimeEnd": selector.$createTimeEnd_Search().val() },
              url: "/ExerciseLibraryManagement/CheckedExerciseLibrary/GetCheckedExerciseListBySearch"    //获取数据源的路径
          };
    var typeAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (data) {
            source.totalrecords = data.TotalRows;
        }
    });
    //创建卡信息列表（主表）
    selector.$ExerciseLibraryList().jqxDataTable(
        {
            pageable: true,
            width: "1165",
            height: 450,
            pageSize: 10,
            serverProcessing: true,
            pagerButtonsCount: 10,
            source: typeAdapter,
            theme: "office",
            columns: [
             { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
              { text: '习题名称', width: 350, datafield: 'ExerciseName', align: 'center', cellsAlign: 'center' },
              { text: '习题录入类型', width: 100, datafield: 'TranslateInputType', align: 'center', cellsAlign: 'center' },
              { text: '习题类型', width: 100, datafield: 'TranslateExerciseType', align: 'center', cellsAlign: 'center' },
              { text: '习题创建日期', width: 180, datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
              { text: '选项', width: 200, datafield: 'Option', align: 'center', cellsAlign: 'center', cellsRenderer: showExercise },
              { text: '答案', width: 100, datafield: 'Answer', align: 'center', cellsAlign: 'center' },
              { text: '分值', width: 100, datafield: 'Score', align: 'center', cellsAlign: 'center' },
              { text: 'ExerciseType', width: 150, datafield: 'ExerciseType', hidden: true },
              { text: 'VGUID', datafield: 'Vguid', hidden: true }
            ]
        });
}
function cellsRendererFunc(row, column, value, rowData) {
    var container = "";
    container = "<input class=\"jqx_datatable_checkbox\"  index=\"" + row + "\" type=\"checkbox\"  onchange=edit(this) style=\"margin:auto;width: 17px;height: 17px;\" />";
    for (var k = 0; k < selectionExercise.length; k++) {
        if (selectionExercise[k] == rowData.Vguid) {
            container = "<input class=\"jqx_datatable_checkbox\"  checked=\"checked\"  index=\"" + row + "\" type=\"checkbox\"  onchange=edit(this) style=\"margin:auto;width: 17px;height: 17px;\" />";
            break;
        } else {
            container = "<input class=\"jqx_datatable_checkbox\"    index=\"" + row + "\" type=\"checkbox\" onchange=edit(this) style=\"margin:auto;width: 17px;height: 17px;\" />";
        }
    }
    return container;
}

function rendererFunc() {
    var checkBox = "<div id='jqx_datatable_checkbox_all' class='jqx_datatable_checkbox_all' style='z-index: 999; margin-left:7px ;margin-top: 7px;'>";
    checkBox += "</div>";
    return checkBox;
}

function renderedFunc(element) {
    var grid = selector.$ExerciseLibraryList();
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
}
function showExercise(row, column, value, rowData) {
    var container = "";
    if (rowData.ExerciseType != "4") {
        var str = "";
        var array = rowData.Option.split('|');
        for (var i = 0; i < array.length; i++) {
            if (i != array.length - 1) {
                str += array[i].substring(0, array[i].lastIndexOf(',')) + "<br/>";
            } else {
                str += array[i] + "<br/>";
            }

        }
        container = "<span>" + str + "</span>";
    }

    return container;
}

//选择习题checkbox改变事件
function edit(obj) {
    var vguid = $(obj).parent().siblings('td:last').html();
    if ($(obj).is(":checked")) {
        if ($.inArray(vguid, selectionExercise) == -1) {
            selectionExercise.push(vguid);
            var siblings = $(obj).parent().nextAll();
            var exerciseType = parseInt(siblings.eq(7).html());  //习题类型
            var arr = siblings.eq(4).html().substring(6, siblings.eq(4).html().indexOf("</span>")).split("<br>");
            var option = $.grep(arr, function (n) { return $.trim(n).length > 0; }).join(',|'); //选项
            var exerciseName = siblings.eq(0).html();  //习题名称
            var answer = siblings.eq(5).html();//答案
            var score = siblings.eq(6).html();//分值
            var dataExerciseModel = new dataExerciseModel_Temp(1, exerciseType, option, exerciseName, answer, score, "0");
            dataExerciseArray.push(dataExerciseModel);
        }
    } else {
        var index = $.inArray(vguid, selectionExercise);
        if (index > -1) {
            selectionExercise.splice(index, 1);
            dataExerciseArray.splice(index, 1);

        }
    }
}

//删除选中的习题
function deleted() {
    for (var i = 0; i < deletedExercise.length; i++) {
        deleteExerciseData(deletedExercise[i]);
    }
}
//全局字典
var letter = ["A", "B", "C", "D", "E", "F", "G", "H"];
//新增习题
function newExerciseDetail() {
    selector.$exerciseTypeRadio().each(function () {
        $(this).removeAttr("checked");
        $(this).removeAttr("disabled");
    });
    selector.$isEntryExerciseLibrary().prop("checked", "checked");
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
        url: "/ExerciseManagement/ExerciseManagement/GetExerciseDetailListByMainVguid",
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
                var isEntry = data[i].IsEntry;
                var exerciseDataModel = new dataExerciseModel_Temp(id, exerciseType, exerciseOption, exerciseName, answer, score, isEntry);
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
                    { name: 'ExercisesInformationVguid', type: 'string' },
                    { name: 'IsEntry', type: 'string' }
                ],
            datatype: "json",
            updaterow: function (rowid, rowdata, commit) {
                commit(true);
            },
            id: "Vguid",//主键
            async: true,
            localdata: data,
        };
    var dataAdapter = new $.jqx.dataAdapter(source);
    selector.$exerciseDetailGrid().jqxDataTable(
    {
        width: "100%",
        height: 320,
        source: dataAdapter,
        pageable: true,
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
          { text: 'IsEntry', datafield: 'IsEntry', hidden: true, align: 'center', cellsalign: 'center' },
        ]
    });
    //翻译判断题答案
    function explainJudgeAnswer(row, column, value, rowData) {
        var container = rowData.Answer;
        if (container != "") {
            if (rowData.Answer == 0) {
                container = "正确";
            } else if (rowData.Answer == 1) {
                container = "错误";
            }
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
var dataExerciseModel_Temp = function (id, exerciseType, exerciseOption, exerciseName, answer, score, IsEntry) {
    this.ExericseTitleID = id;
    this.ExerciseType = exerciseType;
    this.ExerciseOption = exerciseOption;
    this.ExerciseName = exerciseName;
    this.Answer = answer;
    this.Score = score;
    this.IsEntry = IsEntry;
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
    var letter = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];
    if (row.ExerciseType == "1") {
        $(".SingleChoice").removeAttr("style");
        selector.$singleChoiceName().val(row.ExerciseName);
        selector.$singleScore().val(row.Score);
        singleOption = [];//单选题选项数组
        singleOption = row.ExerciseOption.split(",|");
        //加载单选题选项
        $("#field_1").val(singleOption[0].split("A.")[1]);
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
            $("#field_" + fieldCount).val(singleOption[i].split(letter[i] + ".")[1]);
            selector.$singleChecked().each(function () {
                if (row.Answer == letter[$(this).val() - 1]) {
                    $(this).attr("checked", "checked");
                }
            });
            if (row.IsEntry == "1") {
                selector.$isEntryExerciseLibrary().prop("checked", "checked");
            } else {
                selector.$isEntryExerciseLibrary().removeAttr("checked", "checked");
            }
        }

    } else if (row.ExerciseType == "2") {
        $(".MultipleChoice").removeAttr("style");
        selector.$multipleChoiceName().val(row.ExerciseName);
        selector.$multipleScore().val(row.Score);
        multipleOption = [];//所选题选项数组
        multipleOption = row.ExerciseOption.split(",|");
        $("#multipleField_1").val(multipleOption[0].split("A.")[1]);
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
            $("#multipleField_" + multipleFieldCount).val(multipleOption[i].split(letter[i] + ".")[1]);
            selector.$multipleChecked().each(function () {
                for (var i = 0; i < correctAnswer.length; i++) {
                    if (correctAnswer[i] == letter[$(this).val() - 1]) {
                        $(this).attr("checked", "checked");
                    }
                }
            });
            if (row.IsEntry == "1") {
                selector.$isEntryExerciseLibrary().prop("checked", "checked");
            } else {
                selector.$isEntryExerciseLibrary().removeAttr("checked", "checked");
            }
        }

    } else if (row.ExerciseType == "3") {
        $(".JudgeExercise").removeAttr("style");
        selector.$judgeExerciseName().val(row.ExerciseName);
        selector.$judgeScore().val(row.Score);
        selector.$judgeAnswer().val(row.Answer);
        if (row.IsEntry == "1") {
            selector.$isEntryExerciseLibrary().prop("checked", "checked");
        } else {
            selector.$isEntryExerciseLibrary().removeAttr("checked", "checked");
        }
    } else {
        $(".ShortAnswer").removeAttr("style");
        selector.$shortAnswerName().val(row.ExerciseName);
        selector.$shortQuestionAnswer().val(row.Answer);
        selector.$shortScore().val(row.Score);
        if (row.IsEntry == "1") {
            selector.$isEntryExerciseLibrary().prop("checked", "checked");
        } else {
            selector.$isEntryExerciseLibrary().removeAttr("checked", "checked");
        }
    }

    //弹出编辑框
    selector.$isEditDetail().val("1");
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


