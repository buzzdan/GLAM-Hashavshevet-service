using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.Entities
{
    public class Discount
    {
        public double Sum { get; set; }

        public DiscountType DiscountType { get; set; }
    }
}