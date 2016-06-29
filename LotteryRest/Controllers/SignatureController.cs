using Common.Basic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteryRest.Controllers
{
    public class SignatureController : ApiController
    {
        // GET: api/Signature
        public string Get(string nonceStr,string timeStamp,string url)
        {
            return WeChat.GetSignature(nonceStr, timeStamp, url);
        }

        // GET: api/Signature/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Signature
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Signature/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Signature/5
        public void Delete(int id)
        {
        }
    }
}
