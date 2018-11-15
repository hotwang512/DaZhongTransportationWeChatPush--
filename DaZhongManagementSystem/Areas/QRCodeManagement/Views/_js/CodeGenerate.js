var selector = {

    $grid: function () { return $("#grid") },
    $systemGrid: function () { return $("#systemGrid") },
    //按钮
    $btnNew: function () { return $("#btnNew") },
    $btnSysNew: function () { return $("#btnSysNew") },
    $btnReset: function () { return $("#btnReset") },
    $btnDelete: function () { return $("#btnDelete") },
    $btnSysDelete: function () { return $("#btnSysDelete") },
    $btnSave: function () { return $("#btnSave") },
    $imgCode: function () { return $("#imgCode") },
    $spSystem: function () { return $(".spSystem") },
    $txtHiddenVguid: function () { return $("#txtHiddenVguid") },
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
var sysConfigId;
var configArray = [];
//配置对象
var ConfigModel = function (id, configValue, configDesc, sysConfigId) {
    this.ID = id;
    this.ConfigValue = configValue;
    this.ConfigDescription = configDesc;
    this.ModifyUser = sysConfigId;
};
var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        initSystemGrid();
        // initGrid();
    }; //addEvent end

    //新增行
    selector.$btnSysNew().on('click', function () {
        var source = selector.$systemGrid().jqxGrid("source");
        var id = 300;
        var newId = id;
        if (source.cachedrecords.length > 0) {
            id = source.cachedrecords[source.cachedrecords.length - 1].ID;
            newId = id + 1;
        }
        sysConfigId = newId;
        var datarow = {};
        datarow["ID"] = newId;
        datarow["ConfigValue"] = "";
        datarow["ConfigDescription"] = "";
        selector.$systemGrid().jqxGrid('addrow', null, datarow);

    });
    //新增行
    selector.$btnNew().on('click', function () {
        var source = selector.$grid().jqxGrid("source");
        if (!sysConfigId) {
            jqxNotification("请先配置系统参数！", null, "error");
            return false;
        }
        var id = parseInt(50 + parseInt(sysConfigId - 300 + "0"));
        var newId = id;
        if (source.cachedrecords.length > 0) {
            id = source.cachedrecords[source.cachedrecords.length - 1].ID;
            newId = id + 1;
        }
        var datarow = {};
        datarow["ID"] = newId;
        datarow["ConfigValue"] = "";
        datarow["ConfigDescription"] = "";
        datarow["ModifyUser"] = sysConfigId;
        // selector.$grid().jqxGrid('addrow', null, datarow);
        var config = new ConfigModel(newId, "", "", sysConfigId);
        configArray.push(config);
        //initGrid(-1, configArray);
        appendConfig(sysConfigId, configArray);
    });
    //加载配置系统参数的表格
    function initSystemGrid() {
        var source =
            {
                datatype: "json",
                datafields: [
                    { name: 'ConfigValue', type: 'string' },
                    { name: 'ConfigDescription', type: 'string' },
                    { name: 'ID', type: 'string' },

                ],
                id: "ID",
                url: "/QRCodeManagement/CodeGenerate/GetSysConfigurations",
                addrow: function (rowid, rowdata, position, commit) {
                    commit(true);
                },
                updaterow: function (rowid, rowdata, result) {
                    result(true);
                },
                deleterow: function (rowid, commit) {
                    commit(true);
                }
            };
        var adapter = new $.jqx.dataAdapter(source);
        selector.$systemGrid().jqxGrid(
          {
              width: '100%',
              source: adapter,
              selectionmode: 'checkbox',
              editable: true,
              pageable: false,
              autoheight: true,
              altrows: true,
              rowsheight: 30,
              columnsheight: 30,
              columns: [
                 { text: 'ID', datafield: 'ID', hidden: true },
                  {
                      text: '参数名', datafield: 'ConfigValue', columntype: 'textbox', width: '43%', align: 'center', cellsAlign: 'center',
                  },
                  {
                      text: '参数值', datafield: 'ConfigDescription', width: '43%', align: 'center', cellsAlign: 'center'
                  },
                   {
                       text: '操作', datafield: '', width: '10%', align: 'center', cellsAlign: 'center', editable: false, cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                           return "<button   onClick=edit('" + row + "')  class='btn btn-warning' style='margin:0.5% 0 0 25%;padding:3px 12px'>配置参数</button>";
                       }
                   },
              ]
          });

        selector.$systemGrid().on("bindingcomplete", function (event) {
            edit("0");
        });
    }

    selector.$btnSysDelete().on("click", function () {
        var rowIDs = selector.$systemGrid().jqxGrid('getselectedrowindexes');
        var row = [];
        var selection = [];
        for (var i = 0; i < rowIDs.length; i++) {
            var rowid = selector.$systemGrid().jqxGrid('getrowid', rowIDs[i]);
            var data = selector.$systemGrid().jqxGrid('getrowdatabyid', rowid);
            row.push(rowid);
            selection.push(data.ID);
        }
        if (rowIDs.length <= 0) {
            jqxNotification("请选择你要删除的数据!", null, "error");
            return false;
        }
        selector.$systemGrid().jqxGrid('deleterow', row);
        $.ajax({
            url: "/QRCodeManagement/CodeGenerate/DeleteQRCodeConfig",
            type: "post",
            data: { ids: selection },
            dataType: "json",
            traditional: true,
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "1":
                        jqxNotification("删除成功!", null, "success");
                        selector.$systemGrid().jqxGrid('updatebounddata');
                        for (var j = 0; j < selection.length; j++) {
                            configArray.map(function (item, index, array) {
                                if (item.ModifyUser == selection[j]) {
                                    configArray.splice(index, 1);
                                    //return item;
                                }
                            });
                            initGrid(0);
                        }
                        break;
                    case "2":
                        jqxNotification("删除失败!", null, "error");
                        break;
                }
            }
        });
    });
    //删除按钮
    selector.$btnDelete().on("click", function () {
        var rowIDs = selector.$grid().jqxGrid('getselectedrowindexes');
        var row = [];
        var selection = [];
        for (var i = 0; i < rowIDs.length; i++) {
            var rowid = selector.$grid().jqxGrid('getrowid', rowIDs[i]);
            var data = selector.$grid().jqxGrid('getrowdatabyid', rowid);
            row.push(rowid);
            selection.push(data.ID);
        }
        if (rowIDs.length <= 0) {
            jqxNotification("请选择你要删除的数据!", null, "error");
            return false;
        }
        selector.$grid().jqxGrid('deleterow', row);
        $.ajax({
            url: "/QRCodeManagement/CodeGenerate/DeleteQRCodeConfig",
            type: "post",
            data: { ids: selection },
            dataType: "json",
            traditional: true,
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "1":
                        jqxNotification("删除成功!", null, "success");
                        selector.$grid().jqxGrid('updatebounddata');
                        for (var j = 0; j < selection.length; j++) {
                            configArray.map(function (item, index, array) {
                                if (item.ID == selection[j]) {
                                    configArray.splice(index, 1);
                                    //return item;
                                }
                            });
                        }
                        appendConfig(0);
                        break;
                    case "2":
                        //  jqxNotification("删除失败!", null, "error");
                        break;
                }
            }
        });
    });
    //保存
    selector.$btnSave().on('click', function () {
        var source = selector.$grid().jqxGrid("source");
        var sysSource = selector.$systemGrid().jqxGrid("source");
        for (var i = 0; i < source.cachedrecords.length; i++) {
            if (source.cachedrecords[i].ConfigValue == "" || source.cachedrecords[i].ConfigDescription == "") {
                jqxNotification("参数不能为空!", null, "error");
                return false;
            }
        }
        $.ajax({
            url: "/QRCodeManagement/CodeGenerate/SaveQRCodeConfig",
            data: { configStr: JSON.stringify(configArray), sysConfigStr: JSON.stringify(sysSource.cachedrecords) },
            type: "post",
            dataType: "json",
            success: function (msg) {
                switch (msg.respnseInfo) {
                    case "1":
                        jqxNotification("保存成功!", null, "success");
                     //   reGenerate();
                        break;
                    case "2":
                        jqxNotification("保存失败!", null, "error");
                        break;
                }
            }
        });
    });
    //重新生成按钮事件
    selector.$btnReset().on('click', function () {
        WindowConfirmDialog(reGenerate, "您确定要重新生成吗？", "确认框", "确定", "取消");
    });

};
//重新生成
function reGenerate() {
    showLoading();//显示加载等待框
    $.ajax({
        url: "/QRCodeManagement/CodeGenerate/ReGenerate",
        type: "post",
        success: function (msg) {
            switch (msg) {
                case "0":
                    jqxNotification("生成失败！", null, "error");
                    break;
                default:
                    jqxNotification("生成成功！", null, "success");
                    var img = "<img src=data:image/jpeg;base64," + msg + " alt=\"Base64 encoded image\">";
                    // var img = "<img src=\"/UpLoadFile/QRCode/" + selector.$txtHiddenVguid().val() + ".jpg\"  alt=\"二维码\"/>";
                    $("#txtDesc").siblings().remove();
                    $("#txtDesc").before(img);
                    break;
            }
            closeLoading();//关闭加载等待框
        }
    });
}
var list = ['UserID', 'IDNumber', 'Name', 'ID', 'Age', 'Sex', 'JobNumber', 'ServiceNumber', 'OwnedFleet', 'LicensePlate',
    'PhoneNumber', 'ApprovalStatus', 'DepartmenManager'];
