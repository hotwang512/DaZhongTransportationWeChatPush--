using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using DaZhongManagementSystem.Entities.UserDefinedEntity;

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
                rideCheckFeedback = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(c => c.Status == "1" && c.CreateUser == user).OrderBy(c => c.CreateDate, OrderByType.Desc).FirstOrDefault();
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

        public bool Submit(string user, Guid vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Update<Business_RideCheckFeedback>(new { Status = "2", ChangeDate = DateTime.Now, ChangeUser = user }, c => c.VGUID == vguid);
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
                _dbMsSql.Delete<Business_RideCheckFeedback_Attachment>(c => c.AttachmentPath == filePath);
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

        public List<RideCheck> GetRideCheckFailed(string numberPlate)
        {
            string sql = @"select * from (select VGUID,跳车人姓名,提交时间,跳车时间,车号,所属公司,所属车队,上车地点,下车地点,服务卡号,case 跳车检查结果 when '' then '合格'  else 跳车检查结果 end as 跳车检查结果,备注信息
 from(
        SELECT Business_RideCheckFeedback.VGUID,Business_Personnel_Information.Name as 跳车人姓名, (Business_RideCheckFeedback_Item.FeedbackAnswer1 + ' ' + Business_RideCheckFeedback_Item.FeedbackAnswer2) as 跳车时间,
               Business_RideCheckFeedback.ChangeDate as 提交时间, Business_RideCheckFeedback_Item.FeedbackAnswer3 as 车号, New_Organization.name as 所属公司, motacade.name as 所属车队,
               Business_RideCheckFeedback_Item.FeedbackAnswer4 as 上车地点, Business_RideCheckFeedback_Item.FeedbackAnswer5 as 下车地点, Business_RideCheckFeedback_Item.FeedbackAnswer6 as 服务卡号,
               rtrim(ltrim(((case  Business_RideCheckFeedback_Item_num2.FeedbackAnswer1 when'A' then '' else '不合格:' end) + '' +
              (case  Business_RideCheckFeedback_Item_num2.FeedbackAnswer2 when'1' then '肇事痕迹' else '' end) + ' ' +
              (case  Business_RideCheckFeedback_Item_num2.FeedbackAnswer3 when'1' then '车厢内脏' else '' end) + ' ' +
              (case  Business_RideCheckFeedback_Item_num2.FeedbackAnswer4 when'1' then '座套脏' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num2.FeedbackAnswer5 when'1' then '服务设施坏（座椅/空调/音响等）' else ' ' end)) +' ' +
              ((case Business_RideCheckFeedback_Item_num3.FeedbackAnswer1 when'A' then '' else '不合格:' end)+'' +
              (case  Business_RideCheckFeedback_Item_num3.FeedbackAnswer2 when'1' then '未穿识别服' else '' end)+' ' +
              (case  Business_RideCheckFeedback_Item_num3.FeedbackAnswer4 when'1' then '留发/蓄须' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num3.FeedbackAnswer5 when'1' then '仪容不整洁' else ' ' end)) +' ' +
              ((case Business_RideCheckFeedback_Item_num4.FeedbackAnswer1 when'A' then '' else '不合格:' end)+'' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer2 when'1' then '上车问好' else '' end)+' ' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer3 when'1' then '确认行车路线' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer4 when'1' then '提醒前排乘客系好安全带' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer5 when'1' then '到达目的地后询问结算方式' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer6 when'1' then '结算后主动提供发票，提醒勿忘物品' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num4.FeedbackAnswer7 when'1' then '下车前致谢、道别' else ' ' end)) +' ' +
              ((case Business_RideCheckFeedback_Item_num5.FeedbackAnswer1 when'A' then '' else '不合格:' end)+'' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer2 when'1' then '车内吸烟' else '' end)+' ' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer3 when'1' then '行驶过程中打电话' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer4 when'1' then '不协助乘客放取行李' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer5 when'1' then '讲脏话' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer6 when'1' then '抛物' else '' end) +' ' +
              (case  Business_RideCheckFeedback_Item_num5.FeedbackAnswer7 when'1' then '向窗外吐痰' else ' ' end))))as 跳车检查结果,
              Business_RideCheckFeedback_Item_num6.FeedbackAnswer1 as 备注信息
           FROM[DEV_DaZhong_TransportAtion].[dbo].[Business_RideCheckFeedback]
              left join Business_Personnel_Information on Business_RideCheckFeedback.CreateUser = Business_Personnel_Information.Vguid
              left join Business_RideCheckFeedback_Item on Business_RideCheckFeedback.VGUID = Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID
              left join[128.2.9.199].middata.dbo.Cab as Cab on Business_RideCheckFeedback_Item.FeedbackAnswer3 = RIGHT(rtrim(Cab.License), 6)
              left join[128.2.9.199].middata.dbo.New_Organization as New_Organization on Cab.OrganizationID = New_Organization.id
              left join[128.2.9.199].middata.dbo.motacade as motacade on Cab.MotorcadeID = motacade.id
              left join (select * from Business_RideCheckFeedback_Item  where FeedbackNumber = 6) as Business_RideCheckFeedback_Item_num6 on Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID = Business_RideCheckFeedback_Item_num6.RideCheckFeedbackVGUID
              left join(select * from Business_RideCheckFeedback_Item  where FeedbackNumber = 2) as Business_RideCheckFeedback_Item_num2 on Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID = Business_RideCheckFeedback_Item_num2.RideCheckFeedbackVGUID
              left join(select * from Business_RideCheckFeedback_Item  where FeedbackNumber = 3) as Business_RideCheckFeedback_Item_num3 on Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID = Business_RideCheckFeedback_Item_num3.RideCheckFeedbackVGUID
              left join(select * from Business_RideCheckFeedback_Item  where FeedbackNumber = 4) as Business_RideCheckFeedback_Item_num4 on Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID = Business_RideCheckFeedback_Item_num4.RideCheckFeedbackVGUID
              left join(select * from Business_RideCheckFeedback_Item  where FeedbackNumber = 5) as Business_RideCheckFeedback_Item_num5 on Business_RideCheckFeedback_Item.RideCheckFeedbackVGUID = Business_RideCheckFeedback_Item_num5.RideCheckFeedbackVGUID
           where Business_RideCheckFeedback.Status = 2 and Business_RideCheckFeedback_Item.FeedbackNumber = 1 and CONVERT(char(7), Business_RideCheckFeedback_Item.FeedbackAnswer1, 102) = '2019-07'
                 and Cab.VehicleStatus = 1 AND Cab.OrganizationID <> '56'
            )a)t
         where 车号 = '{车号}' and 跳车检查结果!= '合格'
          order by 所属公司,所属车队";
            sql = sql.Replace("{车号}", numberPlate);
            List<RideCheck> rideChecks = new List<RideCheck>();
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {

                try
                {
                    rideChecks = dbMsSql.SqlQuery<RideCheck>(sql);
                    if (rideChecks.Count > 0)
                    {
                        string user = "API_User_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        foreach (var item in rideChecks)
                        {
                            dbMsSql.Update<Business_RideCheckFeedback>(new { Status = "3", ChangeUser = user }, c => c.VGUID == item.VGUID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                    //_logLogic.SaveLog(5, 34, "", userID + personModel.Name, ex.Message);
                }
                return rideChecks;
            }
        }




    }
}
