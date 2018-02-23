using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public class SpecialPrice : PriceBase
    {
        public SpecialPrice(string sku, int requiredQuantity, int price) : base(sku, price)
        {
            RequiredQuantity = requiredQuantity;
        }

        public int RequiredQuantity { get; private set; }
    }
}
