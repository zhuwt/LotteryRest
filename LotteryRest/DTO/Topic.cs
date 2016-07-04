using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryRest.DTO
{
    public class Topic
    {
        public string topic;
        public List<Choice> choices;
    }

    public class Choice
    {
        public string choice;
        public int goal;
    }
}