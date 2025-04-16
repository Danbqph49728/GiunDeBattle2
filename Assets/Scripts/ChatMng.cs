using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;
    private List<string> chatMessages = new List<string>();
    public ChatUI chatUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ReceiveMessage(string plname, string message)
    {
        string formattedMessage = $"Player {plname}: {message}";
        chatMessages.Add(formattedMessage);
        chatUI.segContent.text += formattedMessage + "\n";
    }
    public void SendNuteMessage(string message)
    {
        string playerName = Runner.LocalPlayer.PlayerId.ToString();
        Rpc_ReceiveMessage(playerName, message);
    }
}
