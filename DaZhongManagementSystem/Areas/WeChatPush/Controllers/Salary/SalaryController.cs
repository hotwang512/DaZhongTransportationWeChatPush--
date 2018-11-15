using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.Salary.BusinessLogic;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.Salary
{
    public class SalaryController : Controller
    {
        //
        // GET: /WeChatPush/Salary/
        private readonly WeChatExerciseLogic _wl;
        private readonly PushDetailLogic _pl;
        private readonly SalaryLogic _salaryLogic;
        public SalaryController()
        {
            _wl = new WeChatExerciseLogic();
            _pl = new PushDetailLogic();
            _salaryLogic = new SalaryLogic();
        }

        public ActionResult Salary(string code)
        {

            #region 获取人员表信息

            string accessToken = WeChatTools.GetAccessoken();
            string userInfoStr = WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr); //用户ID
            var personInfoModel = _wl.GetUserInfo(userInfo.UserId); //获取人员表信息 
            ViewData["vguid"] = personInfoModel.Vguid;
            #endregion

            string pushContentVguid = Request.QueryString["Vguid"]; //推送的主键
            ViewData["pushContentVguid"] = pushContentVguid;
            var pushContentModel = _pl.GetPushDetail(pushContentVguid);
            //获取工资信息
            var salaryInfo = _salaryLogic.GetSalaryInfo(personInfoModel.IDNumber, pushContentVguid);
            ViewData["salaryInfo"] = salaryInfo;//new Business_Payroll_Information();
            //bool isValidTime = false; //未过有效期
            // 判断是否过了有效期
            //if (pushContentModel!=null)
            //{
            //    if (pushContentModel.PeriodOfValidity != null)
            //    {
            //        if (DateTime.Now > pushContentModel.PeriodOfValidity)
            //        {
            //            isValidTime = true; //已过有效期
            //        }
            //    }
            //    ViewBag.isValidTime = isValidTime;
            //}
            ViewData["PushContentModel"] = pushContentModel;
            return View();
        }

    }
}
