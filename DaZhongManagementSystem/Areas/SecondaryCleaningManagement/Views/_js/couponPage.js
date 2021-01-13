var code = $("#UserVGUID").val();
var $page = function () {

    this.init = function () {
        addEvent();
    };
    var selector = this.selector = {};

    function addEvent() {
        loadCouponSetInfo();
    }
};

$(function () {
    var page = new $page();
    page.init();
});

function js_method(type) {
    var index = Number(type);
    $(".style-one").eq(index).addClass("have");
    $(".nick2").eq(index).addClass("have");
    $(".receive").eq(index).html("已使用");
    $(".nick").eq(index).css("color", "#C1C1C1");
}
function loadCouponSetInfo() {
    $.ajax({
        url: "/SecondaryCleaningManagement/CouponPage/GetCouponSetInfo",
        data: { code: code },
        type: "post",
        success: function (msg) {
            if (msg != null) {
                createCouponDiv(msg);
            }
        }
    })
}
function createCouponDiv(msg) {
    var html = "";
    for (var i = 0; i < msg.length; i++) {
        var rightsName = msg[i].RightsName;
        var description = msg[i].Description;
        var type = msg[i].Type;
        var startValidity = timesTampFunc(msg[i].StartValidity);
        var endValidity = timesTampFunc(msg[i].EndValidity);
        var vguid = msg[i].VGUID;
        var status = msg[i].Status;
        var cssDiv = "";
        if (status == "已使用") {
            html += '<div class="coupon-item">' +
                    '<div class="style-one have">' +
                '<div class="info-box">' +
                '<table style="width:100%;text-align: center;">' +
                    '<tr>' +
                        '<td style="text-align: left;"><div class="nick" style="color:#C1C1C1">' + rightsName + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of">免</div></td>' +
                    '</tr>' +
                    '<tr>' +
                        '<td style="text-align: left;"><div class="nick2 have">' + description + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of">费</div></td>' +
                    '</tr>' +
                     '<tr>' +
                        '<td style="text-align: left;"><div class="nick2 have">' + startValidity + '--' + endValidity + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of"></div></td>' +
                    '</tr>' +
                '</table>' +
                '</div>' +
                '<a href="#" class="get-btn">' +
                    '<span class="receive">' + status + '</span>' +
                '</a>' +
                '</div>' +
              '</div>';
        } else if (status == "未使用") {
            html += '<div class="coupon-item">' +
                    '<div class="style-one">' +
                '<div class="info-box">' +
                '<table style="width:100%;text-align: center;">' +
                    '<tr>' +
                        '<td style="text-align: left;"><div class="nick">' + rightsName + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of">免</div></td>' +
                    '</tr>' +
                    '<tr>' +
                        '<td style="text-align: left;"><div class="nick2">' + description + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of">费</div></td>' +
                    '</tr>' +
                     '<tr>' +
                        '<td style="text-align: left;"><div class="nick2">' + startValidity + '--' + endValidity + '</div></td>' +
                        '<td style="width: 50px;"><div class="lay of"></div></td>' +
                    '</tr>' +
                '</table>' +
                '</div>' +
                '<a href="#" class="get-btn">' +
                    '<span class="receive">' + status + '</span>' +
                '</a>' +
                '</div>' +
              '</div>';
        }
        
    }
    $("#CouponDiv").append(html);
}
function timesTampFunc(timestamp) {
    var date = new Date(parseInt(timestamp.replace("/Date(", "").replace(")/", ""), 10));
    Y = date.getFullYear() + '-';
    M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    D = (date.getDate() < 10 ? '0' + (date.getDate()) : date.getDate()) + ' ';
    h = (date.getHours() < 10 ? '0' + (date.getHours()) : date.getHours()) + ':';
    m = (date.getMinutes() < 10 ? '0' + (date.getMinutes()) : date.getMinutes()) + ':';
    s = (date.getSeconds() < 10 ? '0' + (date.getSeconds()) : date.getSeconds());
    //var NewDtime = Y + M + D + h + m + s;
    var NewDtime = Y + M + D;
    return NewDtime;
}