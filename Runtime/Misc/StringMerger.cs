using System;

namespace GameUtils
{
    public class StringMerger
    {
        public static string AppendPrefix(string message, string prefix)
        {
            return string.Format("[{0}] {1}", prefix, message);
        }

        public static string AppendTimeAndPrefix(string message, string prefix)
        {
            return string.Format("[{0}{2}] {1}", prefix, message, DateTime.Now.TimeOfDay.ToString());
        }
    }
}