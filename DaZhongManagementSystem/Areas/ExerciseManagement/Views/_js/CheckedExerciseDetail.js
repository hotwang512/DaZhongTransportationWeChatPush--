var selector = {
    //Grid
    $exerciseDetailGrid: function () { return $("#ExerciseDetailGrid") },
    $exerciseDialog: function () { return $("#ExerciseDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框

    //按钮
    $btnCancel: function () { return $("#btnCancel") },
    $btnCancelExercise: function () { return $("#btnCancelExercise") },//取消保存（新增/编辑）习题详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项

    //习题基本信息
    $ExercisesName: function () { return $("#ExercisesName") },
    $VCRTTIME: function () { return $("#VCRTTIME") },
    $EffectiveDate: function () { return $("#EffectiveDate") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $exerciseTypeRadio: function () { return $(".ExerciseTypeRadio") },
    $isEntryExerciseLibrary: function () { return $("#isEntryExerciseLibrary") },
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
    $vguid: function () { return $("#editVguid") },
    $isEditDetail: function () { return $("#isEditDetail") },//习题详情新增/编辑标示
    $editVguidDetail: function () { return $("#editVguidDetail") },//每个题的Vguid
    $exerciseTitleID: function () { return $("#exerciseTitleID") },//题号
    $txtType: function () { return $("#txtType") },
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

    //取消习题主信息按钮事件
    selector.$btnCancel().on('click', function () {
        if (selector.$txtType().val() == "4") {
            window.location.href = "/KnowledgeBaseManagement/Draft/DraftList";
        } else {
            window.location.href = "/ExerciseManagement/CheckedExercise/CheckedList";
        }

    });

    //习题详情弹出框取消按钮事件
    selector.$btnCancelExercise().on('click', function () {
        selector.$exerciseDialog().modal('hide');

    });
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
          "<input id='field_1' type='text' style='width: 75%; margin-left: 45px;background-color: #f5f5f5!important;' disabled='disabled' name='singleOption' class='input_text form-control'/>" +
          "<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              "<input id='firstSingleCheck' type='checkbox' class='singleChecked' disabled='disabled' style='margin-top: 10px;' value='1'/>" +
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
          "<input id='multipleField_1' type='text' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' disabled='disabled' name='multipleOption' class='input_text form-control'/>" +
          "<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              "<input id='firstMultipleCheck' type='checkbox' disabled='disabled' class='multipleChecked' style='margin-top: 10px;' value='1'/>" +
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
        url: "/ExerciseManagement/CheckedExercise/GetExerciseDetailListByMainVguid",
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
        var singleOption = [];//单选题选项数组
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
                  "<input id='field_" + fieldCount + "' type='text' disabled='disabled' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +
                  "<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                      "<input type='checkbox' class='singleChecked' disabled='disabled' style='margin-top: 10px;' value='" + fieldCount + "'/>" +
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
                  "<input id='multipleField_" + multipleFieldCount + "' type='text' disabled='disabled' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
                  "<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                      "<input id='firstMultipleCheck' type='checkbox' class='multipleChecked' disabled='disabled' style='margin-top: 10px;' value='" + multipleFieldCount + "' />" +
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

    } else {
        $(".ShortAnswer").removeAttr("style");
        selector.$shortAnswerName().val(row.ExerciseName);
        selector.$shortQuestionAnswer().val(row.Answer);
        selector.$shortScore().val(row.Score);
    }
    if (row.IsEntry == "1") {
        selector.$isEntryExerciseLibrary().prop("checked", "checked");
    }
    //弹出编辑框
    selector.$isEditDetail().val("1");
    selector.$myModalLabel_title().text("查看习题");
    selector.$exerciseDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$exerciseDialog().modal('show');
});

$(function () {
    var page = new $page();
    page.init();

})


