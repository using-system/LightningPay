using System.Linq;

using Xunit;

namespace LightningPay.Test
{
    public class MoneyTest
    {
        [Fact]
        public void Test_Money_Conversions()
        {
            Assert.Equal(Money.FromSatoshis(1), Money.FromMilliSatoshis(1000));
            Assert.Equal(new Money(1, MoneyUnit.BTC), new Money(1000, MoneyUnit.MilliBTC));
            Assert.Equal(new Money(1, MoneyUnit.BTC), Money.FromSatoshis(100000000));

            Money money = Money.FromSatoshis(1);
            Assert.Equal(1000, money.ToUnit(MoneyUnit.MilliSatoshi));
            Assert.Equal(1, money.ToSatoshis());
            Assert.Equal(0.00000001M, money.ToBTC());
        }

        [Fact]
        public void Test_Money_Operators()
        {
            Assert.True(Money.FromSatoshis(1) == Money.FromMilliSatoshis(1000));
            Assert.True(Money.FromSatoshis(1) != Money.FromMilliSatoshis(1));
            Assert.True(Money.FromSatoshis(1000) > Money.FromMilliSatoshis(1000));
            Assert.True(Money.FromSatoshis(1) >= Money.FromMilliSatoshis(1000));
            Assert.True(Money.FromMilliSatoshis(1000) < Money.FromSatoshis(1000));
            Assert.True(Money.FromMilliSatoshis(1000) <= Money.FromSatoshis(1));
        }

        [Fact]
        public void Test_Money_Sorting()
        {
            var moneys = new Money[]
            {
                Money.FromSatoshis(1),
                Money.FromMilliSatoshis(1),
                new Money(1, MoneyUnit.BTC),
            };

            moneys = moneys.OrderBy(m => m).ToArray();

            Assert.Equal(Money.FromMilliSatoshis(1), moneys[0]);
            Assert.Equal(Money.FromSatoshis(1), moneys[1]);
            Assert.Equal(new Money(1, MoneyUnit.BTC), moneys[2]);

        }
    }
}
