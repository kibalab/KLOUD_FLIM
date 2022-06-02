

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class TwitchEvents
{
    public static Queue<TwitchMessage> MessageEvents = new Queue<TwitchMessage>();

    public static void AddEvent(string name, string message)
    {
        MessageEvents.Enqueue(new TwitchMessage(name, message));
    }

    public static TwitchMessage GetEvent()
    {
        if (isEmpty()) return null;
        return MessageEvents.Dequeue();
    }

    public static bool isEmpty() => MessageEvents.Count <= 0;
}

public class TwitchMessage
{
    public string Name;
    public string Message;

    public TwitchMessage(string name, string message)
    {
        Name = name;
        Message = message;
    }
}

public struct MoveJob : IJobParallelForTransform
{
    public float deltaTime;
    public float speed;

    public void Execute(int index, TransformAccess transform)
    {
        var transformLocalPosition = transform.localPosition;
        transformLocalPosition.x = transformLocalPosition.x - speed * deltaTime;
        transform.localPosition = transformLocalPosition;
    }
}

public class TwitchTesters : MonoBehaviour
{
    public string TwitchChennalID;

    public float ScrollSpeed = 50;

    public Text ReferenceText;

    public TransformAccessArray textTransforms;
    public List<Transform> textTrnsformList = new List<Transform>();

    private MoveJob job;

    public Canvas Canvas;

    public WebsocketConnecter WebsocketConnecter;
    public void Start()
    {
        if (!Canvas) Canvas = GetComponent<Canvas>();

        job = new MoveJob();
        
        WebsocketConnecter = new WebsocketConnecter();
        
        Debug.Log($"{TwitchChennalID} : {WebsocketConnecter.Connect(TwitchChennalID)}");
        
        WebsocketConnecter.onReceivedMessage.AddListener(OnMessage);

        ReferenceText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (textTrnsformList.Count > 0)
        {
            
            var i = 0;
            foreach (var e in textTrnsformList)
            {
                if (e.localPosition.x < -1920 - 600)
                {
                    textTransforms.Dispose();
                    
                    textTrnsformList.Remove(e);
                    
                    Destroy(e.gameObject);
                    break;
                }

                i++;
            }
            
            if(!textTransforms.isCreated || textTransforms.length != textTrnsformList.Count) textTransforms = new TransformAccessArray(textTrnsformList.ToArray());
            job.deltaTime = Time.deltaTime;
            job.speed = ScrollSpeed;
            job.Schedule(textTransforms);
        }
        
        if(TwitchEvents.isEmpty()) return;

        var message = TwitchEvents.GetEvent();
        
        var messageObj = Instantiate(ReferenceText.gameObject, ReferenceText.transform.parent).GetComponent<Text>();
        ((RectTransform) messageObj.transform).pivot = new Vector2(-1, 1);
        ((RectTransform) messageObj.transform).anchorMin = Vector2.one;
        ((RectTransform) messageObj.transform).anchorMax = Vector2.one;
        ((RectTransform) messageObj.transform).localPosition = new Vector3(
            messageObj.transform.localPosition.x, 
            Random.Range(-600, 600),
            messageObj.transform.localPosition.z
        );
        messageObj.text = message.Message;
        messageObj.gameObject.SetActive(true);

        textTrnsformList.Add(messageObj.transform);
    }
    

    public void OnMessage(string text)
    {
        try
        {
            if (!text.Contains("PRIVMSG"))
            {
                Debug.Log(text);
                return;
            }
        
            var (user, message) = GetUserMessage(text);
        
            TwitchEvents.AddEvent(user, message);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
       
    }

    public (string, string) GetUserMessage(string text)
    {
        try
        {
            var data = text.Split(';').ToList();
            var name = data.Find(str => str.Contains("display-name")).Split('=')[1];


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
}