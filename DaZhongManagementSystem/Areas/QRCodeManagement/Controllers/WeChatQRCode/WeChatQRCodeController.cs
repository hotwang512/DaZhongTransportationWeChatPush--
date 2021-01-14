using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.CodeGenerate.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.WeChatQRCode
{
    public class WeChatQRCodeController : Controller
    {
        //
        // GET: /QRCodeManagement/WeChatQRCode/
        private readonly WeChatExerciseLogic _wl;
        private readonly CodeGenerateLogic _codeGenerateLogic;
        public WeChatQRCodeController()
        {
            _wl = new WeChatExerciseLogic();
            _codeGenerateLogic = new CodeGenerateLogic();
        }

        public ActionResult CodeGenerate(string code)
        {
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            //userInfo.UserId = "WangCunBiao";
            var personInfoModel = _wl.GetUserInfo(userInfo.UserId);//获取人员表信息
            string file = personInfoModel.Vguid + ".jpg";
            string forder = "UploadFile/WeChatQRCode";
            string filePath = Path.Combine(forder, file);

            string fileName = Server.MapPath(filePath);
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            var configStr = _codeGenerateLogic.GetPersonConfiguration(personInfoModel);
            QRCodeHelper.GenerateQRCode(configStr, "/Areas/WeChatPush/Views/_img/logo1.png", forder, file);
            ViewData["Vguid"] = personInfoModel.Vguid;
            ViewData["url"] = ConfigSugar.GetAppString("OpenHttpAddress") + forder + "/" + file;
            return View();
        }
        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="configStr">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <param name="userVguid">当前用户</param>  
        /// <returns>二维码图片</returns>  
        public Bitmap Create_ImgCode(string configStr, int size, string userVguid)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder(); //创建二维码生成类  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //设置编码模式  
            qrCodeEncoder.QRCodeScale = size; //设置编码测量度  
            qrCodeEncoder.QRCodeVersion = 0; //设置编码版本  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M; //设置编码错误纠正  
            Bitmap image = qrCodeEncoder.Encode(configStr); //生成二维码图片
            string filename = userVguid + ".jpg";
            string url = "/UploadFile/WeChatQRCode";
            string filePath = Server.MapPath(url);
            if (!Directory.Exists(filePath)) //当前目录不存在，则创建  
            {
                Directory.CreateDirectory(filePath);
            }
            image.Save(filePath + "/" + filename, ImageFormat.Jpeg); //保存二维码图片
            return image;
        }
    }
}
