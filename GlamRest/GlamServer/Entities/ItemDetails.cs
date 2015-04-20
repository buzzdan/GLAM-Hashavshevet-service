using GlamServer.Entities;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class ItemDetails
    {
        public string ItemID { get; set; }

        public int Quantity { get; set; }

        public double DiscountInShekels { get; set; }

        public Product ProductDetails { get; set; }
    }
}