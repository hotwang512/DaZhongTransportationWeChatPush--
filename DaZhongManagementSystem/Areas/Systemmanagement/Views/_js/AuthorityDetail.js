﻿
//所有元素选择器
var selector = {

    //表格元素
    $grid: function () { return $("#modulePermissionsList") },
    $userGrid: function () { return $("#userGrid") },

    //按钮元素
    $btnSave: function () { return $("#btnSave") }, //保存
    $btnBack: function () { return $("#btnBack") }, //返回
    $addUserBtn: function () { return $("#addUserBtn") },
    $OKBtn: function () { return $("#OKBtn") },
    $CancelBtn: function () { return $("#CancelBtn") },
    $btnSearch: function () { return $("#btnSearch") },

    $userListDialog: function () { return $("#userListDialog") },
    $jqx_datatable_checkbox: function () { return $(".jqx_datatable_checkbox") },
    $selectedAllCheckbox: function () { return $(".selectedAllCheckbox") },//全选列的checkbox


    //表单元素
    $roleForm: function () { return $("#roleForm") },
    $isEdit: function () { return $("#isEdit") },
    $RoleName: function () { return $("#RoleName") },
    $RoleVGUID: function () { return $("#RoleVGUID") },

}; //selector end

var dataTotalRows = 0;



