using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable
{
    public class Driver
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string EmployeeNo { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string IdCard { get; set; }

        public string Birthday { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public string HomeStreet { get; set; }

        public string HomeAddress { get; set; }

        public string HomeZip { get; set; }

        public string OperationNo { get; set; }

        public DateTime GettingOperationDate { get; set; }

        public string DrivingLicense { get; set; }

        public DateTime GettingLicenseDate { get; set; }

        public DateTime LicenseExpireDate { get; set; }

        public string LicenseNative { get; set; }

        public string DringClass { get; set; }

        public int? CabID { get; set; }

        public int Star { get; set; }

        public string BankAccount { get; set; }

        public int OriginalId { get; set; }

        public string HireDate { get; set; }

        public DateTime DimissionDate { get; set; }

        public int Status { get; set; }

        public Decimal RiskAmount { get; set; }

        public int OrganizationID { get; set; }

        public int MotorcadeID { get; set; }

        public int AboardKind { get; set; }

        public int TrueStar { get; set; }

        public string Remark { get; set; }

        public int ABType { get; set; }

        public string OwnerName { get; set; }

    }
}
