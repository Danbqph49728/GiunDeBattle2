using UnityEngine;

public class SnakePartsID : MonoBehaviour
{
    public bool isHead;
    public int partID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHead)
        {
            partID = 0;
        }
    }
}
