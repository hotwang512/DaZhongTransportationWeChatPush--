var selector = {
    //$grid: function () { return $("#DraftInfoList") },
    $pushForm: function () { return $("#pushForm") },
    $importForm: function () { return $("#importForm") },
    $grid: function () { return $("#UserInfoList") },
    $uploadPushForm: function () { return $("#uploadPushForm") },
    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnSubmit: function () { return $("#btnSubmit") },
    $imgPop: function () { return $("#imgPop") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnDownload: function () { return $("#btnDownload") },
    $btnUpload: function () { return $("#btnUpload") },
    $btnPreview: function () { return $("#btnPreview") },
    $btnUploadSalary: function () { return $("#btnUploadSalary") },
    $btnUploadMaintenance: function () { return $("#btnUploadMaintenance") },
    $btnSave_sa: function () { return $("#btnSave_sa") },
    $btnSubmit_sa: function () { return $("#btnSubmit_sa") },
    $btnCancel_sa: function () { return $("#btnCancel_sa") },
    //查询
    $name_Search: function () { return $("#Name_Search") },
    $TranslationOwnedFleet_Search: function () { return $("#TranslationOwnedFleet_Search") },
    $OwnedFleet: function () { return $("#OwnedFleet") },
    $MobilePhone_Search: function () { return $("#MobilePhone_Search") },
    $selLabel: function () { return $("#selLabel") },
    //弹出框
    $SelectDialog: function () { return $("#SelectDialog") },
    $btnPopSave: function () { return $("#btnPopSave") },
    $btnPopCancel: function () { return $("#btnPopCancel") },
    $departmentDropDownButton: function () { return $("#DepartmentDropDownButton") },
    $departmentTree: function () { return $("#DepartmentTree") },
    $jqxWidget: function () { return $("#jqxWidget") },
    $SalaryDialog: function () { return $("#SalaryDialog") },
    $PreviewDialog: function () { return $("#PreviewDialog") },
    $txtName_P: function () { return $("#txtName_P") },
    $txtMobilePhone_P: function () { return $("#txtMobilePhone_P") },
    $btnSearch_P: function () { return $("#btnSearch_P") },
    $btnReset_P: function () { return $("#btnReset_P") },
    $jqxTable: function () { return $("#jqxTable") },
    $btnPopSave_P: function () { return $("#btnPopSave_P") },
    $btnPopCance_P: function () { return $("#btnPopCance_P") },
    $notExistDialog: function () { return $("#notExistDialog") },
    $tips: function () { return $("#tips") },
    $notExistTable: function () { return $("#notExistTable") },
    $btnCancel_Not: function () { return $("#btnCancel_Not") },
    $btnDownLoad: function () { return $("#btnDownLoad") },
    //界面元素
    $pushPeople: function () { return $("#pushPeople") },
    $txtTitle: function () { return $("#txtTitle") },
    $txtTitle_sa: function () { return $("#txtTitle_sa") },
    $pushType: function () { return $("#pushType") },
    $isSendTime: function () { return $("#isSendTime") },
    $isSendTime_sa: function () { return $("#isSendTime_sa") },
    $sendTime: function () { return $("#sendTime") },
    $sendTime_sa: function () { return $("#sendTime_sa") },
    $effectiveDate: function () { return $("#effectiveDate") },
    $effectiveDate_sa: function () { return $("#effectiveDate_sa") },
    $isImportant: function () { return $("#isImportant") },
    $isImportant_sa: function () { return $("#isImportant_sa") },
    $pushPeopleDropDownButton: function () { return $("#pushPeopleDropDownButton") },
    $pushPeopleTree: function () { return $("#pushPeopleTree") },
    $weChatTypeRow: function () { return $("#weChatTypeRow") },//微信推送类型那一行
    $weChatMessageType: function () { return $("#weChatMessageType") },//微信推送消息类型
    $weChatCoverMsg: function () { return $(".weChatCoverMsg") },//封面图片和封面描述
    $exerciseList: function () { return $("#ExerciseList") },//习题列表td
    $pushExercise: function () { return $("#pushExercise") },//习题下拉框
    $questionList: function () { return $("#QuestionList") },//问卷列表td
    $pushQuestion: function () { return $("#pushQuestion") },//问卷下拉框
    $coverImg_input: function () { return $("#coverImg_input") },//封面图片
    $coverImg_input_sa: function () { return $("#coverImg_input_sa") },//封面图片
    $coverImgPath_sa: function () { return $("#coverImgPath_sa") },
    $txtCoverDescption_sa: function () { return $("#txtCoverDescption_sa") },
    $importFile: function () { return $("#importFile") },//上传附件
    $importSalary: function () { return $("#importSalary") },
    $upLoadFileTD: function () { return $(".upLoadFileTD") },//上传附件的两个td
    $exercisePushRow: function () { return $(".exercisePushRow") },//习题推送那一行的习题推送的两个td
    $questionPushRow: function () { return $(".questionPushRow") },//习题推送那一行的习题推送的两个td
    $pushHistory: function () { return $(".pushHistory") },   //记录推送历史的td
    $chkHistory: function () { return $("#chkHistory") },
    $uEditor: function () { return $(".UEditor") },//富文本框
    //$edui1_iframeholder: function () { return $(".view") },
    //$revenueType: function () { return $(".revenueType") },
    $agreementType: function () { return $("#agreementType") },
    $revenueType: function () { return $("#revenueType") },
    $pushContent: function () { return $("#pushContent") },
    $textEdit: function () { return $(".textEdit") },
    $isEdit: function () { return $("#isEdit") },//编辑/新增
    $countDown: function () { return $(".countDown") },
    $vguid: function () { return $("#vguid") },
    $tdMoney: function () { return $(".tdMoney") },
    $txtMoney: function () { return $("#txtMoney") },
    $txtWishing: function () { return $("#txtWishing") },
    $paymentMoney: function () { return $("#paymentMoney") },
    $txtDesc: function () { return $("#txtDesc") },
    $defaultEffectiveDate: function () { return $("#defaultEffectiveDate") },//默认的有效日期
    $currentTime: function () { return $("#currentTime") }//当前时间
};
var people = [];
var pushObject = [];
var txtMsgContent = "";
var editor1;
var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        initDropdownList();
        selector.$pushExercise().chosen();  //可搜索下拉框

        //发送时间控件
        selector.$sendTime().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });

        //有效时间控件
        selector.$effectiveDate().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });
        //发送时间控件
        selector.$sendTime_sa().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });

        //有效时间控件
        selector.$effectiveDate_sa().datetimepicker({
            format: "yyyy-mm-dd hh:ii",
            autoclose: true,
            todayHighlight: true,
            orientation: "bottom right",
            showMeridian: true
        });
        $(".glyphicon-arrow-left").text("<<");
        $(".glyphicon-arrow-right").text(">>");
        //取消按钮事件
        selector.$btnCancel().on('click', function () {
            window.location.href = "/WeChatPush/DraftList/DraftList";
        });
        //点击+号
        selector.$imgPop().on('click', function () {
            var parent = selector.$grid().parent();
            selector.$grid().jqxDataTable("destroy");
            $(parent).append('<div id="UserInfoList" class="jqxTable" style="border-left: 0px;"></div>');
            //推送接收人下拉框
            $.ajax({
                url: "/BasicDataManagement/UserInfo/GetOrganizationTreeList",
                data: {},
                traditional: true,
                type: "post",
                success: function (msg) {
                    //推送接收人下拉框
                    selector.$departmentDropDownButton().jqxDropDownButton({
                        width: 185,
                        height: 25
                    });

                    //推送接收人下拉框(树形结构)
                    selector.$departmentTree().on('select', function (event) {
                        var items = selector.$departmentTree().jqxTree('getItems');
                        var flag = -1;
                        for (var i = 0; i < items.length; i++) {
                            if ($("#txtDeparent").val() == items[0].id || $("#txtDeparent").val() == items[1].id) {
                                flag = 0;
                            }
                        }
                        var args = event.args;
                        var item = selector.$departmentTree().jqxTree('getItem', args.element);
                        if ($("#txtDeparent").val() != "" && flag == -1) {  // 说明是子公司
                            if ($("#txtDeparent").val() != item.id && $("#txtDeparent").val() != item.parentId) {
                                jqxNotification("请选择本公司或者本公司部门！", null, "error");
                                return false;
                            }
                        }

                        var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                        selector.$departmentDropDownButton().jqxDropDownButton('setContent', dropDownContent);
                        var items = selector.$departmentTree().jqxTree('getSelectedItem');
                        selector.$OwnedFleet().val("");
                        selector.$TranslationOwnedFleet_Search().val("");
                        selector.$OwnedFleet().val(items.id);
                        selector.$TranslationOwnedFleet_Search().val(items.label);
                        initTable();

                    });
                    var source =
                            {
                                datatype: "json",
                                datafields: [
                                    { name: 'OrganizationName' },
                                    { name: 'ParentVguid' },
                                    { name: 'Vguid' }
                                ],
                                id: 'Vguid',
                                localdata: msg
                            };
                    var dataAdapter = new $.jqx.dataAdapter(source);
                    // perform Data Binding.
                    dataAdapter.dataBind();
                    var records = dataAdapter.getRecordsHierarchy('Vguid', 'ParentVguid', 'items',
                        [
                            {
                                name: 'OrganizationName',
                                map: 'label'
                            },
                            {
                                name: 'Vguid',
                                map: 'id'
                            },
                            {
                                name: 'ParentVguid',
                                map: 'parentId'
                            }
                        ]);
                    selector.$departmentTree().jqxTree({ source: records, width: '207px', height: '250px', incrementalSearch: true });//, checkboxes: true

                    selector.$departmentTree().jqxTree('expandAll');

                }
            });
            selector.$btnReset().click();
            initTable();
            selector.$SelectDialog().modal({ backdrop: 'static', keyboard: false });
            selector.$SelectDialog().modal('show');
        });
        //点击弹出框中的取消按钮
        selector.$btnPopCancel().on('click', function () {
            selector.$SelectDialog().modal('hide');
        });
        //如果是编辑界面
        if (selector.$isEdit().val() == "True") {
            var chs = $("#pushName").val().split(',');
            selector.$pushPeople().val(chs);
            if (selector.$revenueType().val() == "1") {
                selector.$tdMoney().show();
            } else {
                selector.$tdMoney().hide();
            }
            if ($("#txtMessageType").val() == 3) {
                $(".mod-sender__slider").show();
                $("#coverImgPath").val($(".coverHid").val());
                var imgObj = '<div class="video_image_wrap"><img style="width:100%;vertical-align: middle;display:inline-block;" src="' + $(".coverHid").val() + '"></div>';
                $(".image_edit_placeholder").children().remove();
                $(".image_edit_placeholder").append(imgObj);
                //创建侧边预览框
                createModSender();
            }

        }
        //创建侧边预览框
        function createModSender() {
            $.ajax({
                url: "/WeChatPush/DraftList/GetMoreGraphicList",
                type: "post",
                data: { vguid: selector.$vguid().val() },
                dataType: "json",
                success: function (msg) {
                    for (var i = 0; i < msg.length; i++) {
                        onCreate(msg[i]);
                    }
                }
            });
        }

        //checkbox选中都选中，取消选中则都取消
        $(":checkbox").on('change', function () {
            if ($(this).is(":checked")) {
                $(":checkbox").prop("checked", "checked");
            }
            else {
                $(":checkbox").each(function () {
                    if ($(this).not(":checked")) {
                        $(":checkbox").removeAttr("checked", "checked");
                    }
                });
            }
        });

        selector.$btnUpload().on('click', function () {
            $("#import").click();
        });

        //点击弹出框保存按钮
        selector.$btnPopSave().on('click', function () {
            if (people.length != 0) { //说明勾选了具体的人
                selector.$pushPeople().val(people.join(","));
                $("#pushObject").val(pushObject.join("|"));
            }
            if (selector.$departmentTree().jqxTree('getSelectedItem') != null && people.length == 0) {  //说明选择了具体的部门
                $("#pushObject").val(selector.$OwnedFleet().val());
                selector.$pushPeople().val(selector.$TranslationOwnedFleet_Search().val());
            }
            //标签下拉框
            var item = selector.$jqxWidget().jqxDropDownList('getCheckedItems');
            var checkedItems = "";
            if (item.length > 0) {
                $.each(item, function (index) {
                    checkedItems += this.label + ",";
                });
                checkedItems = checkedItems.substr(0, checkedItems.length - 1);
                if (checkedItems != "" && people.length == 0 && selector.$departmentTree().jqxTree('getSelectedItem') == null) { //说明选择的标签
                    $("input[name=Label]").val(checkedItems);
                    $("#pushObject").val('');
                    pushObject = [];
                    selector.$pushPeople().val('');
                } else if (checkedItems != "" && selector.$departmentTree().jqxTree('getSelectedItem') != null && people.length == 0) { //即选择了标签又选择了部门
                    $("input[name=Label]").val(checkedItems);
                }
            }

            //if (flag == 0) {
            //    pushObject = [];
            //    selector.$pushPeople().val('');
            //    $("#pushObject").val('');
            //}

            //var flag = 0;
            ////说明选择的是具体的人
            //if (selector.$OwnedFleet().val() == "") {
            //    selector.$pushPeople().val(people.join(","));
            //    $("#pushObject").val(pushObject.join("|"));
            //} else if (selector.$OwnedFleet().val() != "" && pushObject.length > 0) {
            //    selector.$pushPeople().val(people.join(","));
            //    $("#pushObject").val(pushObject.join("|"));
            //}
            //else {//说明选择的是整个部门
            //    flag = 1;
            //    $("#pushObject").val(selector.$OwnedFleet().val());
            //    selector.$pushPeople().val(selector.$TranslationOwnedFleet_Search().val());
            //}

            //var items = selector.$jqxWidget().jqxDropDownList('getCheckedItems');
            //var checkedItems = "";
            //if (items.length > 0) {
            //    $.each(items, function (index) {
            //        checkedItems += this.label + ",";
            //    });
            //    checkedItems = checkedItems.substr(0, checkedItems.length - 1);
            //    $("input[name=Label]").val(checkedItems);
            //    if (flag == 0) {
            //        pushObject = [];
            //        selector.$pushPeople().val('');
            //        $("#pushObject").val('');
            //    }
            //}
            if (people.length == 0 && $("#txtLabel").val() == "" && selector.$departmentTree().jqxTree('getSelectedItem') == null) {
                jqxNotification("请选择推送人！", null, "error");
                return false;
            }
            selector.$SelectDialog().modal('hide');
        });
        //查询按钮事件
        selector.$btnSearch().on('click', function () {
            var items = selector.$departmentTree().jqxTree('getSelectedItem');
            if (items != null) {
                selector.$TranslationOwnedFleet_Search().val(items.label);
                selector.$OwnedFleet().val(items.id);
            }
            initTable();
        });

        //重置按钮事件
        selector.$btnReset().on('click', function () {
            selector.$name_Search().val("");
            selector.$TranslationOwnedFleet_Search().val("");
            selector.$OwnedFleet().val("");
            selector.$departmentDropDownButton().jqxDropDownButton('setContent', "");
            selector.$departmentTree().jqxTree('clearSelection');
            selector.$MobilePhone_Search().val("");
            selector.$jqxWidget().jqxDropDownList('uncheckAll');
            selector.$jqxWidget().jqxDropDownList('setContent', '===请选择===');

        });

        //提交按钮事件
        selector.$btnSubmit().on('click', function () {
            selector.$pushType().removeAttr("disabled");
            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "1")//文本推送
                {
                    txtMsgContent = selector.$pushContent().val();
                }
                else if (selector.$weChatMessageType().val() == "2")//图片推送
                {
                    //txtMsg = selector.$exerciseList().val();
                }
                else if (selector.$weChatMessageType().val() == "3")//图文推送
                {
                    // var um = UE.getEditor('myEditor');
                    // txtMsgContent = um.getContent();
                }
                else if (selector.$weChatMessageType().val() == "4")//习题推送
                {
                    txtMsgContent = selector.$pushExercise().val();
                }
                else if (selector.$weChatMessageType().val() == "5")//培训推送
                {
                    var um = UE.getEditor('myEditor');
                    txtMsgContent = um.getContent();
                } else if (selector.$weChatMessageType().val() == "6") //知识库推送
                {
                    txtMsgContent = "";
                } else if (selector.$weChatMessageType().val() == "11") {
                    switch (selector.$revenueType().val()) {
                        case "1":
                            txtMsgContent = parseFloat(selector.$txtMoney().val());
                            break;
                        case "2":
                            txtMsgContent = "";
                            break;
                    }
                } else if (selector.$weChatMessageType().val() == "13") {
                    txtMsgContent = selector.$txtWishing().val();
                } else if (selector.$weChatMessageType().val() == "14") {
                    txtMsgContent = selector.$txtDesc().val();
                } else if (selector.$weChatMessageType().val() == "16") {
                    txtMsgContent = selector.$pushQuestion().val();
                } else {
                    var um = UE.getEditor('myEditor');
                    txtMsgContent = um.getContent();
                }
            }
            else if (selector.$pushType().val() == "2")//短信推送
            {
                txtMsgContent = selector.$pushContent().val();
            }
            var validateError = 0;//未通过验证的数量
            if (!Validate(selector.$txtTitle())) {
                validateError++;
            }
            if (selector.$pushPeople().val().length <= 0 && $("#txtLabel").val().length <= 0) {
                // jqxNotification("请选择微信推送接收者！", null, "error");
                jqxNotification("微信推送接收者和人员标签至少选择一个！", null, "error");
                return false;
            }
            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "") {
                    jqxNotification("请选择微信推送类型！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == 4 || selector.$weChatMessageType().val() == 5) {
                if (selector.$pushExercise().val() == "") {
                    jqxNotification("请选择推送习题！", null, "error");
                    return false;
                }
            }
            if (selector.$isImportant().val() == "False") {
                if (selector.$effectiveDate().val() < selector.$currentTime().val()) {
                    jqxNotification("有效日期不能小于当前日期！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == "11" && selector.$revenueType().val() == "1") {
                if (!Validate(selector.$txtMoney())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "12") {
                if (!Validate(selector.$agreementType())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "13") {
                if (!Validate($("#RedpacketType"))) {
                    validateError++;
                }
                if (!Validate(selector.$txtWishing())) {
                    validateError++;
                }
                if ($("#RedpacketType").val() == "1" || $("#RedpacketType").val() == "2") {
                    if (!Validate($("#fixedRedpacket"))) {
                        validateError++;
                    }
                } else {
                    if (!Validate($("#txtRedpacketFrom"))) {
                        validateError++;
                    }
                    if (!Validate($("#txtRedpacketTo"))) {
                        validateError++;
                    }
                }
            }
            if (selector.$weChatMessageType().val() == "14") {
                if (!Validate(selector.$txtDesc())) {
                    validateError++;
                }
                if (!Validate(selector.$paymentMoney())) {
                    validateError++;
                }
            }
            if (validateError <= 0) {
                var titleError = 0;
                $("input[name=Title]").each(function () {
                    if ($(this).val() == "") {
                        titleError++;
                    }
                });
                if (titleError > 0) {
                    jqxNotification("标题不能为空", null, "error");
                    return false;
                }
                WindowConfirmDialog(submit, "您确定要提交本条数据？", "确认框", "确定", "取消");
            }
        });
        //预览框中搜索按钮
        selector.$btnSearch_P().on("click", function () {
            initTable_P();
        });
        //预览框中充值按钮
        selector.$btnReset_P().on("click", function () {
            selector.$txtMobilePhone_P().val("");
            selector.$txtName_P().val("");
        });
        //预览框中取消按钮
        selector.$btnPopCance_P().on("click", function () {
            selector.$PreviewDialog().modal("hide");
        });
        //预览框中保存按钮
        selector.$btnPopSave_P().on("click", function () {
            selector.$pushType().removeAttr("disabled");
            var txtContent = "";
            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "1")//文本推送
                {
                    txtContent = selector.$pushContent().val();
                }
                else if (selector.$weChatMessageType().val() == "2")//图片推送
                {
                    //txtMsg = selector.$exerciseList().val();
                }
                else if (selector.$weChatMessageType().val() == "3")//图文推送
                {
                    var um = UE.getEditor('myEditor');
                    txtContent = um.getContent();
                }
                else if (selector.$weChatMessageType().val() == "4")//习题推送
                {
                    txtContent = selector.$pushExercise().val();
                }
                else if (selector.$weChatMessageType().val() == "5")//培训推送
                {
                    var um = UE.getEditor('myEditor');
                    txtContent = um.getContent();
                } else if (selector.$weChatMessageType().val() == "6") //知识库推送
                {
                    txtContent = "";
                } else if (selector.$weChatMessageType().val() == "11") {
                    switch (selector.$revenueType().val()) {
                        case "1":
                            txtContent = parseFloat(selector.$txtMoney().val());
                            break;
                        case "2":
                            txtContent = "";
                            break;
                    }
                } else if (selector.$weChatMessageType().val() == "13") {
                    txtMsgContent = selector.$txtWishing().val();
                } else if (selector.$weChatMessageType().val() == "14") {
                    txtMsgContent = selector.$txtDesc().val();
                }
                else {
                    var um = UE.getEditor('myEditor');
                    txtContent = um.getContent();
                }
            }
            else if (selector.$pushType().val() == "2")//短信推送
            {
                txtContent = selector.$pushContent().val();
            }

            var selection = [];
            var grid = selector.$jqxTable();
            var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
            checedBoxs.each(function () {
                var th = $(this);
                if (th.is(":checked")) {
                    var index = th.attr("index");
                    var data = grid.jqxDataTable('getRows')[index];
                    selection.push(data.UserID);
                }
            });
            if (selection.length == 0) {
                jqxNotification("请选择预览人员！", null, "error");
                return false;
            }
            //if (selection.length > 1) {
            //    jqxNotification("只能选择一个！", null, "error");
            //    return false;
            //}
            $("#pushObject").val(selection[0]);
            if (selector.$weChatMessageType().val() == 3) {
                var wechatPushList = [];
                var wechatPushMoreGraphic = [];
                var tabType = [];
                var wechatPushModel = new WeChatPush_Temp(selector.$pushType().val(), selector.$txtTitle().val(), selector.$weChatMessageType().val(), selector.$isSendTime().val(), selector.$sendTime().val(), selector.$isImportant().val(), txtContent, selector.$effectiveDate().val(), $("#coverImgPath").val(), $("#txtCoverDescption").val(), selector.$chkHistory().is(":checked"), $("#pushObject").val(), $("#txtLabel").val(), selector.$vguid().val());
                wechatPushList.push(wechatPushModel);
                $("#pushForm table").each(function () {
                    if ($(this).attr("tabType")) {
                        tabType.push($(this).attr("tabType"));
                    }
                });
                tabType = tabType.sort();
                $("#pushForm table").each(function () {
                    if ($(this).attr("tabType")) {
                        var weChatPushMoreGraphicModel = new weChatPushMoreGraphic($("#txtTitle" + $(this).attr("tabType")).val(), UE.getEditor('myEditor' + $(this).attr("tabType")).getContent(), $("#coverImgPath" + $(this).attr("tabType")).val(), $("#txtCoverDescption" + $(this).attr("tabType")).val(), parseInt($.inArray($(this).attr("tabType"), tabType)) + 1);
                        wechatPushMoreGraphic.push(weChatPushMoreGraphicModel);
                    }
                });
                showLoading();
                $.ajax({
                    url: "/WeChatPush/DraftList/SaveImagePushMsg",
                    type: "post",
                    data: { wechatPushList: JSON.stringify(wechatPushList), wechatPushMoreGraphicList: JSON.stringify(wechatPushMoreGraphic), isEdit: "false", saveType: "3", countersignType: selector.$agreementType().val() },
                    dataType: "json",
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                jqxNotification("预览失败！", null, "error");
                                break;
                            case "1":
                                jqxNotification("消息已发送到您手机！", null, "success");

                        }
                        closeLoading();
                    }
                });
            } else {
                selector.$pushForm().ajaxSubmit({
                    url: '/WeChatPush/DraftList/SavePushMsg',
                    type: "post",
                    data: { isEdit: "false", txtMessage: txtMsgContent, history: selector.$chkHistory().is(":checked"), isEdit: "false", saveType: "3" },
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                jqxNotification("预览失败！", null, "error");
                                break;
                            case "1":
                                jqxNotification("消息已发送到您手机！", null, "success");


                        }
                        closeLoading();
                    }
                });
            }


            selector.$PreviewDialog().modal("hide");
        });
        //预览按钮事件
        selector.$btnPreview().on("click", function () {
            var validateError = 0;//未通过验证的数量
            if (!Validate(selector.$txtTitle())) {
                validateError++;
            }
            //if (selector.$pushPeople().val().length <= 0 && $("#txtLabel").val().length <= 0) {
            //    // jqxNotification("请选择微信推送接收者！", null, "error");
            //    jqxNotification("微信推送接收者和人员标签至少选择一个！", null, "error");
            //    return false;
            //}
            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "") {
                    jqxNotification("请选择微信推送类型！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == 4 || selector.$weChatMessageType().val() == 5) {
                if (selector.$pushExercise().val() == "") {
                    jqxNotification("请选择推送习题！", null, "error");
                    return false;
                }
            }
            if (selector.$isImportant().val() == "False") {
                if (selector.$effectiveDate().val() < selector.$currentTime().val()) {
                    jqxNotification("有效日期不能小于当前日期！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == "11" && selector.$revenueType().val() == "1") {
                if (!Validate(selector.$txtMoney())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "12") {
                if (!Validate(selector.$agreementType())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "13") {
                if (!Validate($("#RedpacketType"))) {
                    validateError++;
                }
                if (!Validate(selector.$txtWishing())) {
                    validateError++;
                }
                if ($("#RedpacketType").val() == "1" || $("#RedpacketType").val() == "2") {
                    if (!Validate($("#fixedRedpacket"))) {
                        validateError++;
                    }
                } else {
                    if (!Validate($("#txtRedpacketFrom"))) {
                        validateError++;
                    }
                    if (!Validate($("#txtRedpacketTo"))) {
                        validateError++;
                    }
                }
            }
            if (selector.$weChatMessageType().val() == "14") {
                if (!Validate(selector.$txtDesc())) {
                    validateError++;
                }
                if (!Validate(selector.$paymentMoney())) {
                    validateError++;
                }
            }
            if (validateError <= 0) {
                var titleError = 0;
                $("input[name=Title]").each(function () {
                    if ($(this).val() == "") {
                        titleError++;
                    }
                });
                if (titleError > 0) {
                    jqxNotification("标题不能为空", null, "error");
                    return false;
                }
                var parent = selector.$jqxTable().parent();
                selector.$jqxTable().jqxDataTable("destroy");
                $(parent).append('<div id="jqxTable" class="jqxTable" style="border-left: 0;"></div>');
                initTable_P();
                selector.$PreviewDialog().modal({ backdrop: 'static', keyboard: false });
                selector.$PreviewDialog().modal('show');
            }
        });
        //推送类型下拉框改变时动态加载推送内容
        selector.$pushType().change(function () {
            if (selector.$pushType().val() == "1")//微信推送
            {
                selector.$uEditor().removeAttr("style");
                selector.$textEdit().attr("style", "display:none");
                selector.$weChatTypeRow().removeAttr("style");
            }
            else //if (selector.$pushType().val() == "2")//短信推送
            {
                selector.$uEditor().attr("style", "display:none");
                selector.$textEdit().removeAttr("style");
                selector.$weChatTypeRow().attr("style", "display:none");
                selector.$exerciseList().attr("style", "display:none");
                $("#pushContentText").removeAttr("style");
            }
        });

        //选择推送的习题将有效日期带出来
        selector.$pushExercise().change(function () {
            $.ajax({
                url: "/WeChatPush/DraftList/GetExerciseEffectiveTime",
                data: { vguid: selector.$pushExercise().val() },
                type: "post",
                success: function (data) {
                    selector.$effectiveDate().val(data);
                }
            });
        });

        //是否定时发送按钮改变事件
        selector.$isSendTime().change(function () {
            if (selector.$isSendTime().val() == "True") {
                selector.$sendTime().removeAttr("style");
                selector.$sendTime().removeAttr("disabled");
            }
            else {
                selector.$sendTime().attr("style", "background-color:#f5f5f5!important");
                selector.$sendTime().attr("disabled", "disabled");
            }
        });
        //是否定时发送按钮改变事件
        selector.$isSendTime_sa().change(function () {
            if (selector.$isSendTime_sa().val() == "True") {
                selector.$sendTime_sa().removeAttr("style");
                selector.$sendTime_sa().removeAttr("disabled");
            }
            else {
                selector.$sendTime_sa().attr("style", "background-color:#f5f5f5!important");
                selector.$sendTime_sa().attr("disabled", "disabled");
            }
        });
        //是否永久按钮改变事件
        selector.$isImportant().change(function () {
            if (selector.$isImportant().val() == "False") {
                selector.$effectiveDate().removeAttr("style");
                selector.$effectiveDate().removeAttr("disabled");
                var time = selector.$defaultEffectiveDate().val();
                selector.$effectiveDate().val(time);
            }
            else {
                selector.$effectiveDate().val("");
                selector.$effectiveDate().attr("style", "background-color:#f5f5f5!important");
                selector.$effectiveDate().attr("disabled", "disabled");
            }
        });
        var time = selector.$defaultEffectiveDate().val();
        selector.$effectiveDate_sa().val(time);
        //是否永久按钮改变事件
        selector.$isImportant_sa().change(function () {
            if (selector.$isImportant_sa().val() == "False") {
                selector.$effectiveDate_sa().removeAttr("style");
                selector.$effectiveDate_sa().removeAttr("disabled");
                var time = selector.$defaultEffectiveDate().val();
                selector.$effectiveDate_sa().val(time);
            }
            else {
                selector.$effectiveDate_sa().val("");
                selector.$effectiveDate_sa().attr("style", "background-color:#f5f5f5!important");
                selector.$effectiveDate_sa().attr("disabled", "disabled");
            }
        });

        //微信推送类型下拉框改变事件
        selector.$weChatMessageType().change(function () {
            selector.$effectiveDate().val(getLastMonthYestdy());
            $("#trLabel").hide();
            $(".personLabel1").show();
            $(".knowledgeName").show();
            selector.$countDown().hide();
            $(".mod-sender__slider").hide();
            $(".tdAgreementType").hide();
            $(".redPacket").hide();
            $(".payment").hide();
            switch (selector.$weChatMessageType().val()) {
                case "1"://文本推送
                    selector.$weChatCoverMsg().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$textEdit().removeAttr("style");
                    selector.$exerciseList().attr("style", "display:none");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").removeAttr("style");
                    selector.$pushHistory().removeAttr("style");
                    $(".personLabel").removeAttr("style");
                    $(".revenueType").attr("style", "display:none");
                    selector.$tdMoney().hide();
                    break;
                case "2"://图片推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    //selector.$uEditor().removeAttr("style");
                    selector.$uEditor().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exerciseList().attr("style", "display:none");
                    elector.$questionList().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    //$("#pushContentText").removeAttr("style");
                    $("#edui1").css("width", "870px");
                    selector.$tdMoney().hide();
                    break;
                case "3"://图文推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$textEdit().attr("disabled", "disabled");
                    selector.$uEditor().removeAttr("style");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $("#pushContentText").removeAttr("style");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$upLoadFileTD().removeAttr("style");
                    $(".revenueType").attr("style", "display:none");
                    $("#edui1").css("width", "870px");
                    selector.$tdMoney().hide();
                    $(".mod-sender__slider").show();
                    break;
                case "4"://习题推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exercisePushRow().removeAttr("style");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    selector.$tdMoney().hide();
                    break;
                case "5"://培训推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().removeAttr("style");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").removeAttr("style");
                    selector.$exercisePushRow().removeAttr("style");
                    selector.$upLoadFileTD().removeAttr("style");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    $("#edui1").css("width", "870px");
                    selector.$tdMoney().hide();
                    $(".personLabel1").hide();
                    $("#trLabel").show();
                    selector.$countDown().show();
                    break;
                case "6": //知识库推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exercisePushRow().removeAttr("style");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    selector.$tdMoney().hide();
                    break;
                case "11": //营收消息推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exercisePushRow().removeAttr("style");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $(".revenueType").removeAttr("style");
                    selector.$effectiveDate().val(getNextHourTime());
                    if (selector.$revenueType().val() == "1") {
                        selector.$tdMoney().show();
                        $(".personLabel1").hide();
                        $("#trLabel").show();
                    } else {
                        selector.$tdMoney().hide();
                        $(".personLabel1").show();
                        $("#trLabel").hide();
                    }
                    break;
                case "12":
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$textEdit().attr("disabled", "disabled");
                    selector.$uEditor().removeAttr("style");
                    selector.$exerciseList().removeAttr("style");
                    selector.$questionList().attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $("#pushContentText").removeAttr("style");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$upLoadFileTD().removeAttr("style");
                    $(".revenueType").attr("style", "display:none");
                    $("#edui1").css("width", "870px");
                    selector.$tdMoney().hide();
                    $(".tdAgreementType").show();
                    break;
                case "13":
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$exerciseList().attr("style", "display:none");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    $(".personLabel1").hide();
                    $(".redPacket").show();
                    $(".fixedRedpacket").show();
                    $(".randomRedpacket").hide();
                    if ($("#RedpacketType").val() == "3") { //
                        $(".fixedRedpacket").hide();
                        $(".randomRedpacket").show();
                        $("#trLabel").show();
                        $(".countDown").hide();
                    } else {
                        $(".fixedRedpacket").show();
                        $(".randomRedpacket").hide();
                        $("#trLabel").hide();
                        $(".countDown").show();
                    }
                    $("#fixedRedpacket").attr("name", "RedpacketMoney");
                    break;
                case "14":
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$exerciseList().attr("style", "display:none");
                    selector.$questionList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    selector.$exercisePushRow().attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    $(".personLabel1").hide();
                    $(".redPacket").hide();
                    $(".fixedRedpacket").hide();
                    $(".randomRedpacket").hide();
                    $(".payment").show();
                    $("#fixedRedpacket").removeAttr("name");
                    break;
                case "16"://问卷推送
                    selector.$weChatCoverMsg().removeAttr("style");
                    selector.$textEdit().attr("style", "display:none");
                    selector.$uEditor().attr("style", "display:none");
                    selector.$questionList().removeAttr("style");
                    selector.$exerciseList().attr("style", "display:none");
                    $("#pushContentText").attr("style", "display:none");
                    selector.$questionPushRow().removeAttr("style");
                    selector.$upLoadFileTD().attr("style", "display:none");
                    selector.$pushHistory().attr("style", "display:none");
                    $(".personLabel").attr("style", "display:none");
                    $(".revenueType").attr("style", "display:none");
                    selector.$tdMoney().hide();
                    break;

            }
        });

        $("#RedpacketType").on("change", function () {
            switch ($("#RedpacketType").val()) {
                case "3":
                    $(".fixedRedpacket").hide();
                    $(".randomRedpacket").show();
                    $("#trLabel").show();
                    $(".countDown").hide();
                    break;
                case "1":
                case "2":
                    $(".fixedRedpacket").show();
                    $(".randomRedpacket").hide();
                    $("#trLabel").hide();
                    $(".countDown").show();
                    break;
            }
        });
        //获取下一天的时间
        function getNextHourTime() {
            var d = new Date();
            d.setTime(d.getTime() + 24 * 60 * 60 * 1000);
            var vMon = d.getMonth() + 1;
            var vDay = d.getDate();
            var hours = d.getHours();
            var mins = d.getMinutes();
            var secs = d.getSeconds();
            var s = d.getFullYear() + "-" + (vMon < 10 ? "0" + vMon : vMon) + "-" + (vDay < 10 ? "0" + vDay : vDay) + " " + (hours < 10 ? "0" + hours : hours) + ":" + (mins < 10 ? "0" + mins : mins) + ":" + (secs < 10 ? "0" + secs : secs);
            return s;
        }

        //获取下个月的时间
        function getLastMonthYestdy() {
            var date = new Date();
            var daysInMonth = new Array([0], [31], [28], [31], [30], [31], [30], [31], [31], [30], [31], [30], [31]);
            var strYear = date.getFullYear();
            var strDay = date.getDate();
            var strMonth = date.getMonth() + 1;
            if (strYear % 4 == 0 && strYear % 100 != 0) {
                daysInMonth[2] = 29;
            }
            if (strMonth + 1 == 13) {
                strYear += 1;
                strMonth = 1;
            }
            else {
                strMonth += 1;
            }
            strDay = daysInMonth[strMonth] >= strDay ? strDay : daysInMonth[strMonth];
            if (strMonth < 10) {
                strMonth = "0" + strMonth;
            }
            if (strDay < 10) {
                strDay = "0" + strDay;
            }
            var hours = date.getHours();
            var mins = date.getMinutes();
            var secs = date.getSeconds();
            datastr = strYear + "-" + strMonth + "-" + strDay + " " + (hours < 10 ? "0" + hours : hours) + ":" + (mins < 10 ? "0" + mins : mins) + ":" + (secs < 10 ? "0" + secs : secs);
            return datastr;
        }
        //营收金额下拉框改变事件
        selector.$revenueType().on('change', function () {
            if (selector.$weChatMessageType().val() == "11") {
                switch (selector.$revenueType().val()) {
                    case "1":
                        selector.$tdMoney().show();
                        $(".personLabel1").hide();
                        $("#trLabel").show();
                        break;
                    case "2":
                        selector.$tdMoney().hide();
                        $(".personLabel1").show();
                        $("#trLabel").hide();
                        break;
                }
            }
        });
        //封面图片临时保存地址
        selector.$coverImg_input().on("change", function (event) {

            selector.$pushForm().ajaxSubmit({
                url: "/WeChatPush/DraftList/UploadImg?id=0",
                type: "post",
                dataType: "json",
                traditional: true,
                success: function (data) {
                    $("#coverImgPath").val(data.WebPath);
                    var imgObj = '<div class="video_image_wrap"><img style="width:100%;vertical-align: middle;display:inline-block;" src="' + data.WebPath + '"></div>';
                    $(".image_edit_placeholder").children().remove();
                    $(".image_edit_placeholder").append(imgObj);
                }
            });
        });

        //封面图片临时保存地址
        selector.$coverImg_input_sa().on("change", function () {
            var formData = new FormData();//初始化一个FormData对象
            formData.append("files", selector.$coverImg_input_sa()[0].files[0]);//将文件塞入FormData
            $.ajax({
                url: "/WeChatPush/DraftList/UploadSalaryImg",
                type: "post",
                dataType: "json",
                data: formData,
                processData: false,  // 告诉jQuery不要去处理发送的数据
                contentType: false,   // 告诉jQuery不要去设置Content-Type请求头
                success: function (msg) {
                    if (msg.isSuccess) {
                        selector.$coverImgPath_sa().val(msg.respnseInfo);
                    }
                }
            });
        });
        //上传附件转换成html并展示在Ueditor
        selector.$importFile().on('change', function (event) {
            if (event.target.value == "") {
                return false;
            }
            showLoading();//显示加载等待框    
            var um = UE.getEditor('myEditor');
            selector.$pushForm().ajaxSubmit({
                url: "/WeChatPush/DraftList/ConvertToHtml?id=0",
                type: "post",
                traditional: true,
                success: function (data) {

                    if (data == "") {
                        um.setContent("");
                    } else {
                        um.execCommand('inserthtml', data);
                    }
                    closeLoading();//关闭加载等待框  
                    selector.$importFile()[0].value = "";
                }

            });

        });
        //初始化标签下拉框
        function initDropdownList() {
            var source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'LabelName' },
                     //   { name: 'VGUID' }
                    ],
                    id: 'LabelName',
                    url: '/WeChatPush/DraftList/GetLabels',
                    //async: false,
                };
            var dataAdapter = new $.jqx.dataAdapter(source);
            selector.$jqxWidget().jqxDropDownList({ checkboxes: true, source: dataAdapter, placeHolder: "===请选择===", displayMember: "LabelName", valueMember: "LabelName", width: 210, height: 35 });
            selector.$jqxWidget().jqxDropDownList('checkIndex', -1);

        }

        //保存按钮事件
        selector.$btnSave().on('click', function () {
            selector.$pushType().removeAttr("disabled");
            var txtMsg = "";

            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "1")//文本推送
                {
                    txtMsg = selector.$pushContent().val();
                }
                else if (selector.$weChatMessageType().val() == "3")//图文推送
                {
                    //  var um = UE.getEditor('myEditor');
                    //  txtMsg = um.getContent(); //editor1.txt.html();
                }
                else if (selector.$weChatMessageType().val() == "4")//习题推送
                {
                    txtMsg = selector.$pushExercise().val();
                }
                else if (selector.$weChatMessageType().val() == "5")//培训推送
                {
                    $(".revenueType").remove();
                    var um = UE.getEditor('myEditor');
                    txtMsg = um.getContent();
                }
                else if (selector.$weChatMessageType().val() == "6") //知识库推送
                {
                    txtMsg = "";
                }
                else if (selector.$weChatMessageType().val() == "11") { //营收消息推送
                    switch (selector.$revenueType().val()) {
                        case "1":
                            txtMsg = parseFloat(selector.$txtMoney().val());
                            break;
                        case "2":
                            txtMsg = "";
                            break;
                    }

                } else if (selector.$weChatMessageType().val() == "13") {
                    txtMsg = selector.$txtWishing().val();
                } else if (selector.$weChatMessageType().val() == "14") {
                    txtMsg = selector.$txtDesc().val();
                } else if (selector.$weChatMessageType().val() == "16") {//问卷推送
                    txtMsg = selector.$pushQuestion().val();
                } else {
                    var um = UE.getEditor('myEditor');
                    txtMsg = um.getContent();
                }
            }
            else if (selector.$pushType().val() == "2")//短信推送
            {
                txtMsg = selector.$pushContent().val();
            }
            var validateError = 0;//未通过验证的数量
            if (!Validate(selector.$txtTitle())) {
                validateError++;
            }
            if (selector.$pushPeople().val().length <= 0 && $("#txtLabel").val().length <= 0) {
                // jqxNotification("请选择微信推送接收者！", null, "error");
                jqxNotification("微信推送接收者和人员标签至少选择一个！", null, "error");
                return false;
            }
            if (selector.$pushType().val() == "1")//微信推送
            {
                if (selector.$weChatMessageType().val() == "") {
                    jqxNotification("请选择微信推送类型！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == 4 || selector.$weChatMessageType().val() == 5) {
                if (selector.$pushExercise().val() == "") {
                    jqxNotification("请选择推送习题！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == 16) {
                if (selector.$pushQuestion().val() == "") {
                    jqxNotification("请选择推送问卷！", null, "error");
                    return false;
                }
            }
            if (selector.$isImportant().val() == "False") {
                if (selector.$effectiveDate().val() < selector.$currentTime().val()) {
                    jqxNotification("有效日期不能小于当前日期！", null, "error");
                    return false;
                }
            }
            if (selector.$weChatMessageType().val() == "11" && selector.$revenueType().val() == "1") {
                if (!Validate(selector.$txtMoney())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "12") {
                if (!Validate(selector.$agreementType())) {
                    validateError++;
                }
            }
            if (selector.$weChatMessageType().val() == "13") {
                if (!Validate($("#RedpacketType"))) {
                    validateError++;
                }
                if (!Validate(selector.$txtWishing())) {
                    validateError++;
                }
                if ($("#RedpacketType").val() == "1" || $("#RedpacketType").val() == "2") {
                    if (!Validate($("#fixedRedpacket"))) {
                        validateError++;
                    }
                } else {
                    if (!Validate($("#txtRedpacketFrom"))) {
                        validateError++;
                    }
                    if (!Validate($("#txtRedpacketTo"))) {
                        validateError++;
                    }
                }
            }
            if (selector.$weChatMessageType().val() == "14") {
                if (!Validate(selector.$txtDesc())) {
                    validateError++;
                }
                if (!Validate(selector.$paymentMoney())) {
                    validateError++;
                }
            }
            if (validateError <= 0) {

                var titleError = 0;
                $("input[name=Title]").each(function () {
                    if ($(this).val() == "") {
                        titleError++;
                    }
                });
                if (titleError > 0) {
                    jqxNotification("标题不能为空", null, "error");
                    return false;
                }
                if (selector.$weChatMessageType().val() == "3") {  //图文推送
                    txtMsg = UE.getEditor('myEditor').getContent();
                    var wechatPushList = [];
                    var wechatPushMoreGraphic = [];
                    var tabType = [];
                    var wechatPushModel = new WeChatPush_Temp(selector.$pushType().val(), selector.$txtTitle().val(), 3, selector.$isSendTime().val(), selector.$sendTime().val(), selector.$isImportant().val(), txtMsg, selector.$effectiveDate().val(), $("#coverImgPath").val(), $("#txtCoverDescption").val(), selector.$chkHistory().is(":checked"), $("#pushObject").val(), $("#txtLabel").val(), selector.$vguid().val());
                    wechatPushList.push(wechatPushModel);
                    $("#pushForm table").each(function () {
                        if ($(this).attr("tabType")) {
                            tabType.push($(this).attr("tabType"));
                        }
                    });
                    tabType = tabType.sort();
                    $("#pushForm table").each(function () {
                        if ($(this).attr("tabType")) {
                            var weChatPushMoreGraphicModel = new weChatPushMoreGraphic($("#txtTitle" + $(this).attr("tabType")).val(), UE.getEditor('myEditor' + $(this).attr("tabType")).getContent(), $("#coverImgPath" + $(this).attr("tabType")).val(), $("#txtCoverDescption" + $(this).attr("tabType")).val(), parseInt($.inArray($(this).attr("tabType"), tabType)) + 1);
                            wechatPushMoreGraphic.push(weChatPushMoreGraphicModel);
                        }
                    });
                    showLoading();
                    $.ajax({
                        url: "/WeChatPush/DraftList/SaveImagePushMsg",
                        type: "post",
                        data: { wechatPushList: JSON.stringify(wechatPushList), wechatPushMoreGraphicList: JSON.stringify(wechatPushMoreGraphic), isEdit: selector.$isEdit().val(), saveType: "1", countersignType: "" },
                        dataType: "json",
                        success: function (msg) {
                            switch (msg.respnseInfo) {
                                case "0":
                                    jqxNotification("保存失败！", null, "error");
                                    closeLoading();
                                    break;
                                case "1":
                                    jqxNotification("保存成功！", null, "success");
                                    closeLoading();
                                    window.location.href = "/WeChatPush/DraftList/DraftList";
                            }
                        }
                    });
                } else {
                    showLoading();
                    selector.$pushForm().ajaxSubmit({
                        url: '/WeChatPush/DraftList/SavePushMsg',
                        type: "post",
                        data: { isEdit: selector.$isEdit().val(), txtMessage: txtMsg, history: selector.$chkHistory().is(":checked"), saveType: "1" },
                        success: function (msg) {
                            switch (msg.respnseInfo) {
                                case "0":
                                    jqxNotification("保存失败！", null, "error");
                                    closeLoading();
                                    break;
                                case "1":
                                    jqxNotification("保存成功！", null, "success");
                                    closeLoading();
                                    window.location.href = "/WeChatPush/DraftList/DraftList";

                            }
                        }
                    });
                }

            }
        });

        //初始化表格
        function initTable_P() {
            var source =
           {
               datafields:
               [
                   { name: "checkbox", type: null },
                   { name: 'name', type: 'string' },
                   { name: 'UserID', type: 'string' },
                //   { name: 'OwnedFleet', type: 'string' },
                   { name: 'TranslationOwnedFleet', type: 'string' },
                  // { name: 'ApprovalStatus', type: 'string' },
                   { name: 'vguid', type: 'string' }
               ],
               datatype: "json",
               id: "vguid",//主键
               async: true,
               data: { 'TranslationApprovalStatus': "已关注", "name": selector.$txtName_P().val(), "PhoneNumber": selector.$txtMobilePhone_P().val() },
               url: "/BasicDataManagement/UserInfo/GetUserListBySearch"  //获取数据源的路径
           };
            var typeAdapter = new $.jqx.dataAdapter(source, {
                downloadComplete: function (data) {
                    source.totalrecords = data.TotalRows;
                }
            });
            //创建卡信息列表（主表）
            selector.$jqxTable().jqxDataTable(
                {
                    pageable: true,
                    width: 800,
                    height: 450,
                    pageSize: 1000,
                    serverProcessing: true,
                    pagerButtonsCount: 10,
                    source: typeAdapter,
                    theme: "office",
                    columns: [
                      { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_P, renderer: rendererFunc_P, rendered: renderedFunc_P, autoRowHeight: false },
                      { text: '人员姓名', width: 150, datafield: 'name', align: 'center', cellsAlign: 'center' },
                      { text: '微信UserID', width: 150, datafield: 'UserID', align: 'center', cellsAlign: 'center' },
                      { text: '部门', datafield: 'TranslationOwnedFleet', align: 'center', cellsAlign: 'center' },
                      { text: 'VGUID', datafield: 'vguid', hidden: true }
                    ]
                });
        }

        function cellsRendererFunc_P(row, column, value, rowData) {
            var container = "";
            container = "<input class=\"jqx_datatable_checkbox\"   onchange=edit_P(this)  index=\"" + row + "\" type=\"checkbox\"  style=\"margin:auto;width: 17px;height: 17px;\" />";
            return container;
        }

        function rendererFunc_P() {
            //var checkBox = "<div id='jqx_datatable_checkbox_all' class='jqx_datatable_checkbox_all' style='z-index: 999; margin-left:7px ;margin-top: 7px;'>";
            //checkBox += "</div>";
            //return checkBox;
        }

        function renderedFunc_P(element) {
            //var grid = selector.$jqxTable();
            //element.jqxCheckBox();
            //element.on('change', function (event) {
            //    var checked = element.jqxCheckBox('checked');
            //    if (checked) {
            //        var rows = grid.jqxDataTable('getRows');
            //        for (var i = 0; i < rows.length; i++) {
            //            grid.find(".jqx_datatable_checkbox").eq(i).prop("checked", "checked");
            //        }
            //    } else {
            //        grid.jqxDataTable('clearSelection');
            //        grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked");
            //    }
            //});
            //return true;
        }

        //初始化表格
        function initTable() {
            var items = selector.$jqxWidget().jqxDropDownList('getCheckedItems');
            var checkedItems = "";
            $.each(items, function (index) {
                checkedItems += this.label + ",";
            });
            checkedItems = checkedItems.substr(0, checkedItems.length - 1);
            var source =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'name', type: 'string' },
                    { name: 'UserID', type: 'string' },
                 //   { name: 'OwnedFleet', type: 'string' },
                    { name: 'TranslationOwnedFleet', type: 'string' },
                   // { name: 'ApprovalStatus', type: 'string' },
                    { name: 'vguid', type: 'string' }
                ],
                datatype: "json",
                id: "vguid",//主键
                async: true,
                data: { 'TranslationApprovalStatus': "已关注", "name": selector.$name_Search().val(), "OwnedFleet": selector.$OwnedFleet().val(), "PhoneNumber": selector.$MobilePhone_Search().val(), "LabelName": checkedItems },
                url: "/BasicDataManagement/UserInfo/GetUserListBySearch"  //获取数据源的路径
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
                    width: 800,
                    height: 450,
                    pageSize: 1000,
                    serverProcessing: true,
                    pagerButtonsCount: 10,
                    source: typeAdapter,
                    theme: "office",
                    columns: [
                      { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                      { text: '人员姓名', width: 150, datafield: 'name', align: 'center', cellsAlign: 'center' },
                      { text: '微信UserID', width: 150, datafield: 'UserID', align: 'center', cellsAlign: 'center' },
                      { text: '部门', datafield: 'TranslationOwnedFleet', align: 'center', cellsAlign: 'center' },
                      { text: 'VGUID', datafield: 'vguid', hidden: true }
                    ]
                });

        }
        function cellsRendererFunc(row, column, value, rowData) {
            var container = "";
            container = "<input class=\"jqx_datatable_checkbox\"  index=\"" + row + "\" type=\"checkbox\"  onchange=edit(this) style=\"margin:auto;width: 17px;height: 17px;\" />";
            for (var k = 0; k < pushObject.length; k++) {
                if (pushObject[k] == rowData.UserID) {
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
            var grid = selector.$grid();
            element.jqxCheckBox();
            element.on('change', function (event) {
                var checked = element.jqxCheckBox('checked');
                if (checked) {
                    var rows = grid.jqxDataTable('getRows');
                    for (var i = 0; i < rows.length; i++) {
                        grid.find(".jqx_datatable_checkbox").eq(i).prop("checked", "checked");
                        grid.find(".jqx_datatable_checkbox").eq(i).trigger('change');
                    }
                } else {
                    grid.jqxDataTable('clearSelection');
                    grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked");
                    grid.find(".jqx_datatable_checkbox").trigger('change');
                }
            });
            return true;
        }
        //修改下拉框的样式
        $("#jqxWidget .jqx-icon").css('left', '10%');
        $("#jqxWidget").children('div').css('background-color', 'white');
        //导入推送失败弹出框
        selector.$btnCancel_Not().on("click", function () {
            $.ajax({
                url: "/WeChatPush/DraftList/DropNotExistPersonTable",
                type: "post",
                success: function () {
                    selector.$notExistDialog().modal('hide');
                    CookieHelper.SaveCookie('CheckedList');
                    window.location.href = "/WeChatPush/CheckedList/CheckedList";
                }
            });

        });
        //点击下载按钮
        selector.$btnDownLoad().on('click', function () {
            var eventType = selector.$btnSave_sa().attr('event-data');
            var fileName = "notExist.xlsx";
            if (eventType === "1") {
                fileName = "notExist.xlsx";
            } else {
                fileName = "notMaintanceExist.xlsx";
            }
            window.location.href = "/WeChatPush/DraftList/DownNotExistPeople?fileName=" + fileName;
        });
        //点击上传工资条按钮
        selector.$btnUploadSalary().on('click', function () {
            //先清空文本框内容
            resetSalaryAndMaintence();
            //设置当前点击的是工资条还是车辆保养事件标识,1:代表工资条，2：代表车辆保养
            selector.$btnSave_sa().attr('event-data', "1");
            //设置title
            $("#SalaryDialog .modal-title").text("工资条推送");
            // $("#importSalary").click();
        });
        //点击上传车辆保养
        selector.$btnUploadMaintenance().on('click', function () {
            //先清空文本框内容
            resetSalaryAndMaintence();
            //设置当前点击的是工资条还是车辆保养事件标识,1:代表工资条，2：代表车辆保养
            selector.$btnSave_sa().attr('event-data', "2");
            //设置title
            $("#SalaryDialog .modal-title").text("保养信息推送");
        })

        //上传工资条弹出框中的取消按钮
        selector.$btnCancel_sa().on('click', function () {
            selector.$SalaryDialog().modal('hide');
        });

        //点击上传工资条弹出框中的保存按钮
        selector.$btnSave_sa().on("click", function () {
            var eventType = selector.$btnSave_sa().attr('event-data');
            if (eventType === "1") {
                saveSalaryDialog();
            } else {
                saveMaintenanceDialog();
            }
        });
    }; //addEvent end
};

//微信多图文推送主表临时对象模型
var WeChatPush_Temp = function (pushType, title, messageType, timed, timedSendTime, important, message, periodOfValidity, coverimg, coverDescption, history, pushObject, pushLabel, vguid) {
    this.PushType = pushType;
    this.Title = title;
    this.MessageType = messageType;
    this.Timed = timed;
    this.TimedSendTime = timedSendTime;
    this.Important = important;
    this.Message = message;
    this.PeriodOfValidity = periodOfValidity;
    this.CoverImg = coverimg;
    this.CoverDescption = coverDescption;
    this.History = history;
    this.PushObject = pushObject;
    this.Label = pushLabel;
    this.VGUID = vguid;
}
//微信多图文推送子表临时对象模型
var weChatPushMoreGraphic = function (title, message, coverimg, coverDescption, ranks) {
    this.Title = title;
    this.Message = message;
    this.CoverImg = coverimg;
    this.CoverDescption = coverDescption;
    this.Ranks = ranks;
}

//提交
function submit() {
    showLoading();
    if (selector.$weChatMessageType().val() == "3") {   //图文推送
        txtMsg = UE.getEditor('myEditor').getContent();
        var wechatPushList = [];
        var wechatPushMoreGraphic = [];
        var tabType = [];
        var wechatPushModel = new WeChatPush_Temp(selector.$pushType().val(), selector.$txtTitle().val(), 3, selector.$isSendTime().val(), selector.$sendTime().val(), selector.$isImportant().val(), txtMsg, selector.$effectiveDate().val(), $("#coverImgPath").val(), $("#txtCoverDescption").val(), selector.$chkHistory().is(":checked"), $("#pushObject").val(), $("#txtLabel").val(), selector.$vguid().val());
        wechatPushList.push(wechatPushModel);
        $("#pushForm table").each(function () {
            if ($(this).attr("tabType")) {
                tabType.push($(this).attr("tabType"));
            }
        });
        tabType = tabType.sort();
        $("#pushForm table").each(function () {
            if ($(this).attr("tabType")) {
                var weChatPushMoreGraphicModel = new weChatPushMoreGraphic($("#txtTitle" + $(this).attr("tabType")).val(), UE.getEditor('myEditor' + $(this).attr("tabType")).getContent(), $("#coverImgPath" + $(this).attr("tabType")).val(), $("#txtCoverDescption" + $(this).attr("tabType")).val(), parseInt($.inArray($(this).attr("tabType"), tabType)) + 1);
                wechatPushMoreGraphic.push(weChatPushMoreGraphicModel);
            }
        });
        $.ajax({
            url: "/WeChatPush/DraftList/SaveImagePushMsg",
            type: "post",
            data: { wechatPushList: JSON.stringify(wechatPushList), wechatPushMoreGraphicList: JSON.stringify(wechatPushMoreGraphic), isEdit: selector.$isEdit().val(), saveType: "2", countersignType: "" },
            dataType: "json",
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "0":
                        jqxNotification("提交失败！", null, "error");
                        closeLoading();
                        break;
                    case "1":
                        jqxNotification("提交成功！", null, "success");
                        closeLoading();
                        window.location.href = "/WeChatPush/DraftList/DraftList";
                }
            }
        });
    } else {
        selector.$pushForm().ajaxSubmit({
            url: '/WeChatPush/DraftList/SavePushMsg',
            type: "post",
            data: { isEdit: selector.$isEdit().val(), txtMessage: txtMsgContent, history: selector.$chkHistory().is(":checked"), saveType: "2" },
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "0":
                        jqxNotification("提交失败！", null, "error");
                        break;
                    case "1":
                        jqxNotification("提交成功！", null, "success");
                        closeLoading();
                        window.location.href = "/WeChatPush/DraftList/DraftList";
                }
            }
        });
    }
}

