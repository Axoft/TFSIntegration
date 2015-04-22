using System;
using System.IO;
using System.Xml.Linq;

namespace TFSIntegration.Core.EventHandlers.Resolver
{
    static class TFSEventHandlerResolver
    {
        private const string EventHandlersNamespaceFmt = "TFSIntegration.Core.EventHandlers.{0}Handler";
        public static ITFSEventHandler GetEventHandler(Stream eventStream)
        {
            XDocument eventXML = XDocument.Load(eventStream);
            XName eventName = ((XElement)eventXML.FirstNode).Name;

            Type eventHandler = Type.GetType(string.Format(EventHandlersNamespaceFmt, eventName.LocalName));
            if (eventHandler != null)
            {
                return (ITFSEventHandler)Activator.CreateInstance(eventHandler);
            }

            throw new NotSupportedException(string.Format("Event handler for {0} cannot be resolved", eventName.Namespace));
        }
    }
}
