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

            IDictionary<T1, T2> result = new Dictionary<T1, T2>();
            for (var i = 0; i < keys.Length; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        public static T PickRandom<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Length == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Length)];
        }

        public static T PickRandom<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Count == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        public static (T, int) PickRandomAndIndex<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            int index = UnityEngine.Random.Range(0, collection.Length);
            return collection.Length == 0 ? (default, -1) : (collection[index], index);
        }

        public static (T, int) PickRandomWithIndex<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var index = UnityEngine.Random.Range(0, collection.Count);
            return collection.Count == 0 ? (default, -1) : (collection[index], index);
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(list.Count < elementsCount ? list.Count : elementsCount).ToList();
        }

        public static T First<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }
    }
}
