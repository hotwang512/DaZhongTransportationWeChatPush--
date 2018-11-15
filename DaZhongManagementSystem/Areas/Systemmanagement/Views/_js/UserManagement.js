var selector = {
    $grid: function () { return $("#UserInfoList") },

    //按钮
    $btnSearch: function () { return $("#btnSearch") },
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnEnable: function () { return $("#btnEnable") },
    $btnDisable: function () { return $("#btnDisable") },
    $btnReset: function () { return $("#btnReset") },

    //查询
    $name_Search: function () { return $("#Name_Search") },
    $Department_Search: function () { return $("#Department_Search") },
    $Role_Search: function () { return $("#Role_Search") },
    $email_Search: function () { return $("#Email_Search") },
    $Company_Search: function () { return $("#Company_Search") }
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

    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        window.location.href = "/Systemmanagement/UserManage/UserManagementDetail?isEdit=false";
    });

    //重置按钮事件
    selector.$btnReset().on('click', function () {
        selector.$name_Search().val("");
        selector.$Role_Search().val("");
        selector.$Department_Search().val("");
    });

    //删除按钮事件
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

    //启用按钮事件
    selector.$btnEnable().on('click', function () {
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
            jqxNotification("请选择您要启用的数据！", null, "error");
        } else {
            WindowConfirmDialog(enabled, "您确定要启用选中的数据？", "确认框", "确定", "取消");
        }
    });

    //禁用按钮事件
    selector.$btnDisable().on('click', function () {
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
            jqxNotification("请选择您要禁用的数据！", null, "error");
        } else {
            WindowConfirmDialog(disabled, "您确定要禁用选中的数据？", "确认框", "确定", "取消");
        }
    });

    //加载团队表
    function LoadTable() {

        var UserInfoListSource =
            {
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'LoginName', type: 'string' },
                    //{ name: 'UserName', type: 'string' },
                    { name: 'Company', type: 'string' },
                    { name: 'TranslationCompany', type: 'string' },
                    { name: 'Email', type: 'string' },
                    { name: 'WorkPhone ', type: 'string' },
                    { name: 'MobileNnumber', type: 'string' },
                    { name: 'Enable', type: 'string' },
                    { name: 'TranslationEnable', type: 'string' },
                    { name: 'Role', type: 'string' },
                    { name: 'TranslationRole', type: 'string' },
                    { name: 'Department', type: 'string' },
                    { name: 'TranslationDepartment', type: 'string' },
                    { name: 'CreatedUser', type: 'string' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'Vguid', type: 'string' }
                ],
                datatype: "json",
                id: "VGUID",//主键
                async: true,
                data: { LoginName: selector.$name_Search().val(), Role: selector.$Role_Search().val(), Department: selector.$Department_Search().val() },
                url: "/Systemmanagement/UserManage/GetSysUserListBySearch"    //获取数据源的路径
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
                  { text: '登录名称', datafield: 'LoginName', width: 150, align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc },
                  //{ text: '用户名称', datafield: 'UserName', align: 'center', cellsAlign: 'center' },
                  //{ text: '公司名称', datafield: 'TranslationCompany', width: 150, align: 'center', cellsAlign: 'center' },
                  { text: '部门', datafield: 'TranslationDepartment', width: 250, align: 'center', cellsAlign: 'center' },
                  { text: '角色', datafield: 'TranslationRole', width: 120, align: 'center', cellsAlign: 'center' },
                  { text: '邮箱', datafield: 'Email', align: 'center', width: 130, cellsAlign: 'center' },
                  //{ text: '工作电话', datafield: 'WorkPhone', align: 'center', cellsAlign: 'center' },
                  { text: '手机号码', datafield: 'MobileNnumber', align: 'center', cellsAlign: 'center' },
                  { text: '是否启用', datafield: 'TranslationEnable', width: 150, align: 'center', cellsAlign: 'center' },
                  { text: '创建人', datafield: 'CreatedUser', align: 'center', cellsAlign: 'center' },
                  { text: '创建时间', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center', datatype: 'date', cellsformat: "yyyy-MM-dd HH:mm:ss" },
                  { text: 'Vguid', datafield: 'Vguid', hidden: true }
                ]
            });
    }
    function detailFunc(row, column, value, rowData) {
        var container = "";
        container = "<a href='UserManagementDetail?Vguid=" + rowData.Vguid + "&isEdit=true' style=\"text-decoration: underline;color: #333;\">" + rowData.LoginName + "</a>";
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
            selection.push(data.Vguid);
        }
    });
    $.ajax({
        url: "/Systemmanagement/UserManage/DeleteUserInfo",
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
    })
}

//启用
function enabled() {
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
        url: "/Systemmanagement/UserManage/EnableUser",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("启用失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("启用成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}

//禁用
function disabled() {
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
        url: "/Systemmanagement/UserManage/DisableUser",
        data: { vguidList: selection },
        traditional: true,
        type: "post",
        success: function (msg) {
            switch (msg.respnseInfo) {
                case "0":
                    jqxNotification("禁用失败！", null, "error");
                    break;
                case "1":
                    jqxNotification("禁用成功！", null, "success");
                    selector.$grid().jqxDataTable('updateBoundData');
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}


$(function () {
    var page = new $page();
    page.init();

})


