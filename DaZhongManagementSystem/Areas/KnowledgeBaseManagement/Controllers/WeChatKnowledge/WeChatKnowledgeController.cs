using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.WeChatExercise.BusinessLogic;
using DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.WeChatKnowledge.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushDetailShow.BusinessLogic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.WeChatKnowledge
{
    public class WeChatKnowledgeController : Controller
    {
        //
        // GET: /KnowledgeBaseManagement/WeChatKnowledge/
        private WeChatKnowledgeLogic _knowledgeLogic;
        private WeChatExerciseLogic _wl;
        public PushDetailLogic _pl;
        public WeChatKnowledgeController()
        {
            _knowledgeLogic = new WeChatKnowledgeLogic();
            _wl = new WeChatExerciseLogic();
            _pl = new PushDetailLogic();
        }
        public ActionResult KnowledgeBase(string code)
        {
            //string knowledgeType = Request["knowledgeType"] ?? "";
            string pushContentVguid = Request.QueryString["Vguid"] ?? ""; //"55ca3608-93d3-4245-b6f7-e4af76482edd";//
            if (!string.IsNullOrEmpty(pushContentVguid))
            {
                var pushContentModel = new Business_WeChatPush_Information();
                pushContentModel = _pl.GetPushDetail(pushContentVguid);
                bool isValidTime = false;//未过有效期
                if (pushContentModel != null)
                {
                    if (pushContentModel.PeriodOfValidity != null)
                    {
                        if (DateTime.Now > pushContentModel.PeriodOfValidity)
                        {
                            isValidTime = true;//已过有效期
                        }
                    }
                }
                ViewBag.isValidTime = isValidTime;
                ViewData["PushContentModel"] = pushContentModel;
                ViewData["vguid"] = pushContentVguid;
            }
            string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
            string userInfoStr = Common.WeChatPush.WeChatTools.GetUserInfoByCode(accessToken, code);
            var userInfo = Common.JsonHelper.JsonToModel<U_WeChatUserID>(userInfoStr);//用户ID
            _pl.UpdateIsRead(userInfo.UserId, pushContentVguid);//更新用户是否阅读推送
            Business_Personnel_Information personInfoModel = _wl.GetUserInfo(userInfo.UserId);//获取人员表信息
            ViewBag.PersonInfo = personInfoModel;
            // ViewBag.KnowledgeType = knowledgeType;
            return View();
        }

        /// <summary>
        /// 获取正式知识库的列表信息
        /// </summary> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="personVguid">浏览人主键</param>
        /// <returns></returns>
        public JsonResult GetKnowledgeList(int pageIndex, Guid personVguid)
        {
            var list = _knowledgeLogic.GetKnowledgeList(pageIndex, personVguid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KnowledgeBaseDetail()
        {
            ViewData["Vguid"] = Request["Vguid"];
            ViewData["personVguid"] = Request["personVguid"];
            return View();
        }

        /// <summary>
        /// 获取知识库的详细信息
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <param name="personVguid">人员主键</param>
        /// <returns></returns>
        public JsonResult GetKnowledgeDetail(string vguid, Guid personVguid)
        {
            var model = _knowledgeLogic.GetKnowledgeDetail(vguid, personVguid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
