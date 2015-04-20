using GlamHashAdapter.Entities;
using System;

namespace GlamHashAdapter.DAL
{
    public interface IClientsReader
    {
        Client[] ReadClients();
    }

    public interface IReader<T>
    {
        T[] Read();
    }
}