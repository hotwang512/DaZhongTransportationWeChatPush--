using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_SurveyVaccination
    {
        public Guid VGUID { get; set; }
        public string IsInoculation { get; set; }
        public DateTime? FirstDate { get; set; }
        public string FirstAddress { get; set; }
        public DateTime? SecondDate { get; set; }
        public string SecondAddress { get; set; }
        public string Attachment { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
