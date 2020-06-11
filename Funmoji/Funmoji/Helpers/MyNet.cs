using Funmoji.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Funmoji.Helpers
{
    class MyNet
    {
        private static HttpWebRequest request;
        private static HttpWebResponse response;

        public const string BaseUrl = "http://funjoo.fun";

        /// <summary>
        /// 发起服务器请求，获得Json字符串
        /// </summary>
        /// <returns>获得的JSON</returns>
        public static string GetJson(string url)
        {
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = sr.ReadToEnd();
                return content;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 异步回调
        /// </summary>
        public static Func<List<MyEmoji>> FuncGetResult = GetEmojiResult;
        public static Func<Dictionary<string,string>> FuncGetTransResult = GetTransResult;

        /// <summary>
        /// 额最终是用这个
        /// </summary>
        public static void ReadRemoteEmoji(AsyncCallback callback)
        {
            //用lamda实现
            IAsyncResult res = FuncGetResult.BeginInvoke(callback, "");
        }
        public static void ReadRemoteTrans(AsyncCallback callback)
        {
            //用lamda实现
            IAsyncResult res = FuncGetTransResult.BeginInvoke(callback, "");
        }

        /// <summary>
        /// 从json得到result数据
        /// </summary>
        private static List<MyEmoji> GetEmojiResult()
        {
            string json = GetJson(BaseUrl + "/emoji");
            if (!json.StartsWith("[")) return null;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                MyFiles.WriteLocalEmoji(json);
                var myresult = serializer.Deserialize<List<MyEmoji>>(json);
                return myresult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从json得到trans数据
        /// </summary>
        private static Dictionary<string,string> GetTransResult()
        {
            string json = GetJson(BaseUrl + "/emoji/direct");
            if (!json.StartsWith("{")) return null;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                MyFiles.WriteLocalTrans(json);
                var myresult = serializer.Deserialize<Dictionary<string, string>>(json);
                return myresult;
            }
            catch
            {
                return null;
            }
        }
    }
}
