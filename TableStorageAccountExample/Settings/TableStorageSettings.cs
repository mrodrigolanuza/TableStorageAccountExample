using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableStorageAccountExample.Settings
{
    //Clase que pretende tipar la información ubicada en el fichero appsettings.json, en la parte que recoge las claves y parámetros de acceso al
    //recurso en Azure Storage Account y dentro de éste, a la Table concreta.
    public class TableStorageSettings
    {
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string TableName { get; }

        public TableStorageSettings(string storageAccount, string storageKey, string tableName) {

            if (!string.IsNullOrEmpty(storageAccount) && !string.IsNullOrEmpty(storageKey) && !string.IsNullOrEmpty(tableName)) {
                StorageAccount = storageAccount;
                StorageKey = storageKey;
                TableName = tableName;
            }
        }
    }
}
