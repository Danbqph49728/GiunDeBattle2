using Fusion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public GameStates gameState;
    public SnakeMovement[] pl;
    public List<SnakeMovement> alivePl;
    public float currentIndex;
    public float maxIndex;
    public float minIndex = 0;
    private void Start()
    {
        //gameState = GameObject.Find("GameStates");
        //gameState.GetComponent<GameStates>().AddPlayer(gameObject.GetComponent<NetworkObject>());
    }
    public void SetCam()
    {
        if (Object.HasInputAuthority)
        {
            CameraSetup camera = FindFirstObjectByType<CameraSetup>();
            if (camera != null)
            {
                camera.CamAssignment(transform);
            }
        }
    }
    public void JustDead_Cam()
    {
        if (!pl.Any())
        {
            pl = FindObjectsByType<SnakeMovement>(FindObjectsSortMode.None);
        }
        foreach (SnakeMovement obj in pl)
        {
            if (!obj.GetComponent<SnakeMovement>().isDead)
            {
                alivePl.Add(obj);
            }
        }
        if (Object.HasInputAuthority)
        {
            CameraSetup camera = FindFirstObjectByType<CameraSetup>();
            if (camera != null)
            {
                if(!alivePl.Any()) return;
                camera.CamAssignment(alivePl[Convert.ToInt32(currentIndex)].transform);
            }
        }
    }
    public void DeadCam(float index)
    {
        foreach (SnakeMovement obj in alivePl)
        {
            if (obj.GetComponent<SnakeMovement>().isDead)
            {
                alivePl.Remove(obj);
            }
        }
        if (!alivePl.Any()) return;
        maxIndex = alivePl.Count;
        if (Object.HasInputAuthority)
        {
            CameraSetup camera = FindFirstObjectByType<CameraSetup>();
            if (camera != null)
            {
                int trueIndex = Convert.ToInt32(currentIndex + index);
                if (trueIndex < 0)
                {
                    trueIndex = 0;
                }
                else if(trueIndex > maxIndex)
                {
                    trueIndex = Convert.ToInt32(maxIndex);
                }
                camera.CamAssignment(alivePl[trueIndex].transform);
            }
        }
    }
    //public void PlayersStatus()
    //{
    //    if (!pl.Any())
    //    {
    //        pl = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
    //    }
    //    foreach (NetworkObject obj in pl)
    //    {
    //        if (!obj.GetComponent<SnakeMovement>().isDead)
    //        {
    //            alivePl.Add(obj);
    //        }
    //    }
    //}
}
