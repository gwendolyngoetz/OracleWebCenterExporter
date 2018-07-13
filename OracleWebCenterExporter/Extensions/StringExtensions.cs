using System;
using System.Linq;

namespace OracleWebCenterExporter.Extensions
{
    public static class StringExtensions
    {     
        /// <summary>
        /// Determines if this string and the comparison string have the same value ignoring case.
        /// </summary>
        public static bool EqualsInsensitive(this string source, string value)
        {
            if (source == null && value == null)
            {
                return true;
            }

            if (source == null)
            {
                return false;
            }

            return source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Wraps string.IsNullOrWhiteSpace(...) to make life simpler, much much simpler.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Check if string is in list of possible strings
        /// </summary>
        public static bool In(this string source, params string[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Contains(source);
        }
    }
}
