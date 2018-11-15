using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity.RevenueAPIModel
{
    /// <summary>
    /// 营收API结果实体
    /// </summary>
    public class API_Result
    {
        private bool _success = false;
        /// <summary>
        /// 接口调用成功与否
        /// </summary>
        public bool success
        {
            get { return _success; }

            set { _success = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 接口返回结果
        /// </summary>
        public API_PaymentMonthly data { get; set; }
    }
}
