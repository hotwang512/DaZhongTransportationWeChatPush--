using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Entities;
using JQWidgetsSugar;
using SyntacticSugar;

namespace DaZhongManagementSystem.Controllers
{
    public class ValidateLoginController : Controller
    {
        //
        // GET: /ValidateLogin/

        public ActionResult Index()
        {
            string resulMess = "";
            string vguid = Request["vguid"];
            string token = Request["token"];
            if (!string.IsNullOrEmpty(vguid) && !string.IsNullOrEmpty(token))
            {
                string toolKitValidateUrl = ConfigSugar.GetAppString("ToolKitValidateUrl");
                string url = string.Format(toolKitValidateUrl + "?userVguid={0}&tokenVal={1}", vguid, token);
                string htmlResult = HttpGet(url);
                var actionResultModel = htmlResult.JsonToModel<ActionResultModel<string>>();
                if (actionResultModel.isSuccess)//登录令牌验证成功
                {
                    //UserServer us = new UserServer();
                    //RoleTypeServer rts = new RoleTypeServer();
                    bool isExist = false;//us.IsExistUserAccountByVGUID(vguid);
                    if (isExist)
                    {
                        bool isHavePersion = false;//rts.IsHaveMoudePermision(vguid);
                        if (isHavePersion)
                        {
                            //将用户信息保存到Cookie中
                            //T_User userModel = us.GetUserDetailModelByVguid(vguid);
                            //var cm = CookiesManager<T_User>.GetInstance();
                            //cm.Add(BiomobieAppSystem.Common.CostCookies.COOKIES_KEY_LOGIN, userModel, cm.Hour * 8);
                            resulMess = "";
                        }
                        else
                        {
                            resulMess = "您在本系统中未被分配任何模块访问权限，请联系本系统的管理人员！";
                        }
                    }
                    else
                    {
                        resulMess = "系统中不存在此用户信息！";
                    }
                }
                else//登录令牌验证失败
                {
                    resulMess = "您的登录令牌无效！";
                }
            }
            else
            {
                resulMess = "您的登录令牌无效！";
            }

            if (!string.IsNullOrEmpty(resulMess))
            {
                ViewBag.Message = resulMess;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }
        }




        public string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
