using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.StoredProcedureEntity;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.DraftManagement
{
    public class DraftManagementServer
    {
        public LogLogic _ll;

        public DraftManagementServer()
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
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.PusuType);
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
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.WeChatPusuType);
                return _dbMsSql.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid && i.Visible == "1").OrderBy(i => i.Zorder).ToList();
            }
        }

        /// <summary>
        /// 绑定营收类型收据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetRevenueType()
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(Common.Tools.MasterVGUID.RevenueType);
                return db.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid && i.Visible == "1").OrderBy(i => i.Zorder, OrderByType.Desc).ToList();
            }
        }

        /// <summary>
        /// 获取习题列表(绑定界面下拉框)
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetExerciseList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Status == 2).OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
            }
        }

        /// <summary>
        /// 获取知识库列表(绑定界面下拉框)
        /// </summary>
        /// <returns></returns>
        public List<Business_KnowledgeBase_Information> GetknowledgeList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Business_KnowledgeBase_Information>().Where(i => i.Status == "2").OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
            }
        }

        /// <summary>
        /// 获取问卷列表(绑定界面下拉框)
        /// </summary>
        /// <returns></returns>
        public List<Business_Questionnaire> GetQuestionList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Business_Questionnaire>().Where(i => i.Status == "2").OrderBy(i => i.CreatedDate, OrderByType.Desc).ToList();
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
                var weChatMsgMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(weChatMsgMain);
                _ll.SaveLog(3, 15, CurrentUser.GetCurrentUser().LoginName, weChatMsgMain.Title, logData);

                return weChatMsgMain;
            }
        }
        /// <summary>
        /// 获取多图文子信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Business_WeChatPush_MoreGraphic_Information> GetMoreGraphicList(string vguid)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var weChatMoreGraphic = db.Queryable<Business_WeChatPush_MoreGraphic_Information>().Where(i => i.WeChatPushVguid == Vguid).OrderBy(i => i.Ranks).ToList();
                return weChatMoreGraphic;
            }
        }
        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectList(string vguid)
        {
            string pushObject = string.Empty;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var pushObjectList = _dbMsSql.Queryable<Business_WeChatPushDetail_Information>().Where(i => i.Business_WeChatPushVguid == Vguid).ToList();
                Business_Personnel_Information personModel = new Business_Personnel_Information();
                foreach (var item in pushObjectList)
                {
                    personModel = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == item.PushObject).SingleOrDefault();
                    if (personModel != null)
                    {
                        pushObject += personModel.Vguid + "|";
                    }
                }
                if (pushObject.Length > 0)
                {
                    pushObject = pushObject.TrimEnd('|');
                }
            }
            return pushObject;
        }

        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectStr(string vguid)
        {
            string pushObject = string.Empty;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var wechatMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                if (wechatMain.Department_VGUID != Guid.Empty) //说明推送的是整个部门的人
                {
                    var orgModel = _dbMsSql.Queryable<Master_Organization>().Where(i => i.Vguid == wechatMain.Department_VGUID).SingleOrDefault();
                    pushObject = orgModel.OrganizationName;
                }
                else if (wechatMain.Department_VGUID == Guid.Empty && !string.IsNullOrEmpty(wechatMain.Label))
                {
                    pushObject = "";
                }
                else
                {
                    var pushObjectList = _dbMsSql.Queryable<Business_WeChatPushDetail_Information>().Where(i => i.Business_WeChatPushVguid == Vguid).ToList();
                    Business_Personnel_Information personModel = new Business_Personnel_Information();

                    foreach (var item in pushObjectList)
                    {
                        personModel = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == item.PushObject).SingleOrDefault();
                        if (personModel != null)
                        {
                            pushObject += personModel.Name + ",";
                        }
                    }
                    if (pushObject.Length > 0)
                    {
                        pushObject = pushObject.TrimEnd(',');
                    }
                }
            }
            return pushObject;
        }

        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectPhone(string vguid)
        {
            string pushObject = string.Empty;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid Vguid = Guid.Parse(vguid);
                var wechatMain = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                if (wechatMain.Department_VGUID != Guid.Empty) //说明推送的是整个部门的人
                {
                    pushObject = wechatMain.Department_VGUID.ToString();
                }
                else if (wechatMain.Department_VGUID == Guid.Empty && !string.IsNullOrEmpty(wechatMain.Label))
                {
                    pushObject = "";
                }
                else
                {
                    var pushObjectList = _dbMsSql.Queryable<Business_WeChatPushDetail_Information>().Where(i => i.Business_WeChatPushVguid == Vguid).ToList();
                    Business_Personnel_Information personModel = new Business_Personnel_Information();

                    foreach (var item in pushObjectList)
                    {
                        personModel = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == item.PushObject).SingleOrDefault();
                        if (personModel != null)
                        {
                            pushObject += personModel.UserID + "|";
                        }
                    }
                    if (pushObject.Length > 0)
                    {
                        pushObject = pushObject.TrimEnd('|');
                    }
                }
            }
            return pushObject;
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var jsonResult = new JsonResultModel<V_Business_WeChatPushMain_Information>();
                var query = _dbMsSql.Queryable<V_Business_WeChatPushMain_Information>().Where(i => i.Status == 1);
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
                    DateTime effectiveDate = DateTime.Parse(searchParam.PeriodOfValidity.ToString().Replace("0:00:00", "23:59:59"));
                    query.Where(c => c.PeriodOfValidity < effectiveDate); //有效时间
                }

                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                int pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _ll.SaveLog(3, 12, CurrentUser.GetCurrentUser().LoginName, "推送草稿列表", logData);

                return jsonResult;
            }
        }

        /// <summary>
        /// 推送接收者树形结构
        /// </summary>
        /// <returns></returns>
        public List<V_Business_Personnel_Information> GetOrganizationTreeList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                List<V_Business_Personnel_Information> organizationTreeList = new List<V_Business_Personnel_Information>();
                if (CurrentUser.GetCurrentUser().LoginName == "sysAdmin")
                {
                    organizationTreeList = _dbMsSql.Queryable<V_Business_Personnel_Information>().OrderBy(it => it.UserID).ToList();
                }
                else
                {
                    organizationTreeList = _dbMsSql.SqlQuery<V_Business_Personnel_Information>(" exec usp_getOrganization @vguid", new { vguid = CurrentUser.GetCurrentUser().Department });
                }
                return organizationTreeList;
            }
        }

        public List<v_Business_PersonnelDepartment_Information> GetUserList(v_Business_PersonnelDepartment_Information searchParam)
        {
            using (SqlSugarClient _dbMySql = SugarDao_MsSql.GetInstance())
            {
                var query = _dbMySql.Queryable<v_Business_PersonnelDepartment_Information>();
                if (!string.IsNullOrEmpty(searchParam.name))
                {
                    query.Where(i => i.name.Contains(searchParam.name)); //人员姓名
                }
                if (!string.IsNullOrEmpty(searchParam.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParam.PhoneNumber)); //手机号
                }
                if (!string.IsNullOrEmpty(searchParam.TranslationOwnedFleet))
                {
                    query.Where(i => i.TranslationOwnedFleet.Contains(searchParam.TranslationOwnedFleet)); //部门
                }
                return query.OrderBy(i => i.OwnedFleet).ToList();
            }
        }

        /// <summary>
        /// 获取习题有效时间
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseEffectiveTime(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Business_Exercises_Infomation exercisesModel = new Business_Exercises_Infomation();
                Guid exerciseVguid = Guid.Parse(vguid);
                exercisesModel = _dbMsSql.Queryable<Business_Exercises_Infomation>().Where(i => i.Vguid == exerciseVguid).SingleOrDefault();

                return exercisesModel;
            }
        }

        /// <summary>
        /// 删除推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeletePushMsg(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {

                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();

                    var weChatPushInfo = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson(weChatPushInfo);
                    _ll.SaveLog(2, 9, CurrentUser.GetCurrentUser().LoginName, weChatPushInfo.Title, weChatJson);
                    //存入操作日志表 
                    _dbMsSql.Delete<Business_WeChatPush_Information>(i => i.VGUID == Vguid);

                    var weChatPushDetailList = _dbMsSql.Queryable<Business_WeChatPushDetail_Information>().Where(i => i.Business_WeChatPushVguid == Vguid).ToList();
                    string weChatPushDetailListJson = JsonHelper.ModelToJson(weChatPushDetailList);
                    _ll.SaveLog(2, 9, CurrentUser.GetCurrentUser().LoginName, "推送接收者列表", weChatPushDetailListJson);
                    //存入操作日志表 
                    _dbMsSql.Delete<Business_WeChatPushDetail_Information>(i => i.Business_WeChatPushVguid == Vguid);

                    _dbMsSql.Delete<Business_WeChatPush_MoreGraphic_Information>(i => i.WeChatPushVguid == Vguid);
                    _dbMsSql.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                    _ll.SaveLog(5, 12, CurrentUser.GetCurrentUser().LoginName, "删除推送信息", vguid);
                    return false;
                }
            }
        }

        /// <summary>
        /// 提交推送信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool SubmitDraftList(string vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Update<Business_WeChatPush_Information>(new { Status = 2 }, i => i.VGUID == Vguid);

                    Business_WeChatPush_Information weChatPushInfo = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == Vguid).SingleOrDefault();
                    string weChatJson = JsonHelper.ModelToJson(weChatPushInfo);
                    //存入操作日志表
                    _ll.SaveLog(8, 12, CurrentUser.GetCurrentUser().LoginName, weChatPushInfo.Title, weChatJson);

                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                    _ll.SaveLog(5, 12, CurrentUser.GetCurrentUser().LoginName, "提交推送信息", vguid);
                }
                return result;
            }
        }

        /// <summary>
        /// 保存推送信息（主信息，详细信息）
        /// </summary>
        /// <param name="weChatMain"></param>
        /// <param name="weChatDetailList"></param>
        /// <returns></returns>
        public bool SavePushMsg(Business_WeChatPush_Information weChatMain, List<Business_WeChatPushDetail_Information> weChatDetailList, bool isEdit)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    if (isEdit) //编辑
                    {
                        var model = new
                        {
                            Title = weChatMain.Title,
                            RevenueType = weChatMain.RevenueType,
                            CountersignType = weChatMain.CountersignType,
                            Status = weChatMain.Status,
                            Timed = weChatMain.Timed,
                            TimedSendTime = weChatMain.TimedSendTime,
                            Important = weChatMain.Important,
                            Message = weChatMain.Message,
                            History = weChatMain.History,
                            PeriodOfValidity = weChatMain.PeriodOfValidity,
                            ChangeDate = weChatMain.ChangeDate,
                            ChangeUser = weChatMain.ChangeUser,
                            CoverImg = weChatMain.CoverImg,
                            CoverDescption = weChatMain.CoverDescption,
                            ExercisesVGUID = weChatMain.ExercisesVGUID, //推送习题Vguid
                            KnowledgeVGUID = weChatMain.KnowledgeVGUID,//推送知识库Vguid
                            RedpacketMoney = weChatMain.RedpacketMoney,
                            RedpacketMoneyFrom = weChatMain.RedpacketMoneyFrom,
                            RedpacketMoneyTo = weChatMain.RedpacketMoneyTo
                        };
                        if (model.PeriodOfValidity == null) //说明是永久有效，则加入知识库
                        {
                            //先查询改知识是否存在
                            bool isExist = _dbMsSql.Queryable<Business_KnowledgeBase_Information>().Any(i => i.Title == model.Title);
                            if (!isExist)
                            {
                                //不存在则插入
                                var knowledge = new Business_KnowledgeBase_Information()
                                {
                                    Title = model.Title,
                                    Content = weChatMain.MessageType == 4 ? model.Message + " " + "4" : model.Message,
                                    Type = "1",
                                    Status = "1",
                                    Remark = "",
                                    CreatedDate = DateTime.Now,
                                    CreatedUser = CurrentUser.GetCurrentUser().LoginName,
                                    Vguid = Guid.NewGuid()
                                };
                                _dbMsSql.Insert(knowledge, false);
                            }
                            else
                            {
                                //存在则更新
                                _dbMsSql.Update<Business_KnowledgeBase_Information>(new
                                {
                                    //Title = model.Title,
                                    Content = model.Message,
                                    ChangeUser = CurrentUser.GetCurrentUser().LoginName,
                                    ChangeDate = DateTime.Now,
                                }, i => i.Title == model.Title);
                            }

                        }
                        result = _dbMsSql.Update<Business_WeChatPush_Information>(model, i => i.VGUID == weChatMain.VGUID);

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson<Business_WeChatPush_Information>(weChatMain);
                        _ll.SaveLog(4, 14, Common.CurrentUser.GetCurrentUser().LoginName, weChatMain.Title, logData);
                    }
                    else //新增
                    {
                        if (weChatMain.PeriodOfValidity == null)
                        {
                            var knowledge = new Business_KnowledgeBase_Information()
                            {
                                Title = weChatMain.Title,
                                Content = weChatMain.Message,
                                Type = "1",
                                Status = "1",
                                Remark = "",
                                CreatedDate = weChatMain.CreatedDate,
                                CreatedUser = weChatMain.CreatedUser,
                                Vguid = Guid.NewGuid()
                            };
                            _dbMsSql.Insert(knowledge, false);
                        }
                        result = _dbMsSql.Insert(weChatMain, false) != DBNull.Value;

                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(weChatMain);
                        _ll.SaveLog(1, 13, CurrentUser.GetCurrentUser().LoginName, weChatMain.Title, logData);
                    }
                    _dbMsSql.Delete<Business_WeChatPushDetail_Information>(i => i.Business_WeChatPushVguid == weChatMain.VGUID);
                    //存入操作日志表
                    string log = JsonHelper.ModelToJson(weChatDetailList);
                    _ll.SaveLog(1, 15, CurrentUser.GetCurrentUser().LoginName, weChatMain.Title, log);
                    _dbMsSql.SqlBulkCopy(weChatDetailList);
                    _dbMsSql.CommitTran();

                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    LogHelper.WriteLog(ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }

                return result;
            }
        }

        /// <summary>
        /// 保存多图文信息
        /// </summary>
        /// <param name="weChatMain">主表</param>
        /// <param name="pushMoreGraphicList">多图文子表</param>
        /// <param name="weChatDetailList">推送人表</param>
        /// <param name="isEdit">是否编辑</param>
        /// <returns></returns>
        public bool SaveImagePushMsg(Business_WeChatPush_Information weChatMain, List<Business_WeChatPush_MoreGraphic_Information> pushMoreGraphicList, List<Business_WeChatPushDetail_Information> weChatDetailList, bool isEdit)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();

                    if (isEdit)
                    {
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(weChatMain);
                        _ll.SaveLog(4, 14, Common.CurrentUser.GetCurrentUser().LoginName, weChatMain.Title, logData);
                        db.Delete<Business_WeChatPush_MoreGraphic_Information>(i => i.WeChatPushVguid == weChatMain.VGUID);
                        db.InsertRange(pushMoreGraphicList);
                        db.Delete<Business_WeChatPushDetail_Information>(i => i.Business_WeChatPushVguid == weChatMain.VGUID);
                        db.SqlBulkCopy(weChatDetailList);
                        db.Update(weChatMain);
                    }
                    else
                    {
                        //存入操作日志表
                        string logData = JsonHelper.ModelToJson(weChatMain);
                        _ll.SaveLog(1, 15, Common.CurrentUser.GetCurrentUser().LoginName, weChatMain.Title, logData);
                        db.Insert(weChatMain);
                        db.InsertRange(pushMoreGraphicList);
                        db.SqlBulkCopy(weChatDetailList);
                    }
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    LogHelper.WriteLog(ex.Message + "/n" + ex + "/n" + ex.StackTrace);
                    return false;
                }
            }
        }
        /// <summary>
        /// 获取该部门下的所有人员
        /// </summary>
        /// <param name="orgVguid">部门vguid</param>
        /// <param name="labName">标签名</param>
        /// <returns></returns>
        public string[] GetDepartPerson(Guid orgVguid, string labName)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                if (string.IsNullOrEmpty(labName))
                    return db.SqlQuery<usp_getOrganization>("exec usp_getOrganization @orgvguid", new { orgvguid = orgVguid }).Where(i => i.UserID != null).Select(i => i.UserID).ToArray();
                var labArr = labName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var query = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Select(i => i.PersonnelVVGUID).ToList();
                return db.SqlQuery<usp_getOrganization>("exec usp_getOrganization @orgvguid", new { orgvguid = orgVguid }).Where(i => i.UserID != null).Where(i => query.Contains(i.Vguid)).Select(i => i.UserID).ToArray();
            }
        }

        /// <summary>
        /// 营收推送营收金额时只推送（司机而且欠款的人员），其他人员不推送
        /// </summary>
        /// <param name="orgVguid"></param>
        ///  <param name="labName"></param>
        /// <returns></returns>
        public string[] GetRevenuePerson(Guid orgVguid, string labName)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var listUser = new List<string>();
                ShortMsgServer shortMsgServer = new ShortMsgServer();
                var list = db.SqlQuery<Business_Personnel_Information>("exec usp_getOrganization_UserID @orgvguid", new { orgvguid = orgVguid }); //查找该部门下所有的司机
                if (!string.IsNullOrEmpty(labName))
                {
                    var labArr = labName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var query = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Select(i => i.PersonnelVVGUID).ToList();
                    list = list.Where(i => query.Contains(i.Vguid)).ToList();
                }
                foreach (var personModel in list)
                {
                    var driverModel = shortMsgServer.GetDriverMsg(personModel);
                    var paymentModel = shortMsgServer.GetRevenueMsg(driverModel);
                    if (paymentModel != null)
                    {
                        var accountBalance = (paymentModel.PaidAmount - paymentModel.DueAmount + (paymentModel.DebtAmount * -1)).ToString("F2"); //本期结余
                        if (decimal.Parse(accountBalance) < 0)
                        {
                            listUser.Add(personModel.UserID);
                        }
                    }
                }
                return listUser.ToArray();
            }
        }

        /// <summary>
        /// 营收推送固定金额时只推送（司机），其他人员不推送
        /// </summary>
        /// <param name="orgVguid"></param>
        /// <param name="labName"></param>
        /// <returns></returns>
        public string[] GetFixedPayPerson(Guid orgVguid, string labName)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {

                var list = db.SqlQuery<Business_Personnel_Information>("exec usp_getOrganization_UserID @orgvguid", new { orgvguid = orgVguid }); //查找该部门下所有的司机
                if (string.IsNullOrEmpty(labName))
                    return list.Select(i => i.UserID).ToArray();
                else
                {
                    var labArr = labName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var query = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Select(i => i.PersonnelVVGUID).ToList();
                    return list.Where(i => query.Contains(i.Vguid)).Select(i => i.UserID).ToArray();
                }
            }
        }


        public List<string> GetAllPeople()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_Personnel_Information>().Where(i => i.UserID != null).Select(i => i.UserID).ToList();
            }
        }
        /// <summary>
        /// 保存推送信息（主信息，详细信息）
        /// </summary>
        /// <param name="listPushMain"></param>
        /// <param name="listPushDetail"></param>
        ///  <param name="notExistPeople"></param>
        /// <returns></returns>
        public bool SaveUploadPushMSg(List<Business_WeChatPush_Information> listPushMain, List<Business_WeChatPushDetail_Information> listPushDetail, List<NotExistPerson> notExistPeople)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    string log = JsonHelper.ModelToJson(listPushMain);
                    string logData = JsonHelper.ModelToJson(listPushDetail);
                    _ll.SaveLog(1, 13, CurrentUser.GetCurrentUser().LoginName, "导入推送", log);
                    _ll.SaveLog(1, 15, CurrentUser.GetCurrentUser().LoginName, "导入推送", logData);
                    //创建临时表存储推送失败人员
                    db.SqlQuery<string>("if exists(select * from sysobjects where name ='NotExistPerson') drop table NotExistPerson create table NotExistPerson(Name varchar(50) null,MobilePhone varchar(50) null,IDNumber varchar(50) null) ");
                    db.SqlBulkCopy(notExistPeople);
                    db.SqlBulkCopy(listPushMain);
                    db.SqlBulkCopy(listPushDetail);
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    return false;
                }
            }
        }

        /// <summary>
        /// 保存上传的Excel
        /// </summary>
        /// <param name="salaryTable"></param>
        /// <param name="notExists"></param>
        /// <returns></returns>
        public bool SaveUpLoadSalary(DataTable salaryTable)
        {
            DataTable dt = new DataTable();
            var db = SugarDao_MsSql.GetInstance();
            db.Delete<Business_Payroll_Information>(it => it.PushVGUID == null);   //删除推送为空的数据
            dt = db.Queryable<Business_Payroll_Information>().Where("1=2").ToDataTable();
            string connectionString = ConfigSugar.GetAppString("msSqlLinck");
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                try
                {
                    sqlbulkcopy.BulkCopyTimeout = 60;
                    sqlbulkcopy.DestinationTableName = "Business_Payroll_Information";
                    for (int i = 0; i < salaryTable.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(salaryTable.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    sqlbulkcopy.WriteToServer(salaryTable);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
        /// <summary>
        /// 保存上传保养信息的Excel
        /// </summary>
        /// <param name="salaryTable"></param>
        /// <param name="notExists"></param>
        /// <returns></returns>
        public bool SaveUpLoadMaintence(DataTable salaryTable)
        {
            DataTable dt = new DataTable();
            var db = SugarDao_MsSql.GetInstance();
            db.Delete<Car_Maintain>(it => it.PushVGUID == null);   //删除推送为空的数据
            dt = db.Queryable<Car_Maintain>().Where("1=2").ToDataTable();
            string connectionString = ConfigSugar.GetAppString("msSqlLinck");
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                try
                {
                    sqlbulkcopy.BulkCopyTimeout = 60;
                    sqlbulkcopy.DestinationTableName = "Car_Maintain";
                    for (int i = 0; i < salaryTable.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(salaryTable.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    sqlbulkcopy.WriteToServer(salaryTable);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    throw;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
        /// <summary>
        /// 获取车辆保养的表结构
        /// </summary>
        /// <returns></returns>
        public DataTable GetCar_Maintain()
        {
            DataTable dt = new DataTable();
            var db = SugarDao_MsSql.GetInstance();
            dt = db.Queryable<Car_Maintain>().Where("1=2").ToDataTable();
            return dt;
        }

        /// <summary>
        /// 获取工资条推送人员
        /// </summary>
        /// <returns></returns>
        public List<U_PushObj> GetPushObjs()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<Business_Payroll_Information>()
                    .JoinTable<Business_Personnel_Information>((s1, s2) => s1.IDCard == s2.IDNumber)
                    .Where(s1 => s1.PushVGUID == null)
                    .Select<Business_Payroll_Information, Business_Personnel_Information, U_PushObj>((s1, s3, s2) => new U_PushObj() { UserID = s2.UserID });
                return query.ToList();
            }
        }
        /// <summary>
        /// 获取保养信息推送人员
        /// </summary>
        /// <returns></returns>
        public List<U_Push_Car_Maintain> GetPushMaintenceObjs()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<Car_Maintain>()
                    .JoinTable<Business_Personnel_Information>((s1, s2) => s1.PhoneNumber == s2.PhoneNumber)
                    .Where(s1 => s1.PushVGUID == null)
                    .Select<Car_Maintain, Business_Personnel_Information, U_Push_Car_Maintain>((s1, s3, s2) => new U_Push_Car_Maintain()
                    {
                        UserID = s2.UserID,
                        CarNO = s1.CarNO,
                        JobNumber = s1.JobNumber,
                        Name = s1.Name,
                        PhoneNumber = s1.PhoneNumber,
                        PhoneNumberU = s2.PhoneNumber,
                        MaintainDate = s1.MaintainDate,
                        MaintainTime = s1.MaintainTime,
                        Type = s1.Type,
                        IsVerify = s1.IsVerify,
                        Address = s1.Address,
                        Mileage = s1.Mileage,
                        PersonnelVGUID = s1.PersonnelVGUID,
                        PushVGUID = s1.PushVGUID,
                        Vguid = s1.Vguid
                    });
                return query.ToList();
            }
        }

        public class U_Push_Car_Maintain : Car_Maintain
        {
            public string UserID { get; set; }
            public string PhoneNumberU { get; set; }
        }

        public class U_PushObj
        {
            public string UserID { get; set; }
        }

        /// <summary>
        /// 保存工资条推送消息
        /// </summary>
        /// <param name="weChatMain">主表</param>
        /// <param name="weChatPushDetails">明细表</param>
        /// <param name="msg">明细表</param>
        /// <returns></returns>
        public bool SaveSalaryPush(Business_WeChatPush_Information weChatMain, List<Business_WeChatPushDetail_Information> weChatPushDetails, ref string msg)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    var salarys = db.Queryable<Business_Payroll_Information>().Where(it => it.PushVGUID == null).ToList();
                    var notExists = new List<NotExistPerson>();
                    foreach (var salary in salarys)
                    {
                        var personInfo = GetPersonnelInformation(string.Format(" IDNumber='{0}'", salary.IDCard));
                        if (personInfo != null) continue;
                        var notExistPerson = new NotExistPerson();
                        notExistPerson.IDNumber = salary.IDCard;
                        notExistPerson.Name = salary.Name;
                        notExistPerson.MobilePhone = salary.JobNumber;
                        notExists.Add(notExistPerson);
                    }
                    msg = "共计" + (weChatPushDetails.Count) + "条，成功推送" + (weChatPushDetails.Count - notExists.Count) + "条，失败" + notExists.Count + "条！";
                    //创建临时表存储推送失败人员
                    db.SqlQuery<string>("if exists(select * from sysobjects where name ='NotExistPerson') drop table NotExistPerson create table NotExistPerson(Name varchar(50) null,MobilePhone varchar(50) null,IDNumber varchar(50) null) ");
                    db.Update<Business_Payroll_Information>(new { PushVGUID = weChatMain.VGUID }, it => it.PushVGUID == null);
                    if (weChatPushDetails.Count > notExists.Count)
                    {
                        db.Insert(weChatMain, false);
                    }
                    db.SqlBulkCopy(notExists);
                    weChatPushDetails = weChatPushDetails.Where(i => i.PushObject != null).ToList();
                    db.SqlBulkCopy(weChatPushDetails);
                    db.CommitTran();

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    return false;
                }

            }
        }

        public bool SaveMaintenancePush(List<U_Push_Car_Maintain> pushObjsList, List<Business_WeChatPush_Information> weChatMainList, List<Business_WeChatPushDetail_Information> weChatPushDetails, List<NotExistPerson> noExistList, ref string msg)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    msg = "共计" + (pushObjsList.Count) + "条，成功推送" + (weChatPushDetails.Count) + "条，失败" + noExistList.Count + "条！";
                    //创建临时表存储推送失败人员
                    db.SqlQuery<string>("if exists(select * from sysobjects where name ='NotExistPerson') drop table NotExistPerson create table NotExistPerson(Name varchar(50) null,MobilePhone varchar(50) null,IDNumber varchar(50) null) ");
                    db.SqlBulkCopy(noExistList);

                    if (weChatPushDetails.Count > 0 || weChatMainList.Count > 0)
                    {
                        weChatMainList = weChatMainList.ToList();
                        db.SqlBulkCopy(weChatMainList);

                        weChatPushDetails = weChatPushDetails.Where(i => i.PushObject != null).ToList();
                        db.SqlBulkCopy(weChatPushDetails);
                    }


                    db.CommitTran();

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    return false;
                }

            }
        }

        public void UpdateCarMaintaince(Guid vguid, Guid mainVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                db.Update<Car_Maintain>(new { PushVGUID = mainVguid }, it => it.PushVGUID == null && it.Vguid == vguid);
            }
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetPersonnelInformation(string strWhere)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_Personnel_Information>().Where(strWhere).SingleOrDefault();
            }
        }
        /// <summary>
        /// 获取所有人员标签
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetLabels()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_PersonnelLabel_Information>().GroupBy(i => i.LabelName).OrderBy(i => i.LabelName).Select(i => i.LabelName).ToList();
            }

        }
        /// <summary>
        /// 获取所有人员标签
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonnelLabel> GetAllLabel()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.SqlQuery<PersonnelLabel>("select LabelName from Business_PersonnelLabel_Information group by LabelName order by LabelName");
            }

        }


        /// <summary>
        /// 根据部门和标签获取推送人
        /// </summary>
        /// <param name="depVguid"></param>
        /// <param name="labName"></param>
        /// <returns></returns>
        public IEnumerable<string> GetPeopleByLabelAndDep(Guid depVguid, string labName)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                if (depVguid == Guid.Empty)
                {
                    depVguid = db.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).Single();
                }
                //查找该标签下的人员
                var labArr = labName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var query = db.Queryable<Business_PersonnelLabel_Information>().In(i => i.LabelName, labArr).GroupBy(i => i.PersonnelVVGUID).Select(i => i.PersonnelVVGUID).ToList();
                //查找部门含有标签的人员
                return db.SqlQuery<usp_getOrganization>("exec usp_getOrganization @orgvguid", new { orgvguid = depVguid }).Where(i => i.UserID != null).Where(i => query.Contains(i.Vguid)).Select(i => i.UserID);
            }

        }




        /// <summary>
        /// 获取清单数据（公用方法）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public List<CS_Master_2> GetMasterDataType(string guid)
        {
            using (SqlSugarClient db = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(guid);
                return db.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid && i.Visible == "1").OrderBy(i => i.Zorder).ToList();
            }
        }

        /// <summary>
        /// 分页获取导入推送中不存在的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<NotExistPerson> GetNotExistPeople(GridParams para)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var jsonResult = new JsonResultModel<NotExistPerson>();
                var query = db.Queryable<NotExistPerson>().OrderBy(it => it.Name);
                int pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;
                return jsonResult;
            }
        }

        /// <summary>
        /// 获取导入推送中不存在的人员信息
        /// </summary>
        /// <returns></returns>
        public void DownNotExistPeople(string fileName)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<NotExistPerson>().OrderBy(it => it.Name);
                var dt = query.ToDataTable();
                dt.TableName = "tb";
                if (string.IsNullOrEmpty(fileName))
                {
                    ExportExcel.ExportExcels("notExist.xlsx", "推送失败人员.xls", dt);
                }
                else
                {
                    ExportExcel.ExportExcels(fileName, "推送失败人员.xls", dt);
                }

            }
        }

        /// <summary>
        /// 删除存储推送失败人员的临时表
        /// </summary>
        public void DropNotExistPersonTable()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                db.SqlQuery<string>("if exists(select * from sysobjects where name ='NotExistPerson') drop table NotExistPerson");
            }
        }
    }
}
