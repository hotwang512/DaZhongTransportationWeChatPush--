var _tempHeight = 70;
var _tempWidth_B = 265;
var _tempWidth_S = 75;

$(function () {
    doInitMenu();
    //点击菜单显示的效果
    $("#sidebar .nav >li > a.dropdown-toggle").hover(function () {

    }, function () {

    }).click(function () {
        var className = $(this)[0].parentNode.className;
        if (className.indexOf("open") != -1) {//当前是打开的，则需要关闭
            var $submenu = $(this).next(".submenu");
            $submenu.slideUp(500);
            $(this).parent("li").removeClass("open");
            $(this).css({ "color": "#949494" });
        } else {//当前是关闭的，则需要打开
            $(this).css({ "color": "#E0E0E0" });
            $(this).parent("li").addClass("open");
            var $submenu = $(this).next(".submenu");
            $submenu.slideDown(500);
            //关闭其他打开的菜单
            var tbIndex = $("#sidebar .nav >li > a.dropdown-toggle").index($(this));
            closeOther(tbIndex);
        }
    });
    //菜单项移动效果
    $(".submenu > li").hover(function () {
        $(this).children("a").css({ "color": "#E0E0E0" });
        var imgUrl = '/_theme/images/dot_1.png'.GetRelivatePath();
        $(this).children("a").find("i.icon-double-angle-right").css({ "background": "url('" + imgUrl + "')" });
    }, function () {
        var COOKIE_NAME = "currentMenu_Swatch"; //currentMenu:0,1;  主菜单、二级菜单
        var initdatas = $.cookie(COOKIE_NAME)
        var cdata = $(this).attr("cdata");
        if (cdata == initdatas) {

        } else {
            $(this).children("a").css({ "color": "#949494" });
            var imgUrl = '/_theme/images/dot_2.png'.GetRelivatePath();
            $(this).children("a").find("i.icon-double-angle-right").css({ "background": "url('" + imgUrl + "')" });
        }
    });
    $(".dropdown-menu>li>a").hover(function () {
        $(this).children("div.i_centertext").css({ "color": "#E0E0E0" });
    }, function () {
        $(this).children("div.i_centertext").css({ "color": "#949494" });
    });
    $("#i_loginouttext").click(function () {
        var urlDetail = '/user/login'.GetRelivatePath();
        location.href = urlDetail + "?IsDomain=false";
    });
    //语言切换英文切换成中文
    $("#i_lanencenter").click(function () {
        var type = "EN";
        var data = { "type": type };
        var urlDetail = '/user/Language'.GetRelivatePath();
        $.ajax({
            type: "post",
            url: urlDetail,
            data: data,
            traditional: true,
            dataType: "json",
            success: function (msg) {
                if (msg.isSuccess == true) {
                    location.reload();
                }
            }, error: function (msg) {
                console.log(msg);
            }
        })
    });
    //语言切换英文切换成中文
    $("#i_lanchcenter").click(function () {
        var type = "CH";
        var data = { "type": type };
        var urlDetail = '/user/Language'.GetRelivatePath();
        $.ajax({
            type: "post",
            url: urlDetail,
            data: data,
            traditional: true,
            dataType: "json",
            success: function (msg) {
                if (msg.isSuccess == true) {
                    location.reload();
                }
            }, error: function (msg) {
                console.log(msg);
            }
        })
    });

    //折叠
    $("#i_extend").click(function () {
        silderFunc();
        $smart.extendInterface();
        $smart.onresizeHandler();
        //resizegridrender();
    });
    //小菜单移动效果
    //Small 菜单移动
    $(".sidebar_small_ul_li").hover(function () {
        if (!$(this).find(".sidebar_menu_title_sel").length == 1) {
            doSHover($(this), $(".sidebar_small_ul_li").index($(this)) + 1);
        }
        var $menuContent = $(this).find(".sidebar_small_ul_ul");
        if ($menuContent.is(":hidden")) {
            $menuContent.show();
            var theIndex = $(".sidebar_small_ul_li").index($(this));
            closeSOther(theIndex);
        } else {
            //$menuContent.hide();
        }
    }, function () {
        doSHoverOut($(this), $(".sidebar_small_ul_li").index($(this)) + 1);
        var $menuContent = $(this).find(".sidebar_small_ul_ul");
        if ($menuContent.is(":hidden")) {
        } else {
            $menuContent.hide();
        }
    })
    //SmallMenu Item移动
    $(".sidebar_small_ul_ul_li").hover(function () {
        $(this).css("background-color", "#666666");
        $(this).find(".sidebar_small_a").css("color", "rgb(224, 224, 224)");
    }, function () {
        $(this).css("background-color", "#2D2C32");
        $(this).find(".sidebar_small_a").css("color", "rgb(148, 148, 148)");
    })
})
function resizegridrender() {
    //var modalSize = $(".jqx-window-modal").size();
    //var divBrowerGrid = $("#divBrowerGrid");
    //if (divBrowerGrid.size() > 0) {
    //    var isCompleted = $("#divBrowerGrid").jqxDataTable('isBindingCompleted');
    //    if (isCompleted)
    //        divBrowerGrid.jqxDataTable('render');
    //}
}
function resizegrid() {
    //    var modalSize = $(".jqx-window-modal").size();
    //    var tempModalSize = 0;
    //    $.each($(".jqx-window-modal"), function (i, obj) {
    //        if (!$(".jqx-window-modal").eq(i).is(':hidden')) {
    //            tempModalSize++;
    //        }
    //    })
    var divBrowerGrid = $("#divBrowerGrid");
    //    if (divBrowerGrid.size() > 0 && modalSize == 0) {
    //        var isCompleted = $("#divBrowerGrid").jqxDataTable('isBindingCompleted');
    //        if (isCompleted)
    //            divBrowerGrid.jqxDataTable('render');
    //    }
    //if (divBrowerGrid.find(".jqx_datatable_checkbox_all").size() > 0)
        //divBrowerGrid.find(".jqx_datatable_checkbox_all").jqxCheckBox('checked', false);
}
function doSHover($this, i) {
    $this.find(".sidebar_small_title_c").addClass("menu_hover");
    $this.find(".sidebar_smenu_title_ico").addClass("sidebar_menu_title_icon" + i + "_hover");
}
function doSHoverOut($this, i) {
    $this.find(".sidebar_small_title_c").removeClass("menu_hover");
    $this.find(".sidebar_smenu_title_ico").removeClass("sidebar_menu_title_icon" + i + "_hover");
}

