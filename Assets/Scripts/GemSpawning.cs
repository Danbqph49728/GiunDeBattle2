using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemSpawning : NetworkBehaviour
{
    public NetworkObject gemPref;
    public Transform gemParent;
    public List<NetworkObject> Gems;
    private int maxGem = 10;
    public bool started;
    public override void FixedUpdateNetwork()
    {
        if (!started) return;
        if (Gems.Count() < maxGem)
        {
            int x = Random.Range(-35, 36);
            int y = Random.Range(30, -29);
            Vector2 spawnpoint = new Vector2(x, y);
            NetworkObject gem = Runner.Spawn(gemPref, spawnpoint, Quaternion.identity);
            Gems.Add(gem);
            gem.transform.SetParent(gemParent);
        }
        Gems.RemoveAll(item => item == null);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_DespawnGem(NetworkObject gem)
    {
        Runner.Despawn(gem);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_SpawnDeadGem(float x, float y)
    {
        Vector2 spawnpoint = new Vector2(x, y);
        NetworkObject gem = Runner.Spawn(gemPref, spawnpoint, Quaternion.identity);
        Gems.Add(gem);
        gem.transform.SetParent(gemParent);
    }
}
