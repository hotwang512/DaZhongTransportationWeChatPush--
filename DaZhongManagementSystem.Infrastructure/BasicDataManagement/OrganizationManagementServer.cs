using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace DaZhongManagementSystem.Infrastructure.BasicDataManagement
{
    public class OrganizationManagementServer
    {
        private readonly LogLogic _ll;

        public OrganizationManagementServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 获取组织结构树形结构数据
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganizationModel()
        {
            using (SqlSugarClient dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var organizationModel = dbMsSql.Queryable<Master_Organization>().ToList();
                return organizationModel;
            }
        }

        /// <summary>
        /// 通过vguid获取部门详细信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Master_Organization GetOrganizationDetail(string vguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Guid vGuid = Guid.Parse(vguid);
                var organisationDetail = dbMsSql.Queryable<Master_Organization>().Where(i => i.Vguid == vGuid).SingleOrDefault();
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(organisationDetail);
                _ll.SaveLog(3, 3, CurrentUser.GetCurrentUser().LoginName, organisationDetail.OrganizationName, logData);

                return organisationDetail;
            }
        }

        /// <summary>
        /// 判断同一部门下是否存在同名称部门
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <returns></returns>
        public bool CheckIsExist(Master_Organization organizationModel, string isEdit)
        {
            using (SqlSugarClient dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                bool isEditor = isEdit != "0";
                if (isEditor)//编辑
                {
                    result = dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == organizationModel.ParentVguid && i.Vguid != organizationModel.Vguid).Any(i => i.OrganizationName == organizationModel.OrganizationName);
                }
                else//新增
                {
                    result = dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == organizationModel.ParentVguid).Any(i => i.OrganizationName == organizationModel.OrganizationName);
                }
                return result;
            }
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public bool Save(Master_Organization organizationModel, bool isEdit)
        {
            using (SqlSugarClient dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                if (isEdit)//编辑
                {
                    var model = new
                    {
                        OrganizationName = organizationModel.OrganizationName,
                        Description = organizationModel.Description,
                        ChangeDate = organizationModel.ChangeDate,
                        ChangeUser = organizationModel.ChangeUser
                    };
                    result = dbMsSql.Update<Master_Organization>(model, i => i.Vguid == organizationModel.Vguid);

                    //存入操作日志表
                    string logData = JsonHelper.ModelToJson(organizationModel);
                    _ll.SaveLog(4, 3, CurrentUser.GetCurrentUser().LoginName, organizationModel.OrganizationName, logData);
                }
                else//新增
                {
                    result = dbMsSql.Insert(organizationModel, false) != DBNull.Value;
                    //存入操作日志表
                    string logData = JsonHelper.ModelToJson(organizationModel);
                    _ll.SaveLog(1, 2, CurrentUser.GetCurrentUser().LoginName, organizationModel.OrganizationName, logData);
                }
                return result;
            }
        }

    }
}
