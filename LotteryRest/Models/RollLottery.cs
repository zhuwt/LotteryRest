using Probability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryRest.Models
{
    public class RollLottery
    {
        int n = 0;
        long basicNumber = 0;   //概率基数
        List<KeyValuePair<long, double>> elements = new List<KeyValuePair<long, double>>();
        Dictionary<long, string> prize = new Dictionary<long, string>();
        //public int GetGiftIndex
        public int GetGiftByGiftName(string gift)
        {
            var item = prize.First(p => p.Value == gift);
            return (int)item.Key;
        }

        public void SetGift(string gift, double probability)
        {
            elements.Add(new KeyValuePair<long, double>(n, probability));
            prize.Add(n, gift);
            n++;
        }

        public void GetProbability()
        {
            double[] array = new double[elements.Count];
            int m = 0;
            foreach (KeyValuePair<long, double> item in elements)
            {
                array[m] = item.Value;
                m++;
            }

            basicNumber = ToolMethods.GetBaseNumber(array);

            //判断设置的概率  
            double allRate = 0;
            foreach (var item in elements)
            {
                allRate += item.Value;
            }

            string temp = allRate.ToString();
            allRate = double.Parse(temp);
            if (allRate.CompareTo(1.0) != 0)
            {
                throw new Exception("概率设置错误，请检查概率数值.");
            }
        }

        public long GetGiftIndex(ref Random seed)
        {
            long selectedElement = 0;
            long diceRoll = ToolMethods.GetRandomNumber(seed, 1, basicNumber);
            long cumulative = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                cumulative += (long)(elements[i].Value * basicNumber);
                if (diceRoll <= cumulative)
                {
                    selectedElement = elements[i].Key;
                    break;
                }
            }
            return selectedElement;
        }

        public void Clear()
        {
            n = 0;
            prize.Clear();
            elements.Clear();
        }
    }
}