using System;
using System.Collections.Generic;

namespace GameUtils
{
    public static class ListExtension
    {
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

        public static T Place<T>(this IList<T> self, T newItem)
        {
            self.Add(newItem);
            return newItem;
        }

        public static T FromEnd<T>(this IList<T> self, int index)
        {
            return self[self.Count - (index + 1)];
        }
    }
}