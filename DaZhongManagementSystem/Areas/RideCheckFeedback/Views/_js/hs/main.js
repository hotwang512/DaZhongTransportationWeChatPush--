var basePath = '/hzyz2/';
function getUrlParameter(name) { //解析网页后的路径 读取参数
	name = name.replace(/[]/, "\[").replace(/[]/, "\[").replace(/[]/, "\\\]");
	let regexS = "[\\?&]" + name + "=([^&#]*)";
	let regex = new RegExp(regexS);
	let results = regex.exec(window.parent.location.href);
	return results == null ? "" : decodeURI(results[1]); //decodeURI 解码
}
carData=[];
dirverData=[];
