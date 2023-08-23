using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMAttackTable.Shared.StatefulEnumeration
{
    /// <summary>
    /// This wrap is designed to pass IEnumerables but "tag" them as ones that enumerating these WILL have side effects for user code
    /// and require an extra step of property access to build further LINQ chains.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SideEffectEnumerable<T>
    {
        /// <summary>
        /// The underlying IEnumerable. Do not expose it directly to user code.
        /// The intent of making it public is that one could build LINQ chains with side-effect nodes in the middle while being sure no external clueless code could
        /// receive it. If you really want external code call the enumerable for side-effects, please keep it wrapped and return this <see cref="SideEffectEnumerable{T}"/> instead.
        /// </summary>
        public IEnumerable<T> Enumerable { get; }

        public SideEffectEnumerable(IEnumerable<T> enumerable)
        {
            Enumerable = enumerable;
        }
    }
}
