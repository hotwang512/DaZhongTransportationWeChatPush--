using System;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.Salary;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.Salary.BusinessLogic
{
    public class SalaryLogic
    {

        private readonly SalaryServer _salaryServer;

        public SalaryLogic()
        {
            _salaryServer = new SalaryServer();
        }

        /// <summary>
        /// 根据身份证号和推送vguid获取与员工的工资信息
        /// </summary>
        /// <param name="idNumber">身份证号</param>
        /// <param name="pushVguid">推送vguid</param>
        /// <returns>工资信息</returns>
        public Business_Payroll_Information GetSalaryInfo(string idNumber, string pushVguid)
        {
            var searchParams = new Business_Payroll_Information { IDCard = idNumber, PushVGUID = Guid.Parse(pushVguid) };
            return _salaryServer.GetSalaryInfo(searchParams);
        }
    }
}