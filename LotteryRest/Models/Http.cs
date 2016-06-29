using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Common.Basic.Models
{
    //ContentType:
    //1.text/html
    //2.text/plain
    //3.text/css
    //4.text/javascript
    //5.application/x-www-form-urlencoded
    //6.multipart/form-data
    //7.application/json
    //8.application/xml
    public class Http
    {
        public static String Get(string URLAddress)
        {
            
            try
            {
                WebRequest request = HttpWebRequest.Create(URLAddress);
                //request.ContentType = "application/json";
                WebResponse wr = request.GetResponse();
                StreamReader st = new StreamReader(wr.GetResponseStream());
                return st.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
