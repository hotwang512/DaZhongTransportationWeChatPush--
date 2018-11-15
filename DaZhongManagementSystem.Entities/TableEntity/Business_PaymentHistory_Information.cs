using System;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_PaymentHistory_Information
    {

        /// <summary>
        /// Desc:付款人VGUID
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Guid? PaymentPersonnel { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// Desc:应付款金额 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// 实际付款
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>
        /// 公司到账
        /// </summary>
        public decimal CompanyAccount { get; set; }
        /// <summary>
        /// 营收系统营收金额
        /// </summary>
        public decimal RevenueReceivable { get; set; }
        /// <summary>
        /// Desc:付款类型
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// Desc:付款中间商（支付宝，微信等）
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PaymentBrokers { get; set; }

        /// <summary>
        /// Desc:收款方
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Beneficiary { get; set; }

        /// <summary>
        /// Desc:收款账户
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ReceiptAccount { get; set; }

        /// <summary>
        /// Desc:支付描述
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Desc:支付备注
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Desc:支付时间
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Desc：支付状态
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Desc:错误编码
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Desc:错误原因
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? ChangeDate { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ChangeUser { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Guid VGUID { get; set; }

        /// <summary>
        /// 营收类型
        /// </summary>
        public int RevenueType { get; set; }
        /// <summary>
        /// 推送vguid
        /// </summary>
        public Guid WeChatPush_VGUID { get; set; }

        /// <summary>
        /// 营收状态
        /// </summary>
        public int RevenueStatus { get; set; }
    }
}