//文件上传
function fileUpload() {
    showLoading();
    selector.$importForm().ajaxSubmit({
        url: "/WeChatPush/DraftList/UpLoadPushObject?pushFile=himport",
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg) {
                //case "0":
                //    jqxNotification("导入失败！", null, "error");
                //    break;
                //case "1":
                //    jqxNotification("导入成功！", null, "success");
                //    selector.$grid().jqxDataTable('updateBoundData');
                //    break;
                case "2":
                    jqxNotification("表格为空！", null, "error");
                    break;
                case "3":
                    jqxNotification("模板错误！", null, "error");
                    break;
                default:
                    if (msg.userId == "") {
                        //jqxNotification("导入失败！", null, "error");
                        jqxNotification(msg.exp, null, "error");
                    } else {
                        jqxNotification("导入成功！", null, "success");
                        selector.$SelectDialog().modal('hide');
                        selector.$pushPeople().val(msg.userName);
                        pushObject = msg.userId.split('|');
                    }
                    //  jqxNotification(msg.respnseInfo, null, "error");
                    break;
            }
            $("#himport").remove();
            var inputObj = "<input type=\"file\" name=\"himport\" id=\"himport\" onchange=\"fileUpload()\" accept=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel\"/>";
            $("#dvImport").after(inputObj);
            closeLoading();
            selector.$SelectDialog().modal('hide');
        }
    });
}
//checkbox改变事件
function edit(obj) {
    var userName = $(obj).parent().next().html();
    var userId = $(obj).parent().next().next().html();
    if ($(obj).is(":checked")) {
        if ($.inArray(userName, people) == -1) {
            people.push(userName);
            pushObject.push(userId);
        }
    } else {
        var index = $.inArray(userName, people);
        var index1 = $.inArray(userId, pushObject);
        if (index > -1) {
            people.splice(index, 1);
        }
        if (index1 > -1) {
            pushObject.splice(index, 1);
        }
    }

}
//checkbox实现单选
function edit_P(obj) {
    $("#jqxTable .jqx_datatable_checkbox").each(function () {
        $(this).removeAttr("checked", "checked");
    });
    $(obj).prop("checked", "checked");
}
//上传导入推送文件
function Upload() {
    showLoading();
    selector.$uploadPushForm().ajaxSubmit({
        url: "/WeChatPush/DraftList/UpLoadPush?pushFile=import",
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg.ResponseInfo) {
                case "0":
                    jqxNotification("导入失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("导入成功！", null, "success");
                    var tips = msg.ReturnMsg;
                    selector.$tips().text(tips);
                    initNotExistTable();
                    selector.$notExistDialog().modal({ backdrop: 'static', keyboard: false });
                    selector.$notExistDialog().modal('show');
                    //  CookieHelper.SaveCookie('CheckedList');
                    // window.location.href = "/WeChatPush/CheckedList/CheckedList";
                    break;
                default:
                    jqxNotification(msg.ResponseInfo, null, "error");
                    break;
            }
            //$("#import").remove();
            //var inputObj = "<input type=\"file\" name=\"import\" id=\"import\" onchange=\"Upload()\" accept=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel\" style=\"display:none\"/>";
            //$("#btnUpload").after(inputObj);
            $("#import").val("");
            closeLoading();
        }
    });
}
//上传工资条
function UploadSalary(IDNumber) {
    showLoading();
    var formData = new FormData();//初始化一个FormData对象
    formData.append("files", $("#importSalary")[0].files[0]);//将文件塞入FormData
    var eventType = selector.$btnSave_sa().attr('event-data');
    var url = "";
    if (eventType === "1") {
        url = "/WeChatPush/DraftList/UpLoadSalary";
    } else {
        url = "/WeChatPush/DraftList/UpLoadMaintence";
    }
    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        data: formData,
        processData: false,  // 告诉jQuery不要去处理发送的数据
        contentType: false,   // 告诉jQuery不要去设置Content-Type请求头
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "1":
                    jqxNotification("导入成功！", null, "success");
                    $("#txtSalalryPath").val("1");
                    $("#uploadResult").text("上传成功！");
                    break;
                default:
                    jqxNotification(msg.respnseInfo, null, "error");
                    break;
            }
            $("#importSalary").val("");
            closeLoading();
        }
    });
}

