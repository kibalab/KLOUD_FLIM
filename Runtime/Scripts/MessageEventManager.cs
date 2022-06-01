using System.Collections.Generic;

namespace KLOUD
{
    public class MessageEvent
    {
        public string Message;

        public MessageEvent(string message)
        {
            Message = message;
        }
    }
    
    public static class MessageEventManager
    {
        public static Queue<MessageEvent> Events = new Queue<MessageEvent>();

        public static void Enqueue(string msg)
        {
            Events.Enqueue(new MessageEvent(msg));
        }

        public static MessageEvent Dequeue()
        {
            if (IsEmpty()) return null;

            return Events.Dequeue();
        }

        public static bool IsEmpty() => Events.Count == 0;
    }
}