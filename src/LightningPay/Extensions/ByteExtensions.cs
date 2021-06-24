using System;
using System.Globalization;

namespace LightningPay
{
    internal static class ByteExtensions
    {
        public static string ToBitString(this byte[] source)
        {
            return BitConverter.ToString(source)
                .Replace("-", "")
                .ToLower(CultureInfo.InvariantCulture);
        }
    }
}
