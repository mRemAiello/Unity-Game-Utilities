using System.Collections.Generic;

namespace GameUtils
{
    public static class NullOrEmptyExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this T[] source)
        {
            return source == null || source.Length == 0;
        }

        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            return source == null || source.Keys.Count == 0;
        }
    }
}