using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.SugarDao
{
    public class SugarDao_DriverRevenueSql
    {

        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("RevenueLink");
            return new SqlSugarClient(connection);
        }
    }
}
