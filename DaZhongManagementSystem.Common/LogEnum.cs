using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Common
{
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogEnum
    {

        /// <summary>
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// 增加操作
        /// </summary>
        新增 = 1,

        /// <summary>
        /// 删除操作
        /// </summary>
        删除 = 2,

        /// <summary>
        /// 查询操作
        /// </summary>
        查询 = 3,

        /// <summary>
        /// 编辑操作
        /// </summary>
        编辑 = 4,

        /// <summary>
        /// 错误异常
        /// </summary>
        错误异常 = 5,

        /// <summary>
        /// 注册
        /// </summary>
        注册 = 6,

        /// <summary>
        /// 导入
        /// </summary>
        导入 = 7,

        /// <summary>
        /// 提交
        /// </summary>
        提交 = 8,

        /// <summary>
        /// 审核
        /// </summary>
        审核 = 9,

        /// <summary>
        /// 推送
        /// </summary>
        推送 = 10,

        /// <summary>
        /// 评分
        /// </summary>
        评分 = 11,

        /// <summary>
        /// 数据
        /// </summary>
        数据 = 12,

        /// <summary>
        /// 导出
        /// </summary>
        导出 = 13,

        /// <summary>
        /// 登录
        /// </summary>
        登录 = 14,

        /// <summary>
        /// 启用
        /// </summary>
        启用 = 15,

        /// <summary>
        /// 禁用
        /// </summary>
        禁用 = 16,

        /// <summary>
        /// 回退
        /// </summary>
        回退=17,
        /// <summary>
        /// 支付
        /// </summary>
        支付=18,
    }
}
