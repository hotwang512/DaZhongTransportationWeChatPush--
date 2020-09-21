using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.RideCheckFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback.Controllers.VehicleRepairComplaints.BusinessLogic
{
    public class Business_VehicleRepairComplaintsLogic
    {
        private Business_VehicleRepairComplaintsServer _bvrc;
        public Business_VehicleRepairComplaintsLogic()
        {
            _bvrc = new Business_VehicleRepairComplaintsServer();
        }

        public Business_VehicleRepairComplaints GetVehicleRepairComplaint(string userid, DateTime date)
        {
            return _bvrc.GetVehicleRepairComplaint(userid, date);
        }

        public void UpdateBusiness_VehicleRepairComplaints(Business_VehicleRepairComplaints bvrc)
        {
            _bvrc.UpdateBusiness_VehicleRepairComplaints(bvrc);
        }
    }
}