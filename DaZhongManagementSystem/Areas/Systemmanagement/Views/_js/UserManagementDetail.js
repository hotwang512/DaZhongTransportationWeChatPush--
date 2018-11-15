var selector = {
    $frmtable: function () { return $("#frmtable") },

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },

    //界面元素
    $loginName_Input: function () { return $("#loginName_Input") },
    $userName_Input: function () { return $("#userName_Input") },
    $email_Input: function () { return $("#email_Input") },
    $mobilePhone_Input: function () { return $("#mobilePhone_Input") },
    $workPhone_Input: function () { return $("#workPhone_Input") },
    $role_Input: function () { return $("#role_Input") },
    $company_Input: function () { return $("#company_Input") },
    $department_Input: function () { return $("#department_Input") },
    $enable_Input: function () { return $("#enable_Input") },
    $remark_Input: function () { return $("#remark_Input") },
    $pushPeopleDropDownButton: function () { return $("#pushPeopleDropDownButton") },
    $pushTree: function () { return $("#pushTree") },
    $departmentVguid: function () { return $("#DepartmentVguid") },
    $personnelVguid: function () { return $("#PersonnelVguid") },
    $isEdit: function () { return $("#isEdit") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {



    }; //addEvent end

    //公司下拉框改变事件
    selector.$company_Input().change(function () {
        var selOpt = $("#department_Input option");
        selOpt.remove();
        $.ajax({
            url: "/Systemmanagement/UserManage/GetDepartmentList",
            data: { vguid: selector.$company_Input().val() },
            traditional: true,
            type: "post",
            success: function (data) {
                if (data != null) {
                    selector.$department_Input().append("<option value=\"\">====请选择====</option>");
                    for (var i = 0; i < data.length; i++) {
                        var value = data[i].Vguid;
                        var text = data[i].OrganizationName;
                        selector.$department_Input().append("<option value='" + value + "'>" + text + "</option>");
                    }
                }
            }
        });
    });

    $.ajax({
        url: "/BasicDataManagement/UserInfo/GetOrganizationTreeList",
        data: {},
        traditional: true,
        type: "post",
        success: function (msg) {
            //推送接收人下拉框
            selector.$pushPeopleDropDownButton().jqxDropDownButton({
                width: 185,
                height: 25
            });
            //推送接收人下拉框(树形结构)
            selector.$pushTree().on('select', function (event) {
                var args = event.args;
                var item = selector.$pushTree().jqxTree('getItem', args.element);
                //   
                var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                selector.$pushPeopleDropDownButton().jqxDropDownButton('setContent', dropDownContent);
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
            selector.$pushTree().jqxTree({ source: records, width: '207px', height: '250px', incrementalSearch: true });//, checkboxes: true
            var str = selector.$departmentVguid().val();

            selector.$pushTree().jqxTree('selectItem', $("#" + str)[0]);// $("#home")[0]+ str
            selector.$pushTree().jqxTree('expandAll');

        }
    });
    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/Systemmanagement/UserManage/UserManagement";
    });

    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量

        if (!Validate(selector.$loginName_Input())) {
            validateError++;
        }
        //if (!Validate(selector.$userName_Input())) {
        //    validateError++;
        //}
        //if (!Validate(selector.$email_Input())) {
        //    validateError++;
        //}
        if (!Validate(selector.$mobilePhone_Input())) {
            validateError++;
        }
        if (!Validate(selector.$role_Input())) {
            validateError++;
        }
        if (validateError <= 0) {
            var items = selector.$pushTree().jqxTree('getSelectedItem');
            selector.$departmentVguid().val(items.id);
            selector.$frmtable().ajaxSubmit({
                url: '/Systemmanagement/UserManage/SaveUserInfo',
                type: "post",
                data: { isEdit: selector.$isEdit().val() },
                success: function (msg) {
                    switch (msg.respnseInfo) {
                        case "0":
                            jqxNotification("保存失败！", null, "error");
                            break;
                        case "1":
                            jqxNotification("保存成功！", null, "success");
                            window.location.href = "/Systemmanagement/UserManage/UserManagement";
                            break;
                        case "2":
                            jqxNotification("登录名称已存在，请重新输入！", null, "error");
                            break;
                    }
                }
            });
        }
    });
};




$(function () {
    var page = new $page();
    page.init();

})


