using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Controllers
{
    public class APIController : Controller
    {
        public JsonResult GetRideCheckFailed(string numberPlate)
        {
            ExecutionResult result = new ExecutionResult();
            if (string.IsNullOrEmpty(numberPlate))
            {
                result.Message = "车牌号不能为空！";
            }
            else
            {
                try
                {
                    RideCheckFeedbackLogic logic = new RideCheckFeedbackLogic();
                    var rideCheckFaileds = logic.GetRideCheckFailed(numberPlate);
                    if (rideCheckFaileds.Count > 0)
                    {
                        List<string> list = new List<string>();
                        foreach (var item in rideCheckFaileds)
                        {
                            string str = string.Format("该车于时间：{0} 上车地点：{1} 下车地点：{2} 接受过跳车检查，存在以下不合格项：{3}",
                                item.跳车时间,
                                item.上车地点,
                                item.下车地点,
                                item.跳车检查结果.Replace("不合格", ""));
                            list.Add(str);
                        }
                        result.Result = list; 
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = "该车辆不存在不合格项！";
                    }

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}