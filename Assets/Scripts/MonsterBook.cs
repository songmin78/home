using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoo : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;
    Animator anim;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] bool isGround = false;

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;

    [Header("몬스터 스펙")]
    [SerializeField] float MaxHp = 1.0f;
    private float CurHp;
    [SerializeField] float MsSpeed = 1.0f;
    [SerializeField] float MsDamage = 1.0f;

    private void Awake()
    {
        CurHp = MaxHp;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        checkGround();
        checkGravity();

        MsAttack();
    }

    private void Msmoving()
    {

    }

    private void checkGround()
    {
        isGround = false;
        if (verticalVelocity > 0)//5
        {
            return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    private void checkGravity()
    {
        if (isGround == false)//공중에 떠 있을때
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (verticalVelocity < -10.0f)//만약에 떨어지는 속도가 -10보다 작아지면 -10으로 제한
            {
                verticalVelocity = -10f;
            }
        }
        else//땅에 붙어 있을때
        {
            verticalVelocity = 0;//0
        }

        if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void MsAttack()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }

    public void MsHit(float _damage)
    {

        CurHp -= _damage;
        if(CurHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
