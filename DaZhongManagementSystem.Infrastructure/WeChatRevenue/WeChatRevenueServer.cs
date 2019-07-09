using System;
using System.Linq;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.LogHelper;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable;
using DaZhongManagementSystem.Entities.TableEntity.LiquidationTable;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.WeChatRevenue
{
    public class WeChatRevenueServer
    {
        private LogLogic _logLogic;

        public WeChatRevenueServer()
        {
            _logLogic = new LogLogic();
        }

        /// <summary>
        /// 新增支付历史
        /// </summary>
        /// <param name="paymentHistoryInfo">实体信息</param>
        /// <returns>返回成功与否</returns>
        public bool AddPaymentHistory(Business_PaymentHistory_Information paymentHistoryInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    //var isExist = db.Queryable<Business_PaymentHistory_Information>().Any(i => i.PaymentPersonnel == paymentHistoryInfo.PaymentPersonnel && i.WeChatPush_VGUID == paymentHistoryInfo.WeChatPush_VGUID && i.PaymentStatus == "3");
                    //if (isExist)
                    //{
                    //    db.Delete<Business_PaymentHistory_Information>(i => i.PaymentPersonnel == paymentHistoryInfo.PaymentPersonnel && i.WeChatPush_VGUID == paymentHistoryInfo.WeChatPush_VGUID && i.PaymentStatus == "3");
                    //}
                    db.Insert(paymentHistoryInfo, false);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                }

            }
        }


        public Business_PaymentHistory_Information GetPaymentHistory(string remarks)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_PaymentHistory_Information>().Where(i => i.Remarks == remarks).SingleOrDefault();
            }
        }

        /// <summary>
        /// 更新支付历史
        /// </summary>
        /// <param name="paymentHistoryInfo">实体信息</param>
        /// <returns>返回成功与否</returns>
        public bool UpdatePaymentHistory(Business_Personnel_Information personInfo, Business_PaymentHistory_Information paymentHistoryInfo, Business_PaymentHistory_Information paymentHistoryInfoOld)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                db.BeginTran();
                try
                {
                    var configInfo = db.Queryable<Master_Configuration>().Where(i => i.ID == 15).SingleOrDefault();
                    //var personInfo = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == paymentHistoryInfo.PaymentPersonnel).SingleOrDefault(); //获取人员信息
                    paymentHistoryInfo.ChangeDate = DateTime.Now;
                    paymentHistoryInfo.ChangeUser = "sysadmin_Revenue";
                    //插入操作日志表中
                    string logData = JsonHelper.ModelToJson(paymentHistoryInfo);
                    _logLogic.SaveLog(18, 46, personInfo.UserID + " " + personInfo.Name, "大众出租租赁营收费用", logData);
                    int revenueStauts = 2;
                    if (paymentHistoryInfo.PaymentStatus == "1")
                    {
                        paymentHistoryInfo.PaymentAmount = paymentHistoryInfoOld.PaymentAmount;
                        paymentHistoryInfo.RevenueReceivable = paymentHistoryInfoOld.RevenueReceivable;
                    }
                    paymentHistoryInfo.RevenueStatus = revenueStauts;
                    db.DisableUpdateColumns = new[] { "RevenueType", "WeChatPush_VGUID" };
                    db.Update<Business_PaymentHistory_Information>(paymentHistoryInfo, i => i.Remarks == paymentHistoryInfoOld.Remarks);
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    return false;
                }

            }
        }

        /// <summary>
        /// 判断交易单号是否存在
        /// </summary>
        /// <param name="paymentHistoryInfo"></param>
        /// <returns></returns>
        public bool IsExistTransactionId(Business_PaymentHistory_Information paymentHistoryInfo)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_PaymentHistory_Information>().Any(i => i.TransactionID == paymentHistoryInfo.TransactionID);
            }
        }

        /// <summary>
        /// 添加支付信息到AliPay表中
        /// </summary>
        /// <param name="paymentHistoryInfo">支付信息</param>
        /// <param name="personInfo">人员信息</param>
        /// <param name="revenueStatus"></param>
        /// <returns></returns>
        public bool AddRevenuePayInfo(Business_PaymentHistory_Information paymentHistoryInfo, Business_Personnel_Information personInfo, ref int revenueStatus)
        {
            using (var db = SugarDao_AliPaySql.GetInstance())
            {
                try
                {
                    int id = db.Queryable<ThirdPartyPublicPlatformPayment>().Max(it => it.Id).ObjToInt();
                    var driverInfo = GetDriverInfo(personInfo);
                    revenueStatus = driverInfo.License == null ? 2 : 1;
                    var model = new ThirdPartyPublicPlatformPayment()
                    {
                        Id = id + 1,
                        SerialNumber = paymentHistoryInfo.TransactionID,
                        PaymentDate = paymentHistoryInfo.PayDate,
                        PaymentAmount = paymentHistoryInfo.PaymentAmount,
                        PaymentSource = "1", //1-微信企业号
                        PaymentType = "1", //1-微信
                        CheckOutDate = paymentHistoryInfo.PayDate.AddHours(24),
                        Identifier = driverInfo.License == null ? 2 : 0,
                        DriverName = personInfo.Name,
                        DriverPhoneNum = personInfo.PhoneNumber,
                        DriverServiceNum = driverInfo.OperationNo,
                        CarNo = driverInfo.License == null ? null : driverInfo.License.TrimStart('沪'),
                        UnitCode = driverInfo.UnitCode
                    };
                    db.Insert(model, false);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                    // throw new Exception(ex.ToString());
                }

            }
        }

        /// <summary>
        /// 添加支付信息到清算系统中
        /// </summary>
        /// <param name="paymentHistoryInfo"></param>
        /// <param name="personInfo"></param>
        /// <returns></returns>
        public bool AddRevenuePayInfoToLiquidation(Business_PaymentHistory_Information paymentHistoryInfo, Business_Personnel_Information personInfo)
        {
            using (var db = SugarDao_ReckoningSql.GetInstance())
            {
                try
                {
                    var model = new Business_Revenuepayment_Information()
                    {
                        Name = personInfo.Name,
                        Department = personInfo.OwnedFleet.ToString(),
                        JobNumber = personInfo.JobNumber,
                        ServiceNumber = personInfo.ServiceNumber,
                        UserID = personInfo.UserID,
                        PhoneNumber = personInfo.PhoneNumber,
                        PaymentPersonnel = personInfo.Vguid,
                        TransactionID = paymentHistoryInfo.TransactionID,
                        PaymentType = paymentHistoryInfo.PaymentType,
                        PaymentBrokers = paymentHistoryInfo.PaymentBrokers,
                        Beneficiary = paymentHistoryInfo.Beneficiary,
                        ReceiptAccount = paymentHistoryInfo.ReceiptAccount,
                        Description = paymentHistoryInfo.Description,
                        Remarks = paymentHistoryInfo.Remarks,
                        PayDate = paymentHistoryInfo.PayDate,
                        PaymentStatus = paymentHistoryInfo.PaymentStatus,
                        CompanyAccount = paymentHistoryInfo.CompanyAccount,
                        PaymentAmount = paymentHistoryInfo.PaymentAmount,
                        ActualAmount = paymentHistoryInfo.ActualAmount,
                        RevenueType = paymentHistoryInfo.RevenueType,
                        RevenueStatus = paymentHistoryInfo.RevenueStatus,
                        CreateUser = personInfo.Name,
                        CreateDate = DateTime.Now,
                        VGUID = Guid.NewGuid()
                    };
                    db.Insert(model, false);
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                }


            }
        }
        /// <summary>
        /// 根据guid获取人员相关信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetPersonInfo(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                return db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == vguid).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取司机的服务号、车牌号以及公司代码
        /// </summary>
        /// <param name="personInfo"></param>
        /// <returns></returns>
        public U_DriverInfo GetDriverInfo(Business_Personnel_Information personInfo)
        {
            U_DriverInfo uDriverInfo = null;
            using (var dbDriver = SugarDao_DriverRevenueSql.GetInstance())
            {
                var driverInfo = dbDriver.Queryable<Driver>().Where(i => i.IdCard == personInfo.IDNumber && i.Status == 1).SingleOrDefault();
                if (driverInfo == null || driverInfo.CabID == null)
                {
                    return new U_DriverInfo();
                }
                var cabInfo = dbDriver.Queryable<Cab>().Where(i => i.Id == driverInfo.CabID).SingleOrDefault();
                if (cabInfo == null || cabInfo.License == null)
                {
                    return new U_DriverInfo();
                }
                var organizationInfo = dbDriver.Queryable<Organization>().Where(i => i.ID == driverInfo.OrganizationID).SingleOrDefault();
                uDriverInfo = new U_DriverInfo
                {
                    OperationNo = driverInfo.OperationNo,
                    License = cabInfo.License,
                    UnitCode = organizationInfo.UnitCode
                };
            }
            return uDriverInfo;
        }

        /// <summary>
        /// 获取所有的支付历史
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_PaymentHistory_Information> GetAllPaymentHistoryInfo(U_PaymentHistory_Search searchParas, GridParams para)
        {
            var jsonResult = new JsonResultModel<v_PaymentHistory_Information>();
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<v_PaymentHistory_Information>();
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                    if (listDep.Count > 0)
                    {
                        query.In(i => i.OwnedFleet, listDep);
                    }
                }
                if (!string.IsNullOrEmpty(searchParas.Name))
                {
                    query.Where(i => i.Name.Contains(searchParas.Name));
                }
                if (!string.IsNullOrEmpty(searchParas.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParas.PhoneNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.IDNumber))
                {
                    query.Where(i => i.IDNumber.Contains(searchParas.IDNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.JobNumber))
                {
                    query.Where(i => i.JobNumber.Contains(searchParas.JobNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.PaymentStatus))
                {
                    query.Where(i => i.PaymentStatus == searchParas.PaymentStatus);
                }
                if (!string.IsNullOrEmpty(searchParas.TransactionID))
                {
                    query.Where(i => i.TransactionID.Contains(searchParas.TransactionID));
                }
                if (!string.IsNullOrEmpty(searchParas.Department))
                {
                    Guid department = Guid.Parse(searchParas.Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + department + "')");  //找到该部门以及其所有子部门
                    if (listDep.Count > 0)
                    {
                        query.In(i => i.OwnedFleet, listDep);
                    }
                }
                if (searchParas.PayDateFrom != null)
                {
                    query.Where(i => i.PayDate >= searchParas.PayDateFrom);
                }
                if (searchParas.PayDateTo != null)
                {
                    query.Where(i => i.PayDate <= searchParas.PayDateTo);
                }
                query.OrderBy(para.sortdatafield + " " + para.sortorder);
                var pageCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref pageCount);
                jsonResult.TotalRows = pageCount;
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(jsonResult);
                _logLogic.SaveLog(3, 47, CurrentUser.GetCurrentUser().LoginName, "支付历史", logData);
            }
            return jsonResult;
        }

        /// <summary>
        /// 删除付款历史
        /// </summary>
        /// <param name="vguid">主键</param>
        /// <returns></returns>
        public bool DeletePaymentHistory(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var model = db.Queryable<Business_PaymentHistory_Information>().Where(i => i.VGUID == vguid).SingleOrDefault();
                string logData = JsonHelper.ModelToJson(model);
                _logLogic.SaveLog(2, 47, CurrentUser.GetCurrentUser().LoginName, "支付历史", logData);
                return db.Delete<Business_PaymentHistory_Information>(i => i.VGUID == vguid);
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchParas">导出条件</param>
        public void Export(U_PaymentHistory_Search searchParas)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<v_PaymentHistory_Information>();
                if (CurrentUser.GetCurrentUser().LoginName != "sysAdmin")
                {
                    Guid dep = Guid.Parse(CurrentUser.GetCurrentUser().Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + dep + "')");  //找到该部门以及其所有子部门
                    if (listDep.Count > 0)
                    {
                        query.In(i => i.OwnedFleet, listDep);
                    }
                }
                if (!string.IsNullOrEmpty(searchParas.Name))
                {
                    query.Where(i => i.Name.Contains(searchParas.Name));
                }
                if (!string.IsNullOrEmpty(searchParas.PhoneNumber))
                {
                    query.Where(i => i.PhoneNumber.Contains(searchParas.PhoneNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.IDNumber))
                {
                    query.Where(i => i.IDNumber.Contains(searchParas.IDNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.JobNumber))
                {
                    query.Where(i => i.JobNumber.Contains(searchParas.JobNumber));
                }
                if (!string.IsNullOrEmpty(searchParas.PaymentStatus))
                {
                    query.Where(i => i.PaymentStatus == searchParas.PaymentStatus);
                }
                if (!string.IsNullOrEmpty(searchParas.TransactionID))
                {
                    query.Where(i => i.TransactionID.Contains(searchParas.TransactionID));
                }
                if (!string.IsNullOrEmpty(searchParas.Department))
                {
                    Guid department = Guid.Parse(searchParas.Department);
                    var listDep = db.SqlQuery<Guid>("SELECT * FROM dbo.TF_OrganizationFDetail('" + department + "')");  //找到该部门以及其所有子部门
                    if (listDep.Count > 0)
                    {
                        query.In(i => i.OwnedFleet, listDep);
                    }
                }
                if (searchParas.PayDateFrom != null)
                {
                    query.Where(i => i.PayDate >= searchParas.PayDateFrom);
                }
                if (searchParas.PayDateTo != null)
                {
                    query.Where(i => i.PayDate <= searchParas.PayDateTo);
                }

                query.OrderBy(i => i.PayDate, OrderByType.Desc);
                var dt = query.ToDataTable();
                dt.TableName = "PaymentHistoryInfo";
                ExportExcel.ExportExcels("PaymentHistory.xlsx", "PaymentHistory" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", dt);
                _logLogic.SaveLog(13, 47, CurrentUser.GetCurrentUser().LoginName, "PaymentHistory", Common.Tools.DataTableHelper.Dtb2Json(dt));
            }
        }


        /// <summary>
        /// 查询当前人员是否已经支付过
        /// </summary>
        /// <param name="personVguid"></param>
        /// <param name="pushCongtentVguid"></param>
        /// <param name="revenueType"></param>
        /// <returns></returns>
        public bool HasPaymentHistory(Guid personVguid, Guid pushCongtentVguid, int revenueType)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var intArray = new[] { "1", "4" };  //状态为成功或者已退款
                var isExist = db.Queryable<Business_PaymentHistory_Information>().Any(i => i.PaymentPersonnel == personVguid && i.WeChatPush_VGUID == pushCongtentVguid && intArray.Contains(i.PaymentStatus) && i.RevenueType == revenueType);
                return isExist;
            }
        }

        /// <summary>
        /// 退款成功后，修改付款状态
        /// </summary>
        /// <param name="transaction_id">微信流水号</param>
        /// <returns></returns>
        public bool UpdateStatus(string transaction_id)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    var isSuccess = db.Update<Business_PaymentHistory_Information>(new { PaymentStatus = "4" }, i => i.TransactionID == transaction_id);  //修改状态为已退款
                    if (isSuccess)
                    {
                        Delete(transaction_id);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    return false;
                }

            }
        }
        /// <summary>
        /// 退款成功后，删除营收数据
        /// </summary>
        /// <param name="transaction_id">微信流水号</param>
        /// <returns></returns>
        public bool Delete(string transaction_id)
        {
            using (var db = SugarDao_AliPaySql.GetInstance())
            {
                return db.Delete<ThirdPartyPublicPlatformPayment>(i => i.SerialNumber == transaction_id);
            }
        }
        /// <summary>
        /// 判断推送消息是否过期
        /// </summary>
        /// <param name="pushContentVguid"></param>
        /// <returns></returns>
        public bool IsValid(Guid pushContentVguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var dt = db.Queryable<Business_WeChatPush_Information>().Where(i => i.VGUID == pushContentVguid).SingleOrDefault();
                if (dt != null)
                {
                    if (dt.PeriodOfValidity != null && DateTime.Now > dt.PeriodOfValidity)
                    {
                        return true; //已过有效期
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// 将支付历史表中营收状态为未匹配的重新插入到营收表(ThirdPartyPublicPlatformPayment)中
        /// </summary>
        /// <returns></returns>
        public bool Insert2Revenue(Guid vguid)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    var paymentInfo = db.Queryable<Business_PaymentHistory_Information>().Where(i => i.VGUID == vguid).SingleOrDefault();
                    var personInfo = db.Queryable<Business_Personnel_Information>().Where(i => i.Vguid == paymentInfo.PaymentPersonnel).SingleOrDefault();
                    int revenueStatus = 0;
                    var isSuccess = InsertOrUpdateRevenue(paymentInfo, personInfo, ref revenueStatus);
                    if (isSuccess && revenueStatus == 1)
                    {
                        db.Update<Business_PaymentHistory_Information>(new { RevenueStatus = 1 }, i => i.VGUID == vguid);
                    }
                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.ToString());
                    db.RollbackTran();
                    return false;
                }
            }
        }

        /// <summary>
        /// 插入或者更新营收数据库表中的数据信息
        /// </summary>
        /// <param name="paymentHistoryInfo">付款信息</param>
        /// <param name="personInfo">人员信息</param>
        /// <returns>返回cheng</returns>
        public bool InsertOrUpdateRevenue(Business_PaymentHistory_Information paymentHistoryInfo, Business_Personnel_Information personInfo, ref int revenueStatus)
        {
            using (var db = SugarDao_AliPaySql.GetInstance())
            {
                try
                {
                    bool isExist = db.Queryable<ThirdPartyPublicPlatformPayment>().Any(i => i.SerialNumber == paymentHistoryInfo.TransactionID);
                    var driverInfo = GetDriverInfo(personInfo);   //获取司机信息
                    revenueStatus = driverInfo.License == null ? 2 : 1;
                    if (isExist)  //存在就更新，不存在则插入
                    {
                        db.Update<ThirdPartyPublicPlatformPayment>(new
                        {
                            DriverServiceNum = driverInfo.OperationNo,
                            CarNo = driverInfo.License == null ? null : driverInfo.License.TrimStart('沪'),
                            UnitCode = driverInfo.UnitCode,
                            Identifier = driverInfo.License == null ? 2 : 0,
                        }, i => i.SerialNumber == paymentHistoryInfo.TransactionID);
                    }
                    else
                    {
                        var id = db.Queryable<ThirdPartyPublicPlatformPayment>().Max(it => it.Id).ObjToInt();
                        var model = new ThirdPartyPublicPlatformPayment
                        {
                            Id = id + 1,
                            SerialNumber = paymentHistoryInfo.TransactionID,
                            PaymentDate = paymentHistoryInfo.PayDate,
                            PaymentAmount = paymentHistoryInfo.PaymentAmount,
                            PaymentSource = "1", //1-微信企业号
                            PaymentType = "1", //1-微信
                            CheckOutDate = paymentHistoryInfo.PayDate.AddHours(24),
                            Identifier = driverInfo.License == null ? 2 : 0,
                            DriverName = personInfo.Name,
                            DriverPhoneNum = personInfo.PhoneNumber,
                            DriverServiceNum = driverInfo.OperationNo,
                            CarNo = driverInfo.License == null ? null : driverInfo.License.TrimStart('沪'),
                            UnitCode = driverInfo.UnitCode
                        };
                        db.Insert(model, false);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

            }
        }

    }
}