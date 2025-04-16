using UnityEngine;

public class MoveVector : MonoBehaviour
{
    public GameObject movePoint;
    public SnakeMovement snMovement;
    public float horinp;
    public float vertinp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        horinp = 1;
        vertinp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = movePoint.transform.position + new Vector3(horinp, vertinp, 0f);
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && horinp != -1 && horinp != 1 && snMovement.turned == true)
        {
            horinp = Input.GetAxisRaw("Horizontal");
            vertinp = 0;
            snMovement.turned = false;
        }
        //else if(Input.GetAxisRaw("Horizontal") == -1 && horinp != 1)
        //{
        //    horinp = -1;
        //    vertinp = 0;
        //}
        else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1 && vertinp != -1 && vertinp != 1 && snMovement.turned == true)
        {
            horinp = 0;
            vertinp = Input.GetAxisRaw("Vertical");
            snMovement.turned = false;
        }
        //else if (Input.GetAxisRaw("Vertical") == -1 && vertinp != 1)
        //{
        //    horinp = 0;
        //    vertinp = -1;
        //}
    }
}
