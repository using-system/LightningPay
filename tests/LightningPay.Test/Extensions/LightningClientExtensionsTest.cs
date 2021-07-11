using System;

using Xunit;
using NSubstitute;

namespace LightningPay.Test
{
    public class LightningClientExtensionsTest
    {
        [Fact]
        public void ToRestClient_Should_Return_RestClient_If_Source_Is_RestClient()
        {
            ILightningClient client = Substitute.For<IRestLightningClient>();

            IRestLightningClient target = client.ToRestClient();

            Assert.NotNull(target);
            Assert.IsAssignableFrom<IRestLightningClient>(target);
        }

        [Fact]
        public void ToRestClient_Should_Throw_ArgumentException_If_Source_Is_Null()
        {
            ILightningClient client = null;

            Assert.Throws<ArgumentException>(() => client.ToRestClient());
        }

        [Fact]
        public void ToRestClient_Should_Throw_ArgumentException_If_Source_Is_Null2()
        {
            ILightningClient client = Substitute.For<ILightningClient>();

            Assert.Throws<ArgumentException>(() => client.ToRestClient());
        }
    }
}
