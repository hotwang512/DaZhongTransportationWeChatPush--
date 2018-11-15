var selector = {
    $OrganizationGrid: function () { return $("#OrganizationList") },
    $DepartmentDialog: function () { return $("#DepartmentDialog") },
    $myModalLabel_title: function () { return $("#myModalLabel_title") },
    $organizationForm: function () { return $("#OrganizationForm") },

    //按钮
    $btnAdd: function () { return $("#btnAdd") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },

    //隐藏控件
    $parentVguid: function () { return $("#parentVguid") },
    $isEdit: function () { return $("#isEdit") },
    $Vguid: function () { return $("#Vguid") },
    //界面元素
    $departmentName: function () { return $("#departmentName") },
    $description: function () { return $("#description") },

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


    //新增按钮事件
    selector.$btnAdd().on('click', function () {
        var select = selector.$OrganizationGrid().jqxTreeGrid('getSelection');
        selector.$isEdit().val("");
        selector.$parentVguid().val("");
        selector.$departmentName().val("");
        selector.$description().val("");
        //alert(select[0].ParentVguid);
        if (select[0] == null) {
            jqxNotification("请选择父级部门！", null, "error");
            return;
        } else {
            selector.$isEdit().val("0");
            selector.$parentVguid().val(select[0].Vguid);
            selector.$myModalLabel_title().text("新增部门信息");
            selector.$DepartmentDialog().modal({ backdrop: 'static', keyboard: false });
            selector.$DepartmentDialog().modal('show');
        }
    });

    //删除按钮事件
    selector.$btnDelete().on('click', function () {
        var select = selector.$OrganizationGrid().jqxTreeGrid('getSelection');

        var children = selector.$OrganizationGrid().getChildNodes(select[0].Vguid);
    });

    //双击编辑事件
    selector.$OrganizationGrid().on('rowDoubleClick', function (event) {
        var args = event.args;
        var key = args.key;
        var row = args.row;
        selector.$isEdit().val("1");
        selector.$parentVguid().val(row.ParentVguid);//父级Vguid
        selector.$Vguid().val(row.Vguid);//Vguid
        $.ajax({
            url: "/BasicDataManagement/OrganizationManagement/GetOrganizationDetail",
            data: { vguid: selector.$Vguid().val() },
            type: "post",
            success: function (msg) {
                selector.$departmentName().val(msg.OrganizationName);
                selector.$description().val(msg.Description);
                //弹出编辑框   
                selector.$myModalLabel_title().text("编辑部门信息");
                selector.$DepartmentDialog().modal({ backdrop: 'static', keyboard: false });
                selector.$DepartmentDialog().modal('show');
            }
        });
    });

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        selector.$DepartmentDialog().modal('hide');
    });

    //保存按钮事件
    selector.$btnSave().on('click', function () {
        //var vguid = selector.$parentVguid().val();//获取界面上parentVGUID值
        var validateError = 0;//未通过验证的数量
        if (!Validate(selector.$departmentName())) {
            validateError++;
        }
        //拥有编辑权限
        if (selector.$EditPermission().val() == 1) {
            if (validateError <= 0) {
                selector.$organizationForm().ajaxSubmit({
                    url: '/BasicDataManagement/OrganizationManagement/Save?isEdit=' + selector.$isEdit().val(),
                    type: "post",
                    data: {},
                    traditional: true,
                    success: function (msg) {
                        switch (msg.respnseInfo) {
                            case "0":
                                jqxNotification("保存失败！", null, "error");
                                break;
                            case "1":
                                selector.$DepartmentDialog().modal('hide');
                                jqxNotification("保存成功！", null, "success");
                                window.location.href = "/BasicDataManagement/OrganizationManagement/OrganizationManagement";
                                //selector.$OrganizationGrid(LoadTable());
                                break;
                            case "2":
                                jqxNotification("部门名称重复！", null, "error");
                                break;
                        }
                    }
                });
            }
        }
        else {
            jqxNotification("您没有编辑权限，请联系系统管理员！", null, "error");
        }
    });

    //生效时间控件
    //selector.$vcrtTime_Search().datepicker({
    //    format: "yyyy-mm-dd",
    //    autoclose: true,
    //    todayHighlight: true,
    //    orientation: "bottom right",
    //});

    //加载部门表
    function LoadTable() {
        var OrganizationListSource =
            {
                datatype: "json",
                datafields:
                [
                    { name: "checkbox", type: null },
                    { name: 'OrganizationName', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'CreatedDate', type: 'string' },
                    { name: 'CreatedUser', type: 'string' },
                    { name: 'ChangeDate', type: 'datetime' },
                    { name: 'ChangeUser', type: 'string' },
                    { name: 'ParentVguid', type: 'string' },
                    { name: 'Vguid', type: 'string' }
                ],
                hierarchy:
                {
                    keyDataField: { name: 'Vguid' },
                    parentDataField: { name: 'ParentVguid' }
                },
                id: "Vguid",//主键
                async: true,
                //data: {},
                url: "/BasicDataManagement/OrganizationManagement/GetOrganizationTreeList"    //获取数据源的路径
            };
        var dataAdapter = new $.jqx.dataAdapter(OrganizationListSource);
        //创建信息列表（主表）
        selector.$OrganizationGrid().jqxTreeGrid(
            {
                pageable: false,
                width: "100%",
                height: 450,
                pageSize: 10,
                ready: function () {
                    selector.$OrganizationGrid().jqxTreeGrid('expandRow');
                },
                serverProcessing: true,
                pagerButtonsCount: 10,
                source: dataAdapter,
                theme: "office",
                ready: function () {
                    //默认展开节点
                    var rows = selector.$OrganizationGrid().jqxTreeGrid('getRows');
                    var traverseTree = function (rows) {
                        for (var i = 0; i < rows.length; i++) {
                            if (rows[i].records) {
                                idValue = rows[i][idColumn];
                                selector.$OrganizationGrid().jqxTreeGrid('expandRow', idValue);
                                traverseTree(rows[i].records);
                            }
                        }
                    };
                    var idColumn = selector.$OrganizationGrid().jqxTreeGrid('source')._source.id;
                    traverseTree(rows);
                    //selector.$OrganizationGrid().jqxTreeGrid('expandRow', '2');
                },
                columns: [
                  //{ width: 35, text: "", datafield: "checkbox", align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc, renderer: rendererFunc, rendered: renderedFunc, autoRowHeight: false },
                  { text: '部门名称', width: 500, datafield: 'OrganizationName', align: 'center', cellsAlign: 'center' },
                  { text: '描述', datafield: 'Description', align: 'center', cellsAlign: 'center' },
                  //{ text: '创建时间', datafield: 'CreatedDate', align: 'center', cellsAlign: 'center' },
                  { text: '创建人', width: 150, datafield: 'CreatedUser', align: 'center', cellsAlign: 'center' },
                  //{ text: '修改时间', datafield: 'ChangeDate', align: 'center', cellsAlign: 'center', cellsRenderer: cellsRendererFunc },
                  { text: '修改人', width: 150, datafield: 'ChangeUser', align: 'center', cellsAlign: 'center' },
                  { text: 'VGUID', datafield: 'Vguid', hidden: true }
                ],
                //columnGroups: [
                //  { text: 'OrganizationName', name: 'OrganizationName' }
                //]
            });
    }

    function cellsRendererFunc(row, column, value, rowData) {
        var dateConvert = rowData.ChangeDate.replace("/Date(", "").replace(")/", "");
        var date = new Date(parseInt(dateConvert));
        var year = date.getFullYear() + "-";
        var month = date.getMonth() + 1 + "-";
        var day = date.getDate() + " ";
        var hour = date.getHours() + ":";
        var minute = date.getMinutes();
        var dateDisplay = year + month + day + hour + minute;
        return dateDisplay;
    }

    var tool = {

    }; // tool end
};


$(function () {
    var page = new $page();
    page.init();

})


