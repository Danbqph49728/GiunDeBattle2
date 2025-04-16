using System.Collections;
using TMPro;
using UnityEngine;
using Fusion;
using static Unity.Collections.Unicode;

public class StartingCanvas : NetworkBehaviour
{
    public GameStates gameStates;
    private float timer = 3;
    public TextMeshProUGUI timertxt;
    public GameObject startingButton;

    public SnakeMovement[] pl;
    public GemSpawning gsp;

    public TimerScript timerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStates = FindAnyObjectByType<GameStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timertxt.text = timer.ToString();
        }
        else
        {
            timertxt.text = "Start!";
        }
    }
    public void Starting()
    {
        if(Runner.LocalPlayer.PlayerId != 1) return;
        Runner.SessionInfo.IsOpen = false;
        StartCoroutine(TimerCountdown());
    }
    IEnumerator TimerCountdown()
    {
        Rpc_timertxt();
        startingButton.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1);
            Rpc_timerDec();
        }
        Rpc_BoolSetter(true);
        gsp = FindFirstObjectByType<GemSpawning>();
        gsp.started = true;
        timerScript = FindAnyObjectByType<TimerScript>();
        timerScript.Rpc_CountUp(true);
        yield return new WaitForSeconds(1);
        Runner.Despawn(Object);
        yield break;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_timertxt()
    {
        timertxt.gameObject.SetActive(true);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_timerDec()
    {
        timer -= 1;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_BoolSetter(bool started)
    {
        pl = FindObjectsByType<SnakeMovement>(FindObjectsSortMode.None);
        foreach (var p in pl)
        {
            p.started = true;
        }
    }
}
