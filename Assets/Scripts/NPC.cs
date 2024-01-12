using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider2D;
    Transform layerDynamic;
    BoxCollider2D Talkbox;

    public float npctalkbox = 0;


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
        rigid = GetComponent<Rigidbody2D>();
        //Talkbox = transform.Find("Talkbox1").GetComponent<BoxCollider2D>();
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
                OpenBox();
            }
        }
    }

    public void OpenBox()
    {
        Debug.Log("테스트");
        Talkbox.enabled = true;
    }

    private void Opentalkbox()
    {

    }
}