//初始化导入推送中不存在的人员信息
function initNotExistTable(IDNumber) {
    IDNumber = IDNumber || "身份证号";
    var source =
         {
             datafields:
             [
                 { name: 'Name', type: 'string' },
                 { name: 'MobilePhone', type: 'string' },
                 { name: 'IDNumber', type: 'string' },
             ],
             datatype: "json",
             //id: "IDNumber",//主键
             async: false,
             url: "/WeChatPush/DraftList/GetNotExistPeople"  //获取数据源的路径
         };
    var typeAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (data) {
            source.totalrecords = data.TotalRows;
        }
    });
    //创建卡信息列表（主表）
    selector.$notExistTable().jqxDataTable(
        {
            pageable: true,
            width: 800,
            height: 450,
            pageSize: 10,
            serverProcessing: true,
            pagerButtonsCount: 10,
            source: typeAdapter,
            theme: "office",
            columns: [
              { text: '人员姓名', width: 150, datafield: 'Name', align: 'center', cellsAlign: 'center' },
              { text: '手机号', width: 150, datafield: 'MobilePhone', align: 'center', cellsAlign: 'center' },
              { text: '身份证号', datafield: 'IDNumber', align: 'center', cellsAlign: 'center' }
            ]
        });
}
var div_artical = 0;
//创建预览框
function onCreate(msg) {
    div_artical++;
    var title = msg == null ? "" : msg.Title;
    var coverImg = msg == null ? "" : msg.CoverImg;
    var coverDesc = msg == null ? "" : msg.CoverDescption;
    var message = msg == null ? "" : msg.Message;
    var div = '<div class="video_artical" index="' + div_artical + '"  onclick=onSelect(this)>' +
          '<div class="choose_mode_style"></div>' +
          '<p class="image_edit_placeholder_1"></p>' +
          '<p class="js_article_title video_artical_title">' + title + '</p>' +
          '<span class="video_artical_option"><a href="javascript:;" class="icon_message_delete" title="删除"  onclick=onDelete("' + div_artical + '")></span>' +
          '</div>';

    $(".video_artical_add").before(div);
    if ($(".video_artical").length == 7) {
        $(".video_artical_add").hide();
    }
    if (msg != null) {
        var imgObj = '<div class="video_image_wrap"><img style="width:80px;vertical-align: middle;display:inline-block;" src="' + coverImg + '"></div>';
        $(".video_artical[index=" + div_artical + "]").children("p[class=image_edit_placeholder_1]").append(imgObj);
    }
    $(".video_artical[index=" + div_artical + "]").click();
    var tableObj = '<table style="margin-top: 20px; margin-left: 20px;"  tabType="' + div_artical + '"  class="tableClass_' + div_artical + '">' +
                     '<tr style="height:50px">' +
                         '<td align="right" style="width: 90px;">标题：</td>' +
                         '<td colspan="5" style="width: 210px;">' +
                         '<input id="txtTitle' + div_artical + '" type="text" name="Title" validatetype="required" style="width: 100%!important" class="input_text form-control" onkeyup="keyUpFunc(' + div_artical + ')" value="' + title + '">' +
                         '</td>' +
                         '<td align="center" style="width: 10px;"><span style="color: red; margin-left: 5px;">*</span></td>' +
                    '</tr>' +
                   '<tr style="height:50px">' +
                     '<td align="right">推送方式：</td>' +
                     '<td style="width: 210px;">' +
                           ' <select id="pushType" name="PushType" validatetype="required" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                    '<option value="1" selected="selected">微信推送</option>' +
                                    '<option value="2">短信推送</option>' +
                            '</select>' +
                   ' </td>' +
                   '<td align="right" style="width: 105px;">是否定时发送：</td>' +
                   '<td style="width: 210px;">' +
                        '<select id="isSendTime" name="Timed" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                '<option value="True">是</option>' +
                                '<option value="False" selected="selected">否</option>' +
                        '</select>' +
                    '</td>' +
                    '<td align="right" style="width: 105px;">发送时间：</td>' +
                    '<td><input id="sendTime" name="TimedSendTime" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control" value="">  </td>' +
                     '<tr style="height:50px">' +
                     '<td align="right">推送接收者：</td>' +
                     '<td style="width: 240px;"><input type="text" id="pushPeople" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control"> </td>' +
                     '<td align="right">是否永久：</td>' +
                     '<td>' +
                        '<select id="isImportant" name="Important" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control">' +
                                '<option value="True">是</option>' +
                                '<option value="False" selected="selected">否</option>' +
                        '</select>' +
                    '</td>' +
                    '<td align="right">有效日期：</td>' +
                    '<td>' +
                    '<input id="effectiveDate" name="PeriodOfValidity" disabled="disabled" style="background-color:#f5f5f5!important;" class="input_text form-control" >' +
                    '</td>' +
                    '</tr>' +
                    '<tr id="weChatTypeRow">' +
                        '<td align="right">微信推送类型：</td>' +
                        '<td>' +
                            '<select id="weChatMessageType" name="MessageType" disabled=disabled style="background-color:#f5f5f5!important;"   class="input_text form-control">' +
                                '<option value="">===请选择===</option>' +
                                    '<option value="3" selected="selected">图文推送</option>' +
                            '</select>' +
                        '</td>' +
                        '<td class="weChatCoverMsg" align="right">封面图片：</td>' +
                        '<td class="weChatCoverMsg">' +
                            '<input type="file" id="coverImg_input' + div_artical + '" name="CoverImgs' + div_artical + '" class="input_text form-control" onchange=ImgUpload(' + div_artical + ') accept="image/gif,image/jpeg,image/jpg,image/png">' +
                            '<input type="hidden" id="coverImgPath' + div_artical + '" name="CoverImg" value="' + coverImg + '">' +
                        '</td>' +
                        '<td class="weChatCoverMsg" align="right">封面描述：</td>' +
                        '<td class="weChatCoverMsg">' +
                            '<input id="txtCoverDescption' + div_artical + '" type="text" name="CoverDescption" class="input_text form-control" value="' + coverDesc + '">' +
                       ' </td>' +
                    '</tr>' +
                    '<tr id="ExerciseList">' +
                    '<td align="right" class="upLoadFileTD">上传附件：<br> <span style="color: red;">(只能上传word)</span></td>' +
                    '<td class="upLoadFileTD">' +
                            '<input type="file" name="importFile' + div_artical + '" id="importFile' + div_artical + '"  onchange=UploadWord(' + div_artical + ')  class="input_text form-control" accept="application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/msword">' +
                        '</td>' +
                        '<td class="personLabel1" align="right">人员标签：</td>' +
                        '<td class="personLabel1">' +
                            '<input type="text" readonly="readonly" class="input_text form-control" id="txtLabel" name="Label" style="background-color: #f5f5f5!important;">' +
                        '</td>' +
                        '<td align="right">记录推送历史：</td>' +
                        '<td>' +
                            '<input id="chkHistory" type="checkbox" disabled="disabled"  style="width: 17px; height: 17px;background-color:#f5f5f5!important;">' +
                       ' </td>' +
                       '</tr>' +
                       '<tr style="height: 45px;">' +
                       '<td align="right" id="pushContentText">推送内容：</td>' +
                        '<td colspan="5"> <div class="UEditor" style="display: block;width: 870px;height:278px;">' +
                                '<textarea id="myEditor' + div_artical + '" name="Message" runat="server" onblur="setUeditor()">' + message +
                             '</textarea>' +
                            '</div>' +
                 '</td></tr>' +
               '</table>';
    $(".tableClass").hide();
    $(".tableClass").after(tableObj);
    $("#pushForm table").each(function () {
        if ($(this).attr("tabType")) {
            $(this).css("display", "none");
        }
    });
    $(".tableClass_" + div_artical).show();
    var um = UE.getEditor('myEditor' + div_artical);
    $("#myEditor" + div_artical).css("height", "200px");
    if (msg != null) {
        onSelect_M();
    }
}
//文本框同步
function keyUpFunc(id) {
    var title;
    if (id == 0) {
        title = $("#txtTitle").val();
        $(".video_sub_title").html(title);
    } else {
        title = $("#txtTitle" + id).val();
        $(".video_artical[index=" + id + "] .video_artical_title").html(title);
    }


}
//上传附件转换成html并展示在Ueditor
function UploadWord(id) {
    showLoading(); //显示加载等待框    
    var um = UE.getEditor('myEditor' + id);
    selector.$pushForm().ajaxSubmit({
        url: "/WeChatPush/DraftList/ConvertToHtml?id=" + id,
        type: "post",
        traditional: true,
        success: function (data) {
            if (data == "") {
                um.setContent("", true);
            } else {
                um.execCommand('inserthtml', data);
            }
            closeLoading(); //关闭加载等待框
            $("#importFile" + id)[0].value = "";
        }
    });
}

