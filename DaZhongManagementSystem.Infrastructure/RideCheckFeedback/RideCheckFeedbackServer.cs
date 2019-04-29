using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Business_RideCheckFeedback AddBusiness_RideCheckFeedback(string user)
        {
            Business_RideCheckFeedback rideCheckFeedback = new Business_RideCheckFeedback();
            rideCheckFeedback.FeedbackCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            rideCheckFeedback.CreateUser = "";
            rideCheckFeedback.CreateDate = DateTime.Now;
            rideCheckFeedback.VGUID = Guid.NewGuid();
            return rideCheckFeedback;
        }


        public List<Business_RideCheckFeedback> GetRideCheckFeedbackList(DateTime startTime, DateTime endTime, string status, string user)
        {
            List<Business_RideCheckFeedback> rideCheckFeedbacks = new List<Business_RideCheckFeedback>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedbacks = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(i => i.Status == "2" && i.CreateUser == user && i.CreateDate >= startTime && i.CreateDate <= endTime).OrderBy(i => i.CreateDate, OrderByType.Desc).ToList();
            }
            return rideCheckFeedbacks;
        }

        public List<Business_RideCheckFeedback> GetRideCheckFeedbackList(DateTime startTime, DateTime endTime, string user)
        {
            List<Business_RideCheckFeedback> rideCheckFeedbacks = new List<Business_RideCheckFeedback>();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedbacks = _dbMsSql.Queryable<Business_RideCheckFeedback>().Where(i => i.CreateUser == user && i.CreateDate >= startTime && i.CreateDate <= endTime).OrderBy(i => i.CreateDate, OrderByType.Desc).ToList();
            }
            return rideCheckFeedbacks;
        }

        public Business_RideCheckFeedback_Item GetRideCheckFeedbackInfo(Guid vguid)
        {
            Business_RideCheckFeedback_Item rideCheckFeedback = new Business_RideCheckFeedback_Item();
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                rideCheckFeedback = _dbMsSql.Queryable<Business_RideCheckFeedback_Item>().Where(i => i.RideCheckFeedbackVGUID == vguid).SingleOrDefault();
                rideCheckFeedback.Attachments = _dbMsSql.Queryable<Business_RideCheckFeedback_Item_Attachment>().Where(c => c.RideCheckFeedbackVGUID == vguid).ToList();
            }
            return rideCheckFeedback;
        }
    }
}
