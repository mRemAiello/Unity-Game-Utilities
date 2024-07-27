using System;
using System.Collections.Generic;

namespace GameUtils
{
    public static class ListExtension
    {
        public static T Pop<T>(this List<T> list, int index)
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
    }
}