using GlamHashAdapter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class ItemDetails
    {
        public string ItemID { get; set; }

        public int Quantity { get; set; }

        public Discount Discount { get; set; }
    }
}