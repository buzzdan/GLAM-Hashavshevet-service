using GlamHashAdapter.Entities;
using System;

namespace GlamHashAdapter.DAL
{
    public interface IClientsInserter
    {
        void InsertClients(Client[] clients);
    }

    public interface IInserter<T>
    {
        void Insert(T[] clients);
    }
}