using System;

using Xunit;

namespace LightningPay.Test
{
    public class InvoiceExtensionsTest
    {
        [Fact]
        public void ToExpiryString_Should_Return_Default_Value_If_Invoice_Params_Is_Null()
        {
            //Arrange
            CreateInvoiceOptions options = null;

            //Act
            var actual = options.ToExpiryString();

            //Assert
            Assert.Equal(Constants.INVOICE_DEFAULT_EXPIRY.TotalSeconds.ToString(), actual);
        }

        [Fact]
        public void ToExpiryString_Should_Return_Default_Value_If_Expiry_Property_Is_Null()
        {
            //Arrange
            CreateInvoiceOptions options = new CreateInvoiceOptions();

            //Act
            var actual = options.ToExpiryString();

            //Assert
            Assert.Equal(Constants.INVOICE_DEFAULT_EXPIRY.TotalSeconds.ToString(), actual);
        }

        [Fact]
        public void ToExpiryString_Should_Return_TimeSpan_Value_In_Seconds()
        {
            //Arrange
            var expiry = TimeSpan.FromMinutes(5);
            CreateInvoiceOptions options = new CreateInvoiceOptions(expiry: expiry);

            //Act
            var actual = options.ToExpiryString();

            //Assert
            Assert.Equal(expiry.TotalSeconds.ToString(), actual);
        }

        [Fact]
        public void ToExpiryDate_Should_Return_Default_Value_If_Invoice_Params_Is_Null()
        {
            //Arrange
            CreateInvoiceOptions options = null;

            //Act
            var actual = options.ToExpiryDate();

            //Assert
            Assert.True(actual > DateTimeOffset.UtcNow);
        }


        [Fact]
        public void ToExpiryDate_Should_Use_TimeSpan_If_Expiry_Property_Is_Not_Null()
        {
            //Arrange
            var expiry = TimeSpan.FromMinutes(5);
            CreateInvoiceOptions options = new CreateInvoiceOptions(expiry: expiry);

            //Act
            var actual = options.ToExpiryDate();

            //Assert
            Assert.True(actual < DateTimeOffset.UtcNow.AddMinutes(10));
        }
    }
}
