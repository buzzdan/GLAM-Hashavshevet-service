using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GlamHashAdapter.Entities
{
    [XmlRootAttribute("Clients")]
    public class ClientsCollection
    {
        [XmlElement("Client")]
        public Client[] Clients { get; set; }
    }

    [XmlRootAttribute("Items")]
    public class ProductsCollection
    {
        [XmlElement("Item")]
        public Product[] Products { get; set; }
    }
}