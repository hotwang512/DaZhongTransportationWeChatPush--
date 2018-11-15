using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.KnowledgeBaseManagement;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Formal.BusinessLogic
{
    public class FormalLogic
    {
        private FormalServer _formalServer;
        public FormalLogic()
        {
            _formalServer = new FormalServer();
        }
        /// <summary>
        /// 获取正式知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<V_Business_KnowledgeBase_Information> GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            return _formalServer.GetKnowledgeListBySearch(searchParam, para);
        }
    }
}