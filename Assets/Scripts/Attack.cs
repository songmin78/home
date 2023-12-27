using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;

    [Header("АјАн")]
    [SerializeField] float PKdamage = 1.0f;
    [SerializeField] bool Isattack = false;
    private float damage = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            MonsterBoo Msattack = collision.GetComponent<MonsterBoo>();
            Msattack.MsHit(damage);
        }

    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Akdamage();
    }

    public void Akdamage()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Isattack = true;
            if (gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                MonsterBoo Msattack = gameObject.GetComponent<MonsterBoo>();
                Msattack.MsHit(damage);
                Isattack = false;
            }
            else
            {
                Isattack = false;
                return;
            }
        }
    }
}
