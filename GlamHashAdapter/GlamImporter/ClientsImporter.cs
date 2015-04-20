using GlamHashAdapter.DAL;
using GlamServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlamHashAdapter.AutoOrderFeeder
{
    public class HashToGlamImporter
    {
        private IClientsReader clientsReader;
        private IClientsInserter clientsInserter;

        public HashToGlamImporter(IClientsReader clientsReader, IClientsInserter clientsInserter)
        {
            this.clientsReader = clientsReader;
            this.clientsInserter = clientsInserter;
        }

        public void ImportClients()
        {
            var clients = clientsReader.ReadClients();
            clientsInserter.InsertClients(clients);
        }
    }

    public class HashToGlamImporter<T>
    {
        private IReader<T> reader;
        private IInserter<T> inserter;

        public HashToGlamImporter(IReader<T> reader, IInserter<T> inserter)
        {
            this.reader = reader;
            this.inserter = inserter;
        }

        public void ImportClients()
        {
            var items = reader.Read();

            inserter.Insert(items);
        }
    }
}