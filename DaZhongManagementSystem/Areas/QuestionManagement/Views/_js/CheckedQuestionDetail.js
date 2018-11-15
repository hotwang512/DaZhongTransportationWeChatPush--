var selector = {
    //Grid
    $questionDetailGrid: function () { return $("#QuestionDetailGrid") },
    $questionDialog: function () { return $("#QuestionDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框

    //按钮
    $btnCancel: function () { return $("#btnCancel") },
    $btnCancelQuestion: function () { return $("#btnCancelQuestion") },//取消保存（新增/编辑）问卷详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项

    //问卷基本信息
    $QuestionsName: function () { return $("#QuestionsName") },
    $VCRTTIME: function () { return $("#VCRTTIME") },
    $EffectiveDate: function () { return $("#EffectiveDate") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $questionTypeRadio: function () { return $(".QuestionTypeRadio") },
    $isEntryQuestionLibrary: function () { return $("#isEntryQuestionLibrary") },
    //单选题
    $singleChoiceName: function () { return $("#singleChoiceContent") },
    //多选题
    $multipleChoiceName: function () { return $("#multipleChoiceContent") },
    //判断题
    $judgeQuestionName: function () { return $("#judgeQuestionContent") },
    //简答题
    $shortAnswerName: function () { return $("#shortAnswerContent") },

    //隐藏控件
    $vguid: function () { return $("#editVguid") },
    $isEditDetail: function () { return $("#isEditDetail") },//问卷详情新增/编辑标示
    $editVguidDetail: function () { return $("#editVguidDetail") },//每个题的Vguid
    $questionTitleID: function () { return $("#questionTitleID") },//题号
    $txtType: function () { return $("#txtType") },
    //form提交
    $questionMainModelForm: function () { return $("#QuestionDetailForm") },
    $singleChoiceForm: function () { return $("#singleChoice") },
    $multipleChoiceForm: function () { return $("#multipleChoice") },
    $judgeQuestionForm: function () { return $("#judgeQuestion") },
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
        loadQuestionData();
        //LoadQuestionList();
    }; //addEvent end

    //有效时间控件
    selector.$EffectiveDate().datepicker({
        format: "yyyy-mm-dd",
        autoclose: true,
        todayHighlight: true,
        orientation: "bottom right",
        startDate: selector.$VCRTTIME().val()
    });

    //取消问卷主信息按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/QuestionManagement/CheckedQuestion/CheckedList";
    });

    //问卷详情弹出框取消按钮事件
    selector.$btnCancelQuestion().on('click', function () {
        selector.$questionDialog().modal('hide');

    });
};

//全局字典
var letter = ["A", "B", "C", "D", "E", "F", "G", "H"];
//新增问卷
function newQuestionDetail() {
    selector.$questionTypeRadio().each(function () {
        $(this).removeAttr("checked");
        $(this).removeAttr("disabled");
    });
    $(".SingleChoice").hide();
    $(".MultipleChoice").hide();
    $(".JudgeQuestion").hide();
    $(".ShortAnswer").hide();
    //清空单选题内容
    selector.$singleChoiceName().val("");
    selector.$singleInputsWrapper().find('div').remove();
    selector.$singleInputsWrapper().append("<div style='height: 40px;'>" +
          "<div class='option' style='position: absolute; margin-top: 10px;'>" +
              "<span style='font-size: 14px;'>选项1：</span>" +
          "</div>" +
          "<input id='field_1' type='text' style='width: 75%; margin-left: 45px;background-color: #f5f5f5!important;' disabled='disabled' name='singleOption' class='input_text form-control'/>" +          
      "</div>");
    fieldCount = 1;
    singleCount = 1;
    //清空多选题内容
    selector.$multipleChoiceName().val("");
    selector.$multipleInputsWrapper().find('div').remove();
    selector.$multipleInputsWrapper().append("<div style='height: 40px;'>" +
          "<div class='option' style='position: absolute; margin-top: 10px;'>" +
              "<span style='font-size: 14px;'>选项1：</span>" +
          "</div>" +
          "<input id='multipleField_1' type='text' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' disabled='disabled' name='multipleOption' class='input_text form-control'/>" +         
      "</div>");
    multipleCount = 1;
    multipleFieldCount = 1;
    //清空判断题内容
    selector.$judgeQuestionName().val("");
    //清空简答题内容
    selector.$shortAnswerName().val("");

    selector.$questionDialog().find("input:text,select,input:hidden,input:password,textarea").each(function () {
        if ($(this).hasClass("input_Validate")) {
            $(this).removeClass("input_Validate");
            $(this).next(".msg").remove();
        }
    });

    //弹出新增框
    selector.$isEditDetail().val("0");
    selector.$editVguidDetail().val("");
    selector.$myModalLabel_title().text("新增问卷");
    selector.$questionDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$questionDialog().modal('show');
}

//编辑时加载问卷详细信息列表
function loadQuestionData() {
    var questionMainVguid = selector.$vguid().val();//问卷主信息Vguid
    $.ajax({
        url: "/QuestionManagement/CheckedQuestion/GetQuestionDetailListByMainVguid",
        data: { "Vguid": questionMainVguid },
        type: "post",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var id = data[i].QuestionTitleID;
                var questionType = data[i].QuestionnaireDetailType;
                var questionOption = data[i].QuestionOption;
                var questionName = data[i].QuestionnaireDetailName;
                var questionDataModel = new dataQuestionModel_Temp(id, questionType, questionOption, questionName);
                dataQuestionArray.push(questionDataModel);
            }
            appendQuestion(data);
        }
    });
}