function ImgUpload(id) {
    selector.$pushForm().ajaxSubmit({
        url: "/WeChatPush/DraftList/UploadImg?id=" + id + "&random=" + Math.random(),
        type: "post",
        traditional: true,
        dataType: "json",
        success: function (data) {
            $("#coverImgPath" + id).val(data.WebPath);
            var imgObj = '<div class="video_image_wrap"><img style="width:80px;height:80px;vertical-align: middle;display:inline-block;" src="' + data.WebPath + '"></div>';
            $(".video_artical[index=" + id + "]").children("p[class=image_edit_placeholder_1]").append(imgObj);
        }
    });
}
//删除一个预览框
function onDelete(flag) {
    window.event ? window.event.cancelBubble = true : e.stopPropagation();  //阻止冒泡
    var divArray = [];
    if ($(".video_artical").length == 1) {
        $(".video_artical[index=" + flag + "]").remove();
        $(".tableClass_" + flag).remove();
        $(".tableClass").css("display", "block");
    } else {
        $(".video_artical").each(function () {
            if ($(this).attr("index") != flag) {
                divArray.push($(this).attr("index"));
            }
        });
        var id = Math.max.apply(null, divArray);
        onSelect(".video_artical[index=" + id + "]");
        $(".video_artical[index=" + flag + "]").remove();
        $(".tableClass_" + flag).remove();
        $(".tableClass_" + id).show();
    }
    if ($(".video_artical").length < 7) {
        $(".video_artical_add").show();
    }
}
//选中主预览框
function onSelect_M() {
    if ($(".video_artical").length >= 1) {
        $(".choose_mode_style").css("opacity", "0");
        $(".video-unit").css("border", "2px solid #4a90e2");
        $(".tableClass").show();
        $("#pushForm table").each(function () {
            if ($(this).attr("tabType")) {
                $(this).css("display", "none");
            }
        });
    }
}
//选中子预览框
function onSelect(obj) {
    $(".tableClass").hide();
    $(".video-unit").css("border", "0 solid #ccc");
    $("#pushForm table").each(function () {
        if ($(this).attr("tabType")) {
            $(this).css("display", "none");
        }
    });
    $(".choose_mode_style").css("opacity", "0");
    $(obj).children("div").css("opacity", "1");
    var id = $(obj).attr("index");
    $(".tableClass_" + id).css("display", "block");
}

