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

        Dictionary<string, StockItem> _stockListDict;
        Dictionary<string, SpecialPriceRule> _specialPriceRuleDict;

        [TestFixtureSetUp]
        public void Init()
        {
            _stockListDict = new Dictionary<string, StockItem>();
            _stockListDict.Add("A", new StockItem("A", 50));
            _stockListDict.Add("B", new StockItem("B", 30));
            _stockListDict.Add("C", new StockItem("C", 20));
            _stockListDict.Add("D", new StockItem("D", 15));

            _specialPriceRuleDict = new Dictionary<string, SpecialPriceRule>();
            _specialPriceRuleDict.Add("A", new SpecialPriceRule("A", 3, 130));
            _specialPriceRuleDict.Add("B", new SpecialPriceRule("B", 2, 45));
        }

        [Test]
        [TestCase(new[] { "A", "B" }, 80)]
        [TestCase(new[] { "B", "C", "D" }, 65)]
        [TestCase(new[] { "B", "A", "B" }, 110)]
        [TestCase(new[] { "A", "A", "A" }, 150)]
        public void GetTotalPrice_WhenGetBasicPriceMultipleItems_MustBeCorrect(string[] skuList, int totalPrice)
        {
            //arrange
            var repository = A.Fake<IStockRepository>();
            foreach (var key in _stockListDict.Keys)
            {
                A.CallTo(() => repository.GetStockItem(key)).Returns(_stockListDict[key]);
                A.CallTo(() => repository.GetSpecialPrice(A<string>.Ignored)).Returns(null);
            }
            var sut = new Checkout(repository);

            //act
            foreach (var sku in skuList)
            {
                sut.Scan(sku);
            }
            var result = sut.GetTotalPrice();

            //assert
            Assert.AreEqual(totalPrice, result);
        }

        [Test]
        [TestCase(new[] { "A", "B", "A" }, 130, TestName = "Special price quantity not reached")]
        [TestCase(new[] { "A", "A", "A" }, 130, TestName = "Special price quantity reached for 'A'")]
        public void GetTotalPrice_WhenSpecialPricesIncluded_MustBeCorrect(string[] skuList, int totalPrice)
        {
            //arrange
            var repository = A.Fake<IStockRepository>();
            foreach (var key in _stockListDict.Keys)
            {
                A.CallTo(() => repository.GetStockItem(key)).Returns(_stockListDict[key]);
                var specialPrice = _specialPriceRuleDict.ContainsKey(key)
                    ? _specialPriceRuleDict[key]
                    : null;
                A.CallTo(() => repository.GetSpecialPrice(A<string>.Ignored)).Returns(specialPrice);
            }
            var sut = new Checkout(repository);

            //act
            foreach (var sku in skuList)
            {
                sut.Scan(sku);
            }
            var result = sut.GetTotalPrice();

            //assert
            Assert.AreEqual(totalPrice, result);
        }
    }
}
