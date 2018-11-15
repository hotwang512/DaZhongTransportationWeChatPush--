using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.UserDefinedEntity
{
    public class U_Revenue
    {
        /// <summary>
        /// 历史欠款
        /// </summary>
        public string HistoricalArrears { get; set; }
        /// <summary>
        /// 上期结余
        /// </summary>
        public string TheBalance { get; set; }
        /// <summary>
        /// 应缴金额
        /// </summary>
        public string AmountDue { get; set; }
        /// <summary>
        /// 本期缴款
        /// </summary>
        public string CurrentPayment { get; set; }
        /// <summary>
        /// 本期账户结余
        /// </summary>
        public string CurrentAccountBalance { get; set; }
        /// <summary>
        /// 手续费费率
        /// </summary>
        public string Fee { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public string FeeMoney { get; set; }

        /// <summary>
        /// 合计缴费金额
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// 费用-本期账户结余，只能是正数
        /// </summary>
        public string Fee_CurrentAccountBalance { get; set; }

        /// <summary>
        /// 费用-合计缴费金额，只能是正数
        /// </summary>
        public string Fee_TotalAmount { get; set; }
    }
}
