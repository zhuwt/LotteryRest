using LotteryRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteryRest.Controllers
{
    public class GiftController : ApiController
    {
        // GET: api/Gift
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Gift/5
        public DTO.ReturnJasonConstruct<List<DTO.Gift>> Get(Guid id)
        {
            UserInformationEntities db = new UserInformationEntities();
            var q = from c in db.lottery1
                    from d in db.LotteryGift
                    where c.LotteryGiftID==d.ID && c.UserID == id && d.GiftName != "谢谢惠顾" && c.Receive == false
                    select new
                    {
                        GiftName = d.GiftName,
                        GiftDescribtion = d.GiftDescribtion,
                        remark = c.UberID
                    };
            DTO.ReturnJasonConstruct<List<DTO.Gift>> giftList = new DTO.ReturnJasonConstruct<List<DTO.Gift>>();
            giftList.status = (int)DTO.executeStatus.success;
            giftList.DTOObject = new List<DTO.Gift>();
            foreach (var item in q)
            {
                string remark = "";
                if (item.GiftName.IndexOf("优步") != -1)
                {
                    var uberObj = db.UBer.SingleOrDefault(p => p.ID == item.remark);
                    if (uberObj != null)
                        remark = "优惠码：" + uberObj.ExchangeNumber.ToString();
                }
                //DTO.ReturnJasonConstruct<List<DTO.Gift>> dto = new DTO.ReturnJasonConstruct<List<DTO.Gift>>();
                DTO.Gift dtoObject = new DTO.Gift();
                dtoObject.giftName = item.GiftName;
                dtoObject.giftDescribtion = item.GiftDescribtion;
                dtoObject.remark = remark;

                giftList.DTOObject.Add(dtoObject);
            }

            return giftList;
        }

        // POST: api/Gift
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Gift/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Gift/5
        public void Delete(int id)
        {
        }
    }
}
