using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_ExerciseSsetsRate
    {
        public Guid Vguid { get; set; }

        public decimal Exercisesperson { get; set; }

        public decimal Proficiency { get; set; }

        public decimal Passrate { get; set; }

        public decimal Dontpass { get; set; }

        public decimal ZoreRate { get; set; }

        public decimal OneRate { get; set; }

        public decimal TwoRate { get; set; }

        public decimal ThreeRate { get; set; }

        public int numberofanswer { get; set; }

        public int PushUser { get; set; }

        public int ProficiencyUser { get; set; }

        public int PassrateUser { get; set; }

        public int DontpassUser { get; set; }

        public int ZoreUser { get; set; }

        public int OneUser { get; set; }

        public int TwoUser { get; set; }

        public int ThreeUser { get; set; }
    }
}
