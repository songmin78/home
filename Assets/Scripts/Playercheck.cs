using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playd = collision.GetComponent<Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
