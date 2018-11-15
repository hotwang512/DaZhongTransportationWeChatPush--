using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DraftManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.CommitedList.BusinesLogic
{
    public class CommitedListLogic
    {
        public CommitedServer _cs;
        public CommitedListLogic()
        {
            _cs = new CommitedServer();
        }

        /// <summary>
        /// 绑定推送类别数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetPushTypeList()
        {
            return _cs.GetPushTypeList();
        }

        /// <summary>
        /// 绑定微信推送类型数据
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetWeChatPushType()
        {
            return _cs.GetWeChatPushType();
        }

        /// <summary>
        /// 获取习题列表
        /// </summary>
        /// <returns></returns>
        public List<Business_Exercises_Infomation> GetExerciseList()
        {
            return _cs.GetExerciseList();
        }

        /// <summary>
        /// 通过vguid获取推送主表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_WeChatPush_Information GetWeChatMainByVguid(string vguid)
        {
            return _cs.GetWeChatMainByVguid(vguid);
        }

        /// <summary>
        /// 批量提交推送信息
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool CheckSubmitList(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _cs.CheckSubmitList(item);
            }
            return result;
        }        

        /// <summary>
        /// 通过查询条件获取推送信息列表（已审核）
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<V_Business_WeChatPushMain_Information> GetWeChatPushListBySearch(SearchWeChatPushList searchParam, GridParams para)
        {
            return _cs.GetWeChatPushListBySearch(searchParam, para);
        }
    }
}