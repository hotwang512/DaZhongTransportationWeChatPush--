using System;
using System.Collections.Generic;
using System.Linq;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.StoredProcedureEntity;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.PushHistory
{
    public class PushHistoryServer
    {
        public LogLogic _ll;

        public PushHistoryServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 通过vguid获取推送主表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatMainByVguid(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var weChatMsgMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(weChatMsgMain);
                _ll.SaveLog(3, 45, CurrentUser.GetCurrentUser().LoginName, weChatMsgMain.Title, logData);
                return weChatMsgMain;
            }
        }

        /// <summary>
        /// 手机端获取消息历史的详细信息
        /// </summary>
        /// <param name="vguid">消息历史主键</param>
        ///  <param name="personVguid">用户主键</param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatDetail(string vguid, Guid personVguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var weChatMsgMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(weChatMsgMain);
                var personInfo = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personVguid).SingleOrDefault();
                _ll.SaveLog(3, 45, personInfo.ID + personInfo.Name, weChatMsgMain.Title, logData);
                return weChatMsgMain;
            }
        }


        /// <summary>
        /// 通过查询条件获取推送信息列表（已推送）
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_WeChatPushMain_Information> jsonResult = new JsonResultModel<V_Business_WeChatPushMain_Information>();
                //只查询三个月内的数据
                DateTime endDate = DateTime.Now.AddMonths(-3);
                //   var personModel = new Business_Personnel_Information() { UserID = "1323397305", Vguid = Guid.Parse("8F3D8CCC-82D9-4A76-B0C7-2DC585D86B64") };
                var query = db.Queryable<V_Business_WeChatPushMain_Information>().Where(i => i.Status == 4 && i.History == "1" && i.PushDate > endDate);
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    var mainDep = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();  //大众交通集团
                    var listChildDep = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == mainDep).Select(i => i.Vguid).ToList();  //大众出租租赁
                    listChildDep.Add(mainDep);
                    Guid currentDep = Guid.Parse(CurrentUser.GetCurrentUser().Department);//首先查询登录人的部门
                    if (!listChildDep.Contains(currentDep))
                    {
                        var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + currentDep + "')");  //找到该部门以及其所有子部门
                        var list = db.Queryable<Sys_User>().In(i => i.Department, listDep).Select(i => i.LoginName).ToList();
                        query.Where(i => list.Contains(i.PushPeople));  //找到该部门中人员所推送的消息
                    }
                }
                if (!string.IsNullOrEmpty(searchParam.Title))
                {
                    query.Where(c => c.Title.Contains(searchParam.Title)); //标题
                }
                if (!string.IsNullOrEmpty(searchParam.PushType.ToString()))
                {
                    query.Where(c => c.PushType == searchParam.PushType); //推送类型
                }
                if (!string.IsNullOrEmpty(searchParam.Important.ToString()))
                {
                    query.Where(c => c.Important == searchParam.Important); //是否重要
                }
                if (!string.IsNullOrEmpty(searchParam.PeriodOfValidity.ToString()))
                {
                    DateTime pushDate = DateTime.Parse(searchParam.PushDate.ToString().Replace("0:00:00", "23:59:59"));
                    query.Where(c => c.PushDate < pushDate); //有效时间
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                int pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;
                string logData = JsonHelper.ModelToJson(jsonResult);
                _ll.SaveLog(3, 37, CurrentUser.GetCurrentUser().LoginName, "推送历史", logData);
                return jsonResult;
            }
        }


        /// <summary>
        /// 批量删除推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeletePushHistory(string vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                db.BeginTran();
                try
                {
                    Guid guid = Guid.Parse(vguid);
                    Business_WeChatPush_Information weChatPushInfo = db.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == guid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson(weChatPushInfo);
                    _ll.SaveLog(2, 37, CurrentUser.GetCurrentUser().LoginName, weChatPushInfo.Title, weChatJson);
                    //存入操作日志表 
                    db.Update<Business_WeChatPush_Information>(new { History = 0 }, c => c.VGUID == guid); //假删除
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex + "/n" + ex.StackTrace);
                    _ll.SaveLog(5, 37, CurrentUser.GetCurrentUser().LoginName, "删除推送历史信息", vguid);
                    db.RollbackTran();
                    return false;
                }

            }
        }

        public bool GetPeopleByDepartmentAndLabel(V_Business_WeChatPushMain_Information weChatPushMainInfo, Business_Personnel_Information personInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                //说明选择的既有部门又有标签
                if (weChatPushMainInfo.Department_VGUID != Guid.Empty && !string.IsNullOrEmpty(weChatPushMainInfo.Label))
                {
                    var labArr = weChatPushMainInfo.Label.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var isBelongLabel = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Any(i => i.PersonnelVVGUID == personInfo.Vguid);
                    if (!isBelongLabel)
                    {
                        return false;
                    }
                    var isBelongDep = db.SqlQuery<usp_getOrganization>("exec usp_getOrganization @orgvguid", new { orgvguid = weChatPushMainInfo.Department_VGUID }).Any(i => i.UserID == personInfo.UserID);
                    return isBelongDep;
                }
                else if (weChatPushMainInfo.Department_VGUID != Guid.Empty && string.IsNullOrEmpty(weChatPushMainInfo.Label))  //只选了部门
                {
                    var isBelongDep = db.SqlQuery<usp_getOrganization>("exec usp_getOrganization @orgvguid", new { orgvguid = weChatPushMainInfo.Department_VGUID }).Any(i => i.UserID == personInfo.UserID);
                    return isBelongDep;
                }
                else if (!string.IsNullOrEmpty(weChatPushMainInfo.Label) && weChatPushMainInfo.Department_VGUID == Guid.Empty)  //只选了标签
                {
                    var labArr = weChatPushMainInfo.Label.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var isBelongLabel = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Any(i => i.PersonnelVVGUID == personInfo.Vguid);
                    return isBelongLabel;
                }
                return db.Queryable<Business_WeChatPushDetail_Information>().Any(i => i.Business_WeChatPushVguid == weChatPushMainInfo.VGUID && i.PushObject == personInfo.UserID);
            }
        }


        /// <summary>
        /// 手机端分页获取消息历史记录
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        ///  <param name="personVguid">当前浏览人的vguid</param>
        /// <returns></returns>
        public List<TempWeChatMain> GetWeChatPushList(int pageIndex, Guid personVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var personModel = new Business_Personnel_Information();
                try
                {
                    personModel = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personVguid).SingleOrDefault();

                    #region 使用程序查询
                    //var pushMainInfos = db.Queryable<V_Business_WeChatPushMain_Information>().Where(i => i.Status == 4 && i.History == "1" && i.PushType == 1).OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
                    //var historys = pushMainInfos.Select(i => new TempWeChatMain
                    //{
                    //    Title = i.Title,
                    //    PushDate = i.PushDate,
                    //    MessageType = i.MessageType,
                    //    ExercisesVGUID = i.ExercisesVGUID,
                    //    VGUID = i.VGUID,
                    //    IsShow = GetPeopleByDepartmentAndLabel(i, personModel)
                    //}).Where(i => i.IsShow).Skip((pageIndex - 1) * 10).Take(10).ToList();
                    //var ss = db.SqlQuery<TempWeChatMain>("usp_Show_Business_WeChatPushMain_Information @vguid ",
                    //      new { vguid = personModel.Vguid }).Where(i => i.ShowTYPE == 1).ToList(); 
                    #endregion

                    //使用存储过程查询
                    var historys = db.SqlQuery<TempWeChatMain>("usp_Show_Business_WeChatPushMain_Information @vguid,@Start,@End", new
                    {
                        vguid = personModel.Vguid,
                        Start = (pageIndex - 1) * 10 + 1,
                        End = pageIndex * 10
                    }).ToList();
                    var jsonResult = JsonHelper.ModelToJson(historys);
                    _ll.SaveLog(3, 37, personModel.ID + personModel.Name, "查看消息历史", jsonResult);
                    return historys;
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.ToString());
                    _ll.SaveLog(5, 37, "查看消息历史:", personModel.ID + personModel.Name, ex.Message);
                    return null;
                }

            }

        }

    }
}
