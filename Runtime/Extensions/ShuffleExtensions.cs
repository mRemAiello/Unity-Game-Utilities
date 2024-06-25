using System;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils
{
    public static class ShuffleExtensions
    {
        public static void Shuffle<T>(this T[] source)
        {
            int n = source.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (source[k], source[n]) = (source[n], source[k]);
            }
        }

        public static void Shuffle<T>(this List<T> source)
        {
            int n = source.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (source[k], source[n]) = (source[n], source[k]);
            }
        }

        public static IDictionary<T1, T2> Shuffle<T1, T2>(this IDictionary<T1, T2> source)
        {
            var keys = source.Keys.ToArray();
            var values = source.Values.ToArray();

            int n = source.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (keys[k], keys[n]) = (keys[n], keys[k]);
                (values[k], values[n]) = (values[n], values[k]);
            }

            return MakeDictionary(keys, values);
        }

        public static T Random<T>(this IList<T> list)
        {
            Random rng = new();
            return list[rng.Next(list.Count)];
        }

        public static T First<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(list.Count < elementsCount ? list.Count : elementsCount).ToList();
        }
    }
}
