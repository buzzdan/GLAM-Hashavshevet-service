using GlamHashAdapter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            Items = new List<ItemDetails>();
        }

        public int OrderID { get; set; }

        public int ClientID { get; set; }

        public Discount DiscountPercentage { get; set; }

        public string Comment { get; set; }

        public List<ItemDetails> Items { get; set; }
    }
}