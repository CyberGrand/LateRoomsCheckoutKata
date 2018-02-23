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
            throw new NotImplementedException();
        }

        public void Scan(string sku)
        {
            
        }
    }
}
