using System;
using System.Collections.Generic;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Common.Tools;
using DaZhongManagementSystem.Common.WeChatPush;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using DaZhongManagementSystem.Infrastructure.SugarDao;
using SqlSugar;

namespace DaZhongManagementSystem.Infrastructure.DraftManagement
{
    public class RedPacketServer
    {
        private readonly LogLogic _ll;

        public RedPacketServer()
        {
            _ll = new LogLogic();
        }


        /// <summary>
        /// 根据搜索条件获取红包领取记录历史
        /// </summary>
        /// <param name="searchParas"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public JsonResultModel<v_Business_Redpacket_Push_Information> GetRedPacketHistoryList(Search_RedPacketHistory searchParas, GridParams para)
        {
            GetRedPacketInfo();
            var jsonResult = new JsonResultModel<v_Business_Redpacket_Push_Information>();
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<v_Business_Redpacket_Push_Information>();
                if (!string.IsNullOrEmpty(searchParas.Name))
                {
                    query.Where(i => i.Name.Contains(searchParas.Name));
                }
                if (!string.IsNullOrEmpty(searchParas.UserID))
                {
                    query.Where(i => i.UserID.Contains(searchParas.UserID));
                }
                if (searchParas.RedpacketStatus != null)
                {
                    query.Where(i => i.RedpacketStatus == searchParas.RedpacketStatus);
                }
                if (searchParas.ReceiveDateFrom != null && searchParas.ReceiveDateTo == null)
                {
                    query.Where(i => i.ReceiveDate >= searchParas.ReceiveDateFrom);
                }
                if (searchParas.ReceiveDateFrom == null && searchParas.ReceiveDateTo != null)
                {
                    query.Where(i => i.ReceiveDate <= searchParas.ReceiveDateTo);
                }
                if (searchParas.ReceiveDateFrom != null && searchParas.ReceiveDateTo != null)
                {
                    query.Where(i => i.ReceiveDate >= searchParas.ReceiveDateFrom && i.ReceiveDate <= searchParas.ReceiveDateTo);
                }
                query.OrderBy(i => i.CreatedDate, OrderByType.Desc);
                int totalCount = 0;
                jsonResult.Rows = query.ToPageList(para.pagenum, para.pagesize, ref totalCount);
                jsonResult.TotalRows = totalCount;
                var logData = JsonConverter.Serialize(jsonResult);
                _ll.SaveLog(3, 51, CurrentUser.GetCurrentUser().LoginName, "红包领取历史", logData);
            }
            return jsonResult;
        }
        /// <summary>
        /// 根据搜索条件获取企业付款历史
        /// </summary>
        /// <param name="searchParas">搜索条件</param>
        /// <param name="para">页码信息</param>
        /// <returns></returns>
        public JsonResultModel<v_Business_Redpacket_Push_Information> GetPaymentInfos(Search_RedPacketHistory searchParas, GridParams para)
        {
            var jsonResult = new JsonResultModel<v_Business_Redpacket_Push_Information>();
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var query = db.Queryable<Business_Enterprisepayment_Information>().JoinTable<Business_Personnel_Information>((s1, s2) => s1.UserID == s2.UserID);
                if (!string.IsNullOrEmpty(searchParas.Name))
                {
                    query.Where<Business_Personnel_Information>((s1, s2) => s2.Name.Contains(searchParas.Name));
                }
                if (!string.IsNullOrEmpty(searchParas.UserID))
                {
                    query.Where(s1 => s1.UserID.Contains(searchParas.UserID));
                }
                if (searchParas.RedpacketStatus != null)
                {
                    query.Where(s1 => s1.RedpacketStatus == searchParas.RedpacketStatus);
                }
                if (searchParas.ReceiveDateFrom != null && searchParas.ReceiveDateTo == null)
                {
                    query.Where(s1 => s1.CreatedDate >= searchParas.ReceiveDateFrom);
                }
                if (searchParas.ReceiveDateFrom == null && searchParas.ReceiveDateTo != null)
                {
                    query.Where(s1 => s1.CreatedDate <= searchParas.ReceiveDateTo);
                }
                if (searchParas.ReceiveDateFrom != null && searchParas.ReceiveDateTo != null)
                {
                    query.Where(s1 => s1.CreatedDate >= searchParas.ReceiveDateFrom && s1.CreatedDate <= searchParas.ReceiveDateTo);
                }
                query.OrderBy(s1 => s1.CreatedDate, OrderByType.Desc);
                jsonResult.TotalRows = query.Count();
                jsonResult.Rows = query.Select<Business_Enterprisepayment_Information, Business_Personnel_Information, v_Business_Redpacket_Push_Information>
                    ((s1, s3, s2) => new v_Business_Redpacket_Push_Information
                    {
                        UserID = s1.UserID,
                        Name = s2.Name,
                        RedpacketMoney = s1.RedpacketMoney,
                        RedpacketStatus = s1.RedpacketStatus,
                        CreatedDate = s1.CreatedDate,
                        Reson = s1.Reson,
                        VGUID = s1.VGUID
                    }).ToPageList(para.pagenum, para.pagesize);

            }
            return jsonResult;

        }
        /// <summary>
        /// 调用微信接口获取红包信息
        /// </summary>
        public void GetRedPacketInfo()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                var listOrderNo = db.Queryable<Business_Redpacket_Push_Information>().Where("isnull(RedpacketStatus,0) not in (3,4,6)").Select(i => i.OrderNumber).ToList();
                foreach (string billNo in listOrderNo)
                {
                    WxPayData data = new WxPayData();
                    data.SetValue("mch_billno", billNo);
                    var result = WeChatTools.QueryWorkWxRedPacket(data);
                    if (result.GetValue("return_code").ToString() != "SUCCESS" || result.GetValue("result_code").ToString() != "SUCCESS") continue;
                    Business_Redpacket_Push_Information redpacket = new Business_Redpacket_Push_Information();
                    var status = result.GetValue("status").ToString();  //红包状态  
                    var redPacketStatus = (int)((RedPacketStatus)Enum.Parse(typeof(RedPacketStatus), status));
                    redpacket.RedpacketStatus = redPacketStatus;
                    var sendTime = result.GetValue("send_time").ToString();
                    redpacket.CreatedDate = DateTime.Parse(sendTime);
                    switch (status)
                    {
                        case "RECEIVED":
                            var rcvTime = result.GetValue("rcv_time").ToString();
                            //var rcvTime = hblist.Substring(hblist.Length - 19, 19);
                            redpacket.ReceiveDate = DateTime.Parse(rcvTime);
                            break;
                        case "REFUND":
                            var refundTime = result.GetValue("refund_time").ToString();
                            redpacket.ReceiveDate = DateTime.Parse(refundTime);
                            break;
                        case "FAILED":
                            var reason = result.GetValue("reason").ToString();
                            redpacket.Reson = reason;
                            break;
                        default:
                            redpacket.ReceiveDate = null;
                            break;
                    }
                    redpacket.OrderNumber = billNo;
                    UpdateRedPacketInfo(redpacket);
                }
            }

        }
        /// <summary>
        /// 根据订单号更新表中的红包状态和接收时间字段
        /// </summary>
        /// <param name="redpacket">红包信息</param>
        /// <returns>返回更新是否成功</returns>
        public bool UpdateRedPacketInfo(Business_Redpacket_Push_Information redpacket)
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {

                return db.Update<Business_Redpacket_Push_Information>(new
                   {
                       redpacket.CreatedDate,
                       redpacket.RedpacketStatus,
                       redpacket.ReceiveDate,
                       redpacket.Reson
                   }, it => it.OrderNumber == redpacket.OrderNumber);
            }
        }


        /// <summary>
        /// 获取红包状态
        /// </summary>
        /// <returns></returns>
        public List<CS_Master_2> GetRedPacketStatus()
        {
            using (var db = SugarDao_MsSql.GetInstance())
            {
                Guid vguid = Guid.Parse(MasterVGUID.RedPacketStatus);
                return db.Queryable<CS_Master_2>().Where(i => i.VGUID == vguid).OrderBy(i => i.Zorder).ToList();
            }
        }

    }
}