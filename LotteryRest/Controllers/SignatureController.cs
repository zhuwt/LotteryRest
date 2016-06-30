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
        public DTO.ReturnJasonConstruct<DTO.Signature> Get(string url)
        {
            DTO.ReturnJasonConstruct<DTO.Signature> returnDTO = new DTO.ReturnJasonConstruct<DTO.Signature>();
            DTO.Signature dto = new DTO.Signature();
            dto.nonceStr = Guid.NewGuid().ToString();
            dto.timeStamp = WeChat.GetTimeStamp();
            dto.signature = WeChat.GetSignature(dto.nonceStr, dto.timeStamp, url);
            returnDTO.DTOObject = dto;

            return returnDTO;
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
