using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORISEVICH.Domain.Entities
{
    public class CartItem
    {
        public Book Item { get; set; }
        public int Qty { get; set; }
    }
}
