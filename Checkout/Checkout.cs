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
            return _stockItemList.Sum(s => s.Price);
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
