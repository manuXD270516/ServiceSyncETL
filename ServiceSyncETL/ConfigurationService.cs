using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ServiceSyncETL
{
    public class ConfigurationService
    {
        public const string NAME_DISPLAY_SERVICE="Servicio Sincronizacion Datos ETL";
        public const string DESCRIPTION_SERVICE="Servicio de Windows encargado de ejecutar la sincronizacion automatica de los movimientos de Combustible de aviacion (AIRPBP BOLIVIA - ANH)";

        public static int exitCodeValue;
        internal static void configure()
        {
            var exitCode=HostFactory.Run(
                config =>
                {
                    config.Service<ServiceDataSyncETL>(
                        service =>
                        {
                            service.ConstructUsing(s => new ServiceDataSyncETL(10000));
                            service.WhenStarted(s => s.start());
                            service.WhenStopped(s => s.stop());
                        }
                    );
                    config.RunAsLocalSystem();
                    string idName = new NamespaceUtils().GetNamespace1();
                    config.SetServiceName(idName);
                    config.SetDisplayName(NAME_DISPLAY_SERVICE);
                    config.SetDescription(DESCRIPTION_SERVICE);

                }
            );

            exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }

        

        
    }
}
