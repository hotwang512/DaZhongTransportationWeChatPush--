// 百度地图API功能
var map = new BMapGL.Map("allmap"); 
var point = new BMapGL.Point(121.48212104567115, 31.238460905048402);
map.centerAndZoom("上海", 15);

//根据输入地址搜素位置
var ac = new BMapGL.Autocomplete(    //建立一个自动完成的对象
    {
        "input": "CompanyName",
        "location": map
    });
var comVal = $("#CompanyName").val();
ac.setInputValue(comVal);
var myValue = comVal;
if (myValue != "" && myValue != null) {
    setPlace();
}
function searchCompany() {
    ac.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
        var str = "";
        var _value = e.fromitem.value;
        var value = "";
        if (e.fromitem.index > -1) {
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

        value = "";
        if (e.toitem.index > -1) {
            _value = e.toitem.value;
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
        //if (comVal != "") {
        //    str = comVal;
        //}
        $("#CompanyName").innerHTML = str;
    });
    ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
        var _value = e.item.value;
        myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
        $("#CompanyName").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;
        setPlace();
    });
}
function setPlace() {
    map.clearOverlays();    //清除地图上所有覆盖物
    function myFun() {
        var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果(经纬度)
        map.centerAndZoom(pp, 18);
        map.addOverlay(new BMapGL.Marker(pp));    //添加标注
        setLocation(pp);     //填充经纬度和具体地址
    }
    var local = new BMapGL.LocalSearch(map, { //智能搜索
        onSearchComplete: myFun
    });
    local.search(myValue);
}
//根据半径绘制范围
if ($("#Location").val() != "" && $("#Location").val() != null) {
    searchRadius();
}
function searchRadius() {
    map.clearOverlays();
    var location = $("#Location").val();
    var radius = $("#Radius").val();
    var val = location.split(",");
    // 创建圆
    var circle = new BMapGL.Circle(new BMapGL.Point(val[1], val[0]), parseFloat(radius), {
        strokeColor: 'blue',
        strokeWeight: 2,
        strokeOpacity: 1,
        fillOpacity: 0.1,       //填充的透明度，取值范围0 - 1。  
        fillColor: "blue",
    });
    map.addOverlay(circle);
}

//点击地图获取详细地址和经纬度
var geoc = new BMapGL.Geocoder();
map.addEventListener('click', function (e) {
    //清除地图上所有的覆盖物
    map.clearOverlays();
    console.log(e);
    var pt = e.latlng;
    //填充经纬度和具体地址
    setLocation(pt);
})
function setLocation(pt) {
    $("#Location").val(pt.lat + "," + pt.lng);
    var marker = new BMapGL.Marker(new BMapGL.Point(pt.lng, pt.lat));
    map.addOverlay(marker);
    geoc.getLocation(pt, function (rs) {
        var addComp = rs.addressComponents;
        $('#Address').val(addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber);
    });
    //转换百度坐标为腾讯坐标(用于微信定位比较)
    qq.maps.convertor.translate(new qq.maps.LatLng(pt.lat, pt.lng), 3, function (res) {
        var latlng = res[0];
        $("#TXLocation").val(latlng);
    });
}
// 初始化地图,用城市名设置地图中心点，显示比例级别
function showSH() { map.centerAndZoom("上海", 15); }

//鼠标滚动缩放
map.enableScrollWheelZoom(true);


//var marker = new BMapGL.Marker(new BMapGL.Point(114.04254, 22.561866)); // 创建点
//map.addOverlay(marker);
//marker.setAnimation(BMap_ANIMATION_BOUNCE); //跳动的动画

//marker.disableDragging();// 不可拖拽

//根据浏览器获取定位
//var geolocation = new BMap.Geolocation();
//geolocation.getCurrentPosition(function (r) {
//    if (this.getStatus() == BMAP_STATUS_SUCCESS) {
//        var mk = new BMap.Marker(r.point);
//        map.addOverlay(mk);
//        map.panTo(r.point);
//        alert('您的位置：' + r.point.lng + ',' + r.point.lat);
//    }
//    else {
//        alert('failed' + this.getStatus());
//    }
//}, { enableHighAccuracy: true })

//获取到经纬度之后,与百度地图上所设定的范围比较
//function checkIsPointInCircle(latitude, longitude) {
//    alert(5);
//    var isCheck = false;
//    if (latitude != "" && longitude != "") {
//        var point2 = new BMap.Point(latitude, longitude);
//        if (BMapLib.GeoUtils.isPointInCircle(point, circle)) {
//            isCheck = true;
//        }
//    }
//    return isCheck;
//}