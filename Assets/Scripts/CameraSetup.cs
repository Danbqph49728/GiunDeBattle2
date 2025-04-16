using Unity.Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public CinemachineCamera Camera;
    public CinemachinePositionComposer positionComposer;
    public void CamAssignment(Transform Player)
    {
        Camera.Follow = Player;
        Camera.LookAt = Player;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
