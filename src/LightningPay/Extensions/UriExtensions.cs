using System;

namespace LightningPay
{
    /// <summary>
    ///   Extension methods for Uri
    /// </summary>
    internal static class UriExtensions
    {
        /// <summary>Converts to baseurl without / at the end.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///  The base url cleared
        /// </returns>
        internal static string ToBaseUrl(this Uri source)
        {
            return source?.ToString().TrimEnd('/');
        }
    }
}
