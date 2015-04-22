using System.ServiceProcess;
using TFSIntegration.Core;

namespace TFSIntegration.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Set your credentials here!
            //Credentials.TFSUsername = "";
            //Credentials.TFSPassword = "";
            //Credentials.TFSDomain = "";

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
