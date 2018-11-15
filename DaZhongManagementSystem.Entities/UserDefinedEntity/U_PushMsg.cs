using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_PushMsg
    {
        public int PushType { get; set; }

        public string Title { get; set; }

        public int MessageType { get; set; }

        public Boolean Timed { get; set; }

        public DateTime? TimedSendTime { get; set; }

        public Boolean Important { get; set; }

        public string Message { get; set; }

        public string CoverImg { get; set; }

        public string CoverDescption { get; set; }

        public DateTime? PeriodOfValidity { get; set; }

        public DateTime? PushDate { get; set; }

        public string PushPeople { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        /// <summary>
        /// 推送习题Vguid
        /// </summary>
        public Guid ExercisesVGUID { get; set; }

        /// <summary>
        /// 推送知识库Vguid
        /// </summary>
        public Guid KnowledgeVGUID { get; set; }
        /// <summary>
        /// 推送问卷Vguid
        /// </summary>
        public Guid QuestionVGUID { get; set; }

        public Guid VGUID { get; set; }

        /// <summary>
        /// 微信推送详细信息
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 微信推送详细信息
        /// 推送对象
        /// </summary>
        public string PushObject { get; set; }

        /// <summary>
        /// 推送历史
        /// </summary>
        public string History { get; set; }   
        /// <summary>
        /// 营收类型
        /// </summary>
        public int RevenueType { get; set; }  
       /// <summary>
        ///人员标签 
       /// </summary>
        public string Label { get; set; }  

        /// <summary>
        /// 协议类型
        /// </summary>
        public int? CountersignType { get; set; }

        /// <summary>
        /// 红包类型
        /// </summary>
        public int? RedpacketType { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal? RedpacketMoney { get; set; }

        /// <summary>
        /// 红包区间从
        /// </summary>
        public decimal? RedpacketMoneyFrom { get; set; }

        /// <summary>
        /// 红包区间到
        /// </summary>
        public decimal? RedpacketMoneyTo { get; set; }


    }
}
