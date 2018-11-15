using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Controllers.LoginLogic;
using DaZhongManagementSystem.Entities.View;
using DaZhongManagementSystem.Infrastructure.DailyLogManagement;
using SyntacticSugar;
using System;
using System.Net;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private readonly UserLogin _ul;
        private readonly LogLogic _ll;
        public LoginController()
        {
            _ul = new UserLogin();
            _ll = new LogLogic();
        }

        public ActionResult Index()
        {

         //              WebClient wc = new WebClient();
         //              var data ="{"+
         //  "\"BillLable\":\"WX\","+
         //  "\"DriverID\":\"183601\","+
         //  "\"JobNumber\":\"997402\","+
         //  "\"Name\":\"王存标\","+
         //  "\"OriginalId\":\"183481\","+
         //  "\"OrganizationID\":\"55\","+
         //  "\"PayDate\":\"2017-07-15\","+
         //  "\"TransactionId\":\"12345678900\","+
         //  "\"PaymentBrokers\":\"WeiChat\","+
         //  "\"Channel_Id\":\"1465779302_55\","+
         //  //"\"SubjectId\":\"1465779302_55\","+
         //  "\"PaymentAmount\":1000.00, "+
         //  "\"CopeFee\":0.03," +
         //  "\"ActualAmount\":10.03,"+
         //  "\"ReceiptCategory\":11,"+
         //  "\"Remark\":\"WeiChat\"," +
         //  "\"CompanyAccount\":0" +
         //  "}";
         //  //wc.Headers.Clear();
         //  wc.Headers.Add("Content-Type", "application/json;charset=utf-8");
         //  wc.Encoding= System.Text.Encoding.UTF8;
         //  wc.UploadStringAsync(new Uri("http://192.168.173.51:8088/ExternalDeveloper/Interface2Landa.cfc?method=PaymentReceiptInterface"), data);

            return View();
        }

        /// <summary>
        /// 判断登录的用户名和密码是否正确
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Index(string userName, string pwd)
        {
            string result = _ul.ProcessLogin(userName, pwd);
            if (result == "登陆成功！")
            {
                var userInfo = _ul.GeUserManagement(userName);
                //登陆成功：将登录信息写入cookie
                var cm = CookiesManager<V_User_Information>.GetInstance();
                cm.Add(CostCookies.COOKIES_KEY_LOGIN, userInfo, cm.Hour * 24);//将cookie保存24小时
                //存入操作日志表
                string logData = JsonHelper.ModelToJson(userInfo);
                _ll.SaveLog(14, 0, userName, "", logData);
                return "ok";
            }
            return result;
        }
    }
}
