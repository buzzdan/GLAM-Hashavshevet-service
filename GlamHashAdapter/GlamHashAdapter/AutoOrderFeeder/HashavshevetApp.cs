using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TestStack.White;
using TestStack.White.ScreenObjects;

namespace GlamHashAdapter
{
    public class HashavshevetApp
    {
        private Application application;
        private ScreenRepository screenRepository;

        public static HashavshevetApp LaunchHashavshevet()
        {
            var app = new HashavshevetApp();
            ProcessStartInfo process = new ProcessStartInfo(@"C:\WizSoft\Hashavshevet\has.exe");
            app.application = Application.AttachOrLaunch(process);
            app.screenRepository = new ScreenRepository(app.application.ApplicationSession);
            app.application.WaitWhileBusy();
            return app;
        }

        public OrderWindow GetNewOrderWindow()
        {
            application.WaitWhileBusy();
            var hashWindow = application.GetWindows()[0];
            hashWindow.WaitWhileBusy();
            var orderWindow = OrderWindow.StartNewOrder(hashWindow, screenRepository);
            orderWindow.Focus(TestStack.White.UIItems.WindowItems.DisplayState.Maximized);
            return orderWindow;
        }

        public void Kill()
        {
            application.Kill();
        }
    }
}