var selector = {

    //按钮
    
    $btnCancel: function () { return $("#btnCancel") },

};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        var um = UE.getEditor('txtContent');
        um.setDisabled();
    }; //addEvent end

    //取消按钮事件
    selector.$btnCancel().on('click', function () {
        window.location.href = "/KnowledgeBaseManagement/Formal/FormalList";
    });


};



$(function () {
    var page = new $page();
    page.init();

});


