using System;
using System.Collections.Generic;

namespace OracleWebCenterExporter.Extensions
{
    /// <summary>
    /// Oh the joys of syntactic sugar
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Simple foreach wrapper
        /// </summary>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
