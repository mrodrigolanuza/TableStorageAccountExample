using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableStorageAccountExample.Services
{
    //Interface de uso genérico de cualquier TableStorage en Azure.
    //El tipo genérico T indica el tipo de entidad o molelo que va a manejar, el cual hereda de TableEntity que aporta el manejo 
    //de las claves de acceso a los datos (Partition Key y Row Key).
    public interface IAzureTableStorage<T> where T : TableEntity, new()
    {
        Task<List<T>> GetList();
        Task<List<T>> GetList(string partitionKey);
        Task<T> GetItem(string partitionKey, string rowKey);
        Task Insert(T item);
        Task Update(T item);
        Task Delete(string partitionKey, string rowKey);
    }
}
