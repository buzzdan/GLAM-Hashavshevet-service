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
            ProductsCollection myObject;

            using (TextReader reader = new StreamReader(XmlFilePath))
            {
                var serializer = new XmlSerializer(typeof(ProductsCollection));
                myObject = (ProductsCollection)serializer.Deserialize(reader);
            }

            return myObject.Products;
        }
    }
}