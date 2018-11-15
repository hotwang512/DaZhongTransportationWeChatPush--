using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.TableEntity.DriverRevenueTable;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Infrastructure
{
    public class ShortMsgServer
    {
        /// <summary>
        /// 获取人员姓名以及工号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business_Personnel_Information GetUserNameAndJobNum(string userID)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                Business_Personnel_Information personModel = new Business_Personnel_Information();
                try
                {
                    personModel = _dbMsSql.Queryable<Business_Personnel_Information>().Where(i => i.UserID == userID).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                }
                finally
                {

                }
                return personModel;
            }
        }

        /// <summary>
        /// 获取司机ID和车辆ID
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public Driver GetDriverMsg(Business_Personnel_Information personModel)
        {
            using (SqlSugarClient _dbDriverSql = SugarDao.SugarDao_DriverRevenueSql.GetInstance())
            {
                Driver driverModel = new Driver();
                try
                {
                    driverModel = _dbDriverSql.Queryable<Driver>().Where(i => i.IdCard == personModel.IDNumber && i.Status == 1).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                }
                finally
                {

                }
                return driverModel;
            }
        }

        /// <summary>
        /// 获取营收信息
        /// </summary>
        /// <param name="driverModel"></par am>
        /// <returns></returns>
        public PaymentMonthly GetRevenueMsg(Driver driverModel)
        {
            using (SqlSugarClient _dbDriverSql = SugarDao.SugarDao_DriverRevenueSql.GetInstance())
            {
                PaymentMonthly paymentModel = new PaymentMonthly();
                try
                {
                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;
                    DateTime accountPeriod = DateTime.Parse(year + "-" + month + "-" + "01");
                    paymentModel = _dbDriverSql.Queryable<PaymentMonthly>().Where(i => i.DriverId == driverModel.Id && i.AccountPeriod == accountPeriod).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                }
                finally
                {

                }
                return paymentModel;
            }
        }

        /// <summary>
        /// 获取营收信息是否由短信发送
        /// </summary>
        /// <returns></returns>
        public string GetIsSmsPush()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string isSmsPush = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 2).SingleOrDefault().ConfigValue;
                return isSmsPush;
            }
        }

        /// <summary>
        /// 获取营收信息是否由微信发送
        /// </summary>
        /// <returns></returns>
        public string GetIsWeChatPush()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string isWeChatPush = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 1).SingleOrDefault().ConfigValue;
                return isWeChatPush;
            }
        }

        /// <summary>
        /// 获取营收信息是否由营收发送
        /// </summary>
        /// <returns></returns>
        public string GetIsRevenuePush()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string isRevenuePush = _dbMsSql.Queryable<Master_Configuration>().Where(i => i.ID == 71).SingleOrDefault().ConfigValue;
                return isRevenuePush;
            }
        }

        /// <summary>
        /// 保存要发送的营收信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool SaveRevenueMsg(Business_WeChatPush_Information weChatPush, Business_WeChatPushDetail_Information weChatPushDetail)
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    _dbMsSql.BeginTran();
                    result = _dbMsSql.Insert<Business_WeChatPush_Information>(weChatPush, false) != DBNull.Value;
                    if (result)
                    {
                        //短信接收者
                        result = _dbMsSql.Insert<Business_WeChatPushDetail_Information>(weChatPushDetail, false) != DBNull.Value;
                    }
                    _dbMsSql.CommitTran();
                }
                catch (Exception ex)
                {
                    Common.LogHelper.LogHelper.WriteLog(ex.Message);
                    _dbMsSql.RollbackTran();
                }
                finally
                {

                }

                return result;
            }
        }
    }
}
