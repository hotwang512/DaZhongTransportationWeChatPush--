using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure;
using JQWidgetsSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Common.WeChatPush;
using System.IO;
using System.Text;
using DaZhongManagementSystem.Models.WeChatPush;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers
{
    public class ShortMsgController : Controller
    {
        //
        // GET: /WeChatPush/ShortMsg/
        public ShortMsgLogic.ShortMsgLogic _sl;
        public ShortMsgController()
        {
            _sl = new ShortMsgLogic.ShortMsgLogic();
        }

        public ActionResult Index()
        {
            return View();
        }

       
    }
}
