using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback.BusinessLogic;
using DaZhongManagementSystem.Areas.Systemmanagement.Controllers.AuthorityManagement.BusinessLogic;
using DaZhongManagementSystem.Controllers;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.RideCheckFeedback
{
    public class RideCheckFeedbackController : BaseController
    {
        public RideCheckFeedbackLogic _rl;
        public AuthorityManageLogic _al;
        public RideCheckFeedbackController()
        {
            _rl = new RideCheckFeedbackLogic();
            _al = new AuthorityManageLogic();
        }

        public Action Index()
        {


        }
    }
}