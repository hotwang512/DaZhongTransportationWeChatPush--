using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Infrastructure.BasicDataManagement;
using DaZhongManagementSystem.Common.Tools;
using System.Data;
using DaZhongManagementSystem.Common;

namespace DaZhongManagementSystem.Areas.BasicDataManagement.Controllers.UserInfo.BussinessLogic
{
    public class UserInfoLogic
    {
        public UserInfoServer _us;
        public UserInfoLogic()
        {
            _us = new UserInfoServer();
        }

        /// <summary>
        /// 获取用户详细信息（用于更改用户所在部门）
        /// </summary>
        /// <param name="personVguid"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserDepartment(string personVguid)
        {
            return _us.GetUserDepartment(personVguid);
        }

        /// <summary>
        /// 部门树形结构
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganizationTreeList()
        {
            return _us.GetOrganizationTreeList();
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetDepartmentList()
        {
            return _us.GetDepartmentList();
        }

        /// <summary>
        /// 分页查询用户列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_PersonnelDepartment_Information> GetUserPageList(Business_PersonDepartmrnt_Search searchParam, GridParams para)
        {
            return _us.GetUserPageList(searchParam, para);
        }

        /// <summary>
        /// 更新用户部门
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool UpdateDepartment(string vguid, string personVguid, string labelStr)
        {

            return _us.UpdateDepartment(vguid, personVguid, labelStr);
        }

        /// <summary>
        /// 已离职用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _us.DeleteUserInfo(item);
            }
            return result;
        }



        /// <summary>
        /// 批量手动关注企业号
        /// </summary>
        /// <param name="vguidList"></param>
        /// <returns></returns>
        public bool UserFocusWeChat(string[] vguidList)
        {
            bool result = false;
            foreach (var item in vguidList)
            {
                result = _us.UserFocusWeChat(item);
            }
            return result;
        }
        /// <summary>
        /// 查询人员状态清单
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> ApprovalStatusSelect()
        {
            return _us.ApprovalStatusSelect();
        }

        /// <summary>
        /// 获取用户的标签信息
        /// </summary>
        /// <param name="personVguid">用户vguid</param>
        /// <returns></returns>
        public IEnumerable<string> GetPersonLabel(Guid personVguid)
        {
            return _us.GetPersonLabel(personVguid);
        }

        /// <summary>
        /// 下载导入标签模板
        /// </summary>
        public void DownLoadTemplate()
        {
            UploadHelper.ExportExcel("PushLabelTemplate.xls", "PushLabelTemplate.xls");
        }


        /// <summary>
        /// 上传的文件插入数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            try
            {
                return _us.InsertExcelToDatabase(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable getCorrectDt(DataTable dt)
        {
            try
            {
                return _us.getCorrectDt(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public DataTable getErrorDt(DataTable dt)
        {
            try
            {
                return _us.getErrorDt(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public string getNulldata(DataTable dt)
        {
            try
            {
                return _us.getNulldata(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

      

        public void Existup(DataTable ErrorDt)
        {
            try
            {
                ErrorDt.TableName = "table";
                //string amountFileName = SyntacticSugar.ConfigSugar.GetAppString("Errortabel");
                ExportExcel.ExportExcels("Errortabel.xlsx", "Errortabel.xls", ErrorDt);
                //Common.ExportExcel.ExportExcels("Errortabel.xlsx", amountFileName, ErrorDt);
            }
            catch (Exception)
            {
                
                throw;
            }
         
        }
    }

}