using GlamHashAdapter;
using GlamHashAdapter.AutoOrderFeeder;
using GlamHashAdapter.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace GlamHashAdapter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //ImportClientsToHashavshevet();
            // ImportProductsToHashavshevet();
            Console.WriteLine("\n\nInitializing GLAM Hashavshevet Adpter service...\n\n");
            string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True";

            var sqlOrdersRepository = new SqlOrdersRepository(connectionString);

            Console.WriteLine("\n\nOpening Hashavshevet...\n\n");

            var hashApp = HashavshevetApp.LaunchHashavshevet();

            Console.WriteLine("\n\nOpening orders window...\n\n");
            var orderWindow = hashApp.GetNewOrderWindow();
            orderWindow.Focus(DisplayState.Maximized);

            var orderFeeder = new OrderFeederCordinator(sqlOrdersRepository, orderWindow);
            var timeBetweenOrders = TimeSpan.FromSeconds(15);

            Console.WriteLine("\n\nPolling for new orders from DB...\n\n");
            orderFeeder.RunFeederPolling(timeBetweenOrders);
        }

        private static void ImportProductsToHashavshevet()
        {
            var xmlPath = Environment.CurrentDirectory + @"\..\..\DBScripts\";
            string XmlFilePath = xmlPath + "Items.xml";
            string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True";

            var xmlClientsRepository = new DAL.XmlProductsRepository(XmlFilePath);
            var clientRepo = new SqlProductRepository(connectionString);
            var ClientsImporter = new HashToGlamImporter<Product>(xmlClientsRepository, clientRepo);

            ClientsImporter.ImportClients();
        }

        private static void ImportClientsToHashavshevet()
        {
            var xmlPath = Environment.CurrentDirectory + @"\..\..\DBScripts\";
            string clientsXmlFilePath = xmlPath + "Clients2.xml";
            string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True";

            var xmlClientsRepository = new DAL.XmlClientsRepository(clientsXmlFilePath);
            var clientRepo = new SqlClientsRepository(connectionString);
            var ClientsImporter = new HashToGlamImporter(xmlClientsRepository, clientRepo);

            ClientsImporter.ImportClients();
        }
    }
}