//绘制问卷详细信息表格
function appendQuestion(data) {
    var editedRows = new Array();
    var source =
        {
            datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'QuestionTitleID', type: 'string' },
                    { name: 'QuestionnaireDetailType', type: 'string' },
                    { name: 'QuestionnaireDetailName', type: 'string' },
                    { name: 'QuestionOption', type: 'string' },
                    { name: 'Vguid', type: 'string' },
                    { name: 'QuestionnaireVguid', type: 'string' }
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
    selector.$questionDetailGrid().jqxDataTable(
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
          { text: '题号', datafield: 'QuestionTitleID', width: 50, align: 'center', cellsalign: 'center' },
          { text: '问卷类型', datafield: 'QuestionnaireDetailType', width: 200, align: 'center', cellsalign: 'center', cellsRenderer: explainQuestionType },
          { text: '问卷内容', datafield: 'QuestionnaireDetailName', align: 'center', cellsalign: 'center' },
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

    //翻译问卷类型
    function explainQuestionType(row, column, value, rowData) {
        var container = "";
        switch (rowData.QuestionnaireDetailType.toString()) {
            case "1":
                container = "单选题";
                break;
            case "2":
                container = "多选题";
                break;
            case "3":
                container = "判断题";
                break;
            case "4":
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
        var grid = selector.$questionDetailGrid();
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

//问卷详情临时模型
var dataQuestionModel_Temp = function (id, questionType, questionOption, questionName) {
    this.QuestionTitleID = id;
    this.QuestionnaireDetailType = questionType;
    this.QuestionOption = questionOption;
    this.QuestionnaireDetailName = questionName;
    this.Answer = "";
};
var dataQuestionArray = new Array();//问卷详情数组

function clearRadio() {
    selector.$questionTypeRadio().each(function () {
        if ($(this).is(":checked")) {
            $(this).removeAttr("checked");
        }
    });
};

//双击行编辑事件
selector.$questionDetailGrid().on('rowDoubleClick', function (event) {
    var args = event.args;
    var row = args.row;
    //选中行题号row.QuestionTitleID
    newQuestionDetail();//清空原有记录
    selector.$questionTitleID().val(row.QuestionTitleID);
    //clearRadio();
    //加载选中行数据
    $("#QuestionTypeRadio" + row.QuestionnaireDetailType).trigger("click");
    selector.$questionTypeRadio().each(function () {
        $(this).attr("disabled", "disabled");
    });
    var letter = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];
    if (row.QuestionnaireDetailType == "1") {
        $(".SingleChoice").removeAttr("style");
        selector.$singleChoiceName().val(row.QuestionnaireDetailName);
        var singleOption = [];//单选题选项数组
        singleOption = row.QuestionOption.split(",|");
        //加载单选题选项
        $("#field_1").val(singleOption[0].split("A.")[1]);
        
        for (var i = 1; i < singleOption.length; i++) {
            fieldCount++;
            selector.$singleInputsWrapper().append("<div style='height: 40px;'>" +
                  "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                      "<span style='font-size: 14px;'>选项" + fieldCount + "：</span>" +
                  "</div>" +
                  "<input id='field_" + fieldCount + "' type='text' disabled='disabled' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +                  
              "</div>");
            singleCount++;
            $("#field_" + fieldCount).val(singleOption[i].split(letter[i] + ".")[1]);            
        }

    } else if (row.QuestionnaireDetailType == "2") {
        $(".MultipleChoice").removeAttr("style");
        selector.$multipleChoiceName().val(row.QuestionnaireDetailName);
        var multipleOption = [];//所选题选项数组
        multipleOption = row.QuestionOption.split(",|");
        $("#multipleField_1").val(multipleOption[0].split("A.")[1]);        
        //加载多选题选项
        for (var i = 1; i < multipleOption.length; i++) {
            multipleFieldCount++;
            selector.$multipleInputsWrapper().append("<div style='height: 40px;'>" +
                  "<div class='option' style='position: absolute; margin-top: 10px;'>" +
                      "<span style='font-size: 14px;'>选项" + multipleFieldCount + "：</span>" +
                  "</div>" +
                  "<input id='multipleField_" + multipleFieldCount + "' type='text' disabled='disabled' style='width: 75%;background-color: #f5f5f5!important; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
                  "</div>" +
              "</div>");
            multipleCount++;
            $("#multipleField_" + multipleFieldCount).val(multipleOption[i].split(letter[i] + ".")[1]);
        }

    } else if (row.QuestionnaireDetailType == "3") {
        $(".JudgeQuestion").removeAttr("style");
        selector.$judgeQuestionName().val(row.QuestionnaireDetailName);
    } else {
        $(".ShortAnswer").removeAttr("style");
        selector.$shortAnswerName().val(row.QuestionnaireDetailName);
    }
    //弹出编辑框
    selector.$isEditDetail().val("1");
    selector.$myModalLabel_title().text("查看问卷");
    selector.$questionDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$questionDialog().modal('show');
});

$(function () {
    var page = new $page();
    page.init();

})


