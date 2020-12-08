using DaZhongManagementSystem.Areas.ExerciseLibraryManagement.Controllers.ExerciseLibraryManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Draft.BusinessLogic;
using DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Formal.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Formal
{
    public class FormalController : BaseController
    {
        //
        // GET: /KnowledgeBaseManagement/Formal/
        private AuthorityManageLogic _al;
        private ExerciseLibraryLogic _exerciseLibraryLogic;
        private FormalLogic _formalLogic;
        private DraftLogic _draftLogic;
        public FormalController()
        {
            _exerciseLibraryLogic = new ExerciseLibraryLogic();
            _al = new AuthorityManageLogic();
            _formalLogic = new FormalLogic();
            _draftLogic = new DraftLogic();
        }
        public ActionResult FormalList()
        {
            //绑定录入类型
            var inputType = new List<CS_Master_2>();
            inputType = _exerciseLibraryLogic.GetInputType();
            ViewData["InputType"] = inputType;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.KnowledgeFormalModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }
        public ActionResult FormalDetail()
        {
            Guid vguid = Guid.Parse(Request.QueryString["Vguid"]);
            var knowledgeInfo = _draftLogic.GetKnowledgeBaseInfoByVguid(vguid);
            ViewBag.knowledgeInfo = knowledgeInfo;
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.KnowledgeFormalModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            return View();
        }

        /// <summary>
        /// 获取草稿知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResult GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            if (para.sortdatafield == null)
            {
                para.sortdatafield = "CreatedDate";
                para.sortorder = "desc";
            }
            para.pagenum = para.pagenum + 1;
            var model = _formalLogic.GetKnowledgeListBySearch(searchParam, para);
            var result = new ConfigurableJsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //  return Json(model, JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}
