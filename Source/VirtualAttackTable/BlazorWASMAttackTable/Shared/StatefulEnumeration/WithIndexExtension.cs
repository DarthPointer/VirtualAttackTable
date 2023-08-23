using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMAttackTable.Shared.StatefulEnumeration
{
    public static class WithIndexExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source, int startIndex = 0, int increment = 1)
        {
            int currentIndex = startIndex;

            foreach (T item in source)
            {
                yield return (item, currentIndex);

                currentIndex += increment;
            }
        }
    }
}
