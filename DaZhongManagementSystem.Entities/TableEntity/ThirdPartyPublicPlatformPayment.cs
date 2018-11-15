using System;


namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class ThirdPartyPublicPlatformPayment
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Id {get;set;}

        /// <summary>
        /// Desc:流水号
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SerialNumber {get;set;}

        /// <summary>
        /// Desc:支付日期
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? PaymentDate {get;set;}

        /// <summary>
        /// Desc:姓名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DriverName {get;set;}

        /// <summary>
        /// Desc:手机号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DriverPhoneNum {get;set;}

        /// <summary>
        /// Desc:服务卡
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DriverServiceNum {get;set;}

        /// <summary>
        /// Desc:车号
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string CarNo {get;set;}

        /// <summary>
        /// Desc:公司代码
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string UnitCode {get;set;}

        /// <summary>
        /// Desc:金额
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public decimal? PaymentAmount {get;set;}

        /// <summary>
        /// Desc:来源
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PaymentSource {get;set;}

        /// <summary>
        /// Desc:类型
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PaymentType {get;set;}

        /// <summary>
        /// Desc:对账日期
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? CheckOutDate {get;set;}

        /// <summary>
        /// Desc:标识 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int Identifier {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? ReconciliationDate {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ReservedCharacter {get;set;}

    }
}
