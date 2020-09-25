using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using DaZhongManagementSystem.Entities.TableEntity.DaZhongPersonTable;

namespace DaZhongManagementSystem.Infrastructure.BasicDataManagement
{
    public class UserInfoServer
    {
        private readonly LogLogic _ll;
        public UserInfoServer()
        {
            _ll = new LogLogic();
        }

        /// <summary>
        /// 获取用户详细信息（用于更改用户所在部门）
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserDepartment(string vguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid personVguid = Guid.Parse(vguid);
                var personnelModel = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == personVguid).SingleOrDefault();
                return personnelModel;
            }
        }

        public Business_Personnel_Information GetPerson(string idNumber)
        {
            Business_Personnel_Information user = null;
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                user = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.IDNumber == idNumber).SingleOrDefault();
            }
            return user;
        }
        public Business_Personnel_Information GetPersonByPhoneNumber(string phoneNumber)
        {
            Business_Personnel_Information user = null;
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                user = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.PhoneNumber == phoneNumber && i.UserID != "" && i.UserID != null).SingleOrDefault();
            }
            return user;
        }

        public void UpdatePhoneNumber(string userid, string phoneNumber)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                dbMsSql.Update<Business_Personnel_Information>(new { PhoneNumber = phoneNumber, ApprovalStatus = 4 }, i => i.UserID == userid);
            }
        }
        /// <summary>
        /// 部门树形结构
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganizationTreeList()
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var currentDepartment = CurrentUser.GetCurrentUser().Department;
                var mainDepVguid = dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault();
                var organizationList = dbMsSql.SqlQuery<Master_Organization>("exec usp_OrganizationDetail @orgVguid", new
                {
                    orgVguid = currentDepartment ?? mainDepVguid.ToString()

                });
                return organizationList;
            }
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetDepartmentList()
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var departmentList = dbMsSql.Queryable<Master_Organization>().ToList();
                return departmentList;
            }
        }

        /// <summary>
        /// 分页查询人员列表信息
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_PersonnelDepartment_Information> GetUserPageList(Business_PersonDepartmrnt_Search searchParam, GridParams para)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var jsonResult = new JsonResultModel<v_Business_PersonnelDepartment_Information>();
                var currentUserInfo = CurrentUser.GetCurrentUser();
                string ownfleet = string.Empty;
                //是系统管理员
                if (currentUserInfo.LoginName.ToLower() == "sysadmin")
                {
                    ownfleet = searchParam.OwnedFleet != Guid.Empty ? searchParam.OwnedFleet.ToString() : dbMsSql.Queryable<Master_Organization>().Where(i => i.ParentVguid == null).Select(i => i.Vguid).SingleOrDefault().ToString();
                }
                else  //非系统管理员
                {
                    //查出当前登录人的部门
                    Guid dep = Guid.Parse(currentUserInfo.Department);
                    ownfleet = dep.ToString();
                    var listDep = dbMsSql.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                    if (searchParam.OwnedFleet != Guid.Empty)
                    {
                        ownfleet = !listDep.Contains(searchParam.OwnedFleet) ? dep.ToString() : searchParam.OwnedFleet.ToString();
                    }
                }
                var labelStr = string.Empty;
                if (!string.IsNullOrEmpty(searchParam.LabelName))
                {
                    var labelArr = searchParam.LabelName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < labelArr.Length; i++)
                    {
                        labelArr[i] = "'" + labelArr[i].Trim() + "'";
                    }
                    labelStr = string.Join(",", labelArr);
                }
                string sql = "exec usp_Business_PersonnelDepartment_Information @UserName,@JobNumber,@OwnedFleet,@ServiceNumber,@IDNumber,@Status,@Phone,@Start,@End,@Count output,@LabelName";
                var pars = SqlSugarTool.GetParameters(new
                {
                    UserName = searchParam.name ?? "",
                    JobNumber = searchParam.JobNumber ?? "",
                    OwnedFleet = ownfleet,
                    ServiceNumber = searchParam.ServiceNumber ?? "",
                    IDNumber = searchParam.IDNumber ?? "",
                    Status = searchParam.TranslationApprovalStatus ?? "",
                    Phone = searchParam.PhoneNumber ?? "",
                    Start = (para.pagenum - 1) * para.pagesize + 1,
                    End = para.pagenum * para.pagesize,
                    Count = 0,
                    LabelName = labelStr
                }); //将匿名对象转成SqlParameter
                dbMsSql.IsClearParameters = false;//禁止清除参数
                pars[9].Direction = ParameterDirection.Output; //将Count设为 output
                var query = dbMsSql.SqlQuery<v_Business_PersonnelDepartment_Information>(sql, pars);
                dbMsSql.IsClearParameters = true;//启动请动清除参数
                var outPutValue = pars[9].Value.ObjToInt();//获取output @Count的值
                jsonResult.TotalRows = outPutValue;
                jsonResult.Rows = query;
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _ll.SaveLog(3, 5, CurrentUser.GetCurrentUser().LoginName, searchParam.name + searchParam.JobNumber + searchParam.ServiceNumber + searchParam.UserID, logData);
                return jsonResult;
            }
        }

        /// <summary>
        /// 更新用户部门
        /// </summary>
        /// <param name="vguid"></param>
        /// <param name="personVguid"></param>
        /// <param name="labelStr"></param>
        /// <returns></returns>
        public bool UpdateDepartment(string vguid, string personVguid, string labelStr)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                Guid departmentVguid = Guid.Parse(vguid);
                Guid personVGUID = Guid.Parse(personVguid);
                bool result = false;
                var listLabel = JsonHelper.JsonToModel<List<Business_PersonnelLabel_Information>>(labelStr);
                if (listLabel.Count > 0)
                {
                    //先删除标签，再重新添加
                    dbMsSql.Delete<Business_PersonnelLabel_Information>(i => i.PersonnelVVGUID == personVGUID);
                    foreach (var item in listLabel)
                    {
                        item.VGUID = Guid.NewGuid();
                        item.CreatedDate = DateTime.Now;
                        item.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                        item.ChangeDate = DateTime.Now;
                        item.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        //var rtn = dbMsSql.Insert(item);
                    }

                    result = dbMsSql.SqlBulkCopy(listLabel);
                    //删除空标签
                    dbMsSql.Delete<Business_PersonnelLabel_Information>(i => i.PersonnelVVGUID == personVGUID && i.LabelName == "");
                }
                result = dbMsSql.Update<Business_Personnel_Information>(new { OwnedFleet = departmentVguid }, i => i.Vguid == personVGUID);
                return result;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string vguid)
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                Guid Vguid = Guid.Parse(vguid);
                Business_Personnel_Information userInfo = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                try
                {
                    dbMsSql.BeginTran();

                    string weChatJson = JsonHelper.ModelToJson(userInfo);

                    //从微信中删除
                    string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                    string focusResult = Common.WeChatPush.WeChatTools.DisableWeChatData(accessToken, userInfo.UserID);//禁用微信通讯录人员
                    U_FocusResult resultMsg = new U_FocusResult();
                    resultMsg = JsonHelper.JsonToModel<U_FocusResult>(focusResult);
                    result = dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = 4 }, i => i.Vguid == Vguid);
                    if (resultMsg.errcode == 0)//删除成功
                    {
                        result = true;
                    }
                    else
                    {
                        LogHelper.WriteLog("人员:" + userInfo.UserID + "离职失败：错误码为-" + resultMsg.errcode + ",错误消息为_" + resultMsg.errmsg);
                        result = false;
                    }

                    //存入操作日志表
                    _ll.SaveLog(2, 5, CurrentUser.GetCurrentUser().LoginName, userInfo.Name, weChatJson);
                    dbMsSql.CommitTran();
                    return result;
                }
                catch (Exception ex)
                {
                    dbMsSql.RollbackTran();
                    LogHelper.WriteLog("人员:" + userInfo.UserID + "离职异常：异常信息为_" + ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                }
                return result;
            }
        }

        /// <summary>
        /// 手动关注企业号
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public bool UserFocusWeChat(string vguid, out string outString)
        {
            bool result = false;
            outString = string.Empty;
            using (SqlSugarClient _dbSql = SugarDao.SugarDao_LandaVSql.GetInstance())
            {
                using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
                {
                    Guid Vguid = Guid.Parse(vguid);
                    Business_Personnel_Information userInfo = dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == Vguid).SingleOrDefault();
                    try
                    {
                        AllEmployee landaUser = _dbSql.Queryable<AllEmployee>().Where(i => i.IDCard == userInfo.IDNumber).FirstOrDefault();
                        if (landaUser != null)
                        {
                            dbMsSql.BeginTran();
                            string weChatJson = JsonHelper.ModelToJson(userInfo);
                            //1为未关注的人
                            if (userInfo.ApprovalStatus == 1)
                            {

                                //获取accessToken
                                string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                                //用户关注微信企业号
                                string focusResult = Common.WeChatPush.WeChatTools.GetAuthSucee(accessToken, userInfo.UserID);
                                U_FocusResult resultMsg = new U_FocusResult();
                                resultMsg = JsonHelper.JsonToModel<U_FocusResult>(focusResult);
                                if (resultMsg.errcode == 0)
                                {
                                    dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = 2 }, i => i.Vguid == Vguid);//更新审批状态
                                    result = true;//关注成功
                                }
                                else
                                {
                                    result = false;
                                    //model.respnseInfo = resultMsg.errmsg;
                                    LogHelper.WriteLog("人员" + userInfo.UserID + "手动关注失败：错误码为-" + resultMsg.errcode + ",错误消息为_" + resultMsg.errmsg);
                                }

                                //存入操作日志表
                                _ll.SaveLog(9, 5, CurrentUser.GetCurrentUser().LoginName, userInfo.Name, weChatJson);
                            }
                            else if (userInfo.ApprovalStatus == 4)
                            {
                                //获取accessToken
                                string accessToken = Common.WeChatPush.WeChatTools.GetAccessoken();
                                //用户关注微信企业号
                                string focusResult = Common.WeChatPush.WeChatTools.EnableWeChatData(accessToken, userInfo.UserID);
                                U_FocusResult resultMsg = new U_FocusResult();
                                resultMsg = JsonHelper.JsonToModel<U_FocusResult>(focusResult);
                                if (resultMsg.errcode == 0)
                                {
                                    dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = 2 }, i => i.Vguid == Vguid);//更新审批状态
                                    result = true;//关注成功
                                }
                                else
                                {
                                    result = false;
                                    outString = "人员" + userInfo.UserID + "手动关注失败：错误码为-" + resultMsg.errcode + ",错误消息为_" + resultMsg.errmsg;
                                    LogHelper.WriteLog(outString);
                                }
                                //存入操作日志表
                                _ll.SaveLog(9, 5, CurrentUser.GetCurrentUser().LoginName, userInfo.Name, weChatJson);

                            }
                            else
                            {
                                result = true;
                            }
                            dbMsSql.CommitTran();
                        }
                        else
                        {
                            outString = "人事库中不存在该用户，不能关注！";
                        }
                    }
                    catch (Exception ex)
                    {
                        dbMsSql.RollbackTran();
                        outString = "人员" + userInfo.UserID + "手动关注异常";
                        LogHelper.WriteLog("人员" + userInfo.UserID + "手动关注异常：" + ex.Message + "/n" + ex.ToString() + "/n" + ex.StackTrace);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 查询人员状态清单
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<CS_Master_2> ApprovalStatusSelect()
        {
            using (SqlSugarClient dbMsSql = SugarDao_MsSql.GetInstance())
            {
                var list = dbMsSql.Queryable<CS_Master_1>().Where(i => i.MasterID == "ID10017").ToList();

                var master2 = dbMsSql.Sqlable().From<CS_Master_2>("master2").Join<CS_Master_1>("master1", "master1.VGUID", "master2.VGUID", JoinType.Inner).Where("master1.VGUID=" + "'" + list[0].VGUID + "'").SelectToList<CS_Master_2>("master2.*");
                return master2;
            }
        }

        /// <summary>
        /// 获取用户的标签信息
        /// </summary>
        /// <param name="personVguid">用户vguid</param>
        /// <returns></returns>
        public IEnumerable<string> GetPersonLabel(Guid personVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_PersonnelLabel_Information>().GroupBy(i => i.LabelName).Where(i => i.PersonnelVVGUID == personVguid).Select(i => i.LabelName).ToList();
            }
        }

        /// <summary>
        /// 上传的文件插入数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertExcelToDatabase(DataTable dt)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var list = DatatableToList(dt);
                try
                {
                    db.BeginTran();
                    var logMessage = DataTableHelper.Dtb2Json(dt);
                    _ll.SaveLog(7, 5, CurrentUser.GetCurrentUser().LoginName, "人员标签", logMessage);
                    db.SqlBulkCopy(list);
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    throw;
                }
            }
        }
        /// <summary>
        /// datatable转成list
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Business_PersonnelLabel_Information> DatatableToList(DataTable dt)
        {
            var labelList = new List<Business_PersonnelLabel_Information>();
            using (var db = SugarDao_MsSql.GetInstance())
            {
                for (int i = 2; i < dt.Rows.Count; i++)
                {
                    var labStr = dt.Rows[i]["column4"].ToString();
                    if (string.IsNullOrEmpty(labStr.Trim()))
                        continue;
                    var labArr = labStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < labArr.Length; j++)
                    {
                        var phoneNo = dt.Rows[i]["column2"].ToString();
                        var idCardNo = dt.Rows[i]["column3"].ToString();
                        var PersonVguid = Guid.Empty;
                        //if (string.IsNullOrEmpty(phoneNo) && string.IsNullOrEmpty(idCardNo))
                        //{
                        //    throw new Exception("第" + (i + 1) + "行,手机号和身份证号不可同时为空！");
                        //}
                        if (!string.IsNullOrEmpty(phoneNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.PhoneNumber == phoneNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        else if (!string.IsNullOrEmpty(idCardNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.IDNumber == idCardNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        //if (PersonVguid == Guid.Empty)
                        //{
                        //    throw new Exception("第" + (i + 1) + "行,人员不存在！");
                        //}
                        var labelInfo = new Business_PersonnelLabel_Information();
                        labelInfo.PersonnelVVGUID = PersonVguid;
                        labelInfo.LabelName = labArr[j];
                        labelInfo.CreatedUser = CurrentUser.GetCurrentUser().LoginName;
                        labelInfo.CreatedDate = DateTime.Now;
                        labelInfo.ChangeUser = CurrentUser.GetCurrentUser().LoginName;
                        labelInfo.ChangeDate = DateTime.Now;
                        labelInfo.VGUID = Guid.NewGuid();
                        var isExist = db.Queryable<Business_PersonnelLabel_Information>().Any(it => it.PersonnelVVGUID == labelInfo.PersonnelVVGUID && it.LabelName == labelInfo.LabelName);
                        if (!isExist)
                            labelList.Add(labelInfo);
                    }
                }
            }
            return labelList;
        }



        /// <summary>
        /// 筛选正确表格
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable getCorrectDt(DataTable dt)
        {

            using (var db = SugarDao_MsSql.GetInstance())
            {
                DataTable CorrectDt = new DataTable();
                CorrectDt = dt.Copy();
                //for (int i = 2; i < CorrectDt.Rows.Count; i++)
                //{
                //    CorrectDt.Rows[i].Delete();
                //    i--;
                //}
                for (int i = 2; i < CorrectDt.Rows.Count; i++)
                {
                    var labStr = CorrectDt.Rows[i]["column4"].ToString();
                    if (string.IsNullOrEmpty(labStr.Trim()))
                        continue;
                    var labArr = labStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < labArr.Length; j++)
                    {
                        var phoneNo = CorrectDt.Rows[i]["column2"].ToString();
                        var idCardNo = CorrectDt.Rows[i]["column3"].ToString();
                        var PersonVguid = Guid.Empty;
                        if (string.IsNullOrEmpty(phoneNo) && string.IsNullOrEmpty(idCardNo))
                        {
                            CorrectDt.Rows[i].Delete();
                            i--;
                            break;
                            //throw new Exception("第" + (i + 1) + "行,手机号和身份证号不可同时为空！");
                        }
                        else if (!string.IsNullOrEmpty(phoneNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.PhoneNumber == phoneNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        else if (!string.IsNullOrEmpty(idCardNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.IDNumber == idCardNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        if (PersonVguid == Guid.Empty)
                        {
                            CorrectDt.Rows[i].Delete();
                            i--;
                            break;
                            //throw new Exception("第" + (i + 1) + "行,人员不存在！");
                        }

                    }
                }

                return CorrectDt;

            }


        }



        /// <summary>
        /// 筛选错误表格
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable getErrorDt(DataTable dt)
        {

            using (var db = SugarDao_MsSql.GetInstance())
            {


                DataTable ErrorDt = new DataTable();
                ErrorDt = dt.Copy();
                ErrorDt.Rows[0].Delete();
                ErrorDt.Rows[0].Delete();
                for (int i = 0; i < ErrorDt.Rows.Count; i++)
                {
                    var labStr = ErrorDt.Rows[i]["column4"].ToString();
                    if (string.IsNullOrEmpty(labStr.Trim()))
                        continue;
                    var labArr = labStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < labArr.Length; j++)
                    {
                        var phoneNo = ErrorDt.Rows[i]["column2"].ToString();
                        var idCardNo = ErrorDt.Rows[i]["column3"].ToString();
                        var PersonVguid = Guid.Empty;
                        if (!string.IsNullOrEmpty(phoneNo) && !string.IsNullOrEmpty(idCardNo))
                        {
                            //dt.Rows[i].Delete();
                            //break;
                            //throw new Exception("第" + (i + 1) + "行,手机号和身份证号不可同时为空！");

                            if (!string.IsNullOrEmpty(phoneNo))
                            {
                                PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.PhoneNumber == phoneNo).Select(it => it.Vguid).SingleOrDefault();
                            }
                            else if (!string.IsNullOrEmpty(idCardNo))
                            {
                                PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.IDNumber == idCardNo).Select(it => it.Vguid).SingleOrDefault();
                            }
                            if (PersonVguid != Guid.Empty)
                            {
                                ErrorDt.Rows[i].Delete();
                                break;
                                //throw new Exception("第" + (i + 1) + "行,人员不存在！");
                            }

                        }
                    }
                }

                return ErrorDt;

            }
        }


        public string getNulldata(DataTable ErrorDt)
        {
            string isNullorisnotExist = "";
            using (var db = SugarDao_MsSql.GetInstance())
            {
                for (int i = 2; i < ErrorDt.Rows.Count; i++)
                {
                    var labStr = ErrorDt.Rows[i]["column4"].ToString();
                    if (string.IsNullOrEmpty(labStr.Trim()))
                        continue;

                    var phoneNo = ErrorDt.Rows[i]["column2"].ToString();
                    var idCardNo = ErrorDt.Rows[i]["column3"].ToString();
                    var PersonVguid = Guid.Empty;
                    if (string.IsNullOrEmpty(phoneNo) && string.IsNullOrEmpty(idCardNo))
                    {
                        isNullorisnotExist += "第" + (i + 1) + "行,手机号和身份证号不可同时为空！<br/>";
                    }
                    if (!string.IsNullOrEmpty(phoneNo) && !string.IsNullOrEmpty(idCardNo))
                    {
                        if (!string.IsNullOrEmpty(phoneNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.PhoneNumber == phoneNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        else if (!string.IsNullOrEmpty(idCardNo))
                        {
                            PersonVguid = db.Queryable<Business_Personnel_Information>().Where(it => it.IDNumber == idCardNo).Select(it => it.Vguid).SingleOrDefault();
                        }
                        if (PersonVguid == Guid.Empty)
                        {
                            isNullorisnotExist += "第" + (i + 1) + "行,人员不存在！ <br/>";
                            //throw new Exception("第" + (i + 1) + "行,人员不存在！");
                        }

                    }

                }
            }

            return isNullorisnotExist;


        }


        public void InsertTrainers(U_WeChatRegistered ruser)
        {
            AllTrainers trainer = new AllTrainers();
            trainer.IDCard = ruser.idcard;
            trainer.Name = ruser.name;
            trainer.EmployeeNO = ruser.userid;
            trainer.Gender = 1;
            if (!string.IsNullOrEmpty(ruser.gender))
            {
                trainer.Gender = Convert.ToInt32(ruser.gender);
            }
            trainer.MobilePhone = ruser.mobile;
            using (SqlSugarClient _dbSql = SugarDao_LandaVSql.GetInstance())
            {
                AllTrainers train = _dbSql.Queryable<AllTrainers>().Where(c => c.EmployeeNO == ruser.userid).FirstOrDefault();
                if (train == null)
                {
                    _dbSql.Insert(trainer);
                }
            }
        }
        public void UpdateTrainers(U_WeChatRegistered ruser)
        {
            using (SqlSugarClient _dbSql = SugarDao_LandaVSql.GetInstance())
            {
                _dbSql.Update<AllTrainers>(new { MobilePhone = ruser.mobile }, c => c.EmployeeNO == ruser.userid);
            }
        }
        public AllTrainers GetTrainers(U_WeChatRegistered ruser)
        {
            AllTrainers trainers = null;
            using (SqlSugarClient _dbSql = SugarDao_LandaVSql.GetInstance())
            {
                List<AllTrainers> allTrainers = _dbSql.Queryable<AllTrainers>().Where(c => c.EmployeeNO == ruser.userid).ToList();
                if (allTrainers.Count > 0)
                {
                    trainers = allTrainers[0];
                }
            }
            return trainers;
        }
    }
}
