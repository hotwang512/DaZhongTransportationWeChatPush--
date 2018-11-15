var selector = {
    $grid: function () { return $("#AnswerPersonList") },
    $showDialog: function () { return $("#ShortAnswerDialog") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnSave: function () { return $("#btnSave") },

    //查询
    $exerciseName_Search: function () { return $("#ExerciseName_Search") },
    $txtIsMarking: function () { return $("#txtIsMarking") },
    //弹出框元素
    $exerciseName: function () { return $("#ExerciseName") },
    $exerciseScore: function () { return $("#ExerciseScore") },
    $exerciseDetailVguid: function () { return $("#exerciseDetailVguid") },
    $score: function () { return $("#Score") },
    $personVguid: function () { return $("#personVguid") },
    $exerciseAnswer: function () { return $("#ExerciseAnswer") }

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
        $(".chosen-single span").text("");
        $(".chosen-single span").text("===请选择===");
        selector.$txtIsMarking().val("");
    });
    selector.$exerciseName_Search().chosen();
    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        selector.$showDialog().modal('hide');
    });

    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$score())) {
            validateError++;
        }
        if (validateError <= 0) {
            var personScore = Number(selector.$score().val());//用户得分
            var score = Number(selector.$exerciseScore().val());//简答题题目分值
            if (personScore > score) {
                jqxNotification("得分不能大于本题分值！", null, "error");
                return;
            }
            $.ajax({
                url: "/ExerciseManagement/MarkingExercise/SaveShortAnswerMarking",
                data: { "personVguid": selector.$personVguid().val(), "vguid": selector.$exerciseName_Search().val(), 'exerciseDetailVguid': selector.$exerciseDetailVguid().val(), 'score': selector.$score().val() },
                type: "post",
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            selector.$showDialog().modal('hide');
                            jqxNotification("保存成功！", null, "success");
                            selector.$grid().jqxDataTable('updateBoundData');
                            break;
                    }
                }
            });
        }
    });

    //加载团队表
    function LoadTable() {

        var source =
            {
                datafields:
                [
                    { name: "Vguid", type: 'string' },
                    { name: 'Name', type: 'string' },
                    { name: 'IDNumber', type: 'string' },
                    { name: 'Marking', type: 'string' },
                    { name: 'OwnedFleet', type: 'string' },
                    { name: 'PersonVguid', type: 'string' }
                ],
                datatype: "json",
                id: "PersonVguid",//主键
                async: true,
                data: { "vguid": selector.$exerciseName_Search().val(), 'isMarking': selector.$txtIsMarking().val() },
                url: "/ExerciseManagement/MarkingExercise/GetAnsweredPersonList"    //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(source, {
            downloadComplete: function (data) {
                source.totalrecords = data.TotalRows;

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
                  //{ width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '答题人姓名', datafield: 'Name', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  { text: '身份证号', datafield: 'IDNumber', align: 'center', cellsAlign: 'center' },
                  { text: '是否阅卷', datafield: 'Marking', align: 'center', cellsAlign: 'center', cellsRenderer: translationFunc },
                  { text: 'Vguid', datafield: 'Vguid', hidden: true },//, hidden: true
                  { text: 'PersonVguid', datafield: 'PersonVguid', hidden: true }
                ]
            });
    }

    //跳转编辑页面
    function detailFunc(row, column, value, rowData) { // \'' + data.id + '\'
        var container = "";
        container = "<a href='#' onclick='markingAnswerDetail(\"" + rowData.Vguid + "\",\"" + rowData.PersonVguid + "\")' style=\"text-decoration: underline;color: #333;\">" + rowData.Name + "</a>";
        return container;
    }
    function translationFunc(row, column, value, rowData) {
        var container = "";
        if (rowData.Marking == 2) {
            container = "<span>已阅卷</span>";
        } else {
            container = "<span>未阅卷</span>";
        }
        // container = "<a href='#' onclick='markingAnswerDetail(\"" + rowData.Vguid + "\",\"" + rowData.PersonVguid + "\")' style=\"text-decoration: underline;color: #333;\">" + rowData.Name + "</a>";
        return container;
    }
};

//查看当前人员简答题答案
function markingAnswerDetail(vguid, personVguid) {
    $.ajax({
        url: "/ExerciseManagement/MarkingExercise/IsExerciseMarked",
        data: { 'exerciseVguid': vguid, 'personVguid': personVguid },
        type: "post",
        success: function (data) {
            $.ajax({
                url: "/ExerciseManagement/MarkingExercise/GetShortAnswerDetail",
                data: { 'exerciseVguid': vguid, 'personVguid': personVguid },
                type: "post",
                success: function (msg) {
                    selector.$exerciseName().val(msg[0].ExerciseName);
                    selector.$exerciseScore().val(msg[0].ExercisesDetailScore);
                    selector.$exerciseAnswer().val(msg[0].ExercisesAnswerDetailAnswer);
                    selector.$exerciseDetailVguid().val(msg[0].ExercisesDetailVGUID);
                    selector.$score().val(msg[0].ExercisesAnswerDetailScore);
                    selector.$personVguid().val(personVguid);
                    if (data.respnseInfo == "1") {
                        selector.$score().attr("disabled", "disabled");
                        selector.$score().attr("style", "background-color: #f5f5f5!important;");
                    } else {
                        selector.$score().removeAttr("disabled");
                        selector.$score().removeAttr("style");
                    }
                    //弹出编辑框
                    selector.$myModalLabel_title().text("审阅简答题");
                    selector.$showDialog().modal({ backdrop: 'static', keyboard: false });
                    selector.$showDialog().modal('show');
                }
            });
        }
    });
}



$(function () {
    var page = new $page();
    page.init();

})


