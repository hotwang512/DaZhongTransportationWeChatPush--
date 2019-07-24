using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DaZhongPersonTable;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DaZhongManagementSystem.Infrastructure
{
    public class WeChatValidationServer
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<Master_Organization> GetOrganization()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                return _dbMsSql.Queryable<Master_Organization>().ToList();
            }
        }

        public void TestUser()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_LandaVSql.GetInstance())
            {
                List<AllEmployee> listAllEmployees = _dbMsSql.Queryable<AllEmployee>().Where(i => i.IDCard == "341282199205162413").ToList();
            }
        }

        /// <summary>
        /// 审核用户是否存在并保存至Person表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="userID"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public string CheckUser(AllEmployee userModel, string userID, string position, string mobilePhone)
        {
            try
            {

                using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_LandaVSql.GetInstance())
                {
                    bool result = false;
                    AllEmployee landaUser = null;
                    Queryable<AllEmployee> listAllEmployees = _dbMsSql.Queryable<AllEmployee>().Where(i => i.IDCard == userModel.IDCard);
                    if (listAllEmployees.Count() > 0)
                    {
                        landaUser = listAllEmployees.First();
                    }
                    if (landaUser != null)
                    {
                        Guid vguid = Guid.NewGuid();
                        Business_Personnel_Information personInfo = new Business_Personnel_Information();
                        switch (position)
                        {
                            case "司机":
                                personInfo.DepartmenManager = 1;
                                break;
                            case "普通员工":
                                personInfo.DepartmenManager = 2;
                                break;
                            case "管理人员":
                                personInfo.DepartmenManager = 3;
                                break;
                        }
                        //DateTime now = DateTime.Now;
                        //if (!string.IsNullOrEmpty(landaUser.BirthDay))
                        //{
                        //    DateTime birthday = DateTime.Parse(landaUser.BirthDay);
                        //    int age = now.Year - birthday.Year;
                        //    if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day))
                        //        age--;
                        //    personInfo.Age = age; //年龄
                        //}

                        personInfo.Name = landaUser.Name;
                        personInfo.Vguid = vguid;
                        string gender = "1";
                        if (landaUser.Gender.HasValue)
                        {
                            gender = landaUser.Gender.Value.ToString(); //性别
                        }
                        personInfo.Sex = gender;
                        personInfo.OwnedFleet = Guid.Parse(userModel.OrganizationID); //所属部门
                        //判断微信带过来的手机号是否为空（如果为空取人员系统中的手机号，不为空则取微信中的手机号）
                        if (string.IsNullOrEmpty(mobilePhone))
                        {
                            personInfo.PhoneNumber = landaUser.MobilePhone; //人员系统手机号
                        }
                        else
                        {
                            personInfo.PhoneNumber = mobilePhone; //微信带过来的手机号
                        }
                        personInfo.UserID = userID;
                        personInfo.ID = userID;
                        personInfo.IDNumber = landaUser.IDCard; //身份证号
                        personInfo.JobNumber = landaUser.EmployeeNO; //工号
                        personInfo.LicensePlate = landaUser.DrivingLicense; //车牌号
                        personInfo.ApprovalStatus = 2; //已审核
                        personInfo.ApprovalType = 1; //系统审核
                        personInfo.CreatedDate = DateTime.Now;
                        personInfo.ChangeDate = DateTime.Now;
                        result = SavePersonInfo(personInfo);
                        if (result)
                        {
                            return "1"; //Person表保存成功
                        }
                        else
                        {
                            _dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = 1 }, it => it.Vguid == vguid);
                            return "2"; //Person表保存失败
                        }
                    }
                    else
                    {
                        //Business_Personnel_Information personInfo = new Business_Personnel_Information();
                        //switch (position)
                        //{
                        //    case "司机":
                        //        personInfo.DepartmenManager = 1;
                        //        break;
                        //    case "普通员工":
                        //        personInfo.DepartmenManager = 2;
                        //        break;
                        //    case "管理人员":
                        //        personInfo.DepartmenManager = 3;
                        //        break;
                        //}
                        //DateTime now = DateTime.Now;
                        ////if (!string.IsNullOrEmpty(landaUser.BirthDay))
                        ////{
                        ////    DateTime birthday = DateTime.Parse(landaUser.BirthDay);
                        ////    int age = now.Year - birthday.Year;
                        ////    if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day))
                        ////        age--;
                        ////    personInfo.Age = age.ToString(); //年龄
                        ////}
                        //personInfo.Vguid = Guid.NewGuid();
                        //personInfo.OwnedFleet = Guid.Parse(userModel.OrganizationID); //所属部门
                        ////判断微信带过来的手机号是否为空（如果为空取人员系统中的手机号，不为空则取微信中的手机号）
                        //if (string.IsNullOrEmpty(mobilePhone))
                        //{
                        //    personInfo.PhoneNumber = landaUser.MobilePhone; //人员系统手机号
                        //}
                        //else
                        //{
                        //    personInfo.PhoneNumber = mobilePhone; //微信带过来的手机号
                        //}

                        //personInfo.UserID = userID;
                        //personInfo.ID = userID;
                        //personInfo.IDNumber = userModel.IDCard; //身份证号
                        //if (personInfo.IDNumber.Length == 18)
                        //{
                        //    int gender;
                        //    bool isSuccess = int.TryParse(personInfo.IDNumber.Substring(16, 1), out gender);
                        //    if (isSuccess)
                        //    {
                        //        personInfo.Sex = gender % 2 == 0 ? "2" : "1";
                        //    }

                        //    string strYear = personInfo.IDNumber.Substring(6, 8).Insert(4, "-").Insert(7, "-");	//提取出生年份
                        //    TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(strYear));
                        //    personInfo.Age = ts.Days / 365;
                        //}

                        //personInfo.JobNumber = userModel.EmployeeNO; //工号
                        //personInfo.ApprovalStatus = 1; //未审核
                        //personInfo.ApprovalType = 2; //手动关注
                        //personInfo.CreatedDate = DateTime.Now;
                        //personInfo.ChangeDate = DateTime.Now;
                        //result = SavePersonInfo(personInfo);
                        return "3"; //LandaV9库中不存在
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.LogHelper.WriteLog("二次验证：" + ex.Message);
                return "3"; //LandaV9库中不存在
            }
        }

        /// <summary>
        /// 保存用户信息至Person表
        /// </summary>
        /// <param name="personInfo"></param>
        /// <returns></returns>
        public bool SavePersonInfo(Business_Personnel_Information personInfo)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    _dbMsSql.DisableInsertColumns = new[] { "TranslationOwnedFleet" };
                    Business_Personnel_Information personInfoDetail = new Business_Personnel_Information();
                    personInfoDetail = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.IDNumber == personInfo.IDNumber || i.PhoneNumber == personInfo.PhoneNumber).SingleOrDefault();
                    if (personInfoDetail != null)//更新
                    {
                        var model = new
                        {
                            UserID = personInfo.UserID,
                            IDNumber = personInfo.IDNumber,
                            Name = personInfo.Name,
                            Age = personInfo.Age,
                            Sex = personInfo.Sex,
                            ID = personInfo.ID,
                            JobNumber = personInfo.JobNumber,
                            ServiceNumber = personInfo.ServiceNumber,
                            OwnedFleet = personInfo.OwnedFleet,
                            LicensePlate = personInfo.LicensePlate,
                            PhoneNumber = personInfo.PhoneNumber,
                            DepartmenManager = personInfo.DepartmenManager,
                            ChangeDate = DateTime.Now
                        };
                        result = _dbMsSql.Update<Business_Personnel_Information>(model, i => i.Vguid == personInfoDetail.Vguid);
                    }
                    else//新增
                    {
                        var rtn = _dbMsSql.Insert(personInfo);
                        if (rtn != null && rtn.ToString() == "true")
                        {
                            result = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _dbMsSql.RollbackTran();
                    Common.LogHelper.LogHelper.WriteLog("注册时保存用户信息" + ex.ToString());
                }
                return result;
            }
        }


        public bool UpdateStatus(string idCard)
        {
            using (var db = SugarDao.SugarDao_MsSql.GetInstance())
            {
                return db.Update<Business_Personnel_Information>(new { ApprovalStatus = 1 }, it => it.IDNumber == idCard);
            }
        }
    }
}
