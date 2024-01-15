using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MonsterBoo chaseplayer = transform.parent.GetComponent<MonsterBoo>();
            //MonsterBoo chaseplayer = GetComponentInParent<MonsterBoo>();
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
