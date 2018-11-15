using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable
{
    public class Cab
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string License { get; set; }

        public DateTime LicenseDate { get; set; }

        public string EngineNo { get; set; }

        public string FrameNo { get; set; }

        public DateTime ProductDate { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime RetireDate { get; set; }

        public DateTime StartDate { get; set; }

        public int OriginalId { get; set; }

        public int OperationStatus { get; set; }

        public int VehicleStatus { get; set; }

        public int OrganizationID { get; set; }

        public int VehicleAbbreviationID { get; set; }

        public int MotorcadeID { get; set; }

        public int WorkTypeID { get; set; }

        public string OwnerName { get; set; }

        public int ABType { get; set; }
    }
}
