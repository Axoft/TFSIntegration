using System;
using System.IO;
using System.ServiceModel.Activation;
using System.Text;
using TFSIntegration.Core.EventHandlers;
using TFSIntegration.Core.EventHandlers.Resolver;

namespace TFSIntegration.Core.SOAP
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NotifyService : INotifyService
    {
        public void Notify(string eventXml, string tfsIdentityXml)
        {
            try
            {
                using (MemoryStream eventStream = new MemoryStream(Encoding.Unicode.GetBytes(eventXml)))
                using (MemoryStream itentityStream = new MemoryStream(Encoding.Unicode.GetBytes(tfsIdentityXml)))
                {
                    ITFSEventHandler eventHandler = TFSEventHandlerResolver.GetEventHandler(eventStream);
                    eventHandler.ProcessEvent(eventStream, itentityStream);
                }
            }
            catch (Exception e)
            {
                EventManager.Notify(EventType.Error, e.Message);
            }
            
        }
    }
}
