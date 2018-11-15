var selector = {
    $grid: function () { return $("#ExerciseList") },
    $form: function () { return $("#UpLoadExerciseForm") },
    //Grid
    $exerciseDetailGrid: function () { return $("#ExerciseDetailGrid") },
    $exerciseDialog: function () { return $("#ExerciseDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框

    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnImport: function () { return $("#btnImport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnChecked: function () { return $("#btnChecked") },
    $btnDownload: function () { return $("#btnDownload") },//下载模板
    $importFile: function () { return $("#importFile") },
    $btnReset: function () { return $("#btnReset") },

    $btnSaveExercise: function () { return $("#btnSaveExercise") },//保存（新增/编辑）习题详情
    $btnCancelExercise: function () { return $("#btnCancelExercise") },//取消保存（新增/编辑）习题详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项
    //查询
    $exerciseName_Search: function () { return $("#ExercisesName_Search") },
    $inputType_Search: function () { return $("#InputType_Search") },
    $effectiveDate_Search: function () { return $("#EffectiveDate_Search") },
    $ExerciseType_Search: function () { return $("#ExerciseType_Search") },
    $CreatedDate_Search: function () { return $("#CreatedDate_Search") },

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
    $isEditDetail: function () { return $("#isEditDetail") },//习题详情新增/编辑标示
    $editVguidDetail: function () { return $("#editVguidDetail") },//每个题的Vguid
    $txtVguid: function () { return $("#txtVguid") },
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
        LoadTable();

    }; //addEvent end

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        LoadTable();
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$exerciseName_Search().val("");
        selector.$inputType_Search().val("");
        selector.$ExerciseType_Search().val("");
        selector.$CreatedDate_Search().val("");
    });

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        newExerciseDetail();
    });
    //习题详情弹出框确定按钮事件
    selector.$btnSaveExercise().on('click', function () {
        var page = 1;
        var exerciseType;
        var singleStr = "";
        var multipleStr = "";
        var emptyVguid = "00000000-0000-0000-0000-000000000000";
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
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 1, singleStr, selector.$singleChoiceName().val(), answer, selector.$singleScore().val(), emptyVguid);
                        dataExerciseArray.push(dataExerciseModel);
                        //重新加载题号
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            dataExerciseArray[i].ExericseTitleID = i + 1;
                        }
                        saveExercise(JSON.stringify(dataExerciseArray), false);
                        dataExerciseArray.length = 0;
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
                            multipleCount++;
                        }
                    });
                    if (multipleCount < 1) {
                        jqxNotification("多选题必须有一个正确答案！", null, "error");
                    } else {
                        answer = answer.substring(0, answer.length - 1);
                        var dataExerciseModel = new dataExerciseModel_Temp(page, 2, multipleStr, selector.$multipleChoiceName().val(), answer, selector.$multipleScore().val(), emptyVguid);
                        dataExerciseArray.push(dataExerciseModel);
                        //重新加载题号
                        for (var i = 0; i < dataExerciseArray.length; i++) {
                            dataExerciseArray[i].ExericseTitleID = i + 1;
                        }
                        saveExercise(JSON.stringify(dataExerciseArray), false);
                        dataExerciseArray.length = 0;
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
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 3, judgeOption, selector.$judgeExerciseName().val(), judgeAnswer, selector.$judgeScore().val(), emptyVguid);
                    dataExerciseArray.push(dataExerciseModel);
                    //重新加载题号
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        dataExerciseArray[i].ExericseTitleID = i + 1;
                    }
                    saveExercise(JSON.stringify(dataExerciseArray), false);
                    dataExerciseArray.length = 0;
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
                    var dataExerciseModel = new dataExerciseModel_Temp(page, 4, "", selector.$shortAnswerName().val(), selector.$shortQuestionAnswer().val(), selector.$shortScore().val(), emptyVguid);
                    dataExerciseArray.push(dataExerciseModel);
                    //重新加载题号
                    for (var i = 0; i < dataExerciseArray.length; i++) {
                        dataExerciseArray[i].ExericseTitleID = i + 1;
                    }
                    saveExercise(JSON.stringify(dataExerciseArray), false);
                    dataExerciseArray.length = 0;
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
                        exerciseModel.ExerciseType = "1";
                        exerciseModel.Option = singleStr;
                        exerciseModel.ExerciseName = selector.$singleChoiceName().val();
                        exerciseModel.Answer = answer;
                        exerciseModel.Score = selector.$singleScore().val();
                        exerciseModel.Vguid = selector.$txtVguid().val();
                        dataExerciseArray.push(exerciseModel);
                        saveExercise(JSON.stringify(dataExerciseArray), true);
                        dataExerciseArray.length = 0;
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
                    //page = selector.$exerciseTitleID().val();
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
                        exerciseModel.ExerciseType = "2";
                        exerciseModel.vguid = selector.$txtVguid().val();
                        exerciseModel.Option = multipleStr;
                        exerciseModel.ExerciseName = selector.$multipleChoiceName().val();
                        exerciseModel.Answer = answer;
                        exerciseModel.Score = selector.$multipleScore().val();
                        dataExerciseArray.push(exerciseModel);
                        saveExercise(JSON.stringify(dataExerciseArray), true);
                        //dataExerciseArray.length = 0;
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
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    //查询数组的匹配数据
                    var exerciseModel = new dataExerciseModel_Temp();
                    exerciseModel.vguid = selector.$txtVguid().val();
                    exerciseModel.Option = judgeOption;
                    exerciseModel.ExerciseName = selector.$judgeExerciseName().val();
                    exerciseModel.Answer = judgeAnswer;
                    exerciseModel.Score = selector.$judgeScore().val();
                    dataExerciseArray.push(exerciseModel);
                    saveExercise(JSON.stringify(dataExerciseArray), true);
                    dataExerciseArray.length = 0;
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
                    //  page = selector.$exerciseTitleID().val();
                    //查询数组的匹配数据
                    var exerciseModel = new dataExerciseModel_Temp();
                    exerciseModel.vguid = selector.$txtVguid().val();
                    exerciseModel.ExerciseName = selector.$shortAnswerName().val();
                    exerciseModel.Answer = selector.$shortQuestionAnswer().val();
                    exerciseModel.Score = selector.$shortScore().val();
                    dataExerciseArray.push(exerciseModel);
                    saveExercise(JSON.stringify(dataExerciseArray), true);
                    dataExerciseArray.length = 0;
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

    //下载模板按钮事件
    selector.$btnDownload().on('click', function () {
        window.location.href = "/ExerciseLibraryManagement/ExerciseLibraryManagement/DownLoadTemplate";
    });

    //导入习题按钮事件
    selector.$btnImport().on('click', function () {
        selector.$importFile().click();
    });

    //生效时间控件
    selector.$CreatedDate_Search().datetimepicker({
        format: "yyyy-mm-dd hh:ii",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        showMeridian: true
    });
    $(".glyphicon-arrow-left").text("<<");
    $(".glyphicon-arrow-right").text(">>");
    //审核按钮事件
    selector.$btnChecked().on('click', function () {
        var selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.Vguid);
            }
        });
        if (selection.length < 1) {
            jqxNotification("请选择您要审核的习题！", null, "error");
        } else {
            WindowConfirmDialog(checked, "您确定要审核通过选中的习题？", "确认框", "确定", "取消");
        }
    });

    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        var selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.Vguid);
            }
        });
        if (selection.length < 1) {
            jqxNotification("请选择您要删除的习题！", null, "error");
        } else {
            WindowConfirmDialog(deleted, "您确定要删除选中的习题？", "确认框", "确定", "取消");
        }
    });

    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
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
                data: { "ExerciseName": selector.$exerciseName_Search().val(), "InputType": selector.$inputType_Search().val(), "ExerciseType": selector.$ExerciseType_Search().val(), "CreatedDate": selector.$CreatedDate_Search().val() }, //"EffectiveDate": selector.$effectiveDate_Search().val()
                url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/GetExerciseListBySearch"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(UserInfoListSource, {
            downloadComplete: function (data) {
                UserInfoListSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: true,
                width: "100%",
                height: 450,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '习题名称', width: 350, datafield: 'ExerciseName', align: 'center', cellsAlign: 'center' },
                  { text: '习题录入类型', width: 150, datafield: 'TranslateInputType', align: 'center', cellsAlign: 'center' },
                  { text: '习题类型', width: 150, datafield: 'TranslateExerciseType', align: 'center', cellsAlign: 'center' },
                  //{ text: '习题有效日期', width: 180, datafield: 'EffectiveDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: '习题创建日期', width: 180, datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: '选项', datafield: 'Option', align: 'center', cellsAlign: 'center', cellsRenderer: showExercise },
                  { text: '答案', width: 150, datafield: 'Answer', align: 'center', cellsAlign: 'center', cellsRenderer: translationAnswer },
                  { text: '分值', width: 150, datafield: 'Score', align: 'center', cellsAlign: 'center' },
                  { text: '习题状态', width: 150, datafield: 'TranslateStatusExerciseType', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });
    }

    function cellsRendererFunc(row, column, value, rowData) {
        return "<input class=\"jqx_datatable_checkbox\" index=\"" + row + "\" type=\"checkbox\"  style=\"margin:auto;width: 17px;height: 17px;\" />";
    }

    function rendererFunc() {
        var checkBox = "<div id='jqx_datatable_checkbox_all' class='jqx_datatable_checkbox_all' style='z-index: 999; margin-left:7px ;margin-top: 7px;'>";
        checkBox += "</div>";
        return checkBox;
    }

    function renderedFunc(element) {
        var grid = selector.$grid();
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
    function translationAnswer(row, column, value, rowData) {
        var container = "";
        if (rowData.ExerciseType == "3") {
            switch (rowData.Answer) {
                case "0":
                    container = "正确";
                    break;
                case "1":
                    container = "错误";
                    break;
                default:
                    container = rowData.Answer;
                    break;
            }
        } else {
            container = rowData.Answer;
        }
        return container;
    }
};

