using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.Salary
{
    public class SalaryServer
    {
        /// <summary>
        /// 根据身份证号和推送vguid获取与员工的工资信息
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns>工资信息</returns>
        public Business_Payroll_Information GetSalaryInfo(Business_Payroll_Information searchParams)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_Payroll_Information>().Where(i => i.IDCard == searchParams.IDCard && i.PushVGUID == searchParams.PushVGUID).SingleOrDefault();
            }
        }
    }
}