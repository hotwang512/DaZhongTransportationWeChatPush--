﻿

var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有元素选择器
    var selector = {
        $grid: function () { return $("#roleList") },
        $btnAdd: function () { return $("#btnAdd") },
        $btnDelete: function () { return $("#btnDelete") },
        $btnSearch: function () { return $("#btnSearch") },
        $btnReset: function () { return $("#btnReset") },

        $RoleName_Search: function () { return $("#RoleName_Search") }



    }; //selector end



    //所有事件
    function addEvent() {

        //加载列表数据
        LoadTable();

        //
        selector.$btnSearch().on('click', function () {
            LoadTable();
        });

        //重置按钮事件
        selector.$btnReset().on('click', function () {
            selector.$RoleName_Search().val("");
        });

        //新增
        selector.$btnAdd().on('click', function () {
            window.location.href = "/Systemmanagement/AuthorityManagement/AuthorityDetail?isEdit=false";
        });

        //删除
        selector.$btnDelete().on('click', function () {
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
            })
            if (selection.length < 1) {
                jqxNotification("请选择您要删除的数据！", null, "error");
            } else {
                WindowConfirmDialog(dele, "您确定要删除选中的数据？", "确认框", "确定", "取消");
            }
        });

    }; //addEvent end


    var tool = {

    }; // tool end       

    function LoadTable() {
        var roleTypeSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'Role', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "Vguid",
                async: true,
                data: { "roleName": selector.$RoleName_Search().val() },
                url: "/Systemmanagement/AuthorityManagement/GetRoleTypeList"   //获取数据源的路径
            };
        var typeAdapter = new $.jqx.dataAdapter(roleTypeSource, {
            downloadComplete: function (data) {
                roleTypeSource.totalrecords = data.TotalRows;
            }
        });
        //创建卡信息列表（主表）
        selector.$grid().jqxDataTable(
            {
                pageable: true,
                width: "100%",
                height: 400,
                pageSize: 10,
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: typeAdapter,
                theme: "office",
                columnsHeight: 40,
                columns: [
                  { width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '角色名称', datafield: 'Role', width: 150, align: 'center', cellsAlign: 'center', cellsRenderer: roleDetailFunc },
                  //{ text: '角色名称', datafield: 'RoleName', align: 'center', cellsAlign: 'center', width: 200 },
                  { text: '描述', datafield: 'Description', align: 'center', cellsAlign: 'center' },
                   { text: 'Vguid', datafield: 'Vguid', hidden: true }
                ]
            });

    }

    function roleDetailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='AuthorityDetail?roleTypeVguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.Role + "</a>";
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


    //删除
    function dele() {
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
        })
        $.ajax({
            url: "/Systemmanagement/AuthorityManagement/DeleteRoleType",
            data: { roleTypeVguid: selection },
            traditional: true,
            type: "post",
            success: function (msg) {
                if (msg.isSuccess) {

                    jqxNotification("删除成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                }
                else {

                    jqxNotification("删除失败！", null, "error");
                }
            }
        })
    }
};

$(function () {
    var page = new $page();
    page.init();
})


