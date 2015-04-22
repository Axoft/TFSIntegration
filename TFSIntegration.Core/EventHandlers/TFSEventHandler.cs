using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using TFSIntegration.Core.XMLMaps;
using TFSIntegration.Core.XMLMaps.Events;

namespace TFSIntegration.Core.EventHandlers
{
    public abstract class TFSEventHandler<T> : ITFSEventHandler where T : TFSEvent
    {
        protected abstract void CustomProcessEvent(T eventInfo, TFSIdentity itentity);

        protected ICredentials GetCredentials()
        {
            return new NetworkCredential(Credentials.TFSUsername, Credentials.TFSPassword, Credentials.TFSDomain);
        }

        public void ProcessEvent(Stream eventStream, Stream identityStream)
        {
            XmlSerializer eventSerializer = new XmlSerializer(typeof(T));
            XmlSerializer itentitySerializer = new XmlSerializer(typeof(TFSIdentity));
            try
            {
                identityStream.Position = 0;
                eventStream.Position = 0;

                TFSIdentity itentity = (TFSIdentity)itentitySerializer.Deserialize(identityStream);
                T eventInfo = (T)eventSerializer.Deserialize(eventStream);

                CustomProcessEvent(eventInfo, itentity);
            }
            catch (Exception e)
            {
                EventManager.Notify(EventType.Error, e.Message);
            }
            
        }
    }
}
