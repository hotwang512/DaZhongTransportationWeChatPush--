using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DraftManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Common.WeChatPush;
using SyntacticSugar;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.DraftList.BusinessLogic
{
    public class DraftInfoLogic
    {
        private readonly DraftManagementServer _ds;

        public DraftInfoLogic()
        {
            _ds = new DraftManagementServer();
        }

        /// <summary>
        /// 绑定推送类别数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetPushTypeList()
        {
            return _ds.GetPushTypeList();
        }

        /// <summary>
        /// 绑定微信推送类型数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetWeChatPushType()
        {
            return _ds.GetWeChatPushType();
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetExerciseList()
        {
            return _ds.GetExerciseList();
        }

        /// <summary>
        /// 获取知识库列表
        /// </summary>
        /// <returns></returns>
        public List<Business_KnowledgeBase_Information> GetknowledgeList()
        {
            return _ds.GetknowledgeList();
        }

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Questionnaire> GetQuestionList()
        {
            return _ds.GetQuestionList();
        }
        /// <summary>
        /// 绑定营收类型收据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetRevenueType()
        {
            return _ds.GetRevenueType();
        }
        /// <summary>
        /// 通过vguid获取推送主表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatMainByVguid(string vguid)
        {
            return _ds.GetWeChatMainByVguid(vguid);
        }

        /// <summary>
        /// 获取多图文子信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Business_WeChatPush_MoreGraphic_Information> GetMoreGraphicList(string vguid)
        {
            return _ds.GetMoreGraphicList(vguid);
        }
        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectList(string vguid)
        {
            return _ds.GetPushObjectList(vguid);
        }

        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectStr(string vguid)
        {
            return _ds.GetPushObjectStr(vguid);
        }

        /// <summary>
        /// 获取推送接收者字符串
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public string GetPushObjectPhone(string vguid)
        {
            return _ds.GetPushObjectPhone(vguid);
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            return _ds.GetWeChatPushListBySearch(searchParam, para);
        }

        /// <summary>
        /// 推送接收者树形结构
        /// </summary>
        /// <returns></returns>
        public List<V_Business_Personnel_Information> GetOrganizationTreeList()
        {
            return _ds.GetOrganizationTreeList();
        }

        public List<v_Business_PersonnelDepartment_Information> GetUserList(v_Business_PersonnelDepartment_Information searchParam)
        {
            return _ds.GetUserList(searchParam);
        }

        /// <summary>
        /// 获取习题有效时间
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_Exercises_Infomation GetExerciseEffectiveTime(string vguid)
        {
            return _ds.GetExerciseEffectiveTime(vguid);
        }

        /// <summary>
        /// 批量删除推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DeletePushMsg(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _ds.DeletePushMsg(item);
            }
            return result;
        }

        /// <summary>
        /// 批量提交推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool SubmitDraftList(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _ds.SubmitDraftList(item);
            }
            return result;
        }
        /// <summary>
        /// 保存推送信息（主信息，详细信息）
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <returns></returns>
        public bool APISavePushMsg(U_PushMsg pushMsgModel)
        {
            bool result = false;
            Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();

            //主信息赋值(微信推送信息)
            weChatMain.Title = pushMsgModel.Title;
            weChatMain.PushType = pushMsgModel.PushType;
            weChatMain.MessageType = pushMsgModel.MessageType;
            weChatMain.Timed = pushMsgModel.Timed;
            weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
            weChatMain.Important = pushMsgModel.Important;
            weChatMain.Message = pushMsgModel.Message;
            weChatMain.CoverImg = pushMsgModel.CoverImg;
            weChatMain.CoverDescption = pushMsgModel.CoverDescption;
            weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
            weChatMain.PushDate = pushMsgModel.PushDate;
            weChatMain.ExercisesVGUID = pushMsgModel.ExercisesVGUID; //推送习题Vguid
            weChatMain.KnowledgeVGUID = pushMsgModel.KnowledgeVGUID; //推送知识库Vguid
            weChatMain.QuestionVGUID = pushMsgModel.QuestionVGUID;//问卷Vguid
            weChatMain.PushPeople = pushMsgModel.PushPeople;
            weChatMain.Status = 3;
            weChatMain.CreatedDate = pushMsgModel.CreatedDate;
            weChatMain.CreatedUser = pushMsgModel.CreatedUser;
            weChatMain.VGUID = pushMsgModel.VGUID;
            weChatMain.History = pushMsgModel.History;
            weChatMain.RevenueType = null;
            weChatMain.CountersignType = null;
            weChatMain.Label = pushMsgModel.Label;


            //副表信息赋值(微信推送详细信息)
            string pushObject = pushMsgModel.PushObject.TrimEnd('|');
            string[] arrObject = pushObject.Split('|');
            weChatMain.Department_VGUID = Guid.Empty;
            foreach (var item in arrObject)
            {
                Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                weChatDetail.Type = pushMsgModel.PushType.ToString();
                weChatDetail.Vguid = Guid.NewGuid();
                weChatDetail.ISRead = "0"; //未读
                weChatDetail.PushObject = item;
                weChatDetail.CreatedDate = pushMsgModel.CreatedDate;
                weChatDetail.CreatedUser = pushMsgModel.CreatedUser;
                weChatDetailList.Add(weChatDetail);
            }
            result = _ds.SavePushMsg(weChatMain, weChatDetailList, false);
            return result;
        }
        /// <summary>
        /// 保存推送信息（主信息，详细信息）
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <param name="isEdit"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public bool SavePushMsg(U_PushMsg pushMsgModel, bool isEdit, string saveType)
        {
            bool result = false;
            Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();
            if (isEdit) //编辑
            {
                weChatMain = GetWeChatMainByVguid(pushMsgModel.VGUID.ToString());
                weChatMain.Title = pushMsgModel.Title;
                weChatMain.CoverImg = pushMsgModel.CoverImg;
                weChatMain.CoverDescption = pushMsgModel.CoverDescption;
                weChatMain.Timed = pushMsgModel.Timed;
                weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
                weChatMain.Important = pushMsgModel.Important;
                weChatMain.Message = pushMsgModel.Message;
                weChatMain.ExercisesVGUID = pushMsgModel.ExercisesVGUID; //推送习题Vguid
                weChatMain.KnowledgeVGUID = pushMsgModel.KnowledgeVGUID; //推送知识库Vguid
                weChatMain.QuestionVGUID = pushMsgModel.QuestionVGUID;//问卷Vguid
                weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
                weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.Status = int.Parse(saveType);
                weChatMain.ChangeDate = DateTime.Now;
                weChatMain.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.History = pushMsgModel.History;
                weChatMain.Label = pushMsgModel.Label;
                weChatMain.RevenueType = null;
                weChatMain.CountersignType = null;
                if (weChatMain.MessageType == 5)
                {
                    weChatMain.RevenueType = pushMsgModel.RevenueType;
                }
                if (weChatMain.MessageType == 12)
                {
                    weChatMain.CountersignType = pushMsgModel.CountersignType;
                }
                if (weChatMain.MessageType == 13 || weChatMain.MessageType == 14)
                {
                    weChatMain.RedpacketType = pushMsgModel.RedpacketType;
                    weChatMain.RedpacketMoney = pushMsgModel.RedpacketMoney;
                    weChatMain.RedpacketMoneyFrom = pushMsgModel.RedpacketMoneyFrom;
                    weChatMain.RedpacketMoneyTo = pushMsgModel.RedpacketMoneyTo;
                }
                //副表信息赋值(微信推送详细信息)
                string pushObject = string.Empty;
                Guid vguid;           //部门vguid
                string[] arrObject;    //推送人数组
                if (string.IsNullOrEmpty(pushMsgModel.PushObject))  //说明没有选择推送人 选的是标签
                {
                    weChatMain.Department_VGUID = Guid.Empty;
                    arrObject = _ds.GetPeopleByLabelAndDep(Guid.Empty, pushMsgModel.Label).ToArray();
                }
                else
                {
                    pushObject = pushMsgModel.PushObject.TrimEnd('|');
                    if (!Guid.TryParse(pushObject, out vguid))
                    {
                        arrObject = pushObject.Split('|');
                        weChatMain.Department_VGUID = Guid.Empty;
                    }
                    else
                    {
                        switch (pushMsgModel.MessageType)
                        {
                            case 11:
                                arrObject = pushMsgModel.RevenueType == 1 ? _ds.GetFixedPayPerson(vguid, weChatMain.Label) : _ds.GetRevenuePerson(vguid, weChatMain.Label);
                                break;
                            default:
                                arrObject = _ds.GetDepartPerson(vguid, weChatMain.Label);
                                break;
                        }
                        weChatMain.Department_VGUID = vguid;
                    }
                }
                foreach (var item in arrObject)
                {
                    Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                    weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                    weChatDetail.Vguid = Guid.NewGuid();
                    weChatDetail.PushObject = item;
                    weChatDetail.Type = pushMsgModel.PushType.ToString();
                    weChatDetail.ISRead = "0"; //未读
                    weChatDetail.CreatedDate = DateTime.Now;
                    weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetail.ChangeDate = DateTime.Now;
                    weChatDetail.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetailList.Add(weChatDetail);
                }
                result = _ds.SavePushMsg(weChatMain, weChatDetailList, true);
            }
            else //新增
            {
                //主信息赋值(微信推送信息)
                weChatMain.Title = pushMsgModel.Title;
                weChatMain.PushType = pushMsgModel.PushType;
                weChatMain.MessageType = pushMsgModel.MessageType;
                weChatMain.Timed = pushMsgModel.Timed;
                weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
                weChatMain.Important = pushMsgModel.Important;
                weChatMain.Message = pushMsgModel.Message;
                weChatMain.CoverImg = pushMsgModel.CoverImg;
                weChatMain.CoverDescption = pushMsgModel.CoverDescption;
                weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
                weChatMain.PushDate = pushMsgModel.PushDate;
                weChatMain.ExercisesVGUID = pushMsgModel.ExercisesVGUID; //推送习题Vguid
                weChatMain.KnowledgeVGUID = pushMsgModel.KnowledgeVGUID; //推送知识库Vguid
                weChatMain.QuestionVGUID = pushMsgModel.QuestionVGUID;//问卷Vguid
                weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.Status = int.Parse(saveType);
                weChatMain.CreatedDate = DateTime.Now;
                weChatMain.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.ChangeDate = DateTime.Now;
                weChatMain.VGUID = Guid.NewGuid();
                weChatMain.History = pushMsgModel.History;
                weChatMain.RevenueType = null;
                weChatMain.CountersignType = null;
                if (pushMsgModel.MessageType == 5 || pushMsgModel.MessageType == 11) //培训推送(此时该字段当做"倒计时")或者营收推送
                {
                    weChatMain.RevenueType = pushMsgModel.RevenueType;
                }
                weChatMain.Label = pushMsgModel.Label;
                if (pushMsgModel.MessageType == 12)
                {
                    weChatMain.CountersignType = pushMsgModel.CountersignType;
                }
                if (pushMsgModel.MessageType == 13 || pushMsgModel.MessageType == 14)
                {
                    weChatMain.RedpacketType = pushMsgModel.RedpacketType;
                    weChatMain.RedpacketMoney = pushMsgModel.RedpacketMoney;
                    weChatMain.RedpacketMoneyFrom = pushMsgModel.RedpacketMoneyFrom;
                    weChatMain.RedpacketMoneyTo = pushMsgModel.RedpacketMoneyTo;
                }
                //副表信息赋值(微信推送详细信息)
                string pushObject = string.Empty;
                Guid vguid;           //部门vguid
                string[] arrObject;    //推送人数组
                if (string.IsNullOrEmpty(pushMsgModel.PushObject))  //说明没有选择推送人 选的是标签
                {
                    weChatMain.Department_VGUID = Guid.Empty;
                    arrObject = _ds.GetPeopleByLabelAndDep(Guid.Empty, pushMsgModel.Label).ToArray();
                }
                else
                {
                    pushObject = pushMsgModel.PushObject.TrimEnd('|');
                    if (!Guid.TryParse(pushObject, out vguid))
                    {
                        arrObject = pushObject.Split('|');
                        weChatMain.Department_VGUID = Guid.Empty;
                    }
                    else
                    {
                        switch (pushMsgModel.MessageType)
                        {
                            case 11:
                                arrObject = pushMsgModel.RevenueType == 1 ? _ds.GetFixedPayPerson(vguid, weChatMain.Label) : _ds.GetRevenuePerson(vguid, weChatMain.Label);
                                break;
                            default:
                                arrObject = _ds.GetDepartPerson(vguid, weChatMain.Label);
                                break;
                        }
                        weChatMain.Department_VGUID = vguid;
                    }
                }
                foreach (var item in arrObject)
                {
                    Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                    weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                    weChatDetail.Type = pushMsgModel.PushType.ToString();
                    weChatDetail.Vguid = Guid.NewGuid();
                    weChatDetail.ISRead = "0"; //未读
                    weChatDetail.PushObject = item;
                    weChatDetail.CreatedDate = DateTime.Now;
                    weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetail.ChangeDate = DateTime.Now;
                    weChatDetailList.Add(weChatDetail);
                }
                result = _ds.SavePushMsg(weChatMain, weChatDetailList, false);
            }

            return result;
        }


        /// <summary>
        /// 保存多图文信息
        /// </summary>
        /// <param name="wechatPushList"></param>
        /// <param name="wechatPushMoreGraphicList"></param>
        /// <param name="isEdit"></param>
        /// <param name="saveType"></param>
        ///  <param name="countersignType"></param>
        /// <returns></returns>
        public bool APISaveImagePushMsg(U_PushMsg pushMsgModel, List<Business_WeChatPush_MoreGraphic_Information> pushMoreGraphicList)
        {
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();
            var weChatMain = new Business_WeChatPush_Information();

            weChatMain.Title = pushMsgModel.Title;
            weChatMain.PushType = pushMsgModel.PushType;
            weChatMain.MessageType = pushMsgModel.MessageType;
            weChatMain.Timed = pushMsgModel.Timed;
            weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
            weChatMain.Important = pushMsgModel.Important;
            weChatMain.Message = pushMsgModel.Message;
            weChatMain.CoverImg = pushMsgModel.CoverImg;
            weChatMain.CoverDescption = pushMsgModel.CoverDescption;
            weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
            weChatMain.PushDate = pushMsgModel.PushDate;
            weChatMain.PushPeople = pushMsgModel.PushPeople;
            weChatMain.Status = 4;
            weChatMain.CreatedDate = pushMsgModel.CreatedDate;
            weChatMain.CreatedUser = pushMsgModel.CreatedUser;
            weChatMain.VGUID = pushMsgModel.VGUID;
            weChatMain.History = pushMsgModel.History;
            weChatMain.Label = pushMsgModel.Label;
            weChatMain.CountersignType = null;

            string pushObject = pushMsgModel.PushObject.TrimEnd('|');
            string[] arrObject = pushObject.Split('|');
            foreach (var item in arrObject)
            {
                Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                weChatDetail.Type = pushMsgModel.PushType.ToString();
                weChatDetail.Vguid = Guid.NewGuid();
                weChatDetail.ISRead = "0"; //未读
                weChatDetail.PushObject = item;
                weChatDetail.CreatedDate = pushMsgModel.CreatedDate;
                weChatDetail.CreatedUser = pushMsgModel.CreatedUser;
                weChatDetailList.Add(weChatDetail);
            }
            return _ds.SaveImagePushMsg(weChatMain, pushMoreGraphicList, weChatDetailList, false);

        }

        /// <summary>
        /// 保存多图文信息
        /// </summary>
        /// <param name="wechatPushList"></param>
        /// <param name="wechatPushMoreGraphicList"></param>
        /// <param name="isEdit"></param>
        /// <param name="saveType"></param>
        ///  <param name="countersignType"></param>
        /// <returns></returns>
        public bool SaveImagePushMsg(string wechatPushList, string wechatPushMoreGraphicList, bool isEdit, string saveType, string countersignType)
        {
            var pushMsgModel = JsonHelper.JsonToModel<List<U_PushMsg>>(wechatPushList).SingleOrDefault();// 多图文主表信息
            var pushMoreGraphicList = JsonHelper.JsonToModel<List<Business_WeChatPush_MoreGraphic_Information>>(wechatPushMoreGraphicList); //多图文子表信息
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();
            var weChatMain = new Business_WeChatPush_Information();
            if (isEdit)  //编辑
            {
                weChatMain = GetWeChatMainByVguid(pushMsgModel.VGUID.ToString());
                weChatMain.Title = pushMsgModel.Title;
                weChatMain.CoverImg = pushMsgModel.CoverImg;
                weChatMain.CoverDescption = pushMsgModel.CoverDescption;
                weChatMain.Timed = pushMsgModel.Timed;
                weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
                weChatMain.Important = pushMsgModel.Important;
                weChatMain.Message = pushMsgModel.Message;
                weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
                weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.Status = int.Parse(saveType);
                weChatMain.ChangeDate = DateTime.Now;
                weChatMain.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.History = bool.Parse(pushMsgModel.History) ? "1" : "0";
                weChatMain.Label = pushMsgModel.Label;
                weChatMain.CountersignType = null;
                //多图文子表信息
                foreach (var graphicInformation in pushMoreGraphicList)
                {
                    graphicInformation.CreatedDate = DateTime.Now;
                    graphicInformation.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    graphicInformation.VGUID = Guid.NewGuid();
                    graphicInformation.WeChatPushVguid = weChatMain.VGUID;
                }
                //副表信息赋值(微信推送详细信息)
                string pushObject = string.Empty;
                Guid vguid;           //部门vguid
                string[] arrObject;    //推送人数组
                if (string.IsNullOrEmpty(pushMsgModel.PushObject))  //说明没有选择推送人 选的是标签
                {
                    weChatMain.Department_VGUID = Guid.Empty;
                    arrObject = _ds.GetPeopleByLabelAndDep(Guid.Empty, pushMsgModel.Label).ToArray();
                }
                else
                {
                    pushObject = pushMsgModel.PushObject.TrimEnd('|');
                    if (!Guid.TryParse(pushObject, out vguid))
                    {
                        arrObject = pushObject.Split('|');
                        weChatMain.Department_VGUID = Guid.Empty;
                    }
                    else
                    {
                        arrObject = _ds.GetDepartPerson(vguid, weChatMain.Label);
                        weChatMain.Department_VGUID = vguid;
                    }
                }
                foreach (var item in arrObject)
                {
                    Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                    weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                    weChatDetail.Type = pushMsgModel.PushType.ToString();
                    weChatDetail.Vguid = Guid.NewGuid();
                    weChatDetail.ISRead = "0"; //未读
                    weChatDetail.PushObject = item;
                    weChatDetail.CreatedDate = DateTime.Now;
                    weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetail.ChangeDate = DateTime.Now;
                    weChatDetailList.Add(weChatDetail);
                }
                return _ds.SaveImagePushMsg(weChatMain, pushMoreGraphicList, weChatDetailList, true);
            }
            else         //新增
            {
                weChatMain.Title = pushMsgModel.Title;
                weChatMain.PushType = pushMsgModel.PushType;
                weChatMain.MessageType = pushMsgModel.MessageType;
                weChatMain.Timed = pushMsgModel.Timed;
                weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
                weChatMain.Important = pushMsgModel.Important;
                weChatMain.Message = pushMsgModel.Message;
                weChatMain.CoverImg = pushMsgModel.CoverImg;
                weChatMain.CoverDescption = pushMsgModel.CoverDescption;
                weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
                weChatMain.PushDate = pushMsgModel.PushDate;
                weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.Status = int.Parse(saveType);
                weChatMain.CreatedDate = DateTime.Now;
                weChatMain.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                weChatMain.ChangeDate = DateTime.Now;
                weChatMain.VGUID = Guid.NewGuid();
                weChatMain.History = bool.Parse(pushMsgModel.History) ? "1" : "0";
                weChatMain.Label = pushMsgModel.Label;
                weChatMain.CountersignType = null;
                if (countersignType.TryToInt() != 0)
                {
                    weChatMain.CountersignType = int.Parse(countersignType);
                }
                //多图文子表信息
                foreach (var graphicInformation in pushMoreGraphicList)
                {
                    graphicInformation.CreatedDate = DateTime.Now;
                    graphicInformation.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    graphicInformation.VGUID = Guid.NewGuid();
                    graphicInformation.WeChatPushVguid = weChatMain.VGUID;
                }
                //副表信息赋值(微信推送详细信息)
                string pushObject = string.Empty;
                Guid vguid;           //部门vguid
                string[] arrObject;    //推送人数组
                if (string.IsNullOrEmpty(pushMsgModel.PushObject))  //说明没有选择推送人 选的是标签
                {
                    weChatMain.Department_VGUID = Guid.Empty;
                    arrObject = _ds.GetPeopleByLabelAndDep(Guid.Empty, pushMsgModel.Label).ToArray();
                }
                else
                {
                    pushObject = pushMsgModel.PushObject.TrimEnd('|');
                    if (!Guid.TryParse(pushObject, out vguid))
                    {
                        arrObject = pushObject.Split('|');
                        weChatMain.Department_VGUID = Guid.Empty;
                    }
                    else
                    {
                        arrObject = _ds.GetDepartPerson(vguid, weChatMain.Label);
                        weChatMain.Department_VGUID = vguid;
                    }
                }
                foreach (var item in arrObject)
                {
                    Business_WeChatPushDetail_Information weChatDetail = new Business_WeChatPushDetail_Information();
                    weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                    weChatDetail.Type = pushMsgModel.PushType.ToString();
                    weChatDetail.Vguid = Guid.NewGuid();
                    weChatDetail.ISRead = "0"; //未读
                    weChatDetail.PushObject = item;
                    weChatDetail.CreatedDate = DateTime.Now;
                    weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetail.ChangeDate = DateTime.Now;
                    weChatDetailList.Add(weChatDetail);
                }
                return _ds.SaveImagePushMsg(weChatMain, pushMoreGraphicList, weChatDetailList, false);
            }
        }

        /// <summary>
        /// 保存工资条推送消息
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <param name="notExists"></param>
        /// <returns></returns>
        public bool SaveSalaryPush(U_PushMsg pushMsgModel, ref string msg)
        {
            Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();
            //主信息赋值(微信推送信息)
            weChatMain.Title = pushMsgModel.Title;
            weChatMain.PushType = 1;  //微信推送
            weChatMain.MessageType = 15; //工资条推送
            weChatMain.Timed = pushMsgModel.Timed;
            weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
            weChatMain.Important = pushMsgModel.Important;
            weChatMain.Message = pushMsgModel.Message;
            weChatMain.CoverImg = pushMsgModel.CoverImg;
            weChatMain.CoverDescption = pushMsgModel.CoverDescption;
            weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
            weChatMain.ExercisesVGUID = pushMsgModel.ExercisesVGUID; //推送习题Vguid
            weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
            weChatMain.Status = 3;
            weChatMain.CreatedDate = DateTime.Now;
            weChatMain.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
            weChatMain.ChangeDate = DateTime.Now;
            weChatMain.VGUID = Guid.NewGuid();
            weChatMain.History = "0";
            weChatMain.RevenueType = null;
            weChatMain.CountersignType = null;
            var pushObjs = _ds.GetPushObjs();
            foreach (var pushObj in pushObjs)
            {
                var weChatDetail = new Business_WeChatPushDetail_Information();
                weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                weChatDetail.Type = pushMsgModel.PushType.ToString();
                weChatDetail.Vguid = Guid.NewGuid();
                weChatDetail.ISRead = "0"; //未读
                weChatDetail.PushObject = pushObj.UserID;
                weChatDetail.CreatedDate = DateTime.Now;
                weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                weChatDetail.ChangeDate = DateTime.Now;
                weChatDetailList.Add(weChatDetail);
            }
            return _ds.SaveSalaryPush(weChatMain, weChatDetailList, ref msg);
        }

        /// <summary>
        /// 保存工资条推送消息
        /// </summary>
        /// <param name="pushMsgModel"></param>
        /// <param name="notExists"></param>
        /// <returns></returns>
        public bool SaveMaintenancePush(U_PushMsg pushMsgModel, ref string msg)
        {
            string weChatMaintanceTemp = ConfigSugar.GetAppString("WeChatMaintanceTemp");//获取微信发送车辆保养数据模板
            string weChatMaintanceValidateTemp = ConfigSugar.GetAppString("WeChatMaintanceValidateTemp");//获取微信发送车辆保养数据模板
            var pushObjs = _ds.GetPushMaintenceObjs();
            var notExists = new List<NotExistPerson>();
            List<Business_WeChatPush_Information> weChatMainList = new List<Business_WeChatPush_Information>();
            List<Business_WeChatPushDetail_Information> weChatDetailList = new List<Business_WeChatPushDetail_Information>();
            foreach (var pushObj in pushObjs)
            {
                if (!string.IsNullOrWhiteSpace(pushObj.PhoneNumberU))
                {
                    Guid mainVguid = Guid.NewGuid();
                    pushObj.PushVGUID = mainVguid;

                    //更新车辆数据
                    _ds.UpdateCarMaintaince(pushObj.Vguid, mainVguid);

                    //获取推送的主题信息和详情信息
                    #region
                    //判断是否验车
                    bool isVerify = pushObj.IsVerify;
                    string templateValidate = "";
                    if (isVerify) templateValidate = string.Format(weChatMaintanceValidateTemp, isVerify ? "验" : "否");
                    string template = string.Format(weChatMaintanceTemp, pushObj.CarNO, pushObj.Name, pushObj.MaintainDate.ToShortDateString(), pushObj.Mileage, pushObj.Address, pushObj.Type, templateValidate);

                    //人员存在
                    Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();
                    //主信息赋值(微信推送信息)
                    weChatMain.Title = pushMsgModel.Title;
                    weChatMain.PushType = 1;  //微信推送
                    weChatMain.MessageType = 1; //保养推送\文本推送
                    weChatMain.Timed = pushMsgModel.Timed;
                    weChatMain.TimedSendTime = pushMsgModel.TimedSendTime;
                    weChatMain.Important = pushMsgModel.Important;
                    weChatMain.Message = template;//推送的信息
                    weChatMain.CoverImg = pushMsgModel.CoverImg;
                    weChatMain.CoverDescption = pushMsgModel.CoverDescption;
                    weChatMain.PeriodOfValidity = pushMsgModel.PeriodOfValidity;
                    weChatMain.ExercisesVGUID = pushMsgModel.ExercisesVGUID; //推送习题Vguid
                    weChatMain.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                    weChatMain.Status = 3;
                    weChatMain.CreatedDate = DateTime.Now;
                    weChatMain.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatMain.ChangeDate = DateTime.Now;
                    weChatMain.VGUID = mainVguid;
                    weChatMain.History = "0";
                    weChatMain.RevenueType = null;
                    weChatMain.CountersignType = null;
                    weChatMainList.Add(weChatMain);

                    //推送详细信息
                    var weChatDetail = new Business_WeChatPushDetail_Information();
                    weChatDetail.Business_WeChatPushVguid = weChatMain.VGUID;
                    weChatDetail.Type = pushMsgModel.PushType.ToString();
                    weChatDetail.Vguid = Guid.NewGuid();
                    weChatDetail.ISRead = "0"; //未读
                    weChatDetail.PushObject = pushObj.UserID;
                    weChatDetail.CreatedDate = DateTime.Now;
                    weChatDetail.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                    weChatDetail.ChangeDate = DateTime.Now;
                    weChatDetailList.Add(weChatDetail);
                    #endregion

                    //db.Update<Car_Maintain>(new { PushVGUID = weChatMain.VGUID }, it => it.PushVGUID == null);
                }
                else
                {
                    //人员不存在
                    var notExistPerson = new NotExistPerson();
                    notExistPerson.IDNumber = pushObj.CarNO;
                    notExistPerson.Name = pushObj.Name;
                    notExistPerson.MobilePhone = pushObj.PhoneNumber;
                    notExists.Add(notExistPerson);
                }
            }
            return _ds.SaveMaintenancePush(pushObjs, weChatMainList, weChatDetailList, notExists, ref msg);
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        public void DownLoadTemplate()
        {
            string fileName = ConfigSugar.GetAppString("PushPeopleTemplate");
            UploadHelper.ExportExcel("PushPeopleTemplate.xlsx", fileName);
        }

        /// <summary>
        ///  下载模板
        /// </summary>
        public void DownLoadPushTemplate()
        {
            string fileName = ConfigSugar.GetAppString("PushTemplate");
            UploadHelper.ExportExcel("PushTemplate.xlsx", fileName);
        }

        /// <summary>
        /// 在上传的excel中找到用户名和用户id
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public dynamic SearchUserId(DataTable dt)
        {
            string userName = String.Empty;
            string userId = String.Empty;
            List<string> pushObjects = new List<string>();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["column1"] != null && dr["column2"] != null)
                {
                    userName += dr["column1"] + ",";
                    userId += dr["column2"] + "|";
                    pushObjects.Add(dr["column2"].ToString());
                }
            }
            var listAll = _ds.GetAllPeople();
            var exceptUsers = pushObjects.Except(listAll);

            if (exceptUsers.Any())
            {
                string notExist = string.Join(",", exceptUsers);
                throw new Exception(notExist + "人员不存在");
            }
            userName = userName.TrimEnd(',');
            userId = userId.TrimEnd('|');

            return new { userName = userName, userId = userId };


        }

        /// <summary>
        /// 保存上传的推送信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveUploadPushMSg(DataTable dt, ref string msg)
        {
            Business_WeChatPush_Information weChatMain = new Business_WeChatPush_Information();
            List<Business_WeChatPushDetail_Information> listPushDetail = new List<Business_WeChatPushDetail_Information>();
            //  List<string> pushObjects = new List<string>();
            List<Business_WeChatPush_Information> listPushMain = new List<Business_WeChatPush_Information>();
            var notExistPeople = new List<NotExistPerson>();
            if (string.IsNullOrEmpty(dt.Rows[1]["column2"].ToString()))
            {
                throw new Exception("标题不能为空！");
            }
            weChatMain.Title = dt.Rows[1]["column2"].ToString();
            weChatMain.PushType = dt.Rows[2]["column4"].ToString() == "微信推送" ? 1 : 2;
            weChatMain.MessageType = 1;
            if (string.IsNullOrEmpty(dt.Rows[2]["column8"].ToString()))
            {
                weChatMain.Timed = false;
                weChatMain.TimedSendTime = null;
            }
            else
            {
                weChatMain.Timed = true;
                weChatMain.TimedSendTime = DateTime.Parse(dt.Rows[2]["column8"].ToString());
                if (weChatMain.TimedSendTime < DateTime.Now)
                {
                    throw new Exception("定时发送时间不能小于当前时间！");
                }
            }
            if (string.IsNullOrEmpty(dt.Rows[2]["column12"].ToString()))
            {
                weChatMain.PeriodOfValidity = null;
                weChatMain.Important = true;
            }
            else
            {
                weChatMain.Important = false;
                weChatMain.PeriodOfValidity = DateTime.Parse(dt.Rows[2]["column12"].ToString());
                if (weChatMain.PeriodOfValidity < DateTime.Now)
                {
                    throw new Exception("有效时间不能小于当前时间！");
                }
            }
            //第一行是说明信息，从第二行开始
            for (int i = 4; i < dt.Rows.Count; i++)
            {
                Guid vguid = Guid.NewGuid();
                Business_WeChatPush_Information weChatPush = new Business_WeChatPush_Information();
                weChatPush.Title = weChatMain.Title;
                weChatPush.PushType = weChatMain.PushType;
                weChatPush.MessageType = weChatMain.MessageType;
                weChatPush.Timed = weChatMain.Timed;
                weChatPush.TimedSendTime = weChatMain.TimedSendTime;
                weChatPush.PeriodOfValidity = weChatMain.PeriodOfValidity;
                weChatPush.Important = weChatPush.Important;
                weChatPush.PushPeople = CurrentUser.GetCurrentUser().LoginName;
                weChatPush.CreatedDate = DateTime.Now;
                weChatPush.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                weChatPush.VGUID = vguid;
                Business_WeChatPushDetail_Information weChatPushDetail = new Business_WeChatPushDetail_Information();
                DataRow dr = dt.Rows[i];
                weChatPushDetail.Type = weChatPush.PushType.ToString();
                weChatPushDetail.ISRead = "0";
                var name = dt.Rows[i]["column1"].ToString();  //姓名
                var phoneNo = dt.Rows[i]["column2"].ToString();   //手机号
                var idCardNo = dt.Rows[i]["column3"].ToString();  //身份证号
                NotExistPerson notExistPerson = new NotExistPerson();
                notExistPerson.Name = name;
                if (string.IsNullOrEmpty(phoneNo) && string.IsNullOrEmpty(idCardNo))
                {
                    throw new Exception("手机号和身份证号至少填写一个！");
                }
                if (!string.IsNullOrWhiteSpace(phoneNo))
                {
                    notExistPerson.MobilePhone = phoneNo;
                    var model = _ds.GetPersonnelInformation("PhoneNumber='" + phoneNo + "'");
                    if (model == null)
                    {
                        if (!string.IsNullOrWhiteSpace(idCardNo))
                        {
                            var personInfo = _ds.GetPersonnelInformation("IDNumber='" + idCardNo + "'");
                            if (personInfo != null)
                            {
                                weChatPushDetail.PushObject = personInfo.UserID;
                            }
                            else
                            {
                                notExistPerson.IDNumber = idCardNo;
                                notExistPeople.Add(notExistPerson);
                                continue;
                            }
                        }
                        else
                        {
                            notExistPeople.Add(notExistPerson);
                            continue;
                        }
                    }
                    else
                    {
                        weChatPushDetail.PushObject = phoneNo;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(idCardNo))
                {
                    var personInfo = _ds.GetPersonnelInformation("IDNumber='" + idCardNo + "'");
                    if (personInfo != null)
                    {
                        weChatPushDetail.PushObject = personInfo.UserID;
                    }
                    else
                    {
                        notExistPerson.IDNumber = idCardNo;
                        notExistPeople.Add(notExistPerson);
                        continue;
                    }
                }
                else
                {
                    notExistPeople.Add(notExistPerson);
                    continue;

                }
                // pushObjects.Add(dr["column2"].ToString());
                weChatPushDetail.CreatedUser = weChatPush.CreatedUser;
                weChatPushDetail.CreatedDate = weChatPush.CreatedDate;
                weChatPushDetail.Vguid = Guid.NewGuid();
                weChatPushDetail.Business_WeChatPushVguid = vguid;
                weChatPush.Message = dr["column4"].ToString();
                weChatPush.History = dr["column13"].ToString() == "是" ? "1" : "2";
                weChatPush.Status = 3;
                listPushMain.Add(weChatPush);
                listPushDetail.Add(weChatPushDetail);

            }
            msg = "共计" + (listPushDetail.Count + notExistPeople.Count) + "条，成功推送" + listPushDetail.Count + "条，失败" + notExistPeople.Count + "条！";
            //var listAll = _ds.GetAllPeople();
            //var exceptUsers = pushObjects.Except(listAll);

            //if (exceptUsers.Any())
            //{
            //    string notExist = string.Join(",", exceptUsers);
            //    throw new Exception(notExist + "人员不存在");
            //}
            return _ds.SaveUploadPushMSg(listPushMain, listPushDetail, notExistPeople);
        }

        /// <summary>
        /// 保存上传的工资表
        /// </summary>
        /// <param name="salaryTable">工资表</param>
        public bool SaveUpLoadSalary(DataTable salaryTable)
        {
            salaryTable.Columns.Add("PersonnelVGUID", typeof(Guid));
            salaryTable.Columns.Add("PushVGUID", typeof(Guid));
            salaryTable.Columns.Add("CreateDate", typeof(DateTime));
            salaryTable.Columns.Add("CreateUser", typeof(string));
            salaryTable.Columns.Add("ChangeDate", typeof(DateTime));
            salaryTable.Columns.Add("ChangeUser", typeof(string));
            salaryTable.Columns.Add("VGUID", typeof(Guid));

            for (int i = 0; i < salaryTable.Rows.Count; i++)
            {
                salaryTable.Rows[i]["CreateDate"] = DateTime.Now;
                salaryTable.Rows[i]["CreateUser"] = CurrentUser.GetCurrentUser().LoginName;
                salaryTable.Rows[i]["VGUID"] = Guid.NewGuid();
            }
            return _ds.SaveUpLoadSalary(salaryTable);
        }

        /// <summary>
        /// 保存上传的保养信息
        /// </summary>
        /// <param name="salaryTable">工资表</param>
        public bool SaveUpLoadMaintence(DataTable salaryTable)
        {
            //表格列名修改
            salaryTable.Columns["车号"].ColumnName = "CarNO";
            salaryTable.Columns["工号"].ColumnName = "JobNumber";
            salaryTable.Columns["姓名"].ColumnName = "Name";
            salaryTable.Columns["手机"].ColumnName = "PhoneNumber";
            salaryTable.Columns["日期"].ColumnName = "MaintainDate";
            salaryTable.Columns["时间"].ColumnName = "MaintainTime";
            salaryTable.Columns["类别"].ColumnName = "Type";
            salaryTable.Columns["验车"].ColumnName = "IsVerify";
            salaryTable.Columns["地址"].ColumnName = "Address";
            salaryTable.Columns["里程"].ColumnName = "Mileage";

            DataTable newTable = _ds.GetCar_Maintain();

            for (int i = 0; i < salaryTable.Rows.Count; i++)
            {
                DataRow dr = newTable.NewRow();
                var itemRow = salaryTable.Rows[i];
                //验证手机号
                var phoneNo = itemRow["PhoneNumber"].ToString();
                var jobNumber = itemRow["JobNumber"].ToString();//工号
                var maintainDate = itemRow["MaintainDate"].ToString();//日期
                var maintainDateTime = itemRow["MaintainTime"].ToString();//日期时间
                if (string.IsNullOrEmpty(phoneNo))
                {
                    throw new Exception("手机号必填！");
                }
                //验证日期格式
                if (string.IsNullOrEmpty(maintainDate))
                {
                    throw new Exception("日期必填！");
                }
                DateTime dtDate;
                if (!DateTime.TryParse(maintainDate, out dtDate))
                {
                    throw new Exception("不是正确的日期格式类型！");
                }
                dr["CarNO"] = itemRow["CarNO"];
                dr["JobNumber"] = itemRow["JobNumber"];
                dr["Name"] = itemRow["Name"];
                dr["PhoneNumber"] = itemRow["PhoneNumber"];
                dr["MaintainDate"] = dtDate.ToShortDateString();
                dr["MaintainTime"] = maintainDateTime;
                dr["Type"] = itemRow["Type"];
                dr["IsVerify"] = itemRow["IsVerify"].ToString() == "验" ? true : false;
                dr["Address"] = itemRow["Address"];
                dr["Mileage"] = itemRow["Mileage"];
                dr["CreatedDate"] = DateTime.Now;
                dr["CreatedUser"] = CurrentUser.GetCurrentUser().LoginName;
                dr["Vguid"] = Guid.NewGuid();
                newTable.Rows.Add(dr);
            }
            return _ds.SaveUpLoadMaintence(newTable);
        }

        /// <summary>
        /// 获取所有人员标签
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetLabels()
        {
            return _ds.GetLabels();
        }

        /// <summary>
        /// 获取所有人员标签
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonnelLabel> GetAllLabel()
        {
            return _ds.GetAllLabel();
        }

        /// <summary>
        /// 获取清单数据（公用方法）
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<CS_Master_2> GetMasterDataType(string vguid)
        {
            return _ds.GetMasterDataType(vguid);
        }

        /// <summary>
        /// 分页获取导入推送中不存在的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<NotExistPerson> GetNotExistPeople(GridParams para)
        {
            return _ds.GetNotExistPeople(para);
        }

        /// <summary>
        /// 获取导入推送中不存在的人员信息
        /// </summary>
        /// <returns></returns>
        public void DownNotExistPeople(string fileName)
        {
            _ds.DownNotExistPeople(fileName);
        }

        /// <summary>
        /// 删除存储推送失败人员的临时表
        /// </summary>
        public void DropNotExistPersonTable()
        {
            _ds.DropNotExistPersonTable();
        }
    }
}