var $page = function () {

    this.init = function () {
        addEvent();
    }



    //所有事件
    function addEvent() {


        //加载列表数据
        LoadTable();

        //返回
        selector.$btnBack().on('click', function () {
            window.location.href = "/Systemmanagement/AuthorityManagement/AuthorityManagement";
        });

        //保存
        selector.$btnSave().on('click', function () {
            var validateError = 0;//未通过验证的数量
            if (!Validate(selector.$RoleName())) {
                validateError++;
            }

            if (validateError <= 0) {
                var rolePermissionMode = function (ModuleName, RightType) {
                    this.ModuleName = ModuleName;
                    this.RightType = RightType;
                }
                var rolePermissionArray = new Array();

                $('.permission').each(function () {
                    if ($(this).is(":checked")) {
                        var pageid = $(this).attr("pageid");
                        var buttonid = $(this).attr("buttonid");
                        var rolePermission = new rolePermissionMode(pageid, buttonid);
                        rolePermissionArray.push(rolePermission);
                    }
                });
                selector.$roleForm().ajaxSubmit({
                    url: '/Systemmanagement/AuthorityManagement/SaveRole?isEdit=' + selector.$isEdit().val(),
                    type: "post",
                    data: { permissionList: JSON.stringify(rolePermissionArray) },
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                jqxNotification("保存失败！", null, "error");
                                break;
                            case "1":
                                jqxNotification("保存成功！", null, "success");
                                window.location.href = "/Systemmanagement/AuthorityManagement/AuthorityManagement";
                                break;
                            case "2":
                                jqxNotification("角色名称已经存在！", null, "error");
                                break;
                        }
                    }
                });
            }
        });

    }; //addEvent end


    var tool = {

    }; // tool end

    //加载权限列表
    function LoadTable() {
        var centerSetUpSource =
            {
                datafields:
                [
                    { name: 'ParentID', type: 'string' },
                    { name: 'PageID', type: 'string' },
                    { name: 'PageName', type: 'string' },
                    { name: 'Reads', type: 'string' },
                    { name: 'Adds', type: 'string' },
                    { name: 'Edit', type: 'string' },
                    { name: 'Deletes', type: 'string' },
                    { name: 'Submit', type: 'string' },
                    { name: 'Approved', type: 'string' },
                    { name: 'Import', type: 'string' },
                    { name: 'Export', type: 'string' },
                    { name: 'Vguid', type: 'string' },
                    { name: 'ModuleVGUID', type: 'string' }
                ],
                datatype: "json",
                id: "Vguid",
                async: true,
                data: { "roleVGUID": selector.$RoleVGUID().val() },
                url: "/Systemmanagement/AuthorityManagement/GetModulePermissionsList"   //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(centerSetUpSource, {
            downloadComplete: function (data) {
                centerSetUpSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: false,
                width: "100%",
                height: 500,
                pageSize: 10,
                //serverProcessing: true,
                pagerButtonsCount: 10,
                columnsHeight: 40,
                source: typeAdapter,
                theme: "office",
                groups: ['ParentID'],
                groupsRenderer: function (value, rowData, level) {
                    return "  " + value + "模块";
                },
                columns: [
                  { text: '模块', datafield: 'ParentID', hidden: true, align: 'center', cellsAlign: 'center' },
                  { text: '页面', datafield: 'PageName', align: 'center', cellsAlign: 'center' },
                  { text: '查看', datafield: 'Reads', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Reads },
                  { text: '新增', datafield: 'Adds', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Adds },
                  { text: '编辑', datafield: 'Edit', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Edit },
                  { text: '删除', datafield: 'Deletes', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Deletes },
                  { text: '提交', datafield: 'Submit', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Submit },
                  { text: '同意', datafield: 'Approved', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Approved },
                  { text: '导入', datafield: 'Import', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Import },
                  { text: '导出', datafield: 'Export', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc_Export },
                  { text: '全选', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, rendered: renderedFunc },
                  { text: 'PageID', datafield: 'PageID', hidden: true },
                  { text: 'Vguid', datafield: 'Vguid', hidden: true },
                  { text: 'ModuleVGUID', datafield: 'ModuleVGUID', hidden: true }
                ]
            });
    }
    function cellsRendererFunc(row, column, value, rowData) {
        return "<input class=\"selectedAllCheckbox\" id=\"" + row + "\" index=\"" + row + "\" type=\"checkbox\" onclick=\"selectAll('" + row + "')\" style=\"margin:auto;width: 17px;height: 17px;\" />";
    }

    function renderedFunc(element) {
        var grid = selector.$grid();
        var rows = grid.jqxDataTable('getRows');
        for (var i = 0; i < rows.length; i++) {

        }

        //pageID
        //var grid = selector.$grid();
        //element.jqxCheckBox();
        //element.on('change', function (event) {
        //    var checked = element.jqxCheckBox('checked');

        //    if (checked) {
        //        var rows = grid.jqxDataTable('getRows');
        //        for (var i = 0; i < rows.length; i++) {
        //            grid.jqxDataTable('selectRow', i);
        //            grid.find(".jqx_datatable_checkbox").attr("checked", "checked")
        //        }
        //    } else {
        //        grid.jqxDataTable('clearSelection');
        //        grid.find(".jqx_datatable_checkbox").removeAttr("checked", "checked")
        //    }
        //});
        return true;
    }

    function cellsRendererFunc_Reads(row, column, value, rowData) {
        if (rowData.Reads == "0") {
            return "";
        }
        else if (rowData.Reads == "1") {
            return "<div><input type=\"checkbox\" class=\"permission\"  style=\"width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"1\" /></div>";
        }
        else if (rowData.Reads == "2") {
            return "<input type=\"checkbox\" class=\"permission\"  style=\"width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"1\" />";
        }
    }

    function cellsRendererFunc_Adds(row, column, value, rowData) {
        if (rowData.Adds == "0") {
            return "";
        }
        else if (rowData.Adds == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"2\" />";
        }
        else if (rowData.Adds == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"2\" />";
        }
    }

    function cellsRendererFunc_Edit(row, column, value, rowData) {
        if (rowData.Edit == "0") {
            return "";
        }
        else if (rowData.Edit == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"3\" />";
        }
        else if (rowData.Edit == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"3\" />";
        }
    }

    function cellsRendererFunc_Deletes(row, column, value, rowData) {
        if (rowData.Deletes == "0") {
            return "";
        }
        else if (rowData.Deletes == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"4\" />";
        }
        else if (rowData.Deletes == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"4\" />";
        }
    }

    function cellsRendererFunc_Submit(row, column, value, rowData) {
        if (rowData.Submit == "0") {
            return "";
        }
        else if (rowData.Submit == "1") {
            return "<input type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"5\" />";
        }
        else if (rowData.Submit == "2") {
            return "<input type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"5\" />";
        }
    }

    function cellsRendererFunc_Approved(row, column, value, rowData) {
        if (rowData.Approved == "0") {
            return "";
        }
        else if (rowData.Approved == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"6\" />";
        }
        else if (rowData.Approved == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"6\" />";
        }
    }

    function cellsRendererFunc_Import(row, column, value, rowData) {
        if (rowData.Import == "0") {
            return "";
        }
        else if (rowData.Import == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"7\" />";
        }
        else if (rowData.Import == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"7\" />";
        }
    }

    function cellsRendererFunc_Export(row, column, value, rowData) {
        if (rowData.Export == "0") {
            return "";
        }
        else if (rowData.Export == "1") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"8\" />";
        }
        else if (rowData.Export == "2") {
            return "<input   type=\"checkbox\" class=\"permission\"  style=\"margin:auto;width: 17px;height: 17px;\" checked=\"checked\" pageid=\"" + rowData.ModuleVGUID + "\" buttonid=\"8\" />";
        }
    }
};

//全选checkbox点击事件
function selectAll(id) {
    //对于HTML元素本身就带有的固有属性，在处理时，使用prop方法。
    //对于HTML元素我们自己自定义的DOM属性，在处理时，使用attr方法。
    if ($("#" + id + "").is(":checked")) {
        $("#" + id + "").parent("td").parent("tr").find(".permission").prop("checked", "checked");
    } else {
        $("#" + id + "").parent("td").parent("tr").find(".permission").removeAttr("checked", "checked");
    }
}

$(function () {

    var page = new $page();
    page.init();
})


