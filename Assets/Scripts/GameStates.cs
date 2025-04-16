using UnityEngine;
using Fusion;
using NUnit.Framework;
using System.Collections.Generic;

public class GameStates : NetworkBehaviour
{
    public bool isStarted;
    public List<NetworkObject> players;
    public void AddPlayer(NetworkObject pl)
    {
        players.Add(pl);
    }
    public override void FixedUpdateNetwork()
    {
        if(isStarted)
        {
            foreach(NetworkObject p in players)
            {
                if(p == null) return;
                p.GetComponent<SnakeMovement>().started = isStarted;
            }
        }
    }
}
