using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class PsychologicalEvaluationModel
    {
        public Guid? Vguid { get; set; }
        public string Name { get; set; }

        public string ChangeDate { get; set; }

        public decimal? ptScore { get; set; } = 0;

        public decimal? phqScore { get; set; } = 0;

        public string ColorBlock { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;
    }
}
