using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.DraftManagement
{
    public class CheckedServer
    {
        public LogLogic _ll;
        public CheckedServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 绑定推送类型
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetPushTypeList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.PusuType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 绑定微信推送类型数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetWeChatPushType()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(DaZhongManagementSystem.Common.Tools.MasterVGUID.WeChatPusuType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).ToList();
            }
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetExerciseList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Business_Exercises_Infomation>().ToList().OrderBy("Vguid", OrderByType.Asc).ToList();
            }
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
                Business_WeChatPush_Information weChatMsgMain = new Business_WeChatPush_Information();
                weChatMsgMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(weChatMsgMain);
                _ll.SaveLog(3, 19, Common.CurrentUser.GetCurrentUser().LoginName, weChatMsgMain.Title, logData);

                return weChatMsgMain;
            }
        }

        /// <summary>
        /// 推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool PushSubmitList(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Business_WeChatPush_Information>(new { Status = 4 }, i => i.VGUID == Vguid);

                    Business_WeChatPush_Information weChatPushModel = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                    string jsonResult = JsonHelper.ModelToJson<Business_WeChatPush_Information>(weChatPushModel);
                    //存入操作日志表
                    //_ll.SaveLog(10, 14, weChatPushModel.Title, jsonResult);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表（已审核）
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                JsonResultModel<V_Business_WeChatPushMain_Information> jsonResult = new JsonResultModel<V_Business_WeChatPushMain_Information>();
                var query = _dbMsSql.Queryable<V_Business_WeChatPushMain_Information>().Where(i => i.Status == 3);
                if (CurrentUser.GetCurrentUser().LoginName.ToLower() != "sysadmin")
                {
                    var mainDep = _dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();  //大众交通集团
                    var listChildDep = _dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == mainDep).Select(i => i.Vguid).ToList();  //大众出租租赁
                    listChildDep.Add(mainDep);
                    Guid currentDep = Guid.Parse(CurrentUser.GetCurrentUser().Department);//首先查询登录人的部门
                    if (!listChildDep.Contains(currentDep))
                    {
                        var listDep = _dbMsSql.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + currentDep + "')");  //找到该部门以及其所有子部门
                        var list = _dbMsSql.Queryable<Sys_User>().In(i => i.Department, listDep).Select(i => i.LoginName).ToList();
                        query.Where(i => list.Contains(i.PushPeople));  //找到该部门中人员所推送的消息
                    }
                }
                if (!string.IsNullOrEmpty(searchParam.Title))
                {
                    query.Where(c => c.Title.Contains(searchParam.Title));//标题
                }
                if (!string.IsNullOrEmpty(searchParam.PushType.ToString()))
                {
                    query.Where(c => c.PushType == searchParam.PushType);//推送类型
                }
                if (!string.IsNullOrEmpty(searchParam.Important.ToString()))
                {
                    query.Where(c => c.Important == searchParam.Important);//是否重要
                }
                if (!string.IsNullOrEmpty(searchParam.PeriodOfValidity.ToString()))
                {
                    DateTime effectiveDate = DateTime.Parse(searchParam.PeriodOfValidity.ToString().Replace("0:00:00", "23:59:59"));
                    query.Where(c => c.PeriodOfValidity < effectiveDate);//有效时间
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                int pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _ll.SaveLog(3, 18, CurrentUser.GetCurrentUser().LoginName, "推送已审核列表", logData);

                return jsonResult;
            }
        }
    }
}
