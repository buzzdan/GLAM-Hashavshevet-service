using GlamHashAdapter.AutoOrderFeeder;
using GlamHashAdapter.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && (UserIsAskingForHelp(args) ||
                args.Length != 3) ||
                !ValidParams(args))
            {
                PrintHelp();
                return;
            }


            var importedItemType = args[0].Trim().ToLower();
            var xmlFilePath = args[1].Trim();
            var connectionString = args[2].Trim();

            Console.WriteLine("Running Glam Importer...\n\n");
            Console.WriteLine("importedItemType: " + importedItemType);
            Console.WriteLine("xmlFilePath: " + xmlFilePath);
            Console.WriteLine("connectionString: " + connectionString);
            Console.WriteLine("\n");

            switch (importedItemType)
            {
                case "products":
                    ImportProductsToHashavshevet(xmlFilePath, connectionString);
                    break;
                case "clients":
                    ImportClientsToHashavshevet(xmlFilePath, connectionString);
                    break;
            }

            Console.WriteLine("\nGlam importer is done. Good Bye.\n");
            
        }

        private static bool ValidParams(string[] args)
        {
            var importedItemType = args[0].Trim().ToLower();
            var xmlFilePath = args[1].Trim();
            var connectionString = args[2].Trim();

            if (importedItemType != "products" && importedItemType != "clients")
            {
                return false;
            }
            if (!File.Exists(xmlFilePath))
            {
                return false;
            }
            var connection = new SqlConnection(connectionString);
            if (!connection.TestConnection())
            {
                return false;
            }
            return true;
        }

        private static bool UserIsAskingForHelp(string[] args)
        {
            return args[0] == "/?" || args[0] == "-?" || args[0] == "?" || args[0] == "-help";
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Instructions:\nEnter the imported type, the xml file name or path and a DB connection string.");
            Console.WriteLine("imported type can be: 'products' or 'clients'");
            Console.WriteLine("for example:\n\nGlamImporter.exe \"products\" \"products.xml\" \"Data Source=DAN\\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True\"");
        }
        private static void ImportProductsToHashavshevet(string xmlFilePath, string connectionString)
        {
            /*var xmlPath = Environment.CurrentDirectory + @"\..\..\DBScripts\";
            string XmlFilePath = xmlPath + "Items.xml";
            string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True";*/

            var xmlClientsRepository = new XmlProductsRepository(xmlFilePath);
            var clientRepo = new SqlProductRepository(connectionString);
            var ClientsImporter = new HashToGlamImporter<Product>(xmlClientsRepository, clientRepo);

            ClientsImporter.ImportClients();
        }

        private static void ImportClientsToHashavshevet(string xmlFilePath, string connectionString)
        {
            /*var xmlPath = Environment.CurrentDirectory + @"\..\..\DBScripts\";
            string clientsXmlFilePath = xmlPath + "Clients2.xml";
            string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Integrated Security=True";*/

           
            var xmlClientsRepository = new XmlClientsRepository(xmlFilePath);
            var clientRepo = new SqlClientsRepository(connectionString);
            var ClientsImporter = new HashToGlamImporter(xmlClientsRepository, clientRepo);

            ClientsImporter.ImportClients();
        }
    }
}
