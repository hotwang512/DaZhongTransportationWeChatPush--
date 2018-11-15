var selector = {

    $form: function () { return $("#knowledgeDetail") },
    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnSubmit: function () { return $("#btnSubmit") },
    $btnCancel: function () { return $("#btnCancel") },
    //表单
    $txtTitle: function () { return $("#txtTitle") },
    $txtContent: function () { return $("#txtContent") },
    $txtCreateDate: function () { return $("#txtCreatedDate") },
    $importFile: function () { return $("#importFile") },//上传附件
    //隐藏控件
    $isEdit: function () { return $("#isEdit") },//信息新增/编辑标示
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


    }; //addEvent end
    var um = UE.getEditor('txtContent');
    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/KnowledgeBaseManagement/Draft/DraftList";
    });

    //保存按钮
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$txtTitle())) {
            validateError++;
        }
        //if (!Validate(selector.$txtContent())) {
        //    validateError++;
        //}
        if (um.getContent() == "") {
            jqxNotification("请填写内容！", null, "error");
            return false;
        }
        if (validateError <= 0) {
            selector.$form().ajaxSubmit({
                url: '/KnowledgeBaseManagement/Draft/SaveKnowledge',
                type: "post",
                data: { isEdit: selector.$isEdit().val(), saveType: "1" },
                //  traditional: true,
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            jqxNotification("保存成功！", null, "success");
                            window.location.href = "/KnowledgeBaseManagement/Draft/DraftList";
                            break;
                        case "2":
                            jqxNotification("知识已经存在！", null, "error");
                            break;
                    }
                }
            });
        }
    });
    //提交按钮
    selector.$btnSubmit().on('click', function () {
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$txtTitle())) {
            validateError++;
        }
        //if (!Validate(selector.$txtContent())) {
        //    validateError++;
        //}
        if (um.getContent() == "") {
            jqxNotification("请填写内容！", null, "error");
            return false;
        }
        if (validateError <= 0) {
            selector.$form().ajaxSubmit({
                url: '/KnowledgeBaseManagement/Draft/SaveKnowledge',
                type: "post",
                data: { isEdit: selector.$isEdit().val(), saveType: "2" },
                traditional: true,
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            jqxNotification("保存成功！", null, "success");
                            window.location.href = "/KnowledgeBaseManagement/Draft/DraftList";
                        case "2":
                            jqxNotification("知识已经存在！", null, "error");
                            break;
                    }
                }
            });
        }
    });
    //上传附件转换成html并展示在Ueditor
    selector.$importFile().on('change', function () {
        showLoading();
        selector.$form().ajaxSubmit({
            url: "/KnowledgeBaseManagement/Draft/ConvertToHtml?importFile=" + event.target.id,
            type: "post",
            traditional: true,
            success: function (data) {
                if (data == "") {
                    um.setContent("");
                } else {
                    um.execCommand('inserthtml', data);
                }
                closeLoading();
            }
        });
    });
};



$(function () {
    var page = new $page();
    page.init();

});


