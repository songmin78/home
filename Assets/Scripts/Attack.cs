using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;

    [Header("����")]
    [SerializeField] float PKdamage = 1.0f;
    [SerializeField] bool Isattack = false;
    private float damage = 0.0f;
    private bool PlayerAttack = false;//�÷��̾ �� �����̸� true

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            MonsterBoo Msattack = collision.GetComponent<MonsterBoo>();
            Msattack.MsHit(damage);
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Akdamage();
    }

    public void Akdamage(float _damage)
    {
        damage = _damage;
    }
}
