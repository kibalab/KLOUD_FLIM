using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace KLOUD.Twitch
{
    
    public class TwitchMessageParser
    {
        public static (string, string, List<emoji>) Parse(string text)
        {
            if (!text.Contains("PRIVMSG"))
            {
                return (null, text, null);
            }
            try
            {
                var data = text.Split(';').ToList();
                var name = data.Find(str => str.Contains("display-name")).Split('=')[1];
                
                var msg = text.Split("PRIVMSG")[1].Split(':').ToList();
                msg.RemoveAt(0);
                var msgString = msg.Aggregate((x, y) => x +':'+ y);
            
                List<emoji> emojies;
                (msgString, emojies) = EmojiParse(msgString, data.Find(str => str.Contains("emotes")).Split('=')[1].Split('/'));

                return (name, msgString, emojies);
            
            }
            catch (Exception e)
            {
                Debug.Log(text);
                Debug.LogError(e);
                throw;
            }
            return (null, "", null);
        }

        public struct emoji
        {
            public string uuid;
            public string tag;
            public int start;
            public int end;

            public Texture2D texture;
        }
        
        public static (string, List<emoji>) EmojiParse(string ctx, string[] nativeEmojies)
        {
            List<emoji> emojis = new List<emoji>();

            foreach (var nativEmoji in nativeEmojies)
            {
                if(String.IsNullOrEmpty(nativEmoji)) continue;
                
                var data = nativEmoji.Split(':');
                var range = data[1].Split(',')[0].Split('-');
                
                var emoji = new emoji();
                emoji.uuid = data[0];
                emoji.start = int.Parse(range[0]);
                emoji.end = int.Parse(range[1]);

                emoji.tag = ctx.Substring(emoji.start, emoji.end - emoji.start + 1);
                
                emojis.Add(emoji);
                
                //CoroutineEventManager.Requests.Enqueue(GetTexture(emoji));
            }

            var i = 0;
            foreach (var emoji in emojis)
            {
                ctx.Replace(emoji.tag, i.ToString());
                i++;
            }

            return (ctx, emojis);
        }

        
    }
}