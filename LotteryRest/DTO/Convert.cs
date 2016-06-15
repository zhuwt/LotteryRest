using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryRest.DTO
{
    public static class ConvertDTO
    {
        private static string[] level = new string[3] { "护牙骑士", "护牙骑士长", "护牙军团长" };
        public static DTO.User ToDTO(this Models.User user)
        {
            return new DTO.User
            {
                id = user.ID,
                name = user.UserName,
                tel = user.Tel,
                exp = user.Exp,
                level = ConvertDTO.GetLevelName(user.Level)
            };
        }
        /////////////////////private function/////////////////////////
        private static string GetLevelName(int index)
        {
            return level[index];
        }

        public static int GetLevelInt(string levelString)
        {
            for (int n = 0; n < level.Length; n++)
            {
                if (level[n] == levelString)
                    return n;
            }
            return 0;
        }
    }
}
