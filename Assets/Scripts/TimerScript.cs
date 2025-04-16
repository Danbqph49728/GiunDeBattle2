using Fusion;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : NetworkBehaviour
{
    public bool isStarted;
    public bool ended;
    public float seconds;
    public float minutes;
    public TextMeshProUGUI tmp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        tmp.text = $"{Convert.ToInt32(minutes).ToString()}:{Convert.ToInt32(seconds).ToString()}" ;
        //if (isStarted)
        //{
        //    seconds += Time.deltaTime;
        //    if(seconds >= 60)
        //    {
        //        minutes += 1;
        //        seconds = 0;
        //    }
        //}
    }
    IEnumerator AdvancedCounter()
    {
        while (!ended)
        {
            yield return new WaitForSeconds(1);
            if (isStarted && !ended)
            {
                seconds += 1;
                if (seconds >= 60)
                {
                    minutes += 1;
                    seconds = 0;
                }
            }
        }
    }
    //public override void FixedUpdateNetwork()
    //{
    //    tmp.text = Convert.ToInt32(timerNumber).ToString();
    //    if (isStarted)
    //    {
    //        timerNumber += Runner.DeltaTime;
    //    }
    //}
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_CountUp(bool setbool)
    {
        isStarted = setbool;
        StartCoroutine(AdvancedCounter());
    }
}
