using System;
using System.Collections.Generic;

namespace WordsToNumbers
{
    static class Extensions
    {
        /// <summary>
        /// Creates a shallow copy of a list by start and end indicies (exclusive).
        /// </summary>
        /// <typeparam name="T">List element type.</typeparam>
        /// <param name="list">The list to copy.</param>
        /// <param name="startIndex">The index of <paramref name="list"/> to start the copy.</param>
        /// <param name="endIndex">The index of <paramref name="list"/> to stop copying (not included in resulting list).</param>
        /// <returns>A new list from [startIndex, endIndex).</returns>
        public static List<T> GetRangeByIndices<T>(this List<T> list, int startIndex, int endIndex)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (endIndex < startIndex)
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            var count = Math.Max(0, endIndex - startIndex);
            return list.GetRange(startIndex, count);
        }

        public static List<T> GetRangeToEnd<T>(this List<T> list, int startIndex)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            var count = Math.Max(0, list.Count - startIndex);
            startIndex = Math.Min(startIndex, list.Count - 1);
            return list.GetRange(startIndex, count);
        }
    }
}
