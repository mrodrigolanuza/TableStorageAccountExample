using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableStorageAccountExample.Models
{
    //La clase Log va a ser la entidad o modelo que se almacenará en la Table Storage en Azure.
    //Para ello, debemos hacerla heredar de TableEntity para poder indicar que propiedades van a corresponderse con PartitionKey y RowKey.
    //LongTail Pattern: En un table storage almacena los registros en orden ascendente en base a la RowKey (Ticks o milisegundos desde 1900). Así, al ser ascendente, lo registros más antiguos se mostrarán primero (ticks menor). Para evitar esto, utilizaremos los reverseticks para que se muestren primero los registros más recientes.
    public class Log : TableEntity
    {
        public string Message { get; set; }

        public Log(string applicationUserId, string reverseTicks) {
            this.PartitionKey = applicationUserId;                  //Indicamos quien será la partition key y la row key.
            this.RowKey = reverseTicks;
        }
        
        //Constructor vacío obligatorio para poder utilizar la inyección de independencias desde el Startup (todavía no hay info de los parámetros para el otro constructor)
        public Log() {

        }
    }
}
