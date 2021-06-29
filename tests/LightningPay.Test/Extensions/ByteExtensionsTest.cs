using System;

using Xunit;

namespace LightningPay.Test
{
    public class ByteExtensionsTest
    {
        [Fact]
        public void ToBitString_Should_Return_Null_If_Source_Is_Null()
        {
            //Arrange
            byte[] source = null;

            //Act
            var actual = source.ToBitString();

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void ToBitString_Should_Remove_Dashes()
        {
            //Arrange
            byte[] source = { 1, 2, 4, 8, 16, 32 };

            //Act
            var actual = source.ToBitString();

            //Assert
            Assert.Equal("010204081020", actual);
        }

        [Fact]
        public void HexStringToByteArray_Should_Return_ByteArray()
        {
            //Arrange
            string hexString = "971a5512";

            //Act
            var actual = hexString.HexStringToByteArray();

            //Assert
            Assert.Equal(new byte[] { 151, 26, 85, 18 } , actual);
        }

        [Fact]
        public void HexStringToByteArray_Should_Throw_FormatException_If_Source_Bad_Format()
        {
            //Arrange
            string hexString = "971t5z";

            //Act & Assert
            Assert.Throws<FormatException>(() =>  hexString.HexStringToByteArray());
        }
    }
}
