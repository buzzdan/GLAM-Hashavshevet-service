using System;

namespace GlamHashAdapter.DAL
{
    public interface IOrdersRepository
    {
        GlamHashAdapter.AutoOrderFeeder.OrderDetails GetNextPendingOrder();

        void UpdateOrderStatus(int orderID, bool succeeded);
    }
}