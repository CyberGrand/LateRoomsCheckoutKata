using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private IStockRepository _stockRepository;
        private List<StockItem> _stockItemList = new List<StockItem>();

        public Checkout(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public int GetTotalPrice()
        {
            var priceList = new List<PriceBase>();
            var grouped = _stockItemList.GroupBy(x => x.Sku);
            var totalPrice = 0;

            foreach (var group in grouped)
            {
                var specialPrice = _stockRepository.GetSpecialPrice(group.Key);

                if (specialPrice == null)
                {
                    totalPrice += group.Sum(x => x.Price);
                }
                else
                {
                    totalPrice += CalculateSpecialPrice(specialPrice, group.First(), group.Count());
                }
            }

            return totalPrice;
        }


        private int CalculateSpecialPrice(SpecialPriceRule specialPrice, StockItem stockItem, int stockItemQuantity)
        {
            var specialQuantity = stockItemQuantity / specialPrice.RequiredQuantity;
            var remainder = stockItemQuantity % specialPrice.RequiredQuantity;

            return specialPrice.Price * specialQuantity + stockItem.Price * remainder;
        }

        public void Scan(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new InvalidSkuException();
            }

            var stockItem = _stockRepository.GetStockItem(sku);

            _stockItemList.Add(stockItem);
        }
    }
}
