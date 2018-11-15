var phoneTity; //身份证验证
var phoneNumber; //工号验证
var phoneBranch; //部门验证

$(function () {
    $("#identity").blur(function () {
        var value = $(this).val();
        var reg = /^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/; //正则判断
        var judge = reg.test(value);
        if (judge == true) {
            phoneTity = 1;
            $(".identity").html("");
        } else {
            phoneTity = 0;
            $(".identity").html("身份证输入格式有误，请重新输入");
        }
        if (value == "") {
            phoneTity = 0;
            $(".identity").html("输入不能为空");
        }
        if (phoneBranch == 1 && phoneTity == 1) {
            $(".login").addClass("kk");
        } else {
            $(".login").removeClass("kk");
        }

    });
    //判断工号
    $("#number").blur(function () {
        var value = $(this).val();
        var reg = /^[^\~\`\!\@\#\$\%\^\&\*\(\)\-\_\+\=\{\}\[\]\|\\\;\:\'\"\,\.\<\>\/\?]{0,6}$/;
        var judge = reg.test(value);
        if (judge == true) {
            phoneNumber = 1;
            $(".number").html("");
        } else {
            phoneNumber = 0;
        }
        if (value == "") {
            phoneNumber = 0;
        }
        if (phoneBranch == 1 && phoneTity == 1) {
            $(".login").addClass("kk");
        } else {
            $(".login").removeClass("kk");
        }
    });
});

$("#branch").focus(function () {
    var height = $(window).height() - 145 - $(".login").height() - 50;
    $("#treeDeparment").css('height', height + "px");
    $("#treeDeparment").show();
    $("#treeGrid").focus();

});
//蒙版点击高度
$(".form-hide").focus(function () {
    var height = $(window).height() - 145 - $(".login").height() - 50;
    $("form-hide").css('height', height + "px");
});
$("#treeGrid").blur(function () {
    $("#treeDeparment").hide();
});
$(".form-hide").on("click",function() {
    $("#treeDeparment").hide();
})
initTable();

function initTable() {
    var source =
    {
        dataType: "json",
        dataFields: [
            { name: 'Vguid', type: 'string' },
            { name: 'ParentVguid', type: 'string' },
            { name: 'OrganizationName', type: 'string' },
        ],
        hierarchy:
        {
            keyDataField: { name: 'Vguid' },
            parentDataField: { name: 'ParentVguid' }
        },
        id: 'Vguid',
        url:  "/WeChatPush/WeChatValidation/GetOrganization"
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#treeGrid").jqxTreeGrid(
    {
        width: '99%',
        source: dataAdapter,
        showHeader: false,
        ready: function () {
            $("#treeGrid").jqxTreeGrid('expandAll');
        },
        columns: [
          { text: '部门', align: 'left', cellsAlign: 'left', dataField: 'OrganizationName' }
        ]
    });

    $('#treeGrid').on('rowSelect', function (event) {
        var args = event.args;
        // row data.
        var row = args.row;

        strs = row.Vguid;
        organization = $('#treeGrid').jqxTreeGrid('source').records;
        $("#branch").val(row.OrganizationName);
        $("#organizationVguid").val(strs);
        
    });
}

//选择职位下拉框
//$("#dropDownButton").jqxDropDownButton({
//    width: '65%',
//    height: 31
//});
//var clientHeight = document.documentElement.clientHeight;  //当前屏幕高度
//$("#jqxTree").jqxTree({
//    height: (clientHeight - 200),
//    width: '220px'
//    //	checkboxes:true,
//    //	hasThreeStates: true
//});
////var data = null;

var organization;
var strs;
//$.ajax({
//    type: "post",
//    url: $("#openHttpAddress").val() + "/WeChatPush/WeChatValidation/GetOrganization",
//    //url: "/WeChatPush/WeChatValidation/GetOrganization",
//    success: function (data) {
//        organization = data;
//        var source = {
//            datatype: "json",
//            datafields: [{
//                name: 'Vguid'
//            }, {
//                name: 'ParentVguid'
//            }, {
//                name: 'OrganizationName'
//            },
//            { name: 'ID' }],
//            id: 'Vguid',
//            localdata: data
//        };

//        $("#jqxTree").on('select', function (event) {

//            var args = event.args;
//            var item = $('#jqxTree').jqxTree('getItem', args.element);
//            //console.log(item)
//            var id = item.id;
//            strs = id;
//            $("#organizationVguid").val(id);
//            var dropDownContent = '<div style="position: relative; margin-left: -2px;">' + item.label + '</div>';
//            $("#dropDownButton").jqxDropDownButton('setContent', dropDownContent);
//            document.getElementById("dropDownButton").firstChild.data = "";

//        });

//        // create data adapter.
//        var dataAdapter = new $.jqx.dataAdapter(source);
//        // perform Data Binding.
//        dataAdapter.dataBind();
//        var records = dataAdapter.getRecordsHierarchy('Vguid', 'ParentVguid', 'items', [
//                {
//                    name: 'OrganizationName',
//                    map: 'label'
//                },
//                {
//                    name: 'Vguid',
//                    map: 'id'
//                },
//                {
//                    name: 'ParentVguid',
//                    map: 'parentId'
//                },
//                {
//                    name: 'ID',
//                    map: 'value'
//                }
//        ]);
//        $("#jqxTree").jqxTree({
//            source: records,
//            width: '220'
//        });
//        //默认展开所有节点
//        $("#jqxTree").jqxTree('expandAll');
//    }

//});

//判断所在部门
$("#treeGrid").blur(function () {
    if ($("#branch").val() != "") {
        phoneBranch = 1;
        $(".branch").html("");
    } else {
        phoneBranch = 0;
        $(".branch").html("所选部门不能为空");
    }
    if (phoneBranch == 1 && phoneTity == 1) {
        $(".login").addClass("kk");
    } else {
        $(".login").removeClass("kk");
    }
});
//$("#jqxTree").blur(function () {
//    var value = $("#dropDownButtonContentdropDownButton div").text();
//    if (value != "") {
//        phoneBranch = 1;
//        $(".branch").html("");
//    } else {
//        phoneBranch = 0;
//        $(".branch").html("所选部门不能为空");
//    }
//    //判断是否满足条件
//    console.log(phoneBranch);
//    console.log(phoneTity);
//    console.log(phoneNumber);
//    if (phoneBranch == 1 && phoneTity == 1) {
//        $(".login").addClass("kk");
//    } else {
//        $(".login").removeClass("kk");
//    }
//});
//var firstChild = $("#dropDownButton>:first");
//firstChild.css({ "position": "absolute", "top": "0" });
//点击审核按钮
$(".login").click(function () {
    if ($(".login").hasClass("kk")) {
        var count = 0;
        for (var i = 0; i < organization.length; i++) {
            if (strs == organization[i].ParentVguid) {
                count++;
            }
        }
        if (count > 0) {
            alert("所选部门不正确！");
        } else {
            $("#Uploading").show();
            $("#login").ajaxSubmit({
                url: $("#openHttpAddress").val() + "/WeChatPush/WeChatValidation/CheckUser",
                data: { userID: $("#userID").val(), accessToken: $("#accessToken").val(), position: $("#position").val(), mobilePhone: $("#mobilePhone").val() },
                type: "post",
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    if (data.isSuccess == true && data.respnseInfo == 1) {
                        $("#Uploading").hide();
                        $("#successIng").show();
                    }
                    else {
                        $("#Uploading").hide();
                        alert(data.respnseInfo);
                    }
                }
            });
        }
    } else {
        alert("请按照正确格式填写信息");
    }
});


