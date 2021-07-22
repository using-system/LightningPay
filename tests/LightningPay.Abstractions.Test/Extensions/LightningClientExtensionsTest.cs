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

            ILightningClient target = client.ToRestClient();

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
        public void ToRestClient_Should_Throw_ArgumentException_If_Source_Is_RestClient()
        {
            ILightningClient client = Substitute.For<ILightningClient>();

            Assert.Throws<ArgumentException>(() => client.ToRestClient());
        }


        [Fact]
        public void ToRpcClient_Should_Return_RpcClient_If_Source_Is_RpcClient()
        {
            ILightningClient client = Substitute.For<IRpcLightningClient>();

            ILightningClient target = client.ToRpcClient();

            Assert.NotNull(target);
            Assert.IsAssignableFrom<IRpcLightningClient>(target);
        }

        [Fact]
        public void ToRpcClient_Should_Throw_ArgumentException_If_Source_Is_Null()
        {
            ILightningClient client = null;

            Assert.Throws<ArgumentException>(() => client.ToRpcClient());
        }

        [Fact]
        public void ToRpcClient_Should_Throw_ArgumentException_If_Source_Is_Not_RpcClient()
        {
            ILightningClient client = Substitute.For<ILightningClient>();

            Assert.Throws<ArgumentException>(() => client.ToRpcClient());
        }
    }
}
