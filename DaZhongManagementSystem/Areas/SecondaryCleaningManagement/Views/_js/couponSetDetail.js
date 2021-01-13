$(".input_text").attr("autocomplete", "new-password");
var selector = {
    $frmtable: function () { return $("#frmtable") },

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnCreate: function () { return $("#btnCreate") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    //界面元素
    $rightsName: function () { return $("#RightsName") },
    $isEdit: function () { return $("#isEdit") },
    $pushPeople: function () { return $("#pushPeople") },
    $imgPop: function () { return $("#imgPop") },
    $grid: function () { return $("#UserInfoList") },
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
};
var people = [];
var pushObject = [];
var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        getEquityAllocation();
        initDropdownList();
    }; //addEvent end

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/SecondaryCleaningManagement/CouponSet/Index";
    });
    //有效期类型选择
    $("#ValidType").on('change', function () {
        var val = $("#ValidType").val();
        //根据类型控制字段显示
        validTypeFunc(val);
    });
    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量
        var StartValidity = $("#StartValidity").val();
        var EndValidity = $("#EndValidity").val();
        if (!Validate($(selector.$rightsName()))) {
            validateError++;
        }
        if (validateError <= 0) {
            selector.$frmtable().ajaxSubmit({
                url: '/SecondaryCleaningManagement/CouponSetDetail/SaveEquityAllocation',
                type: "post",
                async: false,
                data: { isEdit: selector.$isEdit().val(), startValidity: StartValidity, endValidity: EndValidity, pushPeople: $("#pushPeople").val() },
                success: function (msg) {
                    if (msg.respnseInfo == "2") {
                        jqxNotification("权益类型已存在，请重新输入！", null, "error");
                        return;
                    }
                    if (msg.respnseInfo == "0") {
                        jqxNotification("保存失败！", null, "error");
                        return;
                    } else {
                        jqxNotification("保存成功！", null, "success");
                        window.location.href = "/SecondaryCleaningManagement/CouponSet/Index";
                    }
                }
            });
        }
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
    //点击弹出框保存按钮
    selector.$btnPopSave().on('click', function () {
        if (people.length != 0) { //说明勾选了具体的人
            selector.$pushPeople().val(people.join(","));
            $("#PushObject").val(pushObject.join("|"));
        }
        if (selector.$departmentTree().jqxTree('getSelectedItem') != null && people.length == 0) {  //说明选择了具体的部门
            $("#PushObject").val(selector.$OwnedFleet().val());
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
                $("#PushObject").val('');
                pushObject = [];
                selector.$pushPeople().val('');
            } else if (checkedItems != "" && selector.$departmentTree().jqxTree('getSelectedItem') != null && people.length == 0) { //即选择了标签又选择了部门
                $("input[name=Label]").val(checkedItems);
            }
        }
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
};

$(function () {
    var page = new $page();
    page.init();
})

var guid = $("#VGUID").val();
function getEquityAllocation() {
    $.ajax({
        url: "/SecondaryCleaningManagement/CouponSetDetail/GetEquityAllocationByVguid",
        data: { vguid: guid },
        type: "post",
        success: function (msg) {
            if (msg != null) {
                if (msg.Status == "已发布") {
                    $("#hideSave").hide();
                }
                $("#RightsName").val(msg.RightsName);
                $("#Description").text(msg.Description);
                if (msg.Description == null || msg.Description == "") {
                    $("#Description").text("");
                }
                $("#Type").val(msg.Type);
                $("#ValidType").val(msg.ValidType);
                if (msg.ValidType == null || msg.ValidType == "") {
                    $("#ValidType").val("截止日期");
                }
                if (msg.StartValidity != null && msg.StartValidity != "") {
                    var startValidity = getVDate(msg.StartValidity);
                    var endValidity = getVDate(msg.EndValidity);
                    $("#StartValidity").val(startValidity);
                    $("#EndValidity").val(endValidity);
                }
                $("#Period").val(msg.Period);
                $("#PushObject").val(msg.PushObject);
                $("#pushPeople").val(msg.PushPeople);
                validTypeFunc(msg.ValidType);

            }
        }
    })
}
function getVDate(vDate) {
    var date = vDate.split(" ")[0];
    var newDate = date.split("/");
    var year = newDate[0];
    var month = newDate[1];
    var day = newDate[2];
    if (Number(month) < 10) {
        month = "0" + month;
    }
    if (Number(day) < 10) {
        day = "0" + day;
    }
    var nDate = year + "-" + month + "-" + day;
    return nDate;
}
function validTypeFunc(val){
    if (val == "截止日期") {
        $(".deadline").show();
        $(".cycle").hide();
        $("#Period").val("");
    } else if (val == "周期") {
        $(".deadline").hide();
        $(".cycle").show();
        $("#StartValidity").val("");
        $("#EndValidity").val("");
    }
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
        data: { 'TranslationApprovalStatus': "已关注", "name": selector.$name_Search().val().trim(), "OwnedFleet": selector.$OwnedFleet().val().trim(), "PhoneNumber": selector.$MobilePhone_Search().val().trim(), "LabelName": checkedItems },
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