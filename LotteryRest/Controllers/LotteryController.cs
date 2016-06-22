using LotteryRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LotteryRest.DTO;

namespace LotteryRest.Controllers
{
    public class LotteryController : ApiController
    {
        const int expIncrease = 10;
        const int maxExp = 100;
        const int levelMax = 2;
        const int position = 45;
        //
        const string defaultGiftName = "谢谢惠顾";
        const int lotteryCount = 8;
        // GET: api/Lottery
        [HttpGet]
        public ReturnJasonConstruct<DTO.RollLettory> Get(Guid userId)
        {
            try
            {
                DateTime dt = DateTime.Now;
                ReturnJasonConstruct<DTO.RollLettory> dto = new ReturnJasonConstruct<DTO.RollLettory>();
                DateTime startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                DateTime endTime = new DateTime(dt.Year, dt.Month, dt.Day + 1, 0, 0, 0);
                UserInformationEntities db = new UserInformationEntities();

                //获取默认奖项
                LotteryGift defaultGift = null;
                if (db.LotteryGift.Where(p => p.GiftName == defaultGiftName).Count() == 0)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "无法获取默认奖项";
                    return dto;
                }
                defaultGift = db.LotteryGift.Single(p => p.GiftName == defaultGiftName);

                //检查抽奖概率基数
                LotteryGift selectGift = null;
                var items = db.LotteryGift.AsQueryable().Where(p => !p.Expired).OrderBy(p => p.ID).ToList();
                if (items.Count() == 0)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "没有抽奖项目.";
                    return dto;
                }
                if (items.Count() != lotteryCount)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "抽奖项目必须为8个.";
                    return dto;
                }

                //初始化奖品信息
                RollLottery rl = new RollLottery();
                foreach (var item in items)
                {
                    rl.SetGift(item.GiftName, item.Probability);
                }
                //首先检查是否有指定的礼物，如果有，直接返回。
                var gift = db.LotteryGift.Where(p => p.Owner == userId && p.Num > 0).ToList();
                if (gift.Count() > 0)
                {
                    selectGift = gift[0];
                    dto.status = (int)executeStatus.success;
                    dto.DTOObject = SaveLottery(userId, ref db, ref selectGift, ref defaultGift, ref rl);
                    return dto;
                }

                //var lotteryObjects = db.lottery1.Where(p => p.UserID == userId && p.LotteryDate >= startTime && p.LotteryDate <= endTime).ToList();
                ////检查当天有没有抽过奖。如果抽过奖，直接跳过。
                //if (!(lotteryObjects.Count == 0 || (lotteryObjects.Count == 1 && lotteryObjects[0].Share)))
                //{
                //    dto.status = (int)executeStatus.warning;
                //    dto.information = "今天已经抽过奖了.";
                //    return dto;
                //}

                rl.GetProbability();
                long index = rl.GetGiftIndex(ref WebApiApplication.randomSeed);
                var giftObject = items[(int)index];

                selectGift = giftObject;
                dto.status = (int)executeStatus.success;
                dto.DTOObject = SaveLottery(userId, ref db, ref selectGift, ref defaultGift, ref rl);
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DTO.RollLettory SaveLottery(Guid userId,ref UserInformationEntities db, ref LotteryGift gift,ref LotteryGift defaultGift, ref RollLottery rl)
        {
            var DTO = new DTO.RollLettory();
            lottery1 lottery = new lottery1();
            lottery.UserID = userId;
            lottery.LotteryDate = DateTime.Now;
            lottery.Share = false;
            lottery.Receive = false;
            if (gift.Num == 0)
            {
                lottery.LotteryGiftID = defaultGift.ID;
                DTO.rotate = rl.GetGiftByGiftName(defaultGift.GiftName)* position + 22;
                DTO.results = defaultGift.GiftName;
                //记录DTO数据
                DTO.giftName = defaultGift.GiftName;
                DTO.giftDes = defaultGift.GiftDescribtion;
            }
            else
            {
                //如果包含Uber信息
                //--优步劵没有了，就给默认奖品，即谢谢惠顾
                //--如果优步劵还有，就指定优步劵并且关联优步优惠劵
                //如果不是优步，就直接减少奖品数量，并且关联奖品
                if (gift.GiftName.Contains("优步") || gift.GiftName.Contains("Uber"))
                {
                    if (db.UBer.Where(p => !p.Use).Count() <= 0)
                    {
                        lottery.LotteryGiftID = defaultGift.ID;
                        DTO.rotate = rl.GetGiftByGiftName(defaultGift.GiftName) * position + 22;
                        DTO.results = defaultGift.GiftName;
                        //记录DTO数据
                        DTO.giftName = defaultGift.GiftName;
                        DTO.giftDes = defaultGift.GiftDescribtion;
                    }
                    else
                    {
                        lottery.LotteryGiftID = gift.ID;
                        DTO.rotate = rl.GetGiftByGiftName(gift.GiftName) * position + 22;
                        DTO.results = gift.GiftName;
                        var UberObj = db.UBer.First(p => !p.Use);
                        lottery.UberID = UberObj.ID;
                        UberObj.Use = true;
                        //记录DTO数据
                        DTO.giftName = gift.GiftName;
                        DTO.giftDes = gift.GiftDescribtion;
                        DTO.remark = UberObj.ExchangeNumber;
                    }
                }
                else
                {
                    lottery.LotteryGiftID = gift.ID;
                    gift.Num--;
                    //记录DTO数据
                    DTO.rotate = rl.GetGiftByGiftName(gift.GiftName) * position + 22;
                    DTO.results = gift.GiftName;
                    DTO.giftName = gift.GiftName;
                    DTO.giftDes = gift.GiftDescribtion;
                }
            }
            db.lottery1.Add(lottery);
            DTO.user = GetExp(userId, ref db);
            db.SaveChanges();
            return DTO;
        }

        private DTO.User GetExp(Guid userId,ref UserInformationEntities db)
        {
            if (db.User.Where(p => p.ID == userId).Count() == 0)
                throw new Exception("没有指定ID的用户信息.");

            try
            {
                var result = db.User.Single(p => p.ID == userId);
                result.Exp += expIncrease;
                if (result.Exp >= maxExp)
                {
                    if (result.Level == levelMax)
                    {
                        result.Exp = maxExp;
                    }
                    else
                    {
                        result.Exp = 0;
                        result.Level++;
                    }
                }

                return result.ToDTO();
            }
            catch
            {
                throw new Exception("设置用户经验时出错.");
            }
        }
    }
}