//初始化表格  

function edit(row) {
    //根据行号获取点击行的ID。
    var rowid = selector.$systemGrid().jqxGrid('getrowid', row);
    var data = selector.$systemGrid().jqxGrid('getrowdatabyid', rowid);
    if (data != undefined && (data.ConfigValue == "" || data.ConfigDescription == "")) {
        jqxNotification("参数不能为空！", null, "error");
        return false;
    }

    selector.$spSystem().text(data.ConfigValue);
    $("#spLink").text(data.ConfigDescription);
    sysConfigId = rowid;
    initGrid(rowid, null);  //第一次加载
}

function initGrid(rowid, rowData) {
    if (rowid == -1) { //说明已经加载过，无需重新加载
        appendConfig(rowid, rowData);
    } else {
        $.ajax({
            url: "/QRCodeManagement/CodeGenerate/GetConfigurations",
            type: "post",
            dataType: "json",
            data: {
                sysConfigId: rowid
            },
            success: function (data) {
                var configStr = [];
                for (var i = 0; i < data.length; i++) {
                    var id = data[i].ID;
                    var configValue = data[i].ConfigValue;
                    var configDesc = data[i].ConfigDescription;
                    var sysConfigId = data[i].ModifyUser;
                    var config = new ConfigModel(id, configValue, configDesc, sysConfigId);
                    var index = JSON.stringify(configArray).indexOf(JSON.stringify(config));
                    configStr.push(configValue + "=" + configDesc);

                    if (index == -1) {
                        configArray.push(config);
                    }
                }
                var link = $("#spLink").text();
                $("#spLink").text(link + "?" + configStr.join("&"));
                appendConfig(rowid, rowData);
            }
        });
    }
}

