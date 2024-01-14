using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    float Openbox = 0;
    BoxCollider2D boxColl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Touch")
        //{
        //    Touchnpc touchnpc = collision.GetComponent<Touchnpc>();
        //    touchnpc.NPCtouch(Openbox);
        //}

        //if(collision.gameObject.tag == "Touch")
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        Debug.Log("아");
        //    }
        //}
    }

    //private void OnTriggerStay2D(Collider2D collision)//실패
    //{
    //    if (collision.gameObject.tag == "Touch")
    //    {
    //        if (Input.GetKeyDown(KeyCode.F))
    //        {
    //            Debug.Log("아");
    //        }
    //        Debug.Log("0");
    //    }
    //}

    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        checkTouchNpc();
    }

    private void checkTouchNpc()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D hit = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0, LayerMask.GetMask("Touch"));
            if (hit != null)
            {
                Debug.Log("아");
            }
        }
    }

    public void OpenBox(float _openbox)
    {
        Openbox = _openbox;
        Debug.Log(Openbox);
    }
}
