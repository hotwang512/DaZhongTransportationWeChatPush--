using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.PushHistory;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushHistory.BusinessLogic
{
    public class PushHistoryLogic
    {
        private PushHistoryServer _pushHistoryServer;
        public PushHistoryLogic()
        {
            _pushHistoryServer = new PushHistoryServer();
        }
        /// <summary>
        /// 通过查询条件获取推送信息列表（已推送）
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            return _pushHistoryServer.GetWeChatPushListBySearch(searchParam, para);
        }

        /// <summary>
        /// 通过vguid获取推送主表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatMainByVguid(string vguid)
        {
            return _pushHistoryServer.GetWeChatMainByVguid(vguid);
        }

        /// <summary>
        /// 手机端获取消息历史的详细信息
        /// </summary>
        /// <param name="vguid">消息历史主键</param>
        ///  <param name="personVguid">用户主键</param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatDetail(string vguid, Guid personVguid)
        {
            return _pushHistoryServer.GetWeChatDetail(vguid, personVguid);
        }

        /// <summary>
        /// 批量删除推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool DeletePushHistory(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _pushHistoryServer.DeletePushHistory(item);
            }
            return result;
        }
        /// <summary>
        /// 手机端分页获取消息历史记录
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        ///  <param name="personVguid">当前浏览人的vguid</param>
        /// <returns></returns>
        public List<TempWeChatMain> GetWeChatPushList(int pageIndex, Guid personVguid)
        {
            return _pushHistoryServer.GetWeChatPushList(pageIndex, personVguid);
        }
    }
}