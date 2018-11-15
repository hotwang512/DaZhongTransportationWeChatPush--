using SqlSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.SugarDao
{
    public class SugarDao_ReckoningSql
    {
        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("ReckoningLink");
            var db = new SqlSugarClient(connection);
            return db;
        }
    }
}