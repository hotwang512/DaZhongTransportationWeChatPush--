using System;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class TempWeChatMain
    {
        public string Title { get; set; }

        public DateTime PushDate { get; set; }

        public int MessageType { get; set; }

        public Guid ExercisesVGUID { get; set; }

        public Guid VGUID { get; set; }

        public bool IsShow { get; set; }

        public int ShowTYPE { get; set; }
    }
}