using System;

using Xunit;

namespace LightningPay.Test
{
    public class UriExtensionsTest
    {
        [Fact]
        public void ToBaseUrl_Should_Return_Null_If_Source_Is_Empty()
        {
            //Arrange
            Uri uri = null;

            //Act
            var actual = uri.ToBaseUrl();

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void ToBaseUrl_Should_Trim_EndSlash_If_Exists()
        {
            //Arrange
            Uri uri = new Uri("http://localhost:42802/");

            //Act
            var actual = uri.ToBaseUrl();

            //Assert
            Assert.Equal("http://localhost:42802", actual);
        }

        [Fact]
        public void ToBaseUrl_Should_Trim_DoubleEndSlash_If_Exists()
        {
            //Arrange
            Uri uri = new Uri("http://localhost:42802//");

            //Act
            var actual = uri.ToBaseUrl();

            //Assert
            Assert.Equal("http://localhost:42802", actual);
        }

        [Fact]
        public void ToBaseUrl_Should_Return_Exact_Uri_If_No_EndSlash()
        {
            //Arrange
            Uri uri = new Uri("http://localhost:42802");

            //Act
            var actual = uri.ToBaseUrl();

            //Assert
            Assert.Equal("http://localhost:42802", actual);
        }
    }
}
