var selector = {
    //Grid
    $questionDetailGrid: function () { return $("#QuestionDetailGrid") },
    $questionDialog: function () { return $("#QuestionDialog") },
    $singleInputsWrapper: function () { return $("#singleInputsWrapper") },//单选题选择项框
    $multipleInputsWrapper: function () { return $("#multipleInputsWrapper") },//多选题选择项框
    $QuestionLibraryList: function () { return $("#QuestionLibraryList") },

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnAdd: function () { return $("#btnAdd") },//新增问卷
    $btnDelete: function () { return $("#btnDelete") },//删除问卷
    $btnSaveQuestion: function () { return $("#btnSaveQuestion") },//保存（新增/编辑）问卷详情
    $btnCancelQuestion: function () { return $("#btnCancelQuestion") },//取消保存（新增/编辑）问卷详情
    $btnAddSingleOption: function () { return $("#btnAddSingleOption") },//添加单选题的选择项
    $btnAddmultipleOption: function () { return $("#btnAddmultipleOption") },//添加多选题选择项
    $btnSelect: function () { return $("#btnSelect") },   //问卷库选择
    $btnRandom: function () { return $("#btnRandom") },   //问卷库随机
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnSaveFormalQuestion: function () { return $("#btnSaveFormalQuestion") },
    $btnCancelFormalQuestion: function () { return $("#btnCancelFormalQuestion") },
    //问卷基本信息
    $QuestionsName: function () { return $("#QuestionnaireName") },
    $VCRTTIME: function () { return $("#VCRTTIME") },
    $EffectiveDate: function () { return $("#EffectiveDate") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $questionTypeRadio: function () { return $(".QuestionTypeRadio") },
    //$isEntryQuestionLibrary: function () { return $(".isEntryQuestionLibrary") },
    $isEntryQuestionLibrary: function () { return $("#isEntryQuestionLibrary") },
    $QuestionLibraryDialog: function () { return $("#QuestionLibraryDialog") },
    //单选题
    $singleChoiceName: function () { return $("#singleChoiceContent") },
    $singleChecked: function () { return $(".singleChecked") },

    //多选题
    $multipleChoiceName: function () { return $("#multipleChoiceContent") },
    $multipleChecked: function () { return $(".multipleChecked") },
    //判断题
    $judgeQuestionName: function () { return $("#judgeQuestionContent") },
    //简答题
    $shortAnswerName: function () { return $("#shortAnswerContent") },
    //隐藏控件
    $isEdit: function () { return $("#isEdit") },//问卷总体信息新增/编辑标示
    $vguid: function () { return $("#editVguid") },
    $isEditDetail: function () { return $("#isEditDetail") },//问卷详情新增/编辑标示
    $editVguidDetail: function () { return $("#editVguidDetail") },//每个题的Vguid
    $questionTitleID: function () { return $("#questionTitleID") },//题号

    //form提交
    $questionMainModelForm: function () { return $("#QuestionDetailForm") },
    $singleChoiceForm: function () { return $("#singleChoice") },
    $multipleChoiceForm: function () { return $("#multipleChoice") },
    $judgeQuestionForm: function () { return $("#judgeQuestion") },
    $shortAnswerForm: function () { return $("#shortAnswer") },
    //查询
    $questionName_Search: function () { return $("#QuestionsName_Search") },
    $QuestionType_Search: function () { return $("#QuestionType_Search") },
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

var deletedQuestion = [];  //要删除的问卷
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
    //保存问卷主信息按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$QuestionsName())) {
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
            selector.$questionMainModelForm().ajaxSubmit({
                url: '/QuestionManagement/QuestionManagement/SaveQuestionMain',
                type: "post",
                data: { isEdit: selector.$isEdit().val(), questionData: JSON.stringify(dataQuestionArray) },
                traditional: true,
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            jqxNotification("保存成功！", null, "success");
                            window.location.href = "/QuestionManagement/QuestionManagement/QuestionList";
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
    //取消问卷主信息按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/QuestionManagement/QuestionManagement/QuestionList";
    });

    //新增问卷按钮事件
    selector.$btnAdd().on('click', function () {
        newQuestionDetail();
    });

    //删除问卷按钮事件
    selector.$btnDelete().on('click', function () {
        var selection = [];
        var grid = selector.$questionDetailGrid();
        var checkBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checkBoxs.each(function () {
            if ($(this).is(":checked")) {
                var index = $(this).attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.QuestionTitleID);
            }
        });
        if (selection.length < 1) {
            jqxNotification("请选择要删除的数据行！", null, "error");
        } else {
            deletedQuestion = selection;
            WindowConfirmDialog(deleted, "您确定要删除这条数据？", "确认框", "确认", "取消");
        }
    });

    //问卷详情弹出框确定按钮事件
    selector.$btnSaveQuestion().on('click', function () {
        var page = 1;
        var questionType;
        var singleStr = "";
        var multipleStr = "";
        selector.$questionTypeRadio().each(function () {
            if ($(this).is(":checked")) {
                questionType = $(this).val();
            }
        });
        if (selector.$isEditDetail().val() == "0") {//新增
            if (questionType == "1")//提交单选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$singleChoiceName())) {
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
                    if (dataQuestionArray.length > 0) {
                        var i = dataQuestionArray.length;
                        page = i + 1;
                    }
                    var dataQuestionModel = new dataQuestionModel_Temp(page, 1, singleStr, selector.$singleChoiceName().val());
                    dataQuestionArray.push(dataQuestionModel);
                    //重新加载题号
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        dataQuestionArray[i].QuestionTitleID = i + 1;
                    }
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "2")//提交多选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$multipleChoiceName())) {
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
                    if (dataQuestionArray.length > 0) {
                        var i = dataQuestionArray.length;
                        page = i + 1;
                    }  
                    var dataQuestionModel = new dataQuestionModel_Temp(page, 2, multipleStr, selector.$multipleChoiceName().val());
                    dataQuestionArray.push(dataQuestionModel);
                    //重新加载题号
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        dataQuestionArray[i].QuestionTitleID = i + 1;
                    }
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "3")//提交判断题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$judgeQuestionName())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    if (dataQuestionArray.length > 0) {
                        var i = dataQuestionArray.length;
                        page = i + 1;
                    }
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    var dataQuestionModel = new dataQuestionModel_Temp(page, 3, judgeOption, selector.$judgeQuestionName().val());
                    dataQuestionArray.push(dataQuestionModel);
                    //重新加载题号
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        dataQuestionArray[i].QuestionTitleID = i + 1;
                    }
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "4")//提交简答题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$shortAnswerName())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    if (dataQuestionArray.length > 0) {
                        var i = dataQuestionArray.length;
                        page = i + 1;
                    }
                    var dataQuestionModel = new dataQuestionModel_Temp(page, 4, "", selector.$shortAnswerName().val());
                    dataQuestionArray.push(dataQuestionModel);
                    //重新加载题号
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        dataQuestionArray[i].ExericseTitleID = i + 1;
                    }
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            }
        } else {//编辑
            if (questionType == "1")//提交单选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$singleChoiceName())) {
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
                    page = selector.$questionTitleID().val();    
                    //查询数组的匹配数据
                    var questionModel = new dataQuestionModel_Temp();
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        if (dataQuestionArray[i].QuestionTitleID == selector.$questionTitleID().val()) {
                            questionModel = dataQuestionArray[i];
                        }
                    }
                    questionModel.QuestionOption = singleStr;
                    questionModel.QuestionnaireDetailName = selector.$singleChoiceName().val();
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "2")//提交多选题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$multipleChoiceName())) {
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
                    page = selector.$questionTitleID().val();                    
                    //查询数组的匹配数据
                    var questionModel = new dataQuestionModel_Temp();
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        if (dataQuestionArray[i].QuestionTitleID == selector.$questionTitleID().val()) {
                            questionModel = dataQuestionArray[i];
                        }
                    }
                    questionModel.QuestionOption = multipleStr;
                    questionModel.QuestionnaireDetailName = selector.$multipleChoiceName().val();
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "3")//提交判断题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$judgeQuestionName())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    //题号
                    page = selector.$questionTitleID().val();
                    //1，正确 ，|2.错误
                    var judgeOption = "A.正确,|B.错误";
                    //查询数组的匹配数据
                    var questionModel = new dataQuestionModel_Temp();
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        if (dataQuestionArray[i].QuestionTitleID == selector.$questionTitleID().val()) {
                            questionModel = dataQuestionArray[i];
                        }
                    }
                    questionModel.QuestionOption = judgeOption;
                    questionModel.QuestionnaireDetailName = selector.$judgeQuestionName().val();
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            } else if (questionType == "4")//提交简答题
            {
                var validateError = 0;//未通过验证的数量
                if (!Validate(selector.$shortAnswerName())) {
                    validateError++;
                }
                if (validateError <= 0) {
                    //题号
                    page = selector.$questionTitleID().val();
                    //查询数组的匹配数据
                    var questionModel = new dataQuestionModel_Temp();
                    for (var i = 0; i < dataQuestionArray.length; i++) {
                        if (dataQuestionArray[i].QuestionTitleID == selector.$questionTitleID().val()) {
                            questionModel = dataQuestionArray[i];
                        }
                    }
                    questionModel.QuestionnaireDetailName = selector.$shortAnswerName().val();
                    appendQuestion(JSON.stringify(dataQuestionArray));
                    selector.$questionDialog().modal('hide');
                }
            }
        }
    });

    //问卷详情弹出框取消按钮事件
    selector.$btnCancelQuestion().on('click', function () {
        selector.$questionDialog().modal('hide');

    });

    //选择不同问卷类型加载不同问卷格式
    selector.$questionTypeRadio().on('click', function () {
        var id = $(this).val();
        if (id == 1) {
            $(".SingleChoice").removeAttr("style");
            $(".MultipleChoice").attr("style", "display:none");
            $(".JudgeQuestion").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else if (id == 2) {
            $(".MultipleChoice").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".JudgeQuestion").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else if (id == 3) {
            $(".JudgeQuestion").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".MultipleChoice").attr("style", "display:none");
            $(".ShortAnswer").attr("style", "display:none");
        } else {
            $(".ShortAnswer").removeAttr("style");
            $(".SingleChoice").attr("style", "display:none");
            $(".MultipleChoice").attr("style", "display:none");
            $(".JudgeQuestion").attr("style", "display:none");
        }
    });    

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$questionName_Search().val("");
        selector.$inputType_Search().val("");
        selector.$createdTimeStart_Search().val("");
        selector.$createTimeEnd_Search().val("");
        selector.$QuestionType_Search().val("");
    });

};
var selectionQuestion = [];
//随机生成问卷
function random() {
    var randomQuestion = [];
    $.ajax({
        url: "/QuestionLibraryManagement/CheckedQuestionLibrary/RandomQuestion",
        type: "post",
        dataType: "json",
        success: function (msg) {
            for (var j = 0; j < msg.length; j++) {
                var model = new dataQuestionModel_Temp(1, msg[j].QuestionType, msg[j].Option, msg[j].QuestionName, msg[j].Answer, msg[j].Score, "0");
                randomQuestion.push(model);
            }
            //重新加载题号
            for (var i = 0; i < randomQuestion.length; i++) {
                randomQuestion[i].ExericseTitleID = i + 1;
            }
            appendQuestion(randomQuestion);
            dataQuestionArray = randomQuestion;
        }
    });

}

