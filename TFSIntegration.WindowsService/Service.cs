using log4net;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using TFSIntegration.Core;
using TFSIntegration.Core.SOAP;

namespace TFSIntegration.WindowsService
{
    public partial class Service : ServiceBase
    {
        private ServiceHost _host;
        private ILog _logger;

        public Service()
        {
            InitializeComponent();
            this._logger = LogManager.GetLogger("TFSIntegration");
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();

            Uri serviceHost = new Uri(ConfigurationManager.AppSettings["host"]);
            this._host = new ServiceHost(typeof(NotifyService), serviceHost);
            this._host.Open();

            this._logger.InfoFormat("Service running: {0}", serviceHost.AbsoluteUri);
            this._logger.InfoFormat("Start time: {0}", DateTime.Now);
            
            EventManager.EventDelegate = (e, m) =>
            {
                switch (e)
                {
                    case EventType.Error :
                        this._logger.Error(m);
                        break;
                    default :
                        this._logger.Info(m);
                        break;
                }
            };
        }

        protected override void OnStop()
        {
            this._logger.InfoFormat("Stoping service at {0}", DateTime.Now);
            this._host.Close();
            ((IDisposable)this._host).Dispose();
        }
    }
}
