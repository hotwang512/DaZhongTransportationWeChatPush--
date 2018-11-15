using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.RevenueAPIModel
{
    /// <summary>
    /// 获取营收数据的接口数据实体
    /// </summary>
    public class API_PaymentMonthly
    {
        /// <summary>
        /// 当前账期
        /// </summary>
        public string AccountPeriod { get; set; }
        /// <summary>
        /// 驾驶员工号,
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 驾驶员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 准营证号,
        /// </summary>
        public string OperationNo { get; set; }
        /// <summary>
        /// 上期结余(小于零：欠款;大于等于零：结余),
        /// </summary>
        public decimal DebtAmount { get; set; }
        /// <summary>
        /// 本期计划
        /// </summary>
        public decimal DueAmount { get; set; }
        /// <summary>
        /// 本期已缴,
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 本期欠款
        /// </summary>
        public decimal PayDebtAmount { get; set; }
        /// <summary>
        /// 手续费费率
        /// </summary>
        public string fee { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal feeMoney { get; set; }
        /// <summary>
        /// 合计缴费金额
        /// </summary>
        public decimal totalAmount { get; set; }
    }
}