//清空工资条或者车辆保养dialog中字段
function resetSalaryAndMaintence() {
    //先清空文本框内容
    selector.$txtTitle_sa().val("");
    selector.$isSendTime_sa().val("False");
    selector.$isSendTime_sa().trigger("change");

    selector.$isImportant_sa().val("False");
    selector.$isImportant_sa().trigger("change");
    selector.$coverImgPath_sa().val("");
    selector.$coverImg_input_sa().val("");
    selector.$txtCoverDescption_sa().val("");
    $("#uploadResult").text("");
    $("#txtSalalryPath").val("");
    selector.$SalaryDialog().modal({ backdrop: 'static', keyboard: false });
    selector.$SalaryDialog().modal('show');
}

//点击上传工资条弹出框中的保存按钮
function saveSalaryDialog() {
    var validateError = 0;
    if (!Validate(selector.$txtTitle_sa())) {
        validateError++;
    }
    if ($("#txtSalalryPath").val() != "1") {
        jqxNotification("请上传工资条Excel!", null, "error");
        return false;
    }
    if (validateError == 0) {
        $.ajax({
            url: "/WeChatPush/DraftList/SaveSalaryPush",
            data: {
                Title: selector.$txtTitle_sa().val(),
                Timed: selector.$isSendTime_sa().val(),
                TimedSendTime: selector.$sendTime_sa().val(),
                Important: selector.$isImportant_sa().val(),
                PeriodOfValidity: selector.$effectiveDate_sa().val(),
                CoverImg: selector.$coverImgPath_sa().val(),
                CoverDescption: selector.$txtCoverDescption_sa().val()
            },
            type: "post",
            dataType: "json",
            success: function (msg) {
                switch (msg.ReturnMsg) {
                    case "0":
                        jqxNotification("保存失败", null, "error");
                        break;
                    case "1":
                        selector.$SalaryDialog().modal('hide');
                        window.location.href = "/WeChatPush/DraftList/DraftList";
                        break;
                    default:
                        selector.$SalaryDialog().modal('hide');
                        var tips = msg.ResponseInfo;
                        selector.$tips().text(tips);
                        initNotExistTable();
                        selector.$notExistDialog().modal({ backdrop: 'static', keyboard: false });
                        selector.$notExistDialog().modal('show');
                        break;
                }
            }

        });
    }
}

