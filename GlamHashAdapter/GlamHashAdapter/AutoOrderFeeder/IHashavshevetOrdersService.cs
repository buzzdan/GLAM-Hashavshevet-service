using System;
using TestStack.White.UIItems.WindowItems;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public interface IHashavshevetOrdersService
    {
        bool InsertNewOrder(OrderDetails order);
    }
}