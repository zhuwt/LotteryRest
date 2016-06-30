using LotteryRest.DTO;
using LotteryRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteryRest.Controllers
{
    public class UserController : ApiController
    {
        //private static string[] level = new string[3] { "护牙骑士", "护牙骑士长", "护牙军团长" };

        // GET: api/User
        //[HttpGet]
        //public string Get()
        //{
        //    return "true";
        //}

        [HttpGet]
        public ReturnJasonConstruct<DTO.User> Get(string tel, string password)
        {
            try
            {
                ReturnJasonConstruct<DTO.User> dto = new ReturnJasonConstruct<DTO.User>();
                UserInformationEntities db = new UserInformationEntities();
                if (db.User.Where(p => p.Tel == tel && p.password == password).Count() == 0)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "没有该用户或用户名密码不对，请检查键入的用户信息.";
                    return dto;
                }

                var result = db.User.Single(p => p.Tel == tel && p.password == password);
                dto.status = (int)executeStatus.success;
                dto.DTOObject = result.ToDTO();
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/User/5
        [HttpGet]
        public ReturnJasonConstruct<DTO.User> Get(Guid id)
        {
            try
            {
                ReturnJasonConstruct<DTO.User> dto = new ReturnJasonConstruct<DTO.User>();
                UserInformationEntities db = new UserInformationEntities();
                if (db.User.Where(p => p.ID == id).Count() == 0)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "没有该用户或用户名密码不对，请检查键入的用户信息.";
                    return dto;
                }

                var result = db.User.Single(p => p.ID == id);
                dto.status = (int)executeStatus.success;
                dto.DTOObject = result.ToDTO();
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: /user
        //jason:
        //{
        //  "name": "小明",
        //  "tel": "18509264675",
        //  "password": "123456789"
        //}
        [HttpPost]
        public ReturnJasonConstruct<DTO.User> Post([FromBody]DTO.User user)
        {
            try
            {
                ReturnJasonConstruct<DTO.User> dto = new ReturnJasonConstruct<DTO.User>();
                if (CheckObjectAvalible(user, ref dto) == false)
                    return dto;

                UserInformationEntities db = new UserInformationEntities();
                var result = db.User.Where(p => p.Tel == user.tel);
                if (result.Count() > 0)
                {
                    dto.status = (int)executeStatus.warning;
                    dto.information = "该电话号码已经被注册.";
                    return dto;
                }

                Models.User ac = new Models.User();
                ac.ID = Guid.NewGuid();
                ac.UserName = user.name;
                ac.Tel = user.tel;
                ac.Exp = 0;
                ac.Level = 0;
                ac.password = user.password;
                ac.CreateDate = DateTime.Now;
                db.User.Add(ac);
                db.SaveChanges();

                dto.status = (int)executeStatus.success;
                dto.DTOObject = ac.ToDTO();
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/User/5
        [HttpPut]
        [Route("user")]
        public ReturnJasonConstruct<DTO.User> Put([FromBody]DTO.User user)
        {
            try
            {
                ReturnJasonConstruct<DTO.User> dto = new ReturnJasonConstruct<DTO.User>();
                if (CheckObjectAvalible(user, ref dto) == false)
                    return dto;

                UserInformationEntities db = new UserInformationEntities();
                if (user.id.ToString() == "" || db.User.Where(p => p.ID == user.id).Count() == 0)
                {//创建用户
                    var result = db.User.Where(p => p.Tel == user.tel);
                    if (result.Count() > 0)
                    {
                        dto.status = (int)executeStatus.warning;
                        dto.information = "该电话号码已经被注册.";
                        return dto;
                    }

                    Models.User ac = new Models.User();
                    ac.ID = Guid.NewGuid();
                    ac.UserName = user.name;
                    ac.Tel = user.tel;
                    ac.Exp = 0;
                    ac.Level = 0;
                    ac.password = user.password;
                    ac.CreateDate = DateTime.Now;
                    db.User.Add(ac);
                    dto.DTOObject = ac.ToDTO();
                }
                else//更新用户
                {
                    var result = db.User.Single(p => p.ID == user.id);
                    result.UserName = user.name;
                    result.Tel = user.tel;
                    result.password = user.password;
                    result.Exp = user.exp;
                    result.Level = ConvertDTO.GetLevelInt(user.level);
                    dto.DTOObject = result.ToDTO();
                }
                db.SaveChanges();
                dto.status = (int)executeStatus.success;
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("user/share/")]
        public ReturnJasonConstruct<DTO.User> SetUserShare([FromBody]DTO.User user)
        {
            DateTime dt = DateTime.Now;
            DateTime startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            dt = dt.AddDays(1);
            DateTime endTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            UserInformationEntities db = new UserInformationEntities();
            var lotteryObjects = db.lottery1.Where(p => p.UserID == user.id && p.LotteryDate >= startTime && p.LotteryDate <= endTime).ToList();

            ReturnJasonConstruct<DTO.User> dto = new ReturnJasonConstruct<DTO.User>();
            //检查当天有没有抽过奖。如果抽过奖，直接跳过。
            if (lotteryObjects.Count == 0)
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "今天还没有抽奖";
            }
            else if (lotteryObjects.Count == 1 && !lotteryObjects[0].Share)
            {
                lotteryObjects[0].Share = true;
                db.SaveChanges();
                dto.status = (int)executeStatus.success;
                dto.information = "恭喜您获得额外抽奖一次的机会";
            }
            else
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "今天分享获得额外抽奖的次数已经满额，请明天再继续分享.";
            }

            return dto;
        }

        //[HttpPut]
        //public DTO.User GetExp(Guid userId)
        //{
        //    UserInformationEntities db = new UserInformationEntities();
        //    if (db.User.Where(p => p.ID == userId).Count() == 0)
        //        throw new Exception("没有指定ID的用户信息.");

        //    try
        //    {
        //        var result = db.User.Single(p => p.ID == userId);
        //        if (result.Exp == (levelupLimit - expIncrease))
        //        {
        //            result.Exp = 0;
        //            if (result.Level != levelMax)
        //                result.Level++;
        //        }
        //        else
        //        {
        //            result.Exp += expIncrease;
        //        }

        //        db.SaveChanges();
        //        return new DTO.User
        //        {
        //            Id = result.ID,
        //            name = result.UserName,
        //            tel = result.Tel,
        //            exp = result.Exp,
        //            level = GetLevelName(result.Level)
        //        };
        //    }
        //    catch
        //    {
        //        throw new Exception("设置用户经验时出错.");
        //    }
        //}

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }

        private bool CheckObjectAvalible(DTO.User user,ref ReturnJasonConstruct<DTO.User> dto)
        {
            if (user.name.Length > 20)
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "用户名称不能大于20个中英文字符.";
                return false;
            }

            if (user.tel.Length != 11)
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "手机号码必须为11位.";
                return false;
            }

            if (user.password == null)
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "用户密码不能为空.";
                return false;
            }

            if (user.password.Length < 6 || user.password.Length > 20)
            {
                dto.status = (int)executeStatus.warning;
                dto.information = "用户密码必须大于6位小于20位.";
                return false;
            }

            return true;
        }
    }
}
