using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using DaZhongTransitionLiquidation.Common.Pub;
using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CouponPage
{
    public class CouponPageController : Controller
    {
        public WeChatExerciseLogic _wl;
        public CouponPageController()
        {
            _wl = new WeChatExerciseLogic();
        }
        public ActionResult Index(string code)
        {
            string accessToken = WeChatTools.GetAccessoken();
            U_WeChatUserID userInfo = new U_WeChatUserID();
            //string userInfoStr = WeChatTools.GetUserInfoByCode(accessToken, code);
            //userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            userInfo.UserId = "13671595340";//合伙人
            //userInfo.UserId = "18936495119";//司机
            Business_Personnel_Information personInfoModel = GetUserInfo(userInfo.UserId);//获取人员表信息
            var key = PubGet.GetUserKey + personInfoModel.Vguid;
            var csche = CacheManager<Business_Personnel_Information>.GetInstance().Get(key);
            if (csche != null)
            {
                CacheManager<Business_Personnel_Information>.GetInstance().Remove(key);
            }
            CacheManager<Business_Personnel_Information>.GetInstance().Add(key, personInfoModel, 8 * 60 * 60 * 1000);
            ViewBag.UserVGUID = personInfoModel.Vguid;
            return View();
        }
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            Business_Personnel_Information personModel = new Business_Personnel_Information();
            personModel = _wl.GetUserInfo(userID);
            return personModel;
        }
        public JsonResult GetCouponSetInfo(string code)
        {
            var cm = CacheManager<Business_Personnel_Information>.GetInstance()[PubGet.GetUserKey + code];
            List<Business_MyRights> myRight = new List<Business_MyRights>();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                //当前登录人的权益信息
                var guid = cm.Vguid.TryToString();
                myRight = _db.Queryable<Business_MyRights>().Where(x=>x.Status != "草稿" && x.UserVGUID == guid).ToList();
            }
            return Json(myRight, JsonRequestBehavior.AllowGet);
        }
    }
}
