using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableStorageAccountExample.Settings;

namespace TableStorageAccountExample.Services
{
    public class AzureTableStorage<T> : IAzureTableStorage<T> where T : TableEntity, new()
    {
        private TableStorageSettings settings;

        public AzureTableStorage(TableStorageSettings settings) {
            this.settings = settings;
        }

        //Método privado que se va a encargar de obtener la referencia a la TableStorage en Azure según las settings indicadas.
        //Lo utilizaremos en cada uno de los métodos publicos.
        private async Task<CloudTable> GetTableAsync() {
            //Storage Account
            var storageAccount = new CloudStorageAccount(new StorageCredentials(this.settings.StorageAccount, this.settings.StorageKey), false); //Indicamos "useHttps = false", ya que estamos trabajando en local. Además en Azure, en la configuration de la StorageAccount indicaremos "Secure transfer required" = Disabled mientras no estemos en Producción, si no dará problemas.
            //Client para el tipo de servicio de storage
            var tableClient = storageAccount.CreateCloudTableClient(); //Podemos elegir un cliente para Table, Blob, Queue o File
            //Table 
            var table = tableClient.GetTableReference(this.settings.TableName);
            await table.CreateIfNotExistsAsync();
            //Devolver referencia al recurso tabla
            return table;
        }

        public async Task Delete(string partitionKey, string rowKey) {
            //Item
            T item = await GetItem(partitionKey, rowKey);
            //Table
            var table = await GetTableAsync();
            //Operation delete
            var operation = TableOperation.Delete(item);
            //Execute operation
            await table.ExecuteAsync(operation);
        }

        public async Task<T> GetItem(string partitionKey, string rowKey) {
            //Table
            var table = await GetTableAsync();
            //Operation retrieve
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            //Execute operation
            var result = await table.ExecuteAsync(operation);
            //Devolver resultados casteado al tipo T en cuestión.
            return (T)(dynamic)result.Result;
        }

        public async Task<List<T>> GetList() {
            //Table
            var table = await GetTableAsync();
            //Query and Results objects 
            var query = new TableQuery<T>();
            var results = new List<T>();
            //Execute query (not operation)
            TableContinuationToken tableContinuationToken = null; //Indicará cuando ya no hay más datos que leer
            do {
                var queryResults = await table.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            }
            while (tableContinuationToken != null);
            return results;
        }

        public async Task<List<T>> GetList(string partitionKey) {
            //Table
            var table = await GetTableAsync();
            //Query and Results objects 
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            var results = new List<T>();
            //Execute query (not operation)
            TableContinuationToken tableContinuationToken = null; //Indicará cuando ya no hay más datos que leer
            do {
                var queryResults = await table.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
                tableContinuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            }
            while (tableContinuationToken != null);
            return results;
        }

        public async Task Insert(T item) {
            //Table
            var table = await GetTableAsync();
            //Operation insert
            var operation = TableOperation.Insert(item); //Si existe lanzará error
            //Execute operation
            await table.ExecuteAsync(operation);
        }

        public async Task Update(T item) {
            //Table
            var table = await GetTableAsync();
            //Operation insert/replace
            var operation = TableOperation.InsertOrReplace(item); //Si no existe lo creará
            //Execute operation
            await table.ExecuteAsync(operation);
        }
    }
}
