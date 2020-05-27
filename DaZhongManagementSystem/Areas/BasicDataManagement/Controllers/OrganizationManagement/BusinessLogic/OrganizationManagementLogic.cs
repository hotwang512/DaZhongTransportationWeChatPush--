using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.BasicDataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.OrganizationManagement.OrganizationManageLogic
{
    public class OrganizationManagementLogic
    {
        public OrganizationManagementServer _os;
        public OrganizationManagementLogic()
        {
            _os = new OrganizationManagementServer();
        }

        /// <summary>
        /// 获取组织结构树形结构数据
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganizationModel()
        {
            return _os.GetOrganizationModel();
        }

        public List<Master_Organization> GetUserOrganizationModel()
        {
            return _os.GetUserOrganizationModel();
        }
        /// <summary>
        /// 通过vguid获取部门详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Master_Organization GetOrganizationDetail(string vguid)
        {
            return _os.GetOrganizationDetail(vguid);
        }

        /// <summary>
        /// 判断同一部门下是否存在同名称部门
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <returns></returns>
        public bool CheckIsExist(Master_Organization organizationModel, string isEdit)
        {
            return _os.CheckIsExist(organizationModel, isEdit);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool Save(Master_Organization organizationModel, bool isEdit)
        {
            Master_Organization organization = new Master_Organization();
            if (isEdit)//编辑
            {
                organization = GetOrganizationDetail(organizationModel.Vguid.ToString());
                organization.OrganizationName = organizationModel.OrganizationName;
                //organization.OrganizationCode = organizationModel.OrganizationCode;
                organization.Description = organizationModel.Description;
                organization.ChangeUser = Common.CurrentUser.GetCurrentUser().LoginName;
                organization.ChangeDate = DateTime.Now;
            }
            else//新增
            {
                //organization = new Master_Organization();
                organization.OrganizationName = organizationModel.OrganizationName;
                //organization.OrganizationCode = organizationModel.OrganizationCode;
                organization.ParentVguid = organizationModel.ParentVguid;
                organization.Vguid = Guid.NewGuid();
                organization.Description = organizationModel.Description;
                organization.CreatedUser = Common.CurrentUser.GetCurrentUser().LoginName;
                organization.CreatedDate = DateTime.Now;
                organization.ChangeDate = DateTime.Now;
            }

            return _os.Save(organization, isEdit);
        }

    }
}