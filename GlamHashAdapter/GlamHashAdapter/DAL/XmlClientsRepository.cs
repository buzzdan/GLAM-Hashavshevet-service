using GlamHashAdapter.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GlamHashAdapter.DAL
{
    public class XmlClientsRepository : GlamHashAdapter.DAL.IClientsReader
    {
        private string clientsXmlFilePath;

        public XmlClientsRepository(string clientsXmlFilePath)
        {
            this.clientsXmlFilePath = clientsXmlFilePath;
        }

        public Client[] ReadClients()
        {
            ClientsCollection myObject;

            //// Construct an instance of the XmlSerializer with the type
            //// of object that is being deserialized.
            //var mySerializer = new XmlSerializer(typeof(ClientsCollection));

            //// To read the file, create a FileStream.
            //var myFileStream = new FileStream(clientsXmlFilePath, FileMode.Open);

            //// Call the Deserialize method and cast to the object type.
            //myObject = (ClientsCollection)mySerializer.Deserialize(myFileStream);

            var lines = File.ReadAllLines(clientsXmlFilePath);
            lines = lines.Where(line => !(line.Contains("<Balance>") || line.Contains("<Maam>") || line.Contains("<SortingCode>"))).ToArray();
            var xml = string.Join("\n", lines);
            var stream = GenerateStreamFromString(xml);

            //using (TextReader reader = new StreamReader(clientsXmlFilePath))
            using (TextReader reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(ClientsCollection));
                myObject = (ClientsCollection)serializer.Deserialize(reader);
            }

            return myObject.Clients;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}