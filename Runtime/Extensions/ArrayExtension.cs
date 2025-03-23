using System;

namespace GameUtils
{
    public static class ArrayExtensions
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (action == null) throw new ArgumentNullException(nameof(action));

            // Itera ogni elemento dell'array ed esegue l'azione specificata
            foreach (var item in array)
            {
                action(item);
            }
        }
    }
}