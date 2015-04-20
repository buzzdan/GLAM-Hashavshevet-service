using GlamHashAdapter.Entities;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GlamHashAdapter.DAL
{
    public class XmlProductsRepository : IReader<Product>
    {
        private string XmlFilePath;

        public XmlProductsRepository(string XmlFilePath)
        {
            this.XmlFilePath = XmlFilePath;
        }

        public Product[] Read()
        {
            Console.Write("Reading from xml...");
            ProductsCollection myObject;
            var lines = File.ReadAllLines(XmlFilePath);

            var xml = string.Join("\n", lines);
            xml = xml.Replace("NONAME>", "Items>")
                .Replace("Row>", "Item>")
                .Replace("שם_פריט", "ItemName")
                .Replace("מפתח_פריט", "ItemID")
                .Replace("מחיר_מכירה", "ItemPrice")
                .CleanInvalidXmlChars();

            var stream = GenerateStreamFromString(xml);
            using (TextReader reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(ProductsCollection));
                myObject = (ProductsCollection)serializer.Deserialize(reader);
            }
            Console.WriteLine(" Done!");

            return myObject.Products;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}