//双击行编辑事件
selector.$grid().on('rowDoubleClick', function (event) {
    var args = event.args;
    var row = args.row;
    newExerciseDetail();//清空原有记录
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
        var singleOption = [];//单选题选项数组
        singleOption = row.Option.split(",|");
        //加载单选题选项
        $("#field_1").val(singleOption[0].split("A.")[1]);
        if (row.Answer == letter[0]) {
            $("#firstSingleCheck").attr("checked", "checked");
        }
        selector.$txtVguid().val(row.Vguid);
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
        }

    } else if (row.ExerciseType == "2") {
        $(".MultipleChoice").removeAttr("style");
        selector.$multipleChoiceName().val(row.ExerciseName);
        selector.$multipleScore().val(row.Score);
        var multipleOption = [];//所选题选项数组
        multipleOption = row.Option.split(",|");
        $("#multipleField_1").val(multipleOption[0].split("A.")[1]);
        var correctAnswer = row.Answer.split(",");
        for (var i = 0; i < correctAnswer.length; i++) {
            if (correctAnswer[i] == letter[0]) {
                $("#firstMultipleCheck").attr("checked", "checked");
            }
        }
        selector.$txtVguid().val(row.Vguid);
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
        }

    } else if (row.ExerciseType == "3") {
        $(".JudgeExercise").removeAttr("style");
        selector.$judgeExerciseName().val(row.ExerciseName);
        selector.$judgeScore().val(row.Score);
        selector.$judgeAnswer().val(row.Answer);
        selector.$txtVguid().val(row.Vguid);
    } else {
        $(".ShortAnswer").removeAttr("style");
        selector.$shortAnswerName().val(row.ExerciseName);
        selector.$shortQuestionAnswer().val(row.Answer);
        selector.$shortScore().val(row.Score);
        selector.$txtVguid().val(row.Vguid);
    }

    //弹出编辑框
    selector.$isEditDetail().val("1");
    selector.$myModalLabel_title().text("编辑习题");
    selector.$exerciseDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$exerciseDialog().modal('show');
});