function doInitMenu() {
    //初始化框架的宽高
    var divHeight = document.documentElement.clientHeight - _tempHeight;
    //获取miancontent的高度。如果miancontent的高度大于窗体高度，则使用miancontent
    var mainHeight = $("#mainContent").outerHeight(true);
    //    if (mainHeight > divHeight) {
    //        $("#sidebar_big").css("height", mainHeight + 110);
    //        $("#sidebar_small").css("height", mainHeight + 110);
    //    } else {
    //        $("#sidebar_big").css("height", divHeight);
    //        $("#sidebar_small").css("height", divHeight);
    //    }

    $("#sidebar_big").css("height", divHeight);
    $("#sidebar_small").css("height", divHeight);
    //$("#mainContent").css({ "height": divHeight });

    var COOKIE_NAME = "currentMenu_Swatch"; //currentMenu:0,1;  主菜单、二级菜单
    var COOKIE_ISHIDE = "currentMenuHide_Swatch"; //currentMenuHide  true:显示  false:隐藏
    var urlDetail_2 = '/_theme/images/dot_2.png'.GetRelivatePath();
    $("#sidebar_big .icon-double-angle-right").css({ "background": "url('" + urlDetail_2 + "')" });
    if ($.cookie(COOKIE_NAME)) {
        var initdatas = $.cookie(COOKIE_NAME).split(','); //分割菜单
        var $this;
        //获取主菜单索引
        if (initdatas.length > 0) {
            $this = $($("ul.submenu")[initdatas[0]]); //获取主菜单
            $index = parseInt(initdatas[1], 10);
            $this.parent("li").addClass("open");
            $this.prev("a.dropdown-toggle").css({ "color": "#E0E0E0" });
            $this.slideDown(500);
            $this.find("a").eq($index).css({ "color": "#E0E0E0" });
            var urlDetail_1 = '/_theme/images/dot_1.png'.GetRelivatePath();
            $this.find("a").eq($index).find("i.icon-double-angle-right").css({ "background": "url('" + urlDetail_1 + "')" });
        }
        //***********************折叠的记忆处理*****************
        if ($.cookie(COOKIE_ISHIDE) == "true") {
            $("#sidebar_big").hide();
            $("#sidebar_space").hide();
            $("#logo").hide();
            $("#sidebar_small").show();
            var divWidth = document.documentElement.clientWidth - _tempWidth_S;
            //$("#sidebar").css("width", 70);
            $("#mainContent").css({ "width": divWidth });
            $("#mainContent").css({ "left": _tempWidth_S });
            $smart.onresizeHandler();
        } else {
            $("#sidebar_small").hide();
            $("#sidebar_big").show();
            $("#sidebar_space").show();
            $("#logo").show();
            var divWidth = document.documentElement.clientWidth - _tempWidth_B;
            //$("#sidebar").css("width", 260);
            $("#mainContent").css({ "width": divWidth });
            $("#mainContent").css({ "left": _tempWidth_B });
            $smart.onresizeHandler();
        }
    } else {
        $("#sidebar_small").hide();
        $("#sidebar_big").show();
        $("#sidebar_space").show();
        $("#logo").show();
        var divWidth = document.documentElement.clientWidth - _tempWidth_B;
        //$("#sidebar").css("width", 260);
        $("#mainContent").css({ "width": divWidth });
        $("#mainContent").css({ "left": _tempWidth_B });
        $smart.onresizeHandler();
    }
}


