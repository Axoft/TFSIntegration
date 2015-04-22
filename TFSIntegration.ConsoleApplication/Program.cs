using System;
using System.Configuration;
using System.ServiceModel;
using TFSIntegration.Core;
using TFSIntegration.Core.SOAP;

namespace TFSIntegration.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri serviceHost = new Uri(ConfigurationManager.AppSettings["host"]);
            using (var host = new ServiceHost(typeof(NotifyService), serviceHost))
            {
                host.Open();
                Console.WriteLine("Service running: {0}", serviceHost.AbsoluteUri);
                Console.WriteLine("Start time: {0}", DateTime.Now);
                
                //Set your credentials here!
                //Credentials.TFSUsername = "";
                //Credentials.TFSPassword = "";
                //Credentials.TFSDomain = "";

                EventManager.EventDelegate = (e, m) => 
                { 
                    Console.WriteLine(m); 
                };

                Console.ReadLine();
                host.Close();
            }
        }
    }
}
