using System;
using System.Web.Mvc;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.AgreementOperation.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using Newtonsoft.Json;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.AgreementOperation
{
    public class AgreementOperationController : BaseController
    {
        //
        // GET: /WeChatPush/AgreementOperation/
        private readonly AuthorityManageLogic _al;
        private readonly AgreementLogic _agreementLogic;
        public AgreementOperationController()
        {
            _al = new AuthorityManageLogic();
            _agreementLogic = new AgreementLogic();
        }

        public ActionResult AgreementOperationList()
        {
            var roleModuleModel = _al.GetRoleModulePermission(CurrentUser.GetCurrentUser().Role, ModuleVguid.Agreement);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        public ActionResult AgreementOperationDetail(Guid vguid)
        {
            var agreementModel = _agreementLogic.GetAgreementDetailByVguid(vguid);
            ViewBag.AgreementModel = agreementModel;
            return View();
        }

        /// <summary>
        /// 获取协议操作历史记录
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResult GetAgreementOpertaionList(Search_AgreementOperation searchParam, GridParams para)
        {
            para.pagenum = para.pagenum + 1;//页0，+1
            var list = _agreementLogic.GetAgreementOpertaionList(searchParam, para);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="para"></param>
        public void Export(string para)
        {
            var model = JsonConvert.DeserializeObject<Search_AgreementOperation>(para);
            _agreementLogic.Export(model);
        }

        public JsonResult GetAgreementTypeList()
        {
            var list = _agreementLogic.GetAgreementTypeList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
