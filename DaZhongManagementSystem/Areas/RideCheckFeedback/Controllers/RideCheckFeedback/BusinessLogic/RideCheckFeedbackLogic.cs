using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.RideCheckFeedback;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic
{
    public class RideCheckFeedbackLogic
    {
        public RideCheckFeedbackServer _rcfs;
        public RideCheckFeedbackLogic()
        {
            _rcfs = new RideCheckFeedbackServer();
        }

        public Business_RideCheckFeedback GetUserNewRideCheckFeedback(string user)
        {
            return _rcfs.GetUserNewRideCheckFeedback(user);
        }


        public int GetRideCheckFeedbackCount(DateTime startTime, DateTime endTime, string user)
        {
            return _rcfs.GetRideCheckFeedbackCount(startTime, endTime, user);
        }

        public Business_RideCheckFeedback AddBusiness_RideCheckFeedback(string user)
        {
            return _rcfs.AddBusiness_RideCheckFeedback(user);
        }

        public bool SaveBusiness_RideCheckFeedbackItem(string user, Guid rideCheckFeedbackVguid, int feedbackNumber, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, string answer7)
        {

            return _rcfs.SaveBusiness_RideCheckFeedbackItem(user, rideCheckFeedbackVguid, feedbackNumber, answer1, answer2, answer3, answer4, answer5, answer6, answer7);
        }
        public string SaveBusiness_RideCheckFeedbackAttachment(string user, Guid rideCheckFeedbackVguid, string fileName, string filePath)
        {
            return _rcfs.SaveBusiness_RideCheckFeedbackAttachment(user, rideCheckFeedbackVguid, fileName, filePath);
        }
        public void DeleteBusiness_RideCheckFeedbackAttachment(string filePath)
        {
            _rcfs.DeleteBusiness_RideCheckFeedbackAttachment(filePath);
        }
        public bool Submit(Guid vguid)
        {
            return _rcfs.Submit(vguid);
        }
        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserInfo(string userID)
        {
            return _rcfs.GetUserInfo(userID);
        }

        public int GetMonthCountConfig()
        {
            return _rcfs.GetMonthCountConfig();
        }

    }
}