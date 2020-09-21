using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure.RideCheckFeedback
{
    public class Business_VehicleRepairComplaintsServer
    {
        /// <summary>
        /// 日志
        /// </summary>
        public LogLogic _logLogic;
        public Business_VehicleRepairComplaintsServer()
        {
            _logLogic = new LogLogic();
        }

        public Business_VehicleRepairComplaints GetVehicleRepairComplaint(string userid, DateTime date)
        {
            Business_VehicleRepairComplaints vehicleRepairComplaints = null;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                vehicleRepairComplaints = _dbMsSql.Queryable<Business_VehicleRepairComplaints>().Where(c => c.CreateUser == userid & c.ReflectDate == date).FirstOrDefault();
            }
            return vehicleRepairComplaints;
        }
        public void UpdateBusiness_VehicleRepairComplaints(Business_VehicleRepairComplaints vehicleRepairComplaints)
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                if (vehicleRepairComplaints.VGUID == Guid.Empty)
                {
                    vehicleRepairComplaints.VGUID = Guid.NewGuid();
                    vehicleRepairComplaints.CreateDate = DateTime.Now;
                    vehicleRepairComplaints.ChangeDate = DateTime.Now;
                    vehicleRepairComplaints.ChangeUser = vehicleRepairComplaints.CreateUser;
                    _dbMsSql.Insert(vehicleRepairComplaints);
                }
                else
                {
                    vehicleRepairComplaints.ReflectDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    vehicleRepairComplaints.ChangeDate = DateTime.Now;
                    vehicleRepairComplaints.ChangeUser = vehicleRepairComplaints.CreateUser;
                    _dbMsSql.Update(vehicleRepairComplaints);
                }
            }

        }
    }
}
