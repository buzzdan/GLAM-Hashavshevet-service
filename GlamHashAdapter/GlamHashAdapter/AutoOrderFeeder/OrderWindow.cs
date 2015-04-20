using GlamHashAdapter.AutoOrderFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems.WindowItems;

namespace GlamHashAdapter
{
    public class OrderWindow : AppScreen, GlamHashAdapter.AutoOrderFeeder.IHashavshevetOrdersService
    {
        private Point submitButtonCordinates = new Point(725, 784);
        private Point discountTextboxCordinates = new Point(535, 778);

        protected OrderWindow(Window hashavshevetWindow, ScreenRepository repo)
            : base(hashavshevetWindow, repo)
        {
        }

        public bool InsertNewOrder(OrderDetails order)
        {
            try
            {
                this.Focus();
                EnterClientNumber(order.ClientID);
                foreach (var item in order.Items)
                {
                    InsertItemDetails(item.ItemID, item.Quantity);
                }
                EnterOrderDiscount(order.DiscountPercentage.Sum);
                PressSubmitButton();
                Thread.Sleep(TimeSpan.FromSeconds(3));

                //SaveOrderOutput(order);
                //Window.WaitWhileBusy();

                //TODO:
                //We should get an answer from the screen that says if it succeeded or failed
                //then we should return the correct answer
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EnterOrderDiscount(double orderDiscountPrecentage)
        {
            Window.Mouse.Location = discountTextboxCordinates;
            Window.Mouse.Click();
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DELETE, 4);
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.BACKSPACE, 4);
            Window.Enter(orderDiscountPrecentage.ToString());
            Window.WaitWhileBusy();
        }

        private void SaveOrderOutput(OrderDetails order)
        {
            string outputFilePath = string.Format(@"C:\Dumper\order{0}.oxps", order.OrderID);

            var printModal = Window.ModalWindows()[0];
            printModal.Focus();

            printModal.WaitWhileBusy();
            printModal.Enter(@"C:\Dumper\");

            printModal.WaitWhileBusy();
            printModal.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            printModal.Enter(outputFilePath);
            printModal.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            //Thread.Sleep(TimeSpan.FromSeconds(1));

            /*
            printModal.Mouse.Location = new Point(1499, 971);
            printModal.WaitWhileBusy();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            printModal.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            printModal.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            printModal.Mouse.Location = new Point(1499, 971);
            printModal.Click();
             */
        }

        private void InsertItemDetails(string itemNumber, int quntity)
        {
            Window.Enter(itemNumber);
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.TAB, 2);
            Window.Enter(quntity.ToString());
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.TAB, 5);
            Window.WaitWhileBusy();
        }

        private void EnterClientNumber(int clientNumber)
        {
            Window.Enter(clientNumber.ToString());
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            Window.WaitWhileBusy();
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            Window.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.TAB, 9);
            Window.WaitWhileBusy();
        }

        public static OrderWindow StartNewOrder(Window hashavshevetWindow, ScreenRepository repo)
        {
            hashavshevetWindow.WaitWhileBusy();
            var hanala = hashavshevetWindow.MenuBar.MenuItem("מסמכים");
            hanala.Click();
            hashavshevetWindow.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DOWN);
            hashavshevetWindow.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            hashavshevetWindow.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.UP, 3);
            hashavshevetWindow.Press(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);

            return new OrderWindow(hashavshevetWindow, repo);
        }

        public override void Focus(DisplayState displayState)
        {
            Window.WaitWhileBusy();
            base.Focus(displayState);
            Window.WaitWhileBusy();
        }

        private void PressSubmitButton()
        {
            //Window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.ALT);
            Window.Mouse.Location = submitButtonCordinates;
            Window.Mouse.Click();
            Window.WaitWhileBusy();
        }
    }
}