function initTable() {
    var source =
          {
              datafields:
              [
                  { name: "checkbox", type: null },
                  { name: 'QuestionName', type: 'string' },
                  { name: 'CreatedDate', type: 'date' },
                  { name: 'Status', type: 'string' },
                  { name: 'TranslateStatusQuestionType', type: 'string' },
                  { name: 'QuestionType', type: 'string' },
                  { name: 'TranslateQuestionType', type: 'string' },
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
              data: { "QuestionName": selector.$questionName_Search().val(), "QuestionType": selector.$QuestionType_Search().val(), "InputType": selector.$inputType_Search().val(), "CreatedTimeStart": selector.$createdTimeStart_Search().val(), "CreatedTimeEnd": selector.$createTimeEnd_Search().val() },
              url: "/QuestionLibraryManagement/CheckedQuestionLibrary/GetCheckedQuestionListBySearch"    //获取数据源的路径
          };
    var typeAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (data) {
            source.totalrecords = data.TotalRows;
        }
    });
    //创建卡信息列表（主表）
    selector.$QuestionLibraryList().jqxDataTable(
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
              { text: '问卷名称', width: 350, datafield: 'QuestionName', align: 'center', cellsAlign: 'center' },
              { text: '问卷录入类型', width: 100, datafield: 'TranslateInputType', align: 'center', cellsAlign: 'center' },
              { text: '问卷类型', width: 100, datafield: 'TranslateQuestionType', align: 'center', cellsAlign: 'center' },
              { text: '问卷创建日期', width: 180, datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
              { text: '选项', width: 200, datafield: 'Option', align: 'center', cellsAlign: 'center', cellsRenderer: showQuestion },
              { text: '答案', width: 100, datafield: 'Answer', align: 'center', cellsAlign: 'center' },
              { text: '分值', width: 100, datafield: 'Score', align: 'center', cellsAlign: 'center' },
              { text: 'QuestionType', width: 150, datafield: 'QuestionType', hidden: true },
              { text: 'VGUID', datafield: 'Vguid', hidden: true }
            ]
        });
}
function cellsRendererFunc(row, column, value, rowData) {
    var container = "";
    container = "<input class=\"jqx_datatable_checkbox\"  index=\"" + row + "\" type=\"checkbox\"  onchange=edit(this) style=\"margin:auto;width: 17px;height: 17px;\" />";
    for (var k = 0; k < selectionQuestion.length; k++) {
        if (selectionQuestion[k] == rowData.Vguid) {
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
    var grid = selector.$QuestionLibraryList();
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
function showQuestion(row, column, value, rowData) {
    var container = "";
    if (rowData.QuestionType != "4") {
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

//选择问卷checkbox改变事件
function edit(obj) {
    var vguid = $(obj).parent().siblings('td:last').html();
    if ($(obj).is(":checked")) {
        if ($.inArray(vguid, selectionQuestion) == -1) {
            selectionQuestion.push(vguid);
            var siblings = $(obj).parent().nextAll();
            var questionType = parseInt(siblings.eq(7).html());  //问卷类型
            var arr = siblings.eq(4).html().substring(6, siblings.eq(4).html().indexOf("</span>")).split("<br>");
            var option = $.grep(arr, function (n) { return $.trim(n).length > 0; }).join(',|'); //选项
            var questionName = siblings.eq(0).html();  //问卷名称
            var answer = siblings.eq(5).html();//答案
            var score = siblings.eq(6).html();//分值
            var dataQuestionModel = new dataQuestionModel_Temp(1, questionType, option, questionName, answer, score, "0");
            dataQuestionArray.push(dataQuestionModel);
        }
    } else {
        var index = $.inArray(vguid, selectionQuestion);
        if (index > -1) {
            selectionQuestion.splice(index, 1);
            dataQuestionArray.splice(index, 1);

        }
    }
}

//删除选中的问卷
function deleted() {
    for (var i = 0; i < deletedQuestion.length; i++) {
        deleteQuestionData(deletedQuestion[i]);
    }
}
//全局字典
var letter = ["A", "B", "C", "D", "E", "F", "G", "H"];
//新增问卷
function newQuestionDetail() {
    selector.$questionTypeRadio().each(function () {
        $(this).removeAttr("checked");
        $(this).removeAttr("disabled");
    });
    selector.$isEntryQuestionLibrary().prop("checked", "checked");
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
          "<input id='field_1' type='text' style='width: 75%; margin-left: 45px;' name='singleOption' class='input_text form-control'/>" +
          //"<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
          //    "<input id='firstSingleCheck' type='checkbox' class='singleChecked' style='margin-top: 10px;' value='1'/>" +
          //"</div>" +
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
          "<input id='multipleField_1' type='text' style='width: 75%; margin-left: 45px;' name='multipleOption' class='input_text form-control'/>" +
          //"<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
          //    "<input id='firstMultipleCheck' type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='1'/>" +
          //"</div>" +
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
        url: "/QuestionManagement/QuestionManagement/GetQuestionDetailListByMainVguid",
        data: { "Vguid": questionMainVguid },
        type: "post",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var id = data[i].QuestionTitleID;
                var questionType = data[i].QuestionnaireDetailType;
                var questionOption = data[i].QuestionOption;
                var questionName = data[i].QuestionnaireDetailName;
                var answer = data[i].Answer;
                var questionDataModel = new dataQuestionModel_Temp(id, questionType, questionOption, questionName, answer);
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
                    { name: 'Answer', type: 'string' },
                    { name: 'Vguid', type: 'string' },
                    { name: 'QuestionnaireVguid', type: 'string' },
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
        if (container != "") {
            if (rowData.Answer == 0) {
                container = "正确";
            } else if (rowData.Answer == 1) {
                container = "错误";
            }
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
var dataQuestionModel_Temp = function (id, questionType,questionOption, questionName) {
    this.QuestionTitleID = id;//题号
    this.QuestionnaireDetailType = questionType;//类型
    this.QuestionnaireDetailName = questionName;//题目
    this.QuestionOption = questionOption;//选项
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
        singleOption = [];//单选题选项数组
        singleOption = row.QuestionOption.split(",|");
        //加载单选题选项
        $("#field_1").val(singleOption[0].split("A.")[1]);        
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
                  //"<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                  //    "<input type='checkbox' class='singleChecked' style='margin-top: 10px;' value='" + fieldCount + "'/>" +
                  //"</div>" +
              "</div>");
            singleCount++;
            $("#field_" + fieldCount).val(singleOption[i].split(letter[i] + ".")[1]);
            selector.$singleChecked().each(function () {
                if (row.Answer == letter[$(this).val() - 1]) {
                    $(this).attr("checked", "checked");
                }
            });
        }

    } else if (row.QuestionnaireDetailType == "2") {//多选题
        $(".MultipleChoice").removeAttr("style");
        selector.$multipleChoiceName().val(row.QuestionnaireDetailName);
        multipleOption = [];//所选题选项数组
        multipleOption = row.QuestionOption.split(",|");
        $("#multipleField_1").val(multipleOption[0].split("A.")[1]);
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
                  //"<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
                  //    "<input id='firstMultipleCheck' type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='" + multipleFieldCount + "' />" +
                  //"</div>" +
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
    selector.$myModalLabel_title().text("编辑问卷");
    selector.$questionDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$questionDialog().modal('show');
});

//列表删除操作
function deleteQuestionData(id) {
    var questionModel = new dataQuestionModel_Temp();
    for (var i = 0; i < dataQuestionArray.length; i++) {
        if (dataQuestionArray[i].QuestionTitleID == id) {
            questionModel = dataQuestionArray[i];
        }
    }
    var index = dataQuestionArray.indexOf(questionModel);
    dataQuestionArray.splice(index, 1);
    for (var i = 0; i < dataQuestionArray.length; i++) {
        dataQuestionArray[i].QuestionTitleID = i + 1;
    }
    appendQuestion(JSON.stringify(dataQuestionArray));
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
              //"<div class='singleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              //    "<input type='checkbox' class='singleChecked' style='margin-top: 10px;' value='" + fieldCount + "'/>" +
              //"</div>" +
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
              //"<div class='multipleCorrectAnswer' style='position: absolute; left: 82%;'>" +
              //    "<input type='checkbox' class='multipleChecked' style='margin-top: 10px;' value='" + multipleFieldCount + "' />" +
              //"</div>" +
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


