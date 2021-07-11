using System;
using System.Globalization;

namespace LightningPay
{
    /// <summary>
    ///   Byte extension methods
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>Converts to bitstring.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static string ToBitString(this byte[] source)
        {
            if(source == null)
            {
                return null;
            }

            return BitConverter.ToString(source)
                .Replace("-", "")
                .ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>Hexadecimals string to byte array.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   Return data in byte array
        /// </returns>
        public static byte[] HexStringToByteArray(this String source)
        {
            if(source == null)
            {
                return null;
            }

            source = source.Replace(":", string.Empty);

            int NumberChars = source.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
            return bytes;
        }
    }
}
