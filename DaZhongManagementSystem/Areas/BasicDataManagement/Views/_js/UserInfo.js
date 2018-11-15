var selector = {
    $grid: function () { return $("#UserInfoList") },
    $userInfoDialog: function () { return $("#UserInfoDialog") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },

    //按钮
    $btnDelete: function () { return $("#btnDelete") },
    $btnSearch: function () { return $("#btnSearch") },
    $btnReset: function () { return $("#btnReset") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnSave: function () { return $("#btnSave") },
    $btnReturnFormal: function () { return $("#btnReturnFormal") },//手动关注按钮
    $btnImport: function () { return $("#btnImport") },
    //查询
    $name_Search: function () { return $("#Name_Search") },
    $jobNumber_Search: function () { return $("#JobNumber_Search") },
    $serviceNum_Search: function () { return $("#ServiceNumber_Search") },
    $Status_Search: function () { return $("#drdStatus") },
    $userID_Search: function () { return $("#UserIDNo_Search") },
    $mobilePhone_Search: function () { return $("#MobilePhone_Search") },
    $TranslationOwnedFleet_Search: function () { return $("#TranslationOwnedFleet_Search") },
    $jqxDepartmentDropDownButton: function () { return $("#jqxDepartmentDropDownButton") },
    $jqxTree: function () { return $("#jqxTree") },
    $OwnedFleet: function () { return $("#OwnedFleet") },
    //隐藏域
    $txtCurrentUser: function () { return $("#txtCurrentUser") },
    $importFile: function () { return $("#importFile") },
    //弹出框元素
    $userName: function () { return $("#UserName") },
    $departmentDropDownButton: function () { return $("#DepartmentDropDownButton") },
    $departmentTree: function () { return $("#DepartmentTree") },
    $departmentVguid: function () { return $("#DepartmentVguid") },
    $personnelVguid: function () { return $("#PersonnelVguid") },
    $txtLabel1: function () { return $("#txtLabel1") },
    $imgPop1: function () { return $("#imgPop1") },

    $ErrorDialog: function () { return $("#ErrorDialog") },
    $ErrorMessage: function () { return $("#ErrorMessage") },
    $btnErrorCancel: function () { return $("#btnErrorCancel") },
    $btnErrorExport: function () { return $("#btnErrorExport") }

};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        LoadTable();
        initDepartment();

    }; //addEvent end

    selector.$btnErrorExport().on("click", function () {
        //$.ajax({
        //    url: "/BasicDataManagement/UserInfo/getDatatalbe",
        //    //data: { datatable: msg.ErrorDataTable },
        //    traditional: true,
        //    type: "post",
        //    success: function (msg) {

        //    }
        //});

        window.location.href = "/BasicDataManagement/UserInfo/getDatatalbe";
    })

    selector.$btnErrorCancel().on("click", function () {
        selector.$ErrorDialog().modal('hide');
    });

    //查询按钮事件
    selector.$btnSearch().on('click', function () {
        LoadTable();
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$name_Search().val("");
        selector.$Status_Search().val("");
        selector.$TranslationOwnedFleet_Search().val("");
        selector.$jobNumber_Search().val("");
        selector.$serviceNum_Search().val("");
        selector.$userID_Search().val("");
        selector.$mobilePhone_Search().val("");
        selector.$jqxDepartmentDropDownButton().jqxDropDownButton('setContent', "");
        selector.$jqxTree().jqxTree('clearSelection');
        selector.$OwnedFleet().val("");
    });

    //双击编辑事件
    selector.$grid().on('rowDoubleClick', function (event) {
        if (selector.$txtCurrentUser().val() == "sysAdmin") {
            var args = event.args;
            var key = args.key;
            var row = args.row;
            $.ajax({
                url: "/BasicDataManagement/UserInfo/GetUserDepartment",
                data: { "personVguid": row.vguid },
                type: "post",
                success: function (msg) {
                    selector.$userName().val(msg.Name);
                    selector.$departmentVguid().val(msg.OwnedFleet);
                    selector.$personnelVguid().val(msg.Vguid);

                    //推送接收人下拉框
                    $.ajax({
                        url: "/BasicDataManagement/UserInfo/GetOrganizationTreeList",
                        data: {},
                        traditional: true,
                        type: "post",
                        success: function (msg) {
                            //推送接收人下拉框
                            selector.$departmentDropDownButton().jqxDropDownButton({
                                width: 150,
                                height: 25
                            });
                            //推送接收人下拉框(树形结构)
                            selector.$departmentTree().on('select', function (event) {
                                var args = event.args;
                                var item = selector.$departmentTree().jqxTree('getItem', args.element);
                                var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                                selector.$departmentDropDownButton().jqxDropDownButton('setContent', dropDownContent);
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
                            var str = selector.$departmentVguid().val();
                            selector.$departmentTree().jqxTree('selectItem', $("#" + str)[0]);// $("#home")[0]+ str
                            selector.$departmentTree().jqxTree('expandAll');
                        }
                    });

                    initLabel();


                    //弹出编辑框
                    selector.$myModalLabel_title().text("编辑员工所在部门");
                    selector.$userInfoDialog().modal({ backdrop: 'static', keyboard: false });
                    selector.$userInfoDialog().modal('show');
                }
            });
        }
    });
    //初始化标签
    function initLabel() {
        $.ajax({
            url: '/BasicDataManagement/UserInfo/GetPersonLabel',
            data: { 'personVguid': selector.$personnelVguid().val() },
            type: 'post',
            dataType: 'json',
            success: function (msg) {
                $(".addLabelClass").remove();
                $("#txtLabel1").val("");
                $("#txtLabel1").val(msg[0]);
                for (var i = 1; i < msg.length; i++) {
                    var id = i + 1;
                    var obj = '<tr style="height: 50px;" class="addLabelClass">' +
             ' <td align="right" style="width: 75px;">标签：</td>' +
             ' <td style="width: 240px;">' +
                 '<input type="text" id="txtLabel' + id + '" name="Label" class="input_text form-control" value=' + msg[i] + '>' +
                 '<div style="float: left;">' +
                    // '<img id="imgPop' + i + '" onclick="addLabel(' + i + ')" style="float: left; margin-top: 5px; width: 25px; cursor: pointer" src="/_theme/images/timg.jpg"/>' +
                     '<a href="#" class="removeclass" onclick="deleteOption(' + id + ')" ><i class="iconfont btn_icon" style="color: black !important;margin-top: 2px;">&#xe60a;</i></a>' +
                 '</div>' +
             '</td>' +
     '</tr>';
                    $("#OrganizationForm table").append(obj);
                }

            }
        });

    }

    //点击导入按钮
    selector.$btnImport().on('click', function () {
        selector.$importFile().click();
    });

    //初始化下拉框
    function initDepartment() {
        //推送接收人下拉框
        $.ajax({
            url: "/BasicDataManagement/UserInfo/GetOrganizationTreeList",
            data: {},
            traditional: true,
            type: "post",
            success: function (msg) {
                //推送接收人下拉框
                selector.$jqxDepartmentDropDownButton().jqxDropDownButton({
                    width: 185,
                    height: 25
                });
                //推送接收人下拉框(树形结构)
                selector.$jqxTree().on('select', function (event) {
                    var args = event.args;
                    var item = selector.$jqxTree().jqxTree('getItem', args.element);
                    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
                    selector.$jqxDepartmentDropDownButton().jqxDropDownButton('setContent', dropDownContent);
                    var items = selector.$jqxTree().jqxTree('getSelectedItem');
                    selector.$OwnedFleet().val("");
                    // selector.$TranslationOwnedFleet_Search().val("");
                    selector.$OwnedFleet().val(items.id);
                    // selector.$TranslationOwnedFleet_Search().val(items.label);
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
                selector.$jqxTree().jqxTree({ source: records, width: '207px', height: '250px', incrementalSearch: true });//, checkboxes: true

                selector.$jqxTree().jqxTree('expandAll');

            }
        });
    }

    var labelModel = function (VGUID, label) {
        this.PersonnelVVGUID = VGUID;
        this.LabelName = label;
    };


    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var id = selector.$departmentVguid().val();
        var items = selector.$departmentTree().jqxTree('getSelectedItem');
        if (items != null) {
            id = items.id;
        }
        var labelArray = new Array();
        var flag = 0;
        var error = 0;
        $(":input[name=Label]").each(function () {
            if (flag === $(this).val()) {
                error++;
            }
            flag = $(this).val();
            var label = new labelModel(selector.$personnelVguid().val(), $(this).val());
            labelArray.push(label);
        });
        if (error != 0) {
            jqxNotification("请添加不同的标签！", null, "error");
            return false;
        }
        $.ajax({
            url: "/BasicDataManagement/UserInfo/UpdateDepartment",
            data: { 'vguid': id, 'personVguid': selector.$personnelVguid().val(), labelStr: JSON.stringify(labelArray) },
            type: "post",
            success: function (data) {
                switch (data.respnseInfo) {
                    case "0":
                        jqxNotification("更新失败！", null, "error");
                        break;
                    case "1":
                        jqxNotification("更新成功！", null, "success");
                        selector.$grid().jqxDataTable('updateBoundData');
                        selector.$userInfoDialog().modal('hide');
                        break;
                }
            }
        });
    });

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        selector.$userInfoDialog().modal('hide');
    });

    //手动关注按钮事件
    selector.$btnReturnFormal().on('click', function () {
        selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                selection.push(data.vguid);
            }
        })
        if (selection.length < 1) {
            jqxNotification("请选择人员数据！", null, "error");
        } else {
            WindowConfirmDialog(focus, "您确定要手动使选中的人员关注企业号？", "确认框", "确定", "取消");
        }
    })

    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        selection = [];
        var grid = selector.$grid();
        var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
        var isFalse = false;
        checedBoxs.each(function () {
            var th = $(this);
            if (th.is(":checked")) {
                var index = th.attr("index");
                var data = grid.jqxDataTable('getRows')[index];
                if (data.ApprovalStatus == "3") {  //未匹配状态才能离职
                    selection.push(data.vguid);
                } else {
                    jqxNotification("该状态不符合离职要求", null, "error");
                    isFalse = true;
                    return false
                }
            }
        })
        if (!isFalse) {
            if (selection.length < 1) {
                jqxNotification("请选择您要离职的人员！", null, "error");
            } else {
                WindowConfirmDialog(dele, "您确定要离职选中的人员？", "确认框", "确定", "取消");
            }
        }

    });

    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'name', type: 'string' },
                    { name: 'UserID', type: 'string' },
                    { name: 'ID', type: 'string' },
                    { name: 'IDNumber', type: 'string' },
                    { name: 'OwnedFleet', type: 'string' },
                    { name: 'TranslationOwnedFleet', type: 'string' },
                    { name: 'Sex', type: 'string' },
                    { name: 'JobNumber', type: 'string' },
                    { name: 'ServiceNumber', type: 'string' },
                    { name: 'PhoneNumber', type: 'string' },
                    { name: 'WeChatNumber', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'ApprovalStatus', type: 'string' },
                    { name: 'CreatedUser', type: 'string' },
                    { name: 'ChangeDate', type: 'date' },
                    { name: 'ChangeUser', type: 'string' },
                    { name: 'vguid', type: 'string' },
                    { name: 'LabName', type: 'string' }
                ],
                datatype: "json",
                id: "vguid",//主键
                async: true,
                data: { "name": selector.$name_Search().val(), "JobNumber": selector.$jobNumber_Search().val(), "ServiceNumber": selector.$serviceNum_Search().val(), "IDNumber": selector.$userID_Search().val(), "PhoneNumber": selector.$mobilePhone_Search().val(), "OwnedFleet": selector.$OwnedFleet().val(), "TranslationApprovalStatus": selector.$Status_Search().val() },
                url: "/BasicDataManagement/UserInfo/GetUserListBySearch"    //获取数据源的路径
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
                height: 480,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columns: [
                  { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                //  { text: '微信UserID', width: 150, datafield: 'UserID', align: 'center', cellsAlign: 'center' },
                  { text: '人员姓名', width: 150, datafield: 'name', align: 'center', cellsAlign: 'center' },
                  { text: '电话号码', width: 150, datafield: 'PhoneNumber', align: 'center', cellsAlign: 'center' },
                  { text: '身份证号', width: 200, datafield: 'IDNumber', align: 'center', cellsAlign: 'center' },
                  { text: '部门', width: 350, datafield: 'TranslationOwnedFleet', align: 'center', cellsAlign: 'center' },
                  { text: '性别', width: 80, datafield: 'Sex', align: 'center', cellsAlign: 'center' },//, cellsRenderer: genderTranslate 
                  { text: '工号', datafield: 'JobNumber', align: 'center', cellsAlign: 'center' },
                  { text: '服务卡号', datafield: 'ServiceNumber', align: 'center', cellsAlign: 'center' },
                  { text: '标签', datafield: 'LabName', align: 'center', cellsAlign: 'center' },
                  //{ text: '创建时间', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd" },
                  //{ text: '创建人', datafield: 'CreatedUser', align: 'center', cellsAlign: 'center' },
                  //{ text: '修改时间', datafield: 'ChangeDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd" },
                  //{ text: '修改人', datafield: 'ChangeUser', align: 'center', cellsAlign: 'center' },
                  { text: '状态', datafield: 'ApprovalStatus', align: 'center', cellsAlign: 'center', cellsRenderer: statusTranslate },
                  //{ text: 'OwnedFleet', datafield: 'OwnedFleet', hidden: true },
                  { text: 'VGUID', datafield: 'vguid', hidden: true }
                ]
            });
    }

    function genderTranslate(row, column, value, rowData) {
        var container = "";
        if (rowData.Sex != null) {
            if (rowData.Sex == "1") {
                container = "男";
            } else {
                container = "女";
            }
        }
        return container;
    }

    //翻译 状态列
    function statusTranslate(row, column, value, rowData) {
        var container = "";
        if (rowData.ApprovalStatus != null) {
            if (rowData.ApprovalStatus == "1") {
                container = "未关注";
            } else if (rowData.ApprovalStatus == "2") {
                container = "已关注";
            } else if (rowData.ApprovalStatus == "3") {
                container = "未匹配";
            } else if (rowData.ApprovalStatus == "4") {
                container = "已离职";
            } else if (rowData.ApprovalStatus == "5") {
                container = "已禁用";
            }
        }
        return container;
    }

    function detailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='#' onclick=\"edit('" + rowData.VGUID + "')\" style=\"text-decoration: underline;color: #333;\">" + rowData.Listkey + "</a>";
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

    function mouseoverFunc(row, column, value, rowData) {
        var container = "";
        $.ajax({
            url: '/BasicDataManagement/UserInfo/GetPersonLabel',
            data: { 'personVguid': rowData.vguid },
            type: 'post',
            async: false,
            dataType: 'json',
            success: function (msg) {
                container = "<span title=" + msg.join(',') + ">" + rowData.name + "</span>";
            }
        });
        return container;
    }

};


