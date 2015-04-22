using System.IO;

namespace TFSIntegration.Core.EventHandlers
{
    interface ITFSEventHandler
    {
        void ProcessEvent(Stream eventStream, Stream identityStream);
    }
}
