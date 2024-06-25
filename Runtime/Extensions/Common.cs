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

        public static int GetNumberInAString(this string str)
        {
            try
            {
                var getNumb = Regex.Match(str, @"\d+").Value;
                return Int32.Parse(getNumb);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}