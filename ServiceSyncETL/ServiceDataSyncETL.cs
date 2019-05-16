using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;


namespace ServiceSyncETL
{
    /// <summary>
    /// Implementa el Cliente que invoca al servicio web de sincronizacion de los movimientos en linea
    /// </summary>
    public class ServiceDataSyncETL
    {
        public Timer timerTask;
        public AutoResetEvent autoEvent;
        public bool stopped;

        public int milisecondsDelay { get; set; }
        public int milisecondsInterval { get; set; }

        private const string URL_DEBUG = "http://localhost:59056/syncMovimientos/";
        private const string URL_LOCAL_PRODUCTION = "http://localhost/WebServicesETL/syncMovimientos/";

        public CancellationTokenSource cancellationTokenSource;

        public enum TIPO_OPERACION_SYNC
        {
            INGRESO_X_RECEPCION=0,
            SALIDA_X_VENTA=1,
            INGRESO_X_MOV_INTERNO=2,
            SALIDA_X_MOV_INTERNO=3,
            INGRESO_X_TRANSFERENCIA=4,
            SALIDA_X_TRANSFERENCIA=5,   
            INGRESO_X_DEMASIA=6,
            SALIDA_X_MERMA=7
        }

        public ServiceDataSyncETL(int milisecondsDelay=1000,int milisecondsInterval = Timeout.Infinite)
        {
            this.stopped = true;
            this.milisecondsDelay = milisecondsDelay;
            this.milisecondsInterval = milisecondsInterval;   
        }

        private void onElapsedTime(object state)
        {
            // logica de la sincronizacion
            if (!stopped)
            {
                timerTask.Change(Timeout.Infinite, Timeout.Infinite);
                syncUpAllMovs();
                timerTask.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
                //timerTask.Change(milisecondsDelay, 10000);
            }
        }

        public void start()
        {
            stopped = false;
            TimerCallback timerCallback = new TimerCallback(onElapsedTime);
            timerTask = new Timer(timerCallback, null, milisecondsDelay, milisecondsInterval);
        }

        public void stop()
        {
            stopped = true;
        }


        public void syncUpAllMovs()
        {
            
            Array tipoOperationArray = Enum.GetValues(typeof(TIPO_OPERACION_SYNC));
            foreach (var itemTipoOp in tipoOperationArray)
            {
                syncUpOperation(itemTipoOp);
            }
            
        }

        private void syncUpOperation(object itemTipoOp)
        {
            try
            {
                var client = new RestClient(URL_DEBUG);
                var request = new RestRequest(Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddParameter(new Parameter("tipoOperacion", (int)itemTipoOp, ParameterType.GetOrPost));
                request.Timeout = Timeout.Infinite;
                //string strJsonRequestHTTP = JsonConvert.SerializeObject(movDel);
                //request.AddParameter("application/json", strJsonRequestHTTP, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                // OBTENER LA RESPUESTA DEL SERVIDOR
                var content = response.Content;
                // DEJAR LOG DE INSERCCION DE DATOS (ARCHIVO TXT)
                //return respuestaDel;
                //respuestaDel.OResultado!=null?respuestaDel.OResultado.ToString():"no hay objeto";
                //textBox4.Text = response.Content;
                /* elminar el id de respuesta (idintegracion) devuelto por la API de la ANH en un base de datos
                 * alterna para mantener un historial de los registros enviados hacia la plataforma 
                 */
                // CONSULTA SQL PARA IMPLEMENTAR POSTERIORMENTE
                
                /*DetalleRegistroAPI bitacoraAPI = new DetalleRegistroAPI();
                  bitacoraAPI._IDIntegracion = movDel.IdIntegracion;
                  while (!bitacoraAPI.remove()) ;*/
                //b = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}