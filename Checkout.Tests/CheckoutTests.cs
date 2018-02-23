using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace Checkout.Tests
{
    [TestFixture]
    public class CheckoutTests
    {
        [Test]
        public void Scan_WhenSkuIsMissing_ShouldThrowException(
            [ValuesAttribute("", null, " ")] string sku)
        {
            //arrange
            var respository = A.Fake<IStockRepository>();
            ICheckout checkout = new Checkout(respository);

            //act + assert
            Assert.Throws<InvalidSkuException>(() => checkout.Scan(sku));
        }
       
    }
}
