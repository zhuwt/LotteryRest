using System;
using System.Collections.Generic;

public class RandomController
{
    #region Member Variables
    //待随机抽取数据集合
    public List<char> datas = new List<char>(
        new char[]{
            'A','B','C','D','E','F',
            'G','H','I','J','K','L',
            'M','N','O','P','Q','R',
            'S','T','U','V','W','X',
            'Y','Z'
    });

    //权值
    public List<ushort> weights = new List<ushort>(
        new ushort[]{
            1,2,3,4,5,6,
            7,8,9,0,1,1,
            1,1,1,1,1,1,
            1,1,1,1,1,1,
            1,1
    });
    #endregion

    #region Contructors
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="count">随机抽取个数</param>
    public RandomController(ushort count)
    {
        if (count > 26)
            throw new Exception("抽取个数不能超过数据集合大小！！");

        _Count = count;
    }
    #endregion
    #region Method
    #region 普通随机抽取
    /// <summary>
    /// 随机抽取
    /// </summary>
    /// <param name="rand">随机数生成器</param>
    /// <returns></returns>
    public char[] RandomExtract(Random rand)
    {
        List<char> result = new List<char>();
        if (rand != null)
        {
            for (int i = Count; i > 0;)
            {
                char item = datas[rand.Next(25)];
                if (result.Contains(item))
                    continue;
                else
                {
                    result.Add(item);
                    i--;
                }
            }
        }

        return result.ToArray();
    }
    #endregion
    #region 受控随机抽取
    /// <summary>
    /// 随机抽取
    /// </summary>
    /// <param name="rand">随机数生成器</param>
    /// <returns></returns>
    public char[] ControllerRandomExtract(Random rand)
    {
        List<char> result = new List<char>();
        if (rand != null)
        {
            //临时变量
            Dictionary<char, int> dict = new Dictionary<char, int>(26);
            //为每个项算一个随机数并乘以相应的权值
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                dict.Add(datas[i], rand.Next(100) * weights[i]);
            }
            //排序
            List<KeyValuePair<char, int>> listDict = SortByValue(dict);
            //拷贝抽取权值最大的前Count项
            foreach (KeyValuePair<char, int> kvp in listDict.GetRange(0, Count))
            {
                result.Add(kvp.Key);
            }
        }

        return result.ToArray();
    }
    #endregion
    #region Tools
    /// <summary>
    /// 排序集合
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    private List<KeyValuePair<char, int>> SortByValue(Dictionary<char, int> dict)
    {
        List<KeyValuePair<char, int>> list = new List<KeyValuePair<char, int>>();
        if (dict != null)
        {
            list.AddRange(dict);
            list.Sort(
              delegate (KeyValuePair<char, int> kvp1, KeyValuePair<char, int> kvp2)
              {
                  return kvp2.Value - kvp1.Value;
              });
        }

        return list;
    }
    #endregion
    #endregion
    #region Properties
    private int _Count;
    /// <summary>
    /// 随机抽取个数
    /// </summary>
    public int Count
    {
        get
        {
            return _Count;
        }
        set
        {
            _Count = value;
        }
    }
    #endregion
}