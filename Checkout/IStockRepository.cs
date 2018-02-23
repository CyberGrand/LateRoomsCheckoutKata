using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public interface IStockRepository
    {
        StockItem GetStockItem(string sku);
        SpecialPriceRule GetSpecialPrice(string sku);
    }
}
