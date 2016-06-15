using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryRest.DTO
{
    public class User
    {
        public Guid id;
        public string name;
        public string tel;
        public string password;
        public int exp;
        public string level;
    }
}