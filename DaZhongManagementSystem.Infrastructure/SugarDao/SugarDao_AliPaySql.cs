using System.Text;
using SqlSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.SugarDao
{
    public class SugarDao_AliPaySql
    {
        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("AlipayLink");
            return new SqlSugarClient(connection);
        }
    }
}
