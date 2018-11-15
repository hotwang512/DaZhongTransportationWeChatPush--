using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers
{
    public class WeChatScanCodeController : Controller
    {
        //
        // GET: /QRCodeManagement/WeChatScanCode/
        public WeChatExerciseLogic _wl;
        public WeChatScanCodeController()
        {
            _wl=new WeChatExerciseLogic();
        }
        public ActionResult Index(string code)
        {
            string vguid = Request.QueryString["userVguid"];
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            string answerCount = _wl.GetAnswerCount();
            //Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            return View();
        }

    }
}