//删除
function dele() {
    showLoading();//显示加载等待框
    selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.vguid);
        }
    });
    $.ajax({
        url: "/BasicDataManagement/UserInfo/DeleteUserInfo",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("离职失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("离职成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

//手动关注
function focus() {
    showLoading();//显示加载等待框
    selection = [];
    var grid = selector.$grid();
    var checedBoxs = grid.find(".jqx_datatable_checkbox:checked");
    checedBoxs.each(function () {
        var th = $(this);
        if (th.is(":checked")) {
            var index = th.attr("index");
            var data = grid.jqxDataTable('getRows')[index];
            selection.push(data.vguid);
        }
    });
    $.ajax({
        url: "/BasicDataManagement/UserInfo/UserFocusWeChat",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("手动关注失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("手动关注成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

//新增标签文本框
function addLabel(id) {
    var i = id + 1;
    var obj = '<tr style="height: 50px;" class="addLabelClass">' +
                ' <td align="right" style="width: 75px;">标签：</td>' +
                ' <td style="width: 240px;">' +
                    '<input type="text" id="txtLabel' + i + '" name="Label" class="input_text form-control">' +
                    '<div style="float: left;">' +
                       // '<img id="imgPop' + i + '" onclick="addLabel(' + i + ')" style="float: left; margin-top: 5px; width: 25px; cursor: pointer" src="/_theme/images/timg.jpg"/>' +
                        '<a href="#" class="removeclass" onclick="deleteOption(' + i + ')" ><i class="iconfont btn_icon" style="color: black !important;margin-top: 2px;">&#xe60a;</i></a>' +
                    '</div>' +
                '</td>' +
        '</tr>';
    $("#OrganizationForm table").append(obj);
    //$("#imgPop" + id).remove();
}

//删除新增的文本框
function deleteOption(id) {
    $("#txtLabel" + id).parent().parent().remove();
}
//导入标签
function fileUpload() {
    $("form").ajaxSubmit({
        url: " /BasicDataManagement/UserInfo/UpLoadLabel?labelFile=importFile",
        type: "post",
        dataType: "json",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("导入失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("导入成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
                case "2":

                    selector.$ErrorMessage().html(msg.ResponseMessage);
                    selector.$ErrorDialog().modal({ backdrop: 'static', keyboard: false });
                    selector.$ErrorDialog().modal('show');
                    selector.$grid().jqxDataTable('updateBoundData');

                    //$.ajax({
                    //    url: "/BasicDataManagement/UserInfo/getDatatalbe",
                    //    data: { datatable: msg.ErrorDataTable },
                    //    traditional: true,
                    //    type: "post",
                    //    success: function (msg) {
                            
                    //    }
                    //});

                    //alert(msg.ResponseMessage);
                    break;
                default:
                    jqxNotification(msg.respnseInfo, null, "error");
                    break;
            }
            selector.$importFile().remove();
            var inputObj = '<input type="file" name="importFile" id="importFile" onchange="fileUpload()" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel" style="display: none" />';
            $("form").append(inputObj);
        }
    });

}

$(function () {
    var page = new $page();
    page.init();

})


