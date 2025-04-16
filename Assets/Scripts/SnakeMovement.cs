using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMovement : NetworkBehaviour
{
    public NetworkObject Player;
    public GameObject moveVector;
    public Transform movePoint;
    public float speed;
    public float speedCountDown;
    public float horinp;
    public float vertinp;
    public bool turned;
    public bool ended;
    public Transform headSprite;

    public List<NetworkObject> bodies;
    public GameObject snakeBody;

    public NetworkObject gem;
    public GameObject scoreScript;
    [Networked] public bool started { get; set; }
    public NetworkObject startCanvas;
    public GameObject canvasPref;
    public GameObject cvo;

    [Networked] public bool isDead {  get; set; }
    private void Start()
    {
        if(Object.HasStateAuthority)
        {
            NetworkObject body = Runner.Spawn(snakeBody, transform.position - new Vector3(1, 0, 0));
            body.GetComponent<PreviousPosition>().head = Player;
            bodies.Add(body);
            movePoint.parent = null;
            speedCountDown = speed;
            horinp = 1;
            vertinp = 0;
        }
        if(Runner.LocalPlayer.PlayerId == 1 && HasStateAuthority)
        {
            //startCanvas = Runner.Spawn(canvasPref);
        }
        else if(Runner.LocalPlayer.PlayerId != 1)
        {
            cvo = GameObject.Find("StartButton");
            if(cvo != null)
            {
                CanvasGroup cv = cvo.GetComponent<CanvasGroup>();
                cv.alpha = 0;
                cv.interactable = false;
                cv.blocksRaycasts = false;
            }
        }
    }
    private void Update()
    {
        if (started && scoreScript == null)
        {
                scoreScript = GameObject.FindWithTag($"sc{Runner.LocalPlayer.PlayerId}");
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            GetComponent<PlayerSetup>().DeadCam(1);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            GetComponent<PlayerSetup>().DeadCam(-1);
        }
    }
    public override void FixedUpdateNetwork()
    {
        SpeedCounter();
        if(!started || isDead || ended) return;
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && horinp != -1 && horinp != 1 && turned == true)
        {
            if(!Object.HasInputAuthority) return;
            horinp = Input.GetAxisRaw("Horizontal");
            vertinp = 0;
            turned = false;
            movePoint.position = Player.transform.position + new Vector3(horinp, vertinp, 0f);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1 && vertinp != -1 && vertinp != 1 && turned == true)
        {
            if(!Object.HasInputAuthority) return;
            horinp = 0;
            vertinp = Input.GetAxisRaw("Vertical");
            turned = false;
            movePoint.position = Player.transform.position + new Vector3(horinp, vertinp, 0f);
        }
        if (Vector3.Distance(transform.position, movePoint.position) < 0.01f)
        {
            movePoint.position = Player.transform.position + new Vector3(horinp, vertinp, 0f);
            turned = true;
        }
        FirstBodyMovement();
    }
    void SpeedCounter()
    {
        if (started == false || isDead || ended) return;
        //if (gameStates.GetComponent<GameStates>().isStarted == false)
        //{
        //    return;
        //}
        else
        {
            if (speedCountDown > 0)
            {
                speedCountDown -= Runner.DeltaTime;
            }
            else
            {
                transform.position = movePoint.position;
                BodyMovement();
                speedCountDown = speed;
                HeadTurning();
            }
        }
    }
    void FirstBodyMovement()
    {
        NetworkObject first = bodies.FirstOrDefault();
        if (first != null && turned)
        {
            first.transform.position = transform.position - new Vector3(horinp, vertinp, 0f);
        }
    }
    void BodyMovement()
    {
        if(bodies.Count <= 1)
        {

        }
        else
        {
            for (int i = 1; i < bodies.Count; i++)
            {
                    bodies[i].transform.position = bodies[i - 1].GetComponent<PreviousPosition>().currentPos;
            }
        }
    }
    void HeadTurning()
    {
        if(horinp == 1 && vertinp == 0)
            headSprite.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if(horinp == 0 && vertinp == 1)
            headSprite.rotation = Quaternion.Euler(0f, 0f, 90f);
        else if (horinp == -1 && vertinp == 0)
            headSprite.rotation = Quaternion.Euler(0f, 0f, 180f);
        else if (horinp == 0 && vertinp == -1)
            headSprite.rotation = Quaternion.Euler(0f, 0f, -90f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("gem"))
        {
            NetworkObject newbody = Runner.Spawn(snakeBody, bodies.Last().GetComponent<PreviousPosition>().prevPos);
            bodies.Add(newbody);
            GemSpawning gemdespawn = FindAnyObjectByType(typeof(GemSpawning)) as GemSpawning;
            gemdespawn.Rpc_DespawnGem(collision.GetComponent<NetworkObject>());
            scoreScript.GetComponent<ScoreScript>().Rpc_ScoreUp(1);
        }
        
        if (collision.CompareTag("body") || collision.CompareTag("border"))
        {
            if(Object.HasStateAuthority)
            {
                //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                foreach (var body in bodies)
                {
                    //body.GetComponent<BoxCollider2D>().enabled = false;
                    //Runner.Spawn(gem, body.transform.position);
                    GemSpawning gemdeadspawn = FindAnyObjectByType(typeof(GemSpawning)) as GemSpawning;
                    gemdeadspawn.Rpc_SpawnDeadGem(body.transform.position.x, body.transform.position.y);
                    Runner.Despawn(body);
                }
                isDead = true;
                GetComponent<PlayerSetup>().JustDead_Cam();
                Rpc_DeadCanvas(true);
                Rpc_DisableCollider(false);
            }
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_DisableCollider(bool collidable)
    {
        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
        col.enabled = collidable;
    }
    [Rpc(RpcSources.StateAuthority,RpcTargets.StateAuthority)]
    public void Rpc_DeadCanvas(bool active)
    {
        GameObject dcv = GameObject.Find("DeadCanvas");
        dcv.GetComponent<CanvasGroup>().alpha = 1;
    }
}
