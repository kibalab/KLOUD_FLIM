using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.PlayerConnection;
using MessageEventArgs = WebSocketSharp.MessageEventArgs;
using Random = UnityEngine.Random;
using WebSocketState = WebSocketSharp.WebSocketState;

public class WebsocketConnecter
{
    public UnityEvent<string> onReceivedMessage = new UnityEvent<string>();

    private WebSocketSharp.WebSocket connection;

    public string userId = $"justinfan{Random.Range(10000, 99999)}";
    public string connectId = "";

    public bool Connect(string chennalID)
    {
        connection = new WebSocketSharp.WebSocket("wss://irc-ws.chat.twitch.tv");
        connection.OnMessage += OnMessage;
        connection.OnOpen += Login;

        connectId = chennalID;
        
        connection.Connect();

        return connection.ReadyState == WebSocketState.Open;
    }

    private void Login(object sender, EventArgs e)
    {
        Debug.Log($"Login : {userId}");
        SendLoginMessage();
    }

    public void SendLoginMessage()
    {
        connection.SendAsync("CAP REQ :twitch.tv/tags twitch.tv/commands", isComplete =>
        {
            if(isComplete) connection.SendAsync("PASS SCHMOOPIIE", isComplete =>
            {
                if(isComplete) connection.SendAsync($"NICK {userId}", isComplete =>
                {
                    if(isComplete) connection.SendAsync($"USER {userId} 8 * :{userId}", isComplete =>
                    {
                        if(isComplete) connection.SendAsync($"JOIN #{connectId}", isComplete =>
                        {
                            if(isComplete) Debug.Log($"Login OK");
                        });
                    });
                });
            });
        });
    }

    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        onReceivedMessage.Invoke(Encoding.UTF8.GetString(messageEventArgs.RawData).Replace("\\n", "\n"));
        onReceivedMessage.Invoke(Encoding.UTF8.GetString(messageEventArgs.RawData).Replace("\\n", "\n"));
    }

    private void OnApplicationQuit()
    {
        connection.Close();
    }
}