function saveMaintenanceDialog() {
    var validateError = 0;
    if (!Validate(selector.$txtTitle_sa())) {
        validateError++;
    }
    if ($("#txtSalalryPath").val() != "1") {
        jqxNotification("请上传保养信息Excel!", null, "error");
        return false;
    }
    if (validateError == 0) {
        $.ajax({
            url: "/WeChatPush/DraftList/SaveMaintenancePush",
            data: {
                Title: selector.$txtTitle_sa().val(),
                Timed: selector.$isSendTime_sa().val(),
                TimedSendTime: selector.$sendTime_sa().val(),
                Important: selector.$isImportant_sa().val(),
                PeriodOfValidity: selector.$effectiveDate_sa().val(),
                CoverImg: selector.$coverImgPath_sa().val(),
                CoverDescption: selector.$txtCoverDescption_sa().val()
            },
            type: "post",
            dataType: "json",
            success: function (msg) {
                switch (msg.ReturnMsg) {
                    case "0":
                        jqxNotification("保存失败", null, "error");
                        break;
                    case "1":
                        selector.$SalaryDialog().modal('hide');
                        window.location.href = "/WeChatPush/DraftList/DraftList";
                        break;
                    default:
                        selector.$SalaryDialog().modal('hide');
                        var tips = msg.ResponseInfo;
                        selector.$tips().text(tips);
                        initNotExistTable("车号");
                        selector.$notExistDialog().modal({ backdrop: 'static', keyboard: false });
                        selector.$notExistDialog().modal('show');
                        break;
                }
            }

        });
    }
}

$(function () {
    var page = new $page();
    page.init();

});
