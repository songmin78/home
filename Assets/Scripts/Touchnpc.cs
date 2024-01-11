using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchnpc : MonoBehaviour
{
    private float npcopen;

    private void Awake()
    {
        npcopen = 0;
    }


    public void NPCtouch(float _openbox)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            npcopen -= _openbox;
            Debug.Log(npcopen);
        }
        npcopen = 0;
    }

}
