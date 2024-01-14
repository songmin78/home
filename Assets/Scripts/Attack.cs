using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;

    [Header("����")]
    [SerializeField] float Monsterdamage = 0.0f;//���Ͱ� �÷��̾�� ���ݵ�����
    [SerializeField] float Playerdamage = 0.0f;//�÷��̾ ���Ϳ��� ���ݵ�����
    [SerializeField] bool Isattack = false;
    private bool PlayerAttack = false;//�÷��̾ �� �����̸� true

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
