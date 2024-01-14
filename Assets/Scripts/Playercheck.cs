using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MonsterBoo chaseplayer = collision.GetComponent<MonsterBoo>();
            chaseplayer.chaseplayer();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
