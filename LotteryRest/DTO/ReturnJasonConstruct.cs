using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryRest.DTO
{
    public enum executeStatus
    {
        success = 0,
        faild = 1,
        warning = 2
    }
    //0，成功
    //1，失败
    //2，执行成功，业务失败带有返回信息
    public class ReturnJasonConstruct<T>
    {
        public int status;
        public string information;
        public T DTOObject;
    }
}
