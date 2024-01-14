using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;

    [Header("공격")]
    [SerializeField] float Monsterdamage = 0.0f;//몬스터가 플레이어에게 공격데미지
    [SerializeField] float Playerdamage = 0.0f;//플레이어가 몬스터에게 공격데미지
    [SerializeField] bool Isattack = false;
    private bool PlayerAttack = false;//플레이어가 한 공격이면 true

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            MonsterBoo Msattack = collision.GetComponent<MonsterBoo>();
            Msattack.MsHit(Playerdamage);
        }
        else if(collision.gameObject.tag == "Player")
        {
            Player playHit = collision.GetComponent<Player>();
            playHit.PlayerHit(Monsterdamage);
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public void Akdamage(float _damage)
    {
        Playerdamage = _damage;
    }
    public void Playddamage(float _pkdamage)
    {
        Monsterdamage = _pkdamage;
    }
}