//编辑时加载习题详细信息列表
function loadExerciseData(vguid) {
    $.ajax({
        url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/GetExerciseDetailInfo",
        data: { "vguid": vguid },
        type: "post",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var id = data[i].ExericseTitleID;
                var exerciseType = data[i].ExerciseType;
                var exerciseOption = data[i].ExerciseOption;
                var exerciseName = data[i].ExerciseName;
                var answer = data[i].Answer;
                var score = data[i].Score;
                var exerciseDataModel = new dataExerciseModel_Temp(id, exerciseType, exerciseOption, exerciseName, answer, score, vguid);
                dataExerciseArray.push(exerciseDataModel);
            }

        }
    });
}
//习题详情临时模型
var dataExerciseModel_Temp = function (id, exerciseType, exerciseOption, exerciseName, answer, score, vguid) {
    this.ExericseTitleID = id;
    this.ExerciseType = exerciseType;
    this.Option = exerciseOption;
    this.ExerciseName = exerciseName;
    this.Answer = answer;
    this.Score = score;
    this.Vguid = vguid;
};

//保存习题
function saveExercise(data, isEdit) {
    $.ajax({
        url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/SaveExerciseMain",
        type: "post",
        data: { exerciseData: data, isEdit: isEdit },
        traditional: true,
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("保存失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("保存成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
        }
    });
}

var dataExerciseArray = new Array();//习题详情数组

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

var maxInputs = 8;//最大input框数量
//单选题添加选项
var singleCount = selector.$singleInputsWrapper().length;
var fieldCount = 1;
//单选题添加选择项按钮事件
selector.$btnAddSingleOption().on('click', function () {
    if (singleCount < maxInputs) {
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
//审核
function checked() {
    showLoading();//显示加载等待框
    var selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.Vguid);
        }
    });
    $.ajax({
        url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/CheckedExercise",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("审核失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("审核成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

//删除
function deleted() {
    showLoading();//显示加载等待框
    var selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.Vguid);
        }
    });
    $.ajax({
        url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/DeletedExercise",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("删除失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("删除成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

function fileUpload() {
    selector.$form().ajaxSubmit({
        url: "/ExerciseLibraryManagement/ExerciseLibraryManagement/UpLoadExercise?exerciseFile=importFile",
        type: "post",
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccess) {
                jqxNotification("习题导入成功！", null, "success");
                selector.$grid().jqxDataTable('updateBoundData');
            } else {
                jqxNotification("习题导入失败！", null, "error");
            }
            selector.$importFile().remove();
            var inputObj = '<input type="file" name="importFile" id="importFile" onchange="fileUpload()" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel" style="display: none" />';
            selector.$btnImport().after(inputObj);
        }
    });
}
$(function () {
    var page = new $page();
    page.init();

});


