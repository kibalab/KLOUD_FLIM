using System.Collections.Generic;
using KLOUD.Twitch;

namespace KLOUD
{
    public class MessageEvent
    {
        public string Message;
        
        public List<TwitchMessageParser.emoji> Emojis;

        public MessageEvent(string message, List<TwitchMessageParser.emoji> emojis)
        {
            Message = message;
            Emojis = emojis;
        }
    }
    
    public static class MessageEventManager
    {
        public static Queue<MessageEvent> Events = new Queue<MessageEvent>();

        public static void Enqueue(string msg, List<TwitchMessageParser.emoji> emojis)
        {
            Events.Enqueue(new MessageEvent(msg, emojis));
        }

        public static MessageEvent Dequeue()
        {
            if (IsEmpty()) return null;

            return Events.Dequeue();
        }

        public static bool IsEmpty() => Events.Count == 0;
    }
}