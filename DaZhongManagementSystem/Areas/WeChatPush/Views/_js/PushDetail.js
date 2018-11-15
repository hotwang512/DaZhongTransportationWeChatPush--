
var selector = {

};


var $page = function () {

    this.init = function () {
        addEvent();
    }

    //所有事件
    function addEvent() {
        // startenLargePicture();
        //让图片居中
        $("img").parent("p").css("text-indent", "0");
        $("img").attr("onclick", "ShowBig(this)");
        //点击放大图片的div,div隐藏
        $("#Bigimg").on("click", function () {
            $("#Bigimg").hide();
        });


    }; //addEvent end


};


var reqAnimationFrame = (function () {
    return window[Hammer.prefixed(window, 'requestAnimationFrame')] || function (callback) {
        window.setTimeout(callback, 1000 / 60);
    };
})();

var el = $("#img");

var START_X;
var START_Y;
var ticking = false;
var transform;   //图像效果
var timer;
//var initAngle = 0;  //旋转角度
var initScale = 1;  //放大倍数


//点击图片拿到当前点击图片的img  赋给放大图片src路径
function ShowBig(obj) {
    $("#Bigimg").show();
    var imgSrc = $(obj).attr("src"); //拿到当前点击图片的src;
    $("#img").attr("src", imgSrc);
    el = $("#img");
    START_X = Math.round((window.innerWidth - el.width()) / 2);
    START_Y = Math.round((window.innerHeight - el.height()) / 2);
    resetElement();
    //enLargePicture();
    var myElement = document.getElementById('Bigimg');
    var hammertime = new Hammer(myElement);
    hammertime.get('pinch').set({ enable: true });
    hammertime.add(new Hammer.Pan({ threshold: 0, pointers: 0 }));
    //为该dom元素指定触屏移动事件
    hammertime.add(new Hammer.Pinch({ threshold: 0 })).recognizeWith([hammertime.get('pan')]);

    //hammertime.add(new Hammer.Pinch());
    //添加事件
    //结束时做一些处理
    hammertime.on("hammer.input", function (ev) {
        if (ev.isFinal) {
            START_X = transform.translate.x;
            START_Y = transform.translate.y;
        }
    });
    hammertime.on("panstart panmove", onPan);
    hammertime.on("pinchstart pinchmove", onPinch);

}
function onPan(ev) {
    if (!ev.isFinal) {
        el.className = '';
        var evX = START_X + ev.deltaX;
        //if (evX < 0) {
        //    evX = 0;
        //}
        var evY = START_Y + ev.deltaY;
        //if (evY < 0) {
        //    evY = 0;
        //}
        transform.translate = {
            x: evX,
            y: evY
        };
        requestElementUpdate();
    }
}

function onPinch(ev) {
    if (ev.type == 'pinchstart') {
        initScale = transform.scale || 1;
    }
    el.className = '';
    transform.scale = initScale * ev.scale;
    requestElementUpdate();
}


function updateElementTransform() {
    var value = [
                'translate3d(' + transform.translate.x + 'px, ' + transform.translate.y + 'px, 0)',
                'scale(' + transform.scale + ', ' + transform.scale + ')'
    ];

    value = value.join(" ");
    //el.style.webkitTransform = value;  /*为Chrome/Safari*/
    //el.style.mozTransform = value; /*为Firefox*/
    el.css("transform", value); /*IE Opera?*/
    ticking = false;
}

function requestElementUpdate() {
    if (!ticking) {
        reqAnimationFrame(updateElementTransform);
        ticking = true;
    }
}
/**
		初始化设置
		*/
function resetElement() {

    el.className = 'animate';
    transform = {
        translate: { x: 0, y: 0 },
        scale: 1,
        //  angle: 0,
        rx: 0,
        ry: 0,
        rz: 0
    };
    requestElementUpdate();
}


$(function () {
    var page = new $page();
    page.init();

})