var selector = {
    //表单元素
    $txtName: function () { return $("#txtName") },
    $txtPwd: function () { return $("#txtPwd") },

    //按钮
    $btnLogin: function () { return $("#btnLogin") }
};


var $page = function () {

    this.init = function () {
        addEvent();
        selector.$txtName().focus(); //页面加载让用户名文本框获得焦点
    }

    function addEvent() {

        //点击登录按钮
        selector.$btnLogin().on("click", function () {
            var reg = /^[0-9a-zA-Z@@]+$/;
            var userName = selector.$txtName().val();
            var pwd = selector.$txtPwd().val();
            if (userName.length == 0) {
                gritterTips("请输入用户名！");
                selector.$txtName().focus();
            } else if (!reg.test(userName)) {
                gritterTips("无效的用户名！");
                selector.$txtName().val("");
                selector.$txtName().focus();
            }
            else if (pwd.length === 0) {
                gritterTips("请输入密码！");
                selector.$txtPwd().focus();
            } else {
                $.ajax({
                    url: "/Login/Index",
                    data: { userName: userName, pwd: pwd },
                    type: "POST",
                    async: false,
                    success: function (data) {
                        if (data === "ok") {
                            CookieHelper.SaveCookie("UserInfoManagement");
                            window.location.href = "/BasicDataManagement/UserInfo/UserInfo";
                        } else {
                            gritterTips(data);
                        }
                    }, error: function (data) {
                        gritterTips(data);
                    }
                });
            }

        });

        //按回车键也可登录
        $(document).keyup(function (event) {
            if (event.keyCode === 13) {
                selector.$btnLogin().click();
            }
        });

    }//addEvent end

};


//错误消息提示
function gritterTips(tips) {
    jQuery.gritter.add({
        title: tips,
        class_name: "growl-warning",
       // image: "/_theme/images/screen.png",
        sticky: false,
        position: "center",
        time: 800
    });
}

$(function () {
    var page = new $page();
    page.init();
});