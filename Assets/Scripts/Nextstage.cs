using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nextstage : MonoBehaviour
{
    [Header("��������������ġ")]
    [SerializeField] float Stageorder = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Passstage();
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void Passstage()
    {
        if(Stageorder == 0)
        {
            Stageorder = 1;
        }
        else if(Stageorder == 1)
        {
            Stageorder = 0;
        }
    }
}
