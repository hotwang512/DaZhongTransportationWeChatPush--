using System.Collections.Generic;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DraftManagement;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.RedPacketOperation.Business
{
    public class RedPacketLogic
    {

        private readonly RedPacketServer _redPacketServer;
        public RedPacketLogic()
        {
            _redPacketServer = new RedPacketServer();
        }

        /// <summary>
        /// 根据搜索条件获取红包领取记录历史
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_Redpacket_Push_Information> GetRedPacketHistoryList(Search_RedPacketHistory searchParas, GridParams para)
        {
            return _redPacketServer.GetRedPacketHistoryList(searchParas, para);
        }

        /// <summary>
        /// 根据搜索条件获取企业付款历史
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        /// <param name="para">页码信息</param>
        /// <returns></returns>
        public JsonResultModel<v_Business_Redpacket_Push_Information> GetPaymentInfos(Search_RedPacketHistory searchParas, GridParams para)
        {
            return _redPacketServer.GetPaymentInfos(searchParas, para);
        }

        /// <summary>
        /// 获取红包状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetRedPacketStatus()
        {
            return _redPacketServer.GetRedPacketStatus();
        }
    }
}