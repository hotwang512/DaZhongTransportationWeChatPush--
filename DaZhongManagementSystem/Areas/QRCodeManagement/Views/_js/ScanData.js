var selector = {

   

    $txtHidden: function () { return $("#txtHidden") },
    //页面
    $ID: function () { return $("#ID") },
    $JobNumber: function () { return $("#JobNumber") },
    $ServiceNumber: function () { return $("#ServiceNumber") },
    $OwnedFleet: function () { return $("#OwnedFleet") },
    $Age: function () { return $("#Age") },
    $UserVguid: function () { return $("#UserVguid") },
    $LicensePlate: function () { return $("#LicensePlate") },
    $Name: function () { return $("#Name") },
    $Sex: function () { return $("#Sex") },
    $ScanUser: function () { return $("#ScanUser") },
    $PhoneNumber: function () { return $("#PhoneNumber") },
    $ScanDate: function () { return $("#ScanDate") },
    $CreatedDate: function () { return $("#CreatedDate") },
    $MachineCode: function () { return $("#MachineCode") }
};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        LoginHtml();

    }; //addEvent end




};
//生成
function LoginHtml() {
    var vguid=selector.$txtHidden().val();
    $.ajax({
        url: "/QRCodeManagement/ScanData/Save?vguid=" + vguid,
        traditional: true,
        type: "post",
        success: function (model) {
         
            selector.$Age().text(model.Age);
            selector.$ID().text(model.ID);
            selector.$JobNumber().text(model.JobNumbe);
            selector.$ServiceNumber().text(model.ServiceNumber);
            selector.$OwnedFleet().text(model.OwnedFleet);
            //selector.$UserVguid().text(model.UserVguid);
            selector.$LicensePlate().text(model.LicensePlate);
            selector.$Name().text(model.Name);
            selector.$ScanUser().text(model.ScanUser);
            selector.$PhoneNumber().text(model.PhoneNumber);
            selector.$ScanDate().text(ChangeDateFormat(model.ScanDate,false));
            selector.$CreatedDate().text(ChangeDateFormat(model.CreatedDate,false));
            if (model.Sex=="1") {
                model.Sex = "女";
            } else {
                model.Sex = "男";
            }
            selector.$Sex().text(model.Sex);
            selector.$MachineCode().text(model.MachineCode);
          
        }
    });
}



$(function () {
    var page = new $page();
    page.init();

});


