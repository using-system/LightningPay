using System;

namespace LightningPay
{
    internal static class UriExtensions
    {
        internal static string ToBaseUrl(this Uri source)
        {
            return source.ToString().TrimEnd('/');
        }
    }
}
