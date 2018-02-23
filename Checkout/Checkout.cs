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

        public Checkout(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public int GetTotalPrice()
        {
            return 0;
        }

        public void Scan(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new InvalidSkuException();
            }
        }
    }
}
