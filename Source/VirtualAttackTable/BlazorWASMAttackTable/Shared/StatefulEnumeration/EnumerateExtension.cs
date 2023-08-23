using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMAttackTable.Shared.StatefulEnumeration
{
    public static class EnumerateExtension
    {
        public static void Enumerate<T>(this SideEffectEnumerable<T> query)
        {
            foreach (T item in query.Enumerable) { }
        }
    }
}
