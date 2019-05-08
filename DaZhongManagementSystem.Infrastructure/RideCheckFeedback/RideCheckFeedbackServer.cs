using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.RideCheckFeedback
{
    public class RideCheckFeedbackServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public RideCheckFeedbackServer()
        {
            _logLogic = new LogLogic();
        }


        public Business_RideCheckFeedback GetUserNewRideCheckFeedback(string user)
        {
            Business_RideCheckFeedback rideCheckFeedback = null;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedback = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(c => c.Status == "1").OrderBy(c => c.CreateDate, OrderByType.Desc).FirstOrDefault();
                if (rideCheckFeedback != null)
                {
                    rideCheckFeedback.RideCheckFeedback_Items = _dbMsSql.Queryable<Business_RideCheckFeedback_Item>().Where(i => i.RideCheckFeedbackVGUID == rideCheckFeedback.VGUID).OrderBy(c => c.FeedbackNumber).ToList();
                    if (rideCheckFeedback.RideCheckFeedback_Items == null)
                    {
                        rideCheckFeedback.RideCheckFeedback_Items = new List<Business_RideCheckFeedback_Item>();
                    }
                    rideCheckFeedback.RideCheckFeedback_Attachments = _dbMsSql.Queryable<Business_RideCheckFeedback_Attachment>().Where(c => c.RideCheckFeedbackVGUID == rideCheckFeedback.VGUID).OrderBy(c => c.CreateDate).ToList();
                    if (rideCheckFeedback.RideCheckFeedback_Attachments == null)
                    {
                        rideCheckFeedback.RideCheckFeedback_Attachments = new List<Business_RideCheckFeedback_Attachment>();
                    }
                }
            }
            return rideCheckFeedback;

        }

        public Business_RideCheckFeedback AddBusiness_RideCheckFeedback(string user)
        {
            Business_RideCheckFeedback rideCheckFeedback = new Business_RideCheckFeedback();
            rideCheckFeedback.FeedbackCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            rideCheckFeedback.ChangeUser = rideCheckFeedback.CreateUser = user;
            rideCheckFeedback.ChangeDate = rideCheckFeedback.CreateDate = DateTime.Now;
            rideCheckFeedback.VGUID = Guid.NewGuid();
            rideCheckFeedback.Status = "1";
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                _dbMsSql.DisableInsertColumns = new string[] { "RideCheckFeedback_Items", "RideCheckFeedback_Attachments" };
                _dbMsSql.Insert(rideCheckFeedback);
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 1, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm"), "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 2, "", "", "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 3, "", "", "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 4, "", "", "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 5, "", "", "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 6, "", "", "", "", "", "", "");
                SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedback.VGUID, 7, "", "", "", "", "", "", "");
                rideCheckFeedback.RideCheckFeedback_Items = _dbMsSql.Queryable<Business_RideCheckFeedback_Item>().Where(c => c.RideCheckFeedbackVGUID == rideCheckFeedback.VGUID).OrderBy(c => c.FeedbackNumber).ToList();
            }

            return rideCheckFeedback;
        }

        public bool Submit(Guid vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Update<Business_RideCheckFeedback>(new { Status = "2" }, c => c.VGUID == vguid);
            }
        }

        public bool SaveBusiness_RideCheckFeedbackItem(string user, Guid rideCheckFeedbackVguid, int feedbackNumber, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, string answer7)
        {
            Business_RideCheckFeedback_Item item = new Business_RideCheckFeedback_Item();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                item = _dbMsSql.Queryable<Business_RideCheckFeedback_Item>().Where(c => c.RideCheckFeedbackVGUID == rideCheckFeedbackVguid && c.FeedbackNumber == feedbackNumber).FirstOrDefault();
                if (item == null)
                {
                    item = new Business_RideCheckFeedback_Item();
                    item.VGUID = Guid.NewGuid();
                    item.RideCheckFeedbackVGUID = rideCheckFeedbackVguid;
                    item.ChangeUser = item.CreateUser = user;
                    item.ChangeDate = item.CreateDate = DateTime.Now;
                    item.FeedbackNumber = feedbackNumber;
                    item.FeedbackAnswer1 = answer1;
                    item.FeedbackAnswer2 = answer2;
                    item.FeedbackAnswer3 = answer3;
                    item.FeedbackAnswer4 = answer4;
                    item.FeedbackAnswer5 = answer5;
                    item.FeedbackAnswer6 = answer6;
                    item.FeedbackAnswer7 = answer7;
                    _dbMsSql.Insert(item);
                }
                else
                {
                    item.FeedbackAnswer1 = answer1;
                    item.FeedbackAnswer2 = answer2;
                    item.FeedbackAnswer3 = answer3;
                    item.FeedbackAnswer4 = answer4;
                    item.FeedbackAnswer5 = answer5;
                    item.FeedbackAnswer6 = answer6;
                    item.FeedbackAnswer7 = answer7;
                    _dbMsSql.Update(item);
                }
            }

            return true;
        }

        public string SaveBusiness_RideCheckFeedbackAttachment(string user, Guid rideCheckFeedbackVguid, string fileName, string filePath)
        {
            Business_RideCheckFeedback_Attachment attachment = new Business_RideCheckFeedback_Attachment();
            attachment.VGUID = Guid.NewGuid();
            attachment.RideCheckFeedbackVGUID = rideCheckFeedbackVguid;
            attachment.ChangeUser = attachment.CreateUser = user;
            attachment.ChangeDate = attachment.CreateDate = DateTime.Now;
            attachment.AttachmentName = fileName;
            attachment.AttachmentPath = filePath;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                _dbMsSql.Insert(attachment);
            }
            return attachment.VGUID.ToString();
        }

        public void DeleteBusiness_RideCheckFeedbackAttachment(string filePath)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                //string.Format("delete Business_RideCheckFeedback_Attachment where AttachmentPath='{0}'", filePath)
                _dbMsSql.Delete<Business_RideCheckFeedback_Attachment>(c=>c.AttachmentPath== filePath);
            }
        }

        /// <summary>
        /// 根据状态和获取用户反馈单
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetRideCheckFeedbackCount(DateTime startTime, DateTime endTime, string user)
        {
            int count = 0;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                count = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(i => i.Status == "2" && i.CreateUser == user && i.CreateDate >= startTime && i.CreateDate <= endTime).Count();
            }
            return count;
        }
        /// <summary>
        /// 获取反馈单列表
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Business_RideCheckFeedback> GetRideCheckFeedbackList(DateTime startTime, DateTime endTime, string user)
        {
            List<Business_RideCheckFeedback> rideCheckFeedbacks = new List<Business_RideCheckFeedback>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedbacks = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(i => i.CreateUser == user && i.CreateDate >= startTime && i.CreateDate <= endTime).OrderBy(i => i.CreateDate, OrderByType.Desc).ToList();
            }
            return rideCheckFeedbacks;
        }
        /// <summary>
        /// 根据反馈单VGUID获取反馈单明细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_RideCheckFeedback_Item GetRideCheckFeedbackInfo(Guid vguid)
        {
            Business_RideCheckFeedback_Item rideCheckFeedback = new Business_RideCheckFeedback_Item();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedback = _dbMsSql.Queryable<Business_RideCheckFeedback_Item>().Where(i => i.RideCheckFeedbackVGUID == vguid).SingleOrDefault();
                //rideCheckFeedback.Attachments = _dbMsSql.Queryable<Business_RideCheckFeedback_Attachment>().Where(c => c.RideCheckFeedbackVGUID == vguid).ToList();
            }
            return rideCheckFeedback;
        }


        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Business_Personnel_Information personModel = new Business_Personnel_Information();
                try
                {
                    personModel = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == userID).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                    _logLogic.SaveLog(5, 34, "", userID + personModel.Name, ex.Message);
                }
                return personModel;
            }
        }

        /// <summary>
        /// 获取配置列表数据
        /// </summary>
        /// <returns></returns>
        public int GetMonthCountConfig()
        {
            int count = 1;
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var configList = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 72 && i.CreateUser != "QRCode").SingleOrDefault();
                if (configList != null)
                {
                    count = Convert.ToInt32(configList.ConfigValue);
                }
                return count;
            }
        }


    }
}
