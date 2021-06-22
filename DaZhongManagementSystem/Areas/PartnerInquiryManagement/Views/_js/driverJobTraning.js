var code = getQueryString("code");
var fleet = decodeURI(getQueryString("fleet"));
var selector = this.selector = {
    $grid: function () { return $("#DriverList") }
};

var $page = function () {

    this.init = function () {
        addEvent();
    };

    function addEvent() {
        //加载车队
        var html = "";
        var fleetList = fleet.split(",");
        if (fleetList.length > 1) {
            html = '<option class="u264_input_option" value="0" selected>全部</option>';
            for (var i = 0; i < fleetList.length; i++) {
                html += '<option class="u264_input_option" value="' + fleetList[i] + '" >' + fleetList[i] + '</option>';
            }
        } else {
            html = '<option class="u264_input_option" value="' + fleet + '" selected>' + fleet + '</option>';
        }
        $("#u264_input").append(html);
        //加载列表
        loadGrid();
        $("#SearchByName").on("change", function () {
            loadGrid();
            //selector.$grid().jqxDataTable('updateBoundData');
        });
        $("#u264_input").on("change", function () {
            loadGrid();
        })
    }
    
};

$(function () {
    var page = new $page();
    page.init();
});

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}
function loadGrid() {
    var OwnedCompanyList = {
        datafields:
            [
                { name: 'IDNumber', type: 'string' },
                { name: "Name", type: 'string' },
                { name: 'Status', type: 'string' }
            ],
        datatype: "json",
        id: "IDNumber",//主键
        async: true,
        data: {
            "fleet": $("#u264_input").val(),
            "name": $("#SearchByName").val(),
            "date": $("#DateSearch").val(),
            "code": code+"K"
        },
        url: "/PartnerInquiryManagement/DriverJobTraning/GetOwnedCompanyList"    //获取数据源的路径
    };
    var typeAdapter = new $.jqx.dataAdapter(OwnedCompanyList, {
        downloadComplete: function (data) {
            OwnedCompanyList.totalrecords = data.TotalRows;
        }
    });
    //创建卡信息列表（主表）
    selector.$grid().jqxDataTable(
        {
            pageable: true,
            width: "100%",
            height: 350,
            pageSize: 10,
            serverProcessing: true,
            pagerButtonsCount: 4,
            source: typeAdapter,
            theme: "office",
            sortable: true,
            columns: [
                { text: '司机ID', width: 130, datafield: 'IDNumber', align: 'center', cellsAlign: 'center', cellsRenderer: IDFunc },
                { text: '司机姓名', width: 80, datafield: 'Name', align: 'center', cellsAlign: 'center' },
                { text: '培训状态', datafield: 'Status', align: 'center', cellsAlign: 'center', cellsRenderer: detailFunc }
            ]
        });
}
function detailFunc(row, column, value, rowData) {
    var container = "";
    if (value != "合格" && value != "不合格") {
        container = "未阅";
    } else {
        if(value == "合格"){
            container = "已答题";
        }
        if (value == "不合格") {
            container = "未答题";
        }
    }
    return container
}
function IDFunc(row, column, value, rowData) {
    var container = value.replace(/^(.{4})(?:\d+)(.{6})$/, "\$1****\$2");
    return container
}