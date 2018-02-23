using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    public interface ICheckout
    {
        void Scan(string sku);
        int GetTotalPrice();
    }
}
