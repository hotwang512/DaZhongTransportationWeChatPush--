using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.KnowledgeBaseManagement;
using System;
using System.Collections.Generic;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.WeChatKnowledge.BusinessLogic
{
    public class WeChatKnowledgeLogic
    {
        private FormalServer _formalServer;
        public WeChatKnowledgeLogic()
        {
            _formalServer = new FormalServer();
        }
        /// <summary>
        /// 获取正式知识库的列表信息
        /// </summary> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="personVguid">浏览人主键</param>
        /// <returns></returns>
        public List<V_Business_KnowledgeBase_Information> GetKnowledgeList(int pageIndex, Guid personVguid, string type = "1")
        {
            return _formalServer.GetKnowledgeList(pageIndex, personVguid, type);
        }

        /// <summary>
        /// 获取知识库的详细信息
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <param name="personVguid">人员主键</param>
        /// <returns></returns>
        public V_Business_KnowledgeBase_Information GetKnowledgeDetail(string vguid, Guid personVguid)
        {
            return _formalServer.GetKnowledgeDetail(vguid, personVguid);
        }
    }
}
