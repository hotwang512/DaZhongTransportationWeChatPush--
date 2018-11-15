using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.KnowledgeBaseManagement;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement.Controllers.Draft.BusinessLogic
{
    public class DraftLogic
    {
        private DraftServer _draftServer;

        public DraftLogic()
        {
            _draftServer = new DraftServer();
        }

        /// <summary>
        /// 获取草稿知识库的列表信息
        /// </summary>
        /// <param name="searchParam">搜索条件</param>
        /// <param name="para">分页信息</param>
        /// <returns></returns>
        public JsonResultModel<V_Business_KnowledgeBase_Information> GetKnowledgeListBySearch(Business_KnowledgeBase_Information searchParam, GridParams para)
        {
            return _draftServer.GetKnowledgeListBySearch(searchParam, para);
        }

        /// <summary>
        /// 批量提交草稿知识
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool SubmitKnowledgeBase(Guid[] vguidList)
        {
            bool result = false;
            foreach (var vguid in vguidList)
            {
                result = _draftServer.SubmitKnowledgeBase(vguid);
            }
            return result;
        }

        /// <summary>
        /// 批量删除草稿知识
        /// </summary>
        /// <param name="vguidList">主键</param>
        /// <returns></returns>
        public bool DeleteKnowledgeBase(Guid[] vguidList)
        {
            bool result = false;
            foreach (var vguid in vguidList)
            {
                result = _draftServer.DeleteKnowledgeBase(vguid);
            }
            return result;
        }

        /// <summary>
        /// 下载习题模板
        /// </summary>
        public void DownLoadTemplate()
        {
            string exerciseFileName = SyntacticSugar.ConfigSugar.GetAppString("KnowledgeBaseFileName");
            UploadHelper.ExportExcels("KnowledgeBaseTemplate .xlsx", exerciseFileName);
        }
        /// <summary>
        /// 将上传的excel插入数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt, ref string msg)
        {
            return _draftServer.InsertExcelToDatabase(dt, ref msg);
        }

        /// <summary>
        /// 判断是否存在相同的知识
        /// </summary>
        /// <param name="knowledgeBaseInfo">知识信息实体</param>
        /// <param name="isEdit">是否编辑</param>
        /// <returns></returns>
        public bool IsExistKnowledge(Business_KnowledgeBase_Information knowledgeBaseInfo, bool isEdit)
        {
            return _draftServer.IsExistKnowledge(knowledgeBaseInfo, isEdit);
        }

        /// <summary>
        /// 保存新增或编辑的知识
        /// </summary>
        /// <param name="knowledgeBaseInfo">知识信息实体</param>
        /// <param name="isEdit">是否编辑</param>
        /// <param name="saveType">保存类型 保存(1)还是提交(2)</param>
        /// <returns></returns>
        public bool SaveKnowledge(Business_KnowledgeBase_Information knowledgeBaseInfo, bool isEdit, string saveType)
        {
            return _draftServer.SaveKnowledge(knowledgeBaseInfo, isEdit, saveType);
        }

        /// <summary>
        /// 通过vguid获取知识的详细信息实体
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <returns></returns>
        public Business_KnowledgeBase_Information GetKnowledgeBaseInfoByVguid(Guid vguid)
        {
            return _draftServer.GetKnowledgeBaseInfoByVguid(vguid);
        }
    }
}