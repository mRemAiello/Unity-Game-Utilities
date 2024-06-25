using System;
using System.Text.RegularExpressions;

namespace GameUtils
{
    public static class Common
    {
        public static bool IsInteger(this float value)
        {
            return value == (int)value;
        }

        public static int GetNumberInString(this string str)
        {
            try
            {
                var getNumb = Regex.Match(str, @"\d+").Value;
                return int.Parse(getNumb);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}