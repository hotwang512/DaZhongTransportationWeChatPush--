using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DaZhongManagementSystem.Common
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将实体序列化为json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToJson<T>(this T model)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Serialize(model);
        }

        /// <summary>
        /// 将json转化为实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TEntity JsonToModel<TEntity>(this string json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Deserialize<TEntity>(json);
        }
    }
}