function appendConfig(rowid, rowData) {
    if (rowid != -1) {
        var data = [];
        for (var j = 0; j < configArray.length; j++) {
            if (configArray[j].ModifyUser == rowid) {
                data.push(configArray[j]);
            }
        }
        rowData = data;
    }
    var ordersSource =
       {
           datatype: "json",
           datafields: [
               { name: 'ConfigValue', type: 'string' },
               { name: 'ConfigDescription', type: 'string' },
               { name: 'ID', type: 'string' },
               { name: 'ModifyUser', type: 'string' }  //这个字段用作存储父ID
           ],
           id: "ID",
           localdata: rowData,
           addrow: function (rowid, rowdata, position, commit) {
               commit(true);
           },
           updaterow: function (rowid, rowdata, result) {
               var config = new ConfigModel(rowid, rowdata.ConfigValue, rowdata.ConfigDescription, rowdata.ModifyUser);
               if (configArray.length > 0) {
                   var isExist = configArray.some(function (item, index, array) {  //判断是否有这项
                       return item.ID == rowid;
                   });
                   if (isExist) {
                       for (var i = 0; i < configArray.length; i++) {
                           if (configArray[i].ID == rowid) {
                               configArray.splice(i, 1);
                               break;
                           }
                       }
                   }
               }
               configArray.push(config);
               result(true);
           },
           deleterow: function (rowid, commit) {
               commit(true);
           }
       };
    var ordersAdapter = new $.jqx.dataAdapter(ordersSource);
    selector.$grid().jqxGrid(
      {
          width: '100%',
          source: ordersAdapter,
          selectionmode: 'checkbox',
          editable: true,
          pageable: false,
          autoheight: true,
          altrows: true,
          rowsheight: 30,
          columnsheight: 30,
          columns: [
             { text: 'ID', datafield: 'ID', hidden: true },
              {
                  text: '参数名', datafield: 'ConfigValue', columntype: 'textbox', width: '48%', align: 'center', cellsAlign: 'center',
              },
              {
                  text: '参数值', datafield: 'ConfigDescription', width: '49%', columntype: 'combobox', align: 'center', cellsAlign: 'center',
                  createeditor: function (row, column, editor) {
                      editor.jqxComboBox({ autoDropDownHeight: true, source: list, promptText: "请选择:" });
                  },
                  cellvaluechanging: function (row, column, columntype, oldvalue, newvalue) {
                      if (newvalue.indexOf('DB') == -1) {
                          if (newvalue == "") {
                              return oldvalue;
                          } else {
                              if ($.inArray(newvalue, list) == -1) {
                                  return newvalue;
                              }
                              return "{DB:" + newvalue + "}";
                          }
                      }
                  }
              },
          ]
      });
}
$(function () {
    var page = new $page();
    page.init();

});


