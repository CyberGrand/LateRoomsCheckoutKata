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
            var repository = A.Fake<IStockRepository>();
            ICheckout checkout = new Checkout(repository);

            //act + assert
            Assert.Throws<InvalidSkuException>(() => checkout.Scan(sku));
        }

        [Test]
        public void Scan_WhenSkuIsMissing_ShouldNotCallStockRepositoryGetStockItemMethod(
            [ValuesAttribute("", null, " ")] string sku)
        {
            //arrange
            var repository = A.Fake<IStockRepository>();
            ICheckout checkout = new Checkout(repository);

            //act
            try
            {
                checkout.Scan(sku);
            }
            catch
            {     
            }

            //assert
            A.CallTo(() => repository.GetStockItem(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void Scan_WhenSkuIsMissing_ShouldNotCallStockRepositoryGetSpecialPrice(
            [ValuesAttribute("", null, " ")] string sku)
        {
            //arrange
            var repository = A.Fake<IStockRepository>();
            ICheckout checkout = new Checkout(repository);

            //act
            try
            {
                checkout.Scan(sku);
            }
            catch
            {
            }

            //assert
            A.CallTo(() => repository.GetSpecialPrice(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        public void GetTotalPrice_WhenGetBasicPriceSingleItem_MustBeCorrect(string sku, int unitPrice)
        {
            //arrange
            var repository = A.Fake<IStockRepository>();
            A.CallTo(() => repository.GetStockItem(sku)).Returns(new StockItem(sku, unitPrice));
            var sut = new Checkout(repository);

            //act
            sut.Scan(sku);
            var result = sut.GetTotalPrice();

            //assert
            Assert.AreEqual(unitPrice, result);
        }
    }
}
