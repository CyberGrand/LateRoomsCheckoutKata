using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public abstract class PriceBase
    {
        public PriceBase(string sku, int price)
        {
            Sku = sku;
            Price = price;
        }
        public string Sku { get; private set; }
        public int Price { get; private set; }
    }
}
