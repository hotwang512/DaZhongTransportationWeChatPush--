using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.QRCodeManagement
{
    public class ScanDataServer
    {
        public LogLogic _ll;
        public ScanDataServer()
        {
            _ll = new LogLogic();
        }
        public Business_ScanData_Information Save(Guid vguid)
        {
            Business_ScanData_Information ScanData = new Business_ScanData_Information();
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var list = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == vguid).SingleOrDefault();
                var Organization = _dbMsSql.Queryable<Master_Organization>().Where(i => i.Vguid == list.OwnedFleet).SingleOrDefault();
                if (list != null)
                {
                    ScanData.MachineCode = "";
                    ScanData.SystemID = "";
                    ScanData.ID = list.IDNumber;
                    ScanData.Age = list.Age;
                    ScanData.Sex = list.Sex;
                    ScanData.JobNumber = list.JobNumber;
                    ScanData.ServiceNumber = list.ServiceNumber;
                    ScanData.OwnedFleet = Organization.OrganizationName.ToString();
                    ScanData.UserVguid = vguid;
                    ScanData.Vguid = Guid.NewGuid();
                    ScanData.LicensePlate = list.LicensePlate;
                    ScanData.Name = list.Name;
                    ScanData.ScanUser = list.UserID;
                    ScanData.PhoneNumber = list.PhoneNumber;
                    ScanData.ScanDate = DateTime.Now;
                    ScanData.CreatedDate = DateTime.Now;
                    ScanData.CreatedUser = list.Name;
                    bool result = false;

                    result = _dbMsSql.Insert(ScanData, false) != DBNull.Value;

                }
                if (list != null)
                {
                    var history = new Business_ScanHistory_Information()
                    {
                        MachineCode = "",
                        SystemID = "",
                        ScanUser = list.UserID,
                        Data = "",
                        User = "",
                        ScanTime = DateTime.Now,
                        CreatedUser = list.Name,
                        CreatedDate = DateTime.Now,
                        Vguid = Guid.NewGuid()
                    };
                    _dbMsSql.Insert(history, false);
                }

                //存入操作日志表
                string logData = JsonHelper.ModelToJson(ScanData);
                _ll.SaveLog(1, 2, list.Name, ScanData.Name, logData);

                return ScanData;
            }
        }
    }
}
