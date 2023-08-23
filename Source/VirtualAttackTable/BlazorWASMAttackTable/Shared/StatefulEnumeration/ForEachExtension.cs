using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMAttackTable.Shared.StatefulEnumeration
{
    public static class ForEachExtension
    {
        public static SideEffectEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            return new(source.ForEachInternal(action));
        }

        public static SideEffectEnumerable<T> ForEach<T>(this SideEffectEnumerable<T> source, Action<T> action)
        {
            return source.Enumerable.ForEach(action);
        }

        private static IEnumerable<T> ForEachInternal<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);

                yield return item;
            }
        }

        public static SideEffectEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action, int startIndex = 0, int increment = 1)
        {
            return new(source.ForEachInternal(action, startIndex, increment));
        }

        public static SideEffectEnumerable<T> ForEach<T>(this SideEffectEnumerable<T> source, Action<T, int> action, int startIndex = 0, int increment = 1)
        {
            return new(source.Enumerable.ForEachInternal(action, startIndex, increment));
        }

        private static IEnumerable<T> ForEachInternal<T>(this IEnumerable<T> source, Action<T, int> action, int startIndex = 0, int increment = 1)
        {
            foreach ((T item, int index) item in source.WithIndex(startIndex, increment))
            {
                action(item.item, item.index);

                yield return item.item;
            }
        }
    }
}
