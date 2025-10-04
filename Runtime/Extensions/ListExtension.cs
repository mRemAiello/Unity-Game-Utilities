using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameUtils
{
    public static class ListExtension
    {
        public static T RandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");

            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T RandomItemRemove<T>(this IList<T> list)
        {
            var item = list.RandomItem();
            list.Remove(item);
            return item;
        }

        public static T Pop<T>(this IList<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null or empty.");
            }

            if (index < 0 || index >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds.");
            }

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static T Get<T>(this IList<T> list, int index)
        {
            if (list.TryGet(index, out var item))
            {
                return item;
            }

            return default;
        }

        public static bool TryGet<T>(this IList<T> list, int index, out T item)
        {
            if (list.Count > 0 && index >= 0 && index < list.Count)
            {
                item = list[index];
                return true;
            }

            //
            item = default;
            return false;
        }

        public static bool ContainsAll<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Items cannot be null.");
            }

            var comparer = EqualityComparer<T>.Default;

            foreach (var item in items)
            {
                var contains = false;
                for (var i = 0; i < list.Count; i++)
                {
                    if (comparer.Equals(list[i], item))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }

        public static T Append<T>(this IList<T> self, T newItem)
        {
            self.Add(newItem);
            return newItem;
        }

        public static T FromEnd<T>(this IList<T> self, int index)
        {
            return self[self.Count - (index + 1)];
        }

        public static void InsertBefore<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item);
            list.Insert(targetPosition, newItem);
        }

        public static void InsertAfter<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item) + 1;
            list.Insert(targetPosition, newItem);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            for (var i = 0; i <= n - 2; i++)
            {
                var rdn = Random.Range(0, n - i);
                (list[i + rdn], list[i]) = (list[i], list[i + rdn]);
            }
        }

        public static void Print<T>(this IList<T> list, string log = "")
        {
            log += "[";
            for (var i = 0; i < list.Count; i++)
            {
                log += list[i].ToString();
                log += i != list.Count - 1 ? ", " : "]";
            }

            Debug.Log(log);
        }
    }
}