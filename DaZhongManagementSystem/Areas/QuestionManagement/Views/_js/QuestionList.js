var selector = {
    $grid: function () { return $("#QuestionList") },
    $form: function () { return $("#UpLoadQuestionForm") },

    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnImport: function () { return $("#btnImport") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnChecked: function () { return $("#btnChecked") },
    $btnDownload: function () { return $("#btnDownload") },//下载模板
    $importFile: function () { return $("#importFile") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $questionName_Search: function () { return $("#QuestionsName_Search") },
    $effectiveDate_Search: function () { return $("#EffectiveDate_Search") },

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
        selector.$questionName_Search().val("");
        selector.$effectiveDate_Search().val("");
    });

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        window.location.href = "/QuestionManagement/QuestionManagement/QuestionDetail?isEdit=false";
    });

    //下载模板按钮事件
    selector.$btnDownload().on('click', function () {
        window.location.href = "/QuestionManagement/QuestionManagement/DownLoadTemplate";
    });

    //导入问卷按钮事件
    selector.$btnImport().on('click', function () {
        selector.$importFile().click();
    });

    //生效时间控件
    selector.$effectiveDate_Search().datetimepicker({
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
        selection = [];
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
            jqxNotification("请选择您要审核的问卷！", null, "error");
        } else {
            WindowConfirmDialog(checked, "您确定要审核通过选中的问卷？", "确认框", "确定", "取消");
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
        })
        if (selection.length < 1) {
            jqxNotification("请选择您要删除的问卷！", null, "error");
        } else {
            WindowConfirmDialog(deleted, "您确定要删除选中的问卷？", "确认框", "确定", "取消");
        }
    });

    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'QuestionnaireName', type: 'string' },
                    { name: 'EffectiveDate', type: 'date' },
                    { name: 'Status', type: 'string' },
                    { name: 'StatusName', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'Remarks', type: 'string' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { "QuestionName": selector.$questionName_Search().val(), "EffectiveDate": selector.$effectiveDate_Search().val() },
                url: "/QuestionManagement/QuestionManagement/GetQuestionListBySearch"    //获取数据源的路径
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
                  { text: '问卷名称', width: 350, datafield: 'QuestionnaireName', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  { text: '问卷有效日期', width: 180, datafield: 'EffectiveDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: '问卷状态', width: 150, datafield: 'StatusName', align: 'center', cellsAlign: 'center' },
                  { text: '描述', datafield: 'Description', align: 'center', cellsAlign: 'center' },
                  { text: '备注', datafield: 'Remarks', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ]
            });
    }

    //跳转编辑页面
    function detailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='QuestionDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.QuestionnaireName + "</a>";
        return container;
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

};

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
        url: "/QuestionManagement/QuestionManagement/CheckedQuestion",
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
    selection = [];
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
        url: "/QuestionManagement/QuestionManagement/DeletedQuestion",
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
        url: "/QuestionManagement/QuestionManagement/UpLoadQuestion?questionFile=importFile",
        type: "post",
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccess) {
                jqxNotification("问卷导入成功！", null, "success");
                selector.$grid().jqxDataTable('updateBoundData');
            } else {
                jqxNotification("问卷导入失败！", null, "error");
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


