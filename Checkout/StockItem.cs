using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public class StockItem : PriceBase
    {
        public StockItem(string sku, int price) : base(sku, price)
        {
        }
    }
}
