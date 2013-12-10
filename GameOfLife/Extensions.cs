using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public static class Extensions
    {
        public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }
    }
}
