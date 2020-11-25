using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.CodeGenerate.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using JQWidgetsSugar;
using SyntacticSugar;
using ThoughtWorks.QRCode.Codec;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.CodeGenerate
{
    public class CodeGenerateController : BaseController
    {
        //
        // GET: /QRCodeManagement/CodeGenerate/
        private readonly CodeGenerateLogic _codeGenerateLogic;
        public CodeGenerateController()
        {
            _codeGenerateLogic = new CodeGenerateLogic();
        }
        public ActionResult Index()
        {
            var personInfoModel = new Business_Personnel_Information() { Vguid = Guid.Parse("726B4C9F-0B5A-4D5B-A8EE-5BA95C38C4C7") };
            //   var personInfoModel = CurrentUser.GetCurrentUser();
            string file = personInfoModel.Vguid + ".jpg";
            string forder = "UploadFile/QRCode";
            string filePath = Path.Combine(forder, file);
            string fileName = Server.MapPath(filePath);
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            var configStr = _codeGenerateLogic.GetPersonConfiguration(personInfoModel);
            //  Create_ImgCode(configStr, personInfoModel.Vguid.ToString());
            //QRCodeHelper.GenerateQRCode(configStr, "/Areas/WeChatPush/Views/_img/logo1.png", forder, file);
            ViewData["Vguid"] = personInfoModel.Vguid;
            ViewData["QRCode"] = personInfoModel.Vguid + ".jpg";
            return View();
        }

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="configStr">要生成二维码的字符串</param>       
        /// <param name="userVguid">当前用户</param>  
        /// <returns>二维码图片</returns>  
        public Image Create_ImgCode(string configStr, string userVguid)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder(); //创建二维码生成类  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //设置编码模式  
            qrCodeEncoder.QRCodeScale = 4; //设置编码测量度  
            qrCodeEncoder.QRCodeVersion = 10; //设置编码版本  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M; //设置编码错误纠正  
                                                                                 //  qrCodeEncoder.QRCodeBackgroundColor = Color.FromArgb(1, 160, 179);
            Image image = qrCodeEncoder.Encode(configStr); //生成二维码图片
                                                           //  CombinImage(image, Server.MapPath("/Areas/WeChatPush/Views/_img/20170912103255.jpg"));
            string filename = userVguid + ".jpg";
            string url = "/UploadFile/QRCode";
            string filePath = Server.MapPath(url);
            if (!Directory.Exists(filePath)) //当前目录不存在，则创建  
            {
                Directory.CreateDirectory(filePath);
            }
            image.Save(filePath + "/" + filename, ImageFormat.Jpeg); //保存二维码图片
            return image;
        }

        public Image CombinImage(Image imgBack, string destImg)
        {
            Image image = Image.FromFile(destImg);
            if (image.Height != 65 || image.Width != 65)
            {
                image = KiResizeImage(image, 65, 65, 0);

            }
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            g.DrawImage(image, imgBack.Width / 2 - image.Width / 2, imgBack.Width / 2 - image.Width / 2, image.Width, image.Height);
            GC.Collect();
            return imgBack;
        }

        public Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 重新生成二维码
        /// </summary>
        public string ReGenerate()
        {
            try
            {
                var personInfoModel = new Business_Personnel_Information() { Vguid = Guid.Parse("726B4C9F-0B5A-4D5B-A8EE-5BA95C38C4C7") };
                // var personInfoModel = CurrentUser.GetCurrentUser();
                string filename = personInfoModel.Vguid + ".jpg";
                string url = "/UploadFile/QRCode";
                string filePath = Server.MapPath(url + "/" + filename);
                System.IO.File.Delete(filePath);
                var configStr = _codeGenerateLogic.GetPersonConfiguration(personInfoModel);
                var img = Create_ImgCode(configStr, personInfoModel.Vguid.ToString());
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return "0";
            }

        }

        /// <summary>
        /// 获取二维码的配置(系统)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSysConfigurations()
        {
            var configuations = _codeGenerateLogic.GetSysConfigurations();
            return Json(configuations, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取二维码的配置（参数）
        /// </summary>
        /// <param name="sysConfigId"></param>
        /// <returns></returns>
        public JsonResult GetConfigurations(string sysConfigId)
        {
            var configuations = _codeGenerateLogic.GetConfigurations(sysConfigId);
            return Json(configuations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存生成二维码配置信息
        /// </summary>
        /// <param name="configStr">参数配置</param>
        ///  <param name="sysConfigStr">系统配置</param>
        public JsonResult SaveQRCodeConfig(string configStr, string sysConfigStr)
        {
            var resultInfo = new ActionResultModel<string>();
            resultInfo.isSuccess = _codeGenerateLogic.SaveQRCodeConfig(configStr, sysConfigStr);
            resultInfo.respnseInfo = resultInfo.isSuccess ? "1" : "2";
            return Json(resultInfo, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除二维码的配置信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteQRCodeConfig(List<int> ids)
        {
            var resultInfo = new ActionResultModel<string>();
            resultInfo.isSuccess = _codeGenerateLogic.DeleteQRCodeConfig(ids);
            resultInfo.respnseInfo = resultInfo.isSuccess ? "1" : "2";
            return Json(resultInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
