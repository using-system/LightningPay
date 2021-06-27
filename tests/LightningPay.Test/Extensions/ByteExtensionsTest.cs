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
    }
}
