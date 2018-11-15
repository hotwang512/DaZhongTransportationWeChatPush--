using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DaZhongPersonTable;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.WeChatPush.WeChatValidationBusiness
{
    public class WeChatValidationLogic
    {
        public WeChatValidationServer _vl;
        public WeChatValidationLogic()
        {
            _vl = new WeChatValidationServer();
        }

        /// <summary>
        /// 审核用户是否存在并保存至Person表
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string CheckUser(AllEmployee userModel, string userID, string position, string mobilePhone)
        {
            return _vl.CheckUser(userModel, userID, position, mobilePhone);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganization()
        {
            return _vl.GetOrganization();
        }

        public bool UpdateStatus(string idCard)
        {
            return _vl.UpdateStatus(idCard);
        }
    }
}