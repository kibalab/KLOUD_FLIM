using System;
using System.Collections;
using KLOUD.Twitch;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KLOUD
{
    public class EmojiRenderer : MonoBehaviour
    {
        public RawImage Image;
        public TwitchMessageParser.emoji Emoji;

        private void Start()
        {
            Image.color = Color.clear;
        }

        public void Load()
        {
            StartCoroutine(GetTexture());
        }
        
        IEnumerator GetTexture() {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture($"https://static-cdn.jtvnw.net/emoticons/v2/{Emoji.uuid}/default/dark/1.0");
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                Emoji.texture = (Texture2D) ((DownloadHandlerTexture) www.downloadHandler).texture;
                
                Image.texture = Emoji.texture;

                if (Image.texture)
                {
                    Image.color = Color.white;
                }
            }
        }
    }
}