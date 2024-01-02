using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using UnityEditor.SearchService;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;
    Animator anim;
    Transform layerDynamic;

    [SerializeField] bool isGround = false;//false 공중에 떠있는 상태, true 땅에 붙어있는 상태

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;
    private Camera mainCam;

    [Header("프리맵")]
    [SerializeField] private GameObject objAttack;

    [Header("공격")]
    [SerializeField] private bool Akchecking = false;//false = 유저가 키를 입력해야 공격
    private float timer = 0.0f;
    [SerializeField] bool Isattack = false;
    //[SerializeField, Range(0.0f, 3.0f)] private float timerAttack = 0.5f;//공격하는 기준 
    //[SerializeField] private float Attackdamage = 0.0f;

    [Header("플레이어스팩")]
    [SerializeField] float PlayerHP = 10.0f;//플레이어 HP
    [SerializeField] float moveSpeed = 5f;//플에이어 이속
    float hitdamage = 0.0f;//플레이어 공격데미지
    private float CurHp;
    

    BoxCollider2D atboxcollider;


    //[Header("벽점프")]
    //private bool wallStep = false;
    //private bool doWallStep = false;
    //private bool doWallStepTimer = false;
    //private float wallStepTimer = 0.0f;
    //[SerializeField] private float wallStepTime = 0.3f;

    void Awake()
    {
        CurHp = PlayerHP;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        atboxcollider = transform.Find("A").GetComponent<BoxCollider2D>();
    }
    
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        checkGround();

        moving();
        turning();

        jumping();
        Attackcheck();

        checkGravity();

        doAnimation();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Monster")
    //    {
    //        MonsterBoo Msattack = collision.GetComponent<MonsterBoo>();
    //        Msattack.MsHit(hitdamage);
    //    }
    //}

    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
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

    private void turning()
    {
        if (moveDir.x > 0 && transform.localScale.x == -1)//오른쪽으로 이동중
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
        else if (moveDir.x < 0 && transform.localScale.x == 1)//왼쪽으로 이동중
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
    }

    private void jumping()
    {
        //if (isGround == false)
        //{
        //    if (isGround == false && Input.GetKeyDown(KeyCode.Space) && wallStep == true && moveDir.x != 0.0f)//(moveDir.x <  0 || moveDir.x >0)
        //    {
        //        doWallStep = true;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        //{
        //    isJump = true;
        //}

        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isJump = true;
        }

    }

    private void checkGravity()
    {
        //if (dash == true) return;

        //if (doWallStep == true)//벽점프를 해야할때
        //{
        //    Vector2 dir = rigid.velocity;
        //    dir.x *= -1;
        //    rigid.velocity = dir;//튀는 방향
        //    verticalVelocity = jumpForce;//힘은 점프 포스의 힘으로 점프
        //    doWallStep = false;
        //    doWallStepTimer = true;
        //}

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

    private void Attackcheck()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(Isattack == true)
            {
                Isattack = false;
                return;
            }
            Isattack = true;
            anim.SetTrigger("IsAttack");
            //Invoke("testFunction", 1.5f);

        }
        //if(Akchecking == false && Input.GetKeyDown(KeyCode.L))
        //{
        //    ShootAtteck();
        //}
        //else if ( Akchecking == true)
        //{
            //timer += Time.deltaTime;//타이머증가
            //if(timerAttack <= timer)//타이머가 기준을 오버하면
            //{
            //    ShootAtteck();//공격
            //    timer = 0.0f;//타이머가 초기화
            //}
        //}
    }

    public void PlayerHit(float _damage)//플레이어가 맞는 데미지
    {
        CurHp -= _damage;
        if (CurHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void doAnimation()
    {
        anim.SetBool("Isjump", isGround);
        anim.SetInteger("Horizontal", (int)moveDir.x);
        //if (Isattack == true)
        //{
        //    anim.SetTrigger("IsAttack");
        //    Isattack = false;
        //}
        //anim.SetBool("attacking", Isattack);
    }

    //private void ShootAtteck()
    //{
    //    GameObject obj = Instantiate(objAttack, transform.position, Quaternion.identity, layerDynamic);
    //    Attack objSc = obj.GetComponent<Attack>();
    //    objSc.Akdamage(hitdamage);
    //}

    private void testFunction()
    {
    }

    private void AttackInBox()
    {
        atboxcollider.enabled = true;
    }
    private void AttackoutBox()
    {
        atboxcollider.enabled = false;
    }
    
}
