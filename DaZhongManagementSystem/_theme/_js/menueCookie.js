var CookieHelper = {
    SaveCookie: function (menueId) { //保存信息至Cookie        
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = "BiomobieAppSystemMenue=" + escape(menueId) + ";expires=" + exp.toGMTString() + ";path=/";

        // window.location.href = Config.Html.html_Index;
    },
    GetCookie: function () {  //获取Cookie中信息
        var arr, reg = new RegExp("(^| )BiomobieAppSystemMenue=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg)) {
            return unescape(arr[2]);
        }
        else {
            return null;
            // window.location.href = Config.Html.html_Login;
        }
    },
    CheckCookie: function () {  //检测cookie是否存在      
        if (document.cookie.indexOf("BiomobieAppSystemMenue=") <= -1) {
            return false;
        }
        else {
            return true;
        }
    },
    ClearCookie: function () {//清空Cookie中的用户信息
        var expires = new Date();
        expires.setTime(expires.getTime() - 1000); //当前时间减去一秒,相当于立即过期(可以增减) 
        document.cookie = "BiomobieAppSystemMenue='';expires=" + expires.toGMTString() + ";path=/"; //expires是对应过期时间的设置,不设这个值,cookie默认在关闭浏览器时失效 

    }
}



var menueHelp = {
    drawMenueStyle: function () {
        var menueId = CookieHelper.GetCookie();       
        $(".menue_body").css("display", "none");
        $("#" + menueId).parents(".menue_body").css("display", "block");
        $(".menue_item").removeClass("munue_ItemSelcted");
        $("#" + menueId).addClass("munue_ItemSelcted");

    }
}


$(function () {

    menueHelp.drawMenueStyle();
});

