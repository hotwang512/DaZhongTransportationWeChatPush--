using System;
using DaZhongManagementSystem.Common.LogHelper;
using SqlSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Infrastructure.SugarDao
{
    public static class SugarDao_MsSql
    {
        public static SqlSugarClient GetInstance()
        {

            string connection = ConfigSugar.GetAppString("msSqlLinck");
            var db = new SqlSugarClient(connection);
            return db;
        }
    }
}
