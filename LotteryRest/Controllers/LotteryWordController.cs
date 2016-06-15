using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteryRest.Controllers
{
    public class LotteryWordController : ApiController
    {
        // GET: api/LotteryWord
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LotteryWord/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LotteryWord
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LotteryWord/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LotteryWord/5
        public void Delete(int id)
        {
        }
    }
}
