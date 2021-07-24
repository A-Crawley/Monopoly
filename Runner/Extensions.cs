using System;
using System.Collections.Generic;
using System.Linq;

namespace Runner
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);
        }

        public static IEnumerable<Tout> ForEach<Tin, Tout>(this IEnumerable<Tin> @this, Func<Tout> func) 
            => @this.Select(item => func());

        public static IEnumerable<Tout> ForEach<Tin, Tout>(this IEnumerable<Tin> @this, Func<Tin, Tout> func)
            => @this.Select(func);
    }
}