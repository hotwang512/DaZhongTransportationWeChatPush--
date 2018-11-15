using SqlSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.SugarDao
{
    public class SugarDao_LandaVSql
    {
        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("LandaV9Link");
            return new SqlSugarClient(connection);
        }
    }
}
