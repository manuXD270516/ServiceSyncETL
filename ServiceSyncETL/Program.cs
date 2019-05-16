using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace ServiceSyncETL
{
    class Program
    { 
        static void Main(string[] args)
        {
            /*var x= new WaitOne();
            x.Main();
            Console.Read();*/

            ConfigurationService.configure();
            /*ServiceDataSyncETL servicesETL = new ServiceDataSyncETL(2000);
            servicesETL.start();
            ReadLine();*/
            
        }
    }
}
