using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.RideCheckFeedback;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.HomecomingSurvey.BusinessLogic
{
    public class HomecomingSurveyLogic
    {

        public HomecomingSurveyServer _hss;
        public HomecomingSurveyLogic()
        {
            _hss = new HomecomingSurveyServer();
        }

        public Business_HomecomingSurvey GetHomecomingSurvey(string user, string year)
        {

            return _hss.GetHomecomingSurvey(user, year);
        }

        public void AddHomecomingSurvey(Business_HomecomingSurvey hs)
        {
            hs.Vguid = Guid.NewGuid();
            hs.Year = DateTime.Now.Year.ToString();
            hs.CreatedDate = hs.CreatedDate = DateTime.Now;
            _hss.AddHomecomingSurvey(hs);
        }

        public void UpdateHomecomingSurvey(Business_HomecomingSurvey hs)
        {
            hs.ChangeUser = hs.CreatedUser;
            hs.ChangeDate = DateTime.Now;
            _hss.UpdateHomecomingSurvey(hs);
        }

        public List<ReturnHomeStatistics> ReturnHomeStatistics(string year, string dept)
        {
            return _hss.ReturnHomeStatistics(year, dept);
        }

        public void ReturnHomeStatisticsExport(string year, string dept)
        {
            var datasource = _hss.ExportReturnHomeStatistics(year, dept);
            Common.ExportExcel.ExportExcels("HomecomingSurveyTemplate.xlsx", "返乡统计.xls", datasource);
        }
    }
}