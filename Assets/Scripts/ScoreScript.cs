using UnityEngine;
using Fusion;
using TMPro;

public class ScoreScript : NetworkBehaviour
{
    public int scoreId;
    public float score;
    public TextMeshProUGUI scoreTxt;
    private void Start()
    {
        scoreTxt = GetComponent<TextMeshProUGUI>();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ScoreUp(float point)
    {
        score += point;
        scoreTxt.text =$"{score.ToString()}/10";
    }
}
