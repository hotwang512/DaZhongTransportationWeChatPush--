using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace DaZhongManagementSystem
{
    public partial class ValidateLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Convert.ToString(Request["userId"]);                 //Toolkit系统传过来的用户识别码
            string tokenId = Convert.ToString(Request["tokenId"]);               //用户点击登录时产生的登录时的随机令牌
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(tokenId))
            {
                
                string toolKitPath = ConfigSugar.GetAppString("ToolKitValidateUrl"); //配置主系统的站点地址
                string url = string.Format(toolKitPath + "?userid={0}&tokenid={1}", userId, tokenId);
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.UseDefaultCredentials = true;
                string html = wc.DownloadString(url);               
                
                if (html == "True")
                {
                    //UserBLL userBLL = new BLL.UserBLL();
                    //UserModel userModel = userBLL.GetModel(userId);
                    //if (userModel != null)
                    //{                        
                    //    Response.Redirect("FramePage/Main.aspx");
                    //}
                    //else
                    //{
                    //    string script = string.Format(@"<script>alert('系统中没有此账号信息，请联系管理员!');  window.close();</script>");
                    //    Response.Write(script);
                    //}
                }
                else
                {
                    string script = string.Format(@"<script>alert('您没有权限访问此系统!'); window.close();</script>");
                    Response.Write(script);

                }
            }
            else
            {
                string script = string.Format(@"<script>alert('您没有权限访问此系统!');window.close();</script>");
                Response.Write(script);              

            }
        }
    }
}