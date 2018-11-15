namespace DaZhongManagementSystem.Common
{
    public enum RedPacketStatus
    {
        /// <summary>
        /// 发放
        /// </summary>
        SENDING=1,

        /// <summary>
        /// 已发放待领取
        /// </summary>
        SENT=2,

        /// <summary>
        /// 发放失败 
        /// </summary>
        FAILED=3,


        /// <summary>
        /// 已领取 
        /// </summary>
        RECEIVED=4,


        /// <summary>
        /// 退款中 
        /// </summary>
        RFUND_ING=5,


        /// <summary>
        /// 已退款 
        /// </summary>
        REFUND=6

    }
}