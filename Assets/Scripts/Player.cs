using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;
    Animator anim;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] bool isGround = false;//false 공중에 떠있는 상태, true 땅에 붙어있는 상태

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;
    private Camera mainCam;

    [Header("공격")]
    [SerializeField] float hitdamage = 1.0f;
    [SerializeField] bool Isattack = false;

    


    //[Header("벽점프")]
    //private bool wallStep = false;
    //private bool doWallStep = false;
    //private bool doWallStepTimer = false;
    //private float wallStepTimer = 0.0f;
    //[SerializeField] private float wallStepTime = 0.3f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
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

        Attackcheck();

        jumping();
        checkGravity();

        doAnimation();
    }

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
        if (moveDir.x > 0 && transform.localScale.x > -1)//오른쪽으로 이동중
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (moveDir.x < 0 && transform.localScale.x < 1)//왼쪽으로 이동중
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
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
            Isattack = true;
            anim.SetTrigger("IsAttack");
            //Invoke("testFunction", 1.5f);
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

    private void testFunction()
    {
    }
}
