var selector = {
    $frmtable: function () { return $("#frmtable") },

    //按钮
    $btnSave: function () { return $("#btnSave") },
    $btnCancel: function () { return $("#btnCancel") },
    $btnCreate: function () { return $("#btnCreate") },

    //界面元素
    $companyName: function () { return $("#CompanyName") },
    $address: function () { return $("#Address") },
    $location: function () { return $("#Location") },
    $contactPerson: function () { return $("#ContactPerson") },
    $contactNumber: function () { return $("#ContactNumber") },
    $isEdit: function () { return $("#isEdit") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        
    }; //addEvent end

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/SecondaryCleaningManagement/CleaningCompany/Index";
    });

    //保存按钮事件
    selector.$btnSave().on('click', function () {
        var validateError = 0;//未通过验证的数量

        if (!Validate($(selector.$companyName()))) {
            validateError++;
        }
        if (!Validate(selector.$contactPerson())) {
            validateError++;
        }
        if (!Validate(selector.$contactNumber())) {
            validateError++;
        }
        if (validateError <= 0) {
            selector.$frmtable().ajaxSubmit({
                url: '/SecondaryCleaningManagement/CleaningCompanyDetail/SaveCleaningCompany',
                type: "post",
                async: false,
                data: { isEdit: selector.$isEdit().val() },
                success: function (msg) {
                    if (msg.respnseInfo == "2") {
                        jqxNotification("公司名称已存在，请重新输入！", null, "error");
                        return;
                    }
                    if (msg.respnseInfo == "0") {
                        jqxNotification("保存失败！", null, "error");
                        return;
                    } else {
                        jqxNotification("保存成功！", null, "success");
                        var val = msg.respnseInfo.split(",");
                        if (selector.$isEdit().val() == "False") {
                            $("#Vguid").val(val[1]);
                            selector.$isEdit().val(true);
                            window.location.href = "/SecondaryCleaningManagement/CleaningCompanyDetail/Index?Vguid=" + val[1] + "&isEdit=true";
                        } else {
                            window.location.reload();
                        }
                        $("#QRCodeImg").attr('src',"");
                        $("#QRCodeImg").attr('src', val[0]);
                        $("#QRCodeImg").show();
                        $("#QRCode").val(val[0]);
                    }
                }
            });
        }
    });

    //生成二维码
    selector.$btnCreate().on('click', function () {
        selector.$frmtable().ajaxSubmit({
            url: "/SecondaryCleaningManagement/CleaningCompanyDetail/CreateQRCode",
            type: "post",
            data: { },
            dataType: "json",
            traditional: true,
            success: function (msg) {
                if (msg != "" && msg != null) {
                    $("#QRCodeImg").attr('src', msg);
                    $("#QRCodeImg").show();
                    $("#QRCode").val(msg);
                } else {
                    jqxNotification("生成失败!", null, "error");
                }
            }
        });
    });
};

$(function () {
    var page = new $page();
    page.init();
})