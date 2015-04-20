using GlamServer.Entities;
using System;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class OrderDetails
    {
        public int OrderID { get; set; }

        public int ClientID { get; set; }

        public double DiscountPercentage { get; set; }

        public DateTime OrderDate { get; set; }

        public string Comment { get; set; }

        public Status Status { get; set; }

        public ItemDetails[] Items { get; set; }
    }
}