var silderFunc = function () {
    var isHide = $("#sidebar_big").is(":hidden");
    if (!isHide) {
        $("#sidebar_big").hide();
        $("#sidebar_space").hide();
        $("#logo").hide();
        $("#sidebar_small").show();
        var divWidth = document.documentElement.clientWidth - _tempWidth_S;
        //$("#sidebar").css("width", 70);
        $("#mainContent").css({ "width": divWidth });
        $("#mainContent").css({ "left": _tempWidth_S });
        $.cookie("currentMenuHide_Swatch", "true", { path: '/', expires: 10 }); //记录显示主菜单
    } else {
        $("#sidebar_small").hide();
        $("#sidebar_big").show();
        $("#sidebar_space").show();
        $("#logo").show();
        var divWidth = document.documentElement.clientWidth - _tempWidth_B;
        //$("#sidebar").css("width", 260);
        $("#mainContent").css({ "width": divWidth });
        $("#mainContent").css({ "left": _tempWidth_B });
        $.cookie("currentMenuHide_Swatch", "false", { path: '/', expires: 10 }); //记录隐藏主菜单
    }
    $smart.onresizeHandler();
}
//窗口大小的调整
window.onresize = function () {
    $smart.onresizeHandler();
    //resizegrid();
}

//公共接口
var $smart = window.$smart = {
    onresizeHandler: function () {
        //客户端程序调用代码
        var divHeight = document.documentElement.clientHeight - _tempHeight;
        var mainHeight = $("#mainContent").outerHeight(true);
        //        if (mainHeight > divHeight) {
        //            $("#sidebar_big").css("height", mainHeight + 110);
        //            $("#sidebar_small").css("height", mainHeight + 110);
        //        } else {
        //            $("#sidebar_big").css("height", divHeight);
        //            $("#sidebar_small").css("height", divHeight);
        //        }
        $("#sidebar_big").css("height", divHeight);
        $("#sidebar_small").css("height", divHeight);
        //$("#mainContent").css({ "height": divHeight });
        if ($("#sidebar_big").css("display") == "none") {
            var divWidth = document.documentElement.clientWidth - _tempWidth_S;
            //$("#sidebar").css("width", 70);
            $("#mainContent").css({ "width": divWidth });
            $("#mainContent").css({ "left": _tempWidth_S });
        } else {
            var divWidth = document.documentElement.clientWidth - _tempWidth_B;
            //$("#sidebar").css("width", 260);
            $("#mainContent").css({ "width": divWidth });
            $("#mainContent").css({ "left": _tempWidth_B });
        }
        //resizegrid();
    },
    extendInterface: function () {

    }
}
//折叠起其他的菜单项
function closeOther(i) {
    $("#sidebar .nav >li > a.dropdown-toggle").each(function (index, e) {
        if (i != index) {
            var $submenu = $(this).next(".submenu");
            var className = $(this)[0].parentNode.className;
            if (className.indexOf("open") == -1) {
            } else {
                $submenu.slideUp(500);
                $(this).parent("li").removeClass("open");
                $(this).css({ "color": "#949494" });
            }
        }
    })
}
//折叠起小菜单其他的菜单项
function closeSOther(i) {
    $(".sidebar_small_ul_li").each(function (index, e) {
        if (i != index) {
            var $menuContent = $(this).find(".sidebar_small_ul_ul");
            if ($menuContent.is(":hidden")) {
            } else {
                $menuContent.hide();
            }
        }
    })
}
//链接跳转
function doHref(obj) {
    var datas = $(obj).attr("cdata");
    $.cookie("currentMenu_Swatch", datas, { path: '/', expires: 10 });
    location.href = $(obj).attr("href");
}
