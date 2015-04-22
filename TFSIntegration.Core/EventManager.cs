
namespace TFSIntegration.Core
{
    public enum EventType { Info, Error };

    public delegate void OnEventDelegate(EventType eventType, string eventMessage);
    static public class EventManager
    {
        public static void Notify(EventType eventType, string eventMessage)
        {
            if (EventDelegate != null)
                EventDelegate(eventType, eventMessage);
        }

        public static OnEventDelegate EventDelegate { get; set; }
    }
}
