using System;

namespace LightningPay
{
    /// <summary>
    ///   Extension methods for Uri
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>Converts to baseurl without / at the end.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///  The base url cleared
        /// </returns>
        public static string ToBaseUrl(this Uri source)
        {
            return source?.ToString().TrimEnd('/');
        }
    }
}
