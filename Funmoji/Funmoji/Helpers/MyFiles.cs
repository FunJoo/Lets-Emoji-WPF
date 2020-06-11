using Funmoji.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Funmoji.Helpers
{
    class MyFiles
    {
        public const string EmojiPath = "config_emoji.json";
        public const string TransPath = "config_trans.json";

        public static List<MyEmoji> ReadLocalEmoji()
        {
            try
            {
                string json = File.ReadAllText(EmojiPath);
                //这个类需要添加引用：System.Web.Extensions
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var myresult = serializer.Deserialize<List<MyEmoji>>(json);
                return myresult;
            }
            catch
            {
                return null;
            }
        }

        public static bool WriteLocalEmoji(string json)
        {
            try
            {
                File.WriteAllText(EmojiPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Dictionary<string,string> ReadLocalTrans()
        {
            try
            {
                string json = File.ReadAllText(TransPath);
                //这个类需要添加引用：System.Web.Extensions
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var myresult = serializer.Deserialize<Dictionary<string, string>>(json);
                return myresult;
            }
            catch
            {
                return null;
            }
        }

        public static bool WriteLocalTrans(string json)
        {
            try
            {
                File.WriteAllText(TransPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
