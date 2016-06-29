using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Basic.Models
{
    public class WeChat
    {
        private static string token;
        private static string ticket;
        private static DateTime tokenTime;
        private static double ChangeTime = 1.0000;

        private static string appID = "wxd922779a0b7a366c";
        private static string appSecret = "1d04993bc72d7f7e2ed894bea5bfbf8f";

        /// <summary>
        /// 获取指定微信公众账号的access_token
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <para>这里一定要注意，Token存在时间是7200秒(两个小时)</para>
        /// <para>所以需要在服务器端缓存Token信息，并且全局计算</para>
        /// <para>当时间接近2小时才去服务器端重新获取</para>
        /// </remarks>
        private static string GetToken(string appID, string appSecret)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appID + "&secret=" + appSecret;
            string strJson = Http.Get(url);
            JObject json = JObject.Parse(strJson);
            WeChat.token = json["access_token"].ToString();
            return WeChat.token;
        }

        /// <summary>
        /// 根据Access-Token获取Ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static string GetTickete(string accessToken)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessToken + "&type=jsapi";
            string strJson = Http.Get(url);
            JObject json = JObject.Parse(strJson);
            WeChat.ticket = json["ticket"].ToString();
            return WeChat.ticket;
        }

        private static string GetSignatureSourceString(string ticket, string randomString, string timeStamp, string url)
        {
            return string.Format(@"jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",
                                                     ticket, randomString, timeStamp, url);
        }

        public static string GetSignature(string randomString, string timeStamp, string url)
        {
            //没有值就直接获取Token,或者有值但是当前时间比获取Token时间长,需要刷新Token
            //其他情况下直接使用缓存的Token和Ticket
            if (string.IsNullOrEmpty(token) || (!string.IsNullOrEmpty(token)) && (DateTime.Now - WeChat.tokenTime).TotalHours > ChangeTime)
            {
                WeChat.tokenTime = DateTime.Now;
                WeChat.token = GetToken(WeChat.appID, WeChat.appSecret);
                WeChat.ticket = GetTickete(WeChat.token);
            }
            string strSignature = GetSignatureSourceString(WeChat.ticket, randomString, timeStamp, url);
            return SHA1_Encrypt(strSignature);
        }

        /// <summary>
        /// use sha1 to encrypt string
        /// </summary>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
    }
}
