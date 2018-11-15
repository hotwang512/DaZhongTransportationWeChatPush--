using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_QuestionReport
    {
        public List<U_QuestionMainCollect> questionMain { get; set; }

        public List<U_QuestionDetailCollect> questionDetail { get; set; }
    }
    public class U_QuestionMainCollect
    {
        public Guid Vguid { get; set; }

        public string QuestionName { get; set; }

        public int CountPersion { get; set; }

        public int HighScore { get; set; }

        public int LowScore { get; set; }

        public int AvgScore { get; set; }
    }
    public class U_QuestionDetailCollect
    {
        public Guid Vguid { get; set; }

        public string QuestionName { get; set; }

        public Guid BusinessQuestionDetailVguid { get; set; }

        public string QuestionNameDetail { get; set; }

        public int True { get; set; }

        public int False { get; set; }

        public Decimal Correct { get; set; }
    }
}
