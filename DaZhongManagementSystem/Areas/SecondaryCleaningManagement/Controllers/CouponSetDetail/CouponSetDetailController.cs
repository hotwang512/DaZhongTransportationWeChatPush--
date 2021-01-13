using DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Models;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.UserManagement.BussinessLogic;
using DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using JQWidgetsSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement.Controllers.CouponSetDetail
{
    public class CouponSetDetailController : BaseController
    {
        public UserManageLogic _ul;
        public AuthorityManageLogic _al;
        public CouponSetDetailController()
        {
            _ul = new UserManageLogic();
            _al = new AuthorityManageLogic();
        }
        public ActionResult Index()
        {
            bool isEdit = bool.Parse(Request.QueryString["isEdit"]);
            string vguid = Request.QueryString["VGUID"];
            Business_EquityAllocation equityAllocation = new Business_EquityAllocation();
            Sys_Role_Module roleModuleModel = _al.GetRoleModulePermission(Common.CurrentUser.GetCurrentUser().Role, Common.Tools.ModuleVguid.UserSystemModule);
            ViewBag.CurrentModulePermission = roleModuleModel;
            ViewBag.Equity = equityAllocation;
            ViewBag.isEdit = isEdit;
            ViewBag.VGUID = vguid;
            ViewData["currentUserDepartment"] = "";
            if (!string.IsNullOrEmpty(CurrentUser.GetCurrentUser().Department))
            {
                ViewData["currentUserDepartment"] = CurrentUser.GetCurrentUser().Department;
            }
            return View();
        }
        public JsonResult GetEquityAllocationByVguid(string vguid)
        {
            Business_EquityAllocation equityAllocation = new Business_EquityAllocation();
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                if(vguid != "" && vguid != null)
                {
                    equityAllocation = _db.SqlQuery<Business_EquityAllocation>(@"select * from Business_EquityAllocation where VGUID=@VGUID",
                                new { VGUID = vguid }).ToList().FirstOrDefault();
                }
            }
            return Json(equityAllocation, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]//敏感值验证
        public JsonResult SaveEquityAllocation(Business_EquityAllocation equity, bool isEdit,string startValidity,string endValidity,string pushPeople)
        {
            var model = new ActionResultModel<string>();
            model.isSuccess = false;
            using (SqlSugarClient _db = SugarDao_MsSql.GetInstance())
            {
                bool isExistType = _db.Queryable<Business_EquityAllocation>().Any(x => x.Type == equity.Type);
                if (!isEdit && isExistType)
                {
                    model.isSuccess = false;
                    model.respnseInfo = "2";
                }
                else
                {
                    if (equity.ValidType == "周期")
                    {
                        //getPeriodDate(equity, equity.Period);
                        equity.StartValidity = null;
                        equity.EndValidity = null;
                    }
                    else if (equity.ValidType == "截止日期")
                    {
                        equity.StartValidity = startValidity.ObjToDate();
                        equity.EndValidity = endValidity.ObjToDate();
                    }
                    if (isEdit)
                    {
                        equity.ChangeDate = DateTime.Now;
                        equity.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        var data = new
                        {
                            RightsName = equity.RightsName,
                            Description = equity.Description,
                            Type = equity.Type,
                            ValidType = equity.ValidType,
                            StartValidity = equity.StartValidity,
                            EndValidity = equity.EndValidity,
                            PushObject = equity.PushObject,
                            PushPeople = pushPeople,
                            //Status = equity.Status,
                            Period = equity.Period
                        };
                        model.isSuccess = _db.Update<Business_EquityAllocation>(data, i => i.VGUID == equity.VGUID);
                        //同时更新权益展示表,先删除再新增
                        var eVguid = equity.VGUID.ToString();
                        _db.Delete<Business_MyRights>(i => i.EquityVGUID == eVguid);
                        InsertMyRights(_db, equity, equity.PushObject);
                        //UpdateMyRights(_db, equity);
                    }
                    else
                    {
                        equity.VGUID = Guid.NewGuid();
                        equity.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                        equity.CreatedDate = DateTime.Now;
                        equity.ChangeDate = DateTime.Now;
                        equity.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        equity.Status = "未发布";
                        model.isSuccess = _db.Insert(equity, false) != DBNull.Value;
                        //同时插入权益展示表
                        InsertMyRights(_db, equity, equity.PushObject);
                    }
                    model.isSuccess = true;
                    model.respnseInfo = model.isSuccess == true ? "1" : "0";
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private void InsertMyRights(SqlSugarClient _db, Business_EquityAllocation equity,string PushObject)
        {
            List<Business_MyRights> myRightsList = new List<Business_MyRights>();
            string[] userID = null;
            var userData = _db.SqlQuery<Business_Personnel_Information>(@"select * from Business_Personnel_Information where DepartmenManager in ('1','2','3')").ToList();
            if (PushObject.Contains("-"))
            {
                //部门
                Guid pushObject = Guid.Parse(PushObject);
                userData = userData.Where(x => x.OwnedFleet == pushObject).ToList();
            }
            else if(PushObject.Contains("|") || !PushObject.Contains("-"))
            {
                //一个或多人司机
                List<Business_Personnel_Information> newDataLsit = new List<Business_Personnel_Information>();
                userID = PushObject.Split(new char[1] { '|' });
                foreach (var item in userID)
                {
                    Business_Personnel_Information newData = new Business_Personnel_Information();
                    newData = userData.Where(x => x.UserID == item).FirstOrDefault();
                    newDataLsit.Add(newData);
                }
                userData = newDataLsit;
            }
            if (equity.ValidType == "周期")
            {
                getPeriodDate(equity, equity.Period);
            }
            
            foreach (var item in userData)
            {
                Business_MyRights myRights = new Business_MyRights();
                myRights.RightsName = equity.RightsName;
                myRights.Description = equity.Description;
                myRights.Type = equity.Type;
                myRights.StartValidity = equity.StartValidity;
                myRights.EndValidity = equity.EndValidity;
                myRights.UsageTime = null;
                myRights.UserVGUID = item.Vguid.ToString();
                myRights.Status = "草稿";
                myRights.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                myRights.CreatedDate = DateTime.Now;
                myRights.ChangeDate = DateTime.Now;
                myRights.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                myRights.EquityVGUID = equity.VGUID.ToString();
                myRights.VGUID = Guid.NewGuid();
                myRightsList.Add(myRights);
            }
            _db.InsertRange(myRightsList, false);
        }
        private void UpdateMyRights(SqlSugarClient _db, Business_EquityAllocation equity)
        {
            if (equity.ValidType == "周期")
            {
                getPeriodDate(equity, equity.Period);
            }
            var myRights = _db.Queryable<Business_MyRights>().Where(x => x.VGUID == equity.VGUID).FirstOrDefault();
            var data = new
            {
                RightsName = equity.RightsName,
                Description = equity.Description,
                Type = equity.Type,
                StartValidity = equity.StartValidity,
                EndValidity = equity.EndValidity,
                ChangeUser = equity.ChangeUser,
                ChangeDate = equity.ChangeDate
            };
            _db.Update<Business_MyRights>(data, i => i.VGUID == equity.VGUID);
        }
        private void getPeriodDate(Business_EquityAllocation equity, string period)
        {
            var type = "";
            switch (period)
            {
                case "按周": type = "Week";break;
                case "按月": type = "Month"; break;
                case "按年": type = "Year"; break;
                default:break;
            }
            var start = GetTimeStartByType(type, DateTime.Now);
            var end = GetTimeEndByType(type, DateTime.Now);
            var newStart = start.Value.ToString("yyyy-MM-dd");
            var newEnd = end.Value.ToString("yyyy-MM-dd");
            equity.StartValidity = (newStart + " 00:00:00").ObjToDate();
            equity.EndValidity = (newEnd + " 23:59:59").ObjToDate();
        }
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeStartByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Week":
                    return now.AddDays(-(int)now.DayOfWeek + 1);
                case "Month":
                    return now.AddDays(-now.Day + 1);
                case "Season":
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return time.AddDays(-time.Day + 1);
                case "Year":
                    return now.AddDays(-now.DayOfYear + 1);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeEndByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Week":
                    return now.AddDays(7 - (int)now.DayOfWeek);
                case "Month":
                    return now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddDays(-1);
                case "Season":
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    return time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1);
                case "Year":
                    var time2 = now.AddYears(1);
                    return time2.AddDays(-time2.DayOfYear);
                default:
                    return null;
            }
        }
        #endregion
    }
}
