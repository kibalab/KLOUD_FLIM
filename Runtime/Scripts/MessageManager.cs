using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

namespace KLOUD
{
    public struct MessageJob : IJobParallelFor
    {
        public Text[] Texts;
        public float Speed;
        public float TimeDelta;
        
        public void Execute(int index)
        {
            var transform = (Texts[index].transform);

            var Pos = transform.localPosition;
            Pos.x += Speed * TimeDelta * Texts[index].text.Length;
            transform.localPosition = Pos;
        }
    }
    
    public class MessageManager : MonoBehaviour
    {
        public float Speed = 10;
        public List<Text> SpawnedTexts = new List<Text>();
        public MessageJob JobCommand = new MessageJob();

        public WebsocketConnecter WebsocketConnecter;

        private void Start()
        {
            WebsocketConnecter = new WebsocketConnecter();
            
            WebsocketConnecter.onReceivedMessage.AddListener(
                msg => MessageEventManager.Enqueue(msg));
        }

        public void UpdateJob()
        {
            JobCommand.Speed = Speed;
            JobCommand.Texts = SpawnedTexts.ToArray();
            JobCommand.TimeDelta = Time.deltaTime;
        }
    }
}
