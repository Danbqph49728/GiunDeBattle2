using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public GemSpawning gemspawn;

    public Transform[] spawnPoints;
    private float x;
    private float y;
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            if(player.PlayerId == 1)
            {
                x = spawnPoints[0].position.x;
                y = spawnPoints[0].position.y;
            }
            else if(player.PlayerId == 2)
            {
                x = spawnPoints[1].position.x;
                y = spawnPoints[1].position.y;
            }
            else if (player.PlayerId == 3)
            {
                x = spawnPoints[2].position.x;
                y = spawnPoints[2].position.y;
            }
            else if(player.PlayerId == 4)
            {
                x = spawnPoints[3].position.x;
                y = spawnPoints[3].position.y;
            }
            Vector2 spawnpoint = new Vector2(x, y);

            Runner.Spawn(PlayerPrefab, spawnpoint, Quaternion.identity, Runner.LocalPlayer, (runner, obj) =>
            {
                var _player = obj.GetComponent<PlayerSetup>();
                _player.SetCam();
            });
        }
    }
}
