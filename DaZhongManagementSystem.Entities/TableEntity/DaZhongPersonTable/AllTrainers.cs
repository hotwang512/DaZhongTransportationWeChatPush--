using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.DaZhongPersonTable
{
    public class AllTrainers
    {
        public int? ID { get; set; }

        public string IDCard { get; set; }

        public string Name { get; set; }

        public int? Gender { get; set; }

        public string BirthDay { get; set; }

        public string OrganizationID { get; set; }

        public string OrganizationCode { get; set; }

        public string OrganizationName { get; set; }

        public string EmployeeNO { get; set; }

        public string HomeAddress { get; set; }

        public string HomePhone { get; set; }

        public string HomeZip { get; set; }

        public string HomeStreet { get; set; }

        public string MobilePhone { get; set; }

        public string DrivingLicense { get; set; }

        public DateTime? GettingLicenseDate { get; set; }

        public string HireDate { get; set; }
    }
}
