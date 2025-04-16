using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndingScript : MonoBehaviour
{
    public ScoreScript[] scores;
    public bool ended;
    private CanvasGroup cg;
    private string wonPlayer;
    private Color txtColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scores = FindObjectsByType<ScoreScript>(FindObjectsSortMode.InstanceID);
        StartCoroutine(ScoreCheck());
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ScoreCheck()
    {
        while (!ended)
        {
            foreach (var score in scores)
            {
                if(score.score == 10)
                {
                    ended = true;
                    if (score.gameObject.name == "scorep1") { wonPlayer = "Player 1"; txtColor = Color.red; }
                    if (score.gameObject.name == "scorep2") { wonPlayer = "Player 2"; txtColor = Color.green; }
                    if (score.gameObject.name == "scorep3") { wonPlayer = "Player 3"; txtColor = Color.yellow; }
                    if (score.gameObject.name == "scorep4") { wonPlayer = "Player 4"; txtColor = Color.blue; }
                    Rpc_End();
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_End()
    {
        SnakeMovement[] player = FindObjectsByType<SnakeMovement>(FindObjectsSortMode.None);
        foreach(var pl in player)
        {
            pl.ended = true;
        }
        TimerScript tm = FindFirstObjectByType<TimerScript>();
        tm.ended = true;
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        GameObject wintxt = GameObject.Find("PlWin Text");
        wintxt.GetComponent<TextMeshProUGUI>().text = $"{wonPlayer} Win!";
        wintxt.GetComponent<TextMeshProUGUI>().color = txtColor;
    }
}
