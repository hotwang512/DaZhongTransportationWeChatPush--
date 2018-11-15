using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DraftManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.PushedList.BusinessLogic
{
    public class PushedListLogic
    {
        public PushedListServer _ps;
        public PushedListLogic()
        { 
            _ps = new PushedListServer();
        }

        /// <summary>
        /// 绑定推送类别数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetPushTypeList()
        {
            return _ps.GetPushTypeList();
        }

        /// <summary>
        /// 绑定微信推送类型数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetWeChatPushType()
        {
            return _ps.GetWeChatPushType();
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetExerciseList()
        {
            return _ps.GetExerciseList();
        }

        /// <summary>
        /// 通过vguid获取推送主表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatMainByVguid(string vguid)
        {
            return _ps.GetWeChatMainByVguid(vguid);
        }

        /// <summary>
        /// 通过查询条件获取推送信息列表（已推送）
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            return _ps.GetWeChatPushListBySearch(searchParam, para);
        }
    }
}