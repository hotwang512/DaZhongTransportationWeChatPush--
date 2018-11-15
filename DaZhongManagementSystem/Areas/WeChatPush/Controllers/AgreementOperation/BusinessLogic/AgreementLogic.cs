using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DraftManagement;

namespace DaZhongManagementSystem.Areas.WeChatPush.Controllers.AgreementOperation.BusinessLogic
{
    public class AgreementLogic
    {
        private readonly AgreementServer _agreementServer;
        public AgreementLogic()
        {
            _agreementServer = new AgreementServer();
        }

        /// <summary>
        /// 获取协议操作历史记录
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_ProtocolOperations_Information> GetAgreementOpertaionList(Search_AgreementOperation searchParam, GridParams para)
        {
            return _agreementServer.GetAgreementOpertaionList(searchParam, para);
        }

        /// <summary>
        /// 获取协议的详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public v_Business_ProtocolOperations_Information GetAgreementDetailByVguid(Guid vguid)
        {
            return _agreementServer.GetAgreementDetailByVguid(vguid);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="para"></param>
        public void Export(Search_AgreementOperation para)
        {
            _agreementServer.Export(para);
        }

        /// <summary>
        /// 获取协议类型
        /// </summary>
        /// <returns></returns>
        public List<Business_ProtocolOperations_Information> GetAgreementTypeList()
        {
            return _agreementServer.GetAgreementTypeList();
        }
    }
}