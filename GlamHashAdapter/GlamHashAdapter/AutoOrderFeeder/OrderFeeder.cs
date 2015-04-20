using GlamHashAdapter.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestStack.White.UIItems.WindowItems;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class OrderFeederCordinator
    {
        private IOrdersRepository ordersRepository;
        private IHashavshevetOrdersService hashavshevetOrdersService;
        private bool runPolling;

        public OrderFeederCordinator(IOrdersRepository ordersRepository, IHashavshevetOrdersService hashavshevetOrdersService)
        {
            this.ordersRepository = ordersRepository;
            this.hashavshevetOrdersService = hashavshevetOrdersService;
        }

        public void RunFeederPolling(TimeSpan timeBetweenPolls)
        {
            runPolling = true;
            while (runPolling)
            {
                FeedNewOrdersToHashavshevet();
                Thread.Sleep(timeBetweenPolls);
            }
        }

        public void StopPolling()
        {
            runPolling = false;
        }

        public void FeedNewOrdersToHashavshevet()
        {
            var nextOrderToProcess = ordersRepository.GetNextPendingOrder();
            while (nextOrderToProcess != null)
            {
                //insert into hashvshevet
                Console.WriteLine("\n\nInserting new order to hashvshevet");
                bool orderInsertedSuccessfully = hashavshevetOrdersService.InsertNewOrder(nextOrderToProcess);

                //update order status
                Console.WriteLine("\nUpdating Status");
                ordersRepository.UpdateOrderStatus(nextOrderToProcess.OrderID, orderInsertedSuccessfully);
                Console.WriteLine("\nDone!");

                //Get next one
                nextOrderToProcess = ordersRepository.GetNextPendingOrder();
            }
        }
    }
}