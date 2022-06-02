using System;
using System.Linq;
using UnityEngine;

namespace KLOUD.Twitch
{
    
    public static class TwitchMessageParser
    {
        public static (string, string) Parse(string text)
        {
            if (!text.Contains("PRIVMSG"))
            {
                return ("", text);
            }
            try
            {
                var data = text.Split(';').ToList();
                var name = data.Find(str => str.Contains("display-name")).Split('=')[1];
                
                Material[] emojies;
                (text, emojies) = EmojiParse(text, data.Find(str => str.Contains("display-name")).Split('=')[1].Split('/'));


                var msg = text.Split("PRIVMSG")[1].Split(':').ToList();
                msg.RemoveAt(0);
                var msgString = msg.Aggregate((x, y) => x +':'+ y);
            
                
                
                return (name, msgString);
            
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            return ("", "");
        }

        public static (string, Material[]) EmojiParse(string text, string[] emojies)
        {
            foreach (var emoji in emojies)
            {
                var data = emoji.Split(':');
            }

            return (null, null);
        }
    }
}