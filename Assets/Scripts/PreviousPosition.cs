using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPosition : MonoBehaviour
{
    public NetworkObject head;
    public Vector3 currentPos;
    public Vector3 prevPos;
    void Update()
    {
        PositionUpdater();
    }
    public void PositionUpdater()
    {
        if(head.GetComponent<SnakeMovement>() != null)
        {
            if (head.GetComponent<SnakeMovement>().speedCountDown <= 0)
            {
                if (currentPos == null)
                {
                    currentPos = gameObject.transform.position;
                }
                else
                {
                    prevPos = currentPos;
                    currentPos = gameObject.transform.position;
                }
            }
        }
    }
}
