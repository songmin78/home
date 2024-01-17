using UnityEngine;

public class MonsterBoo : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D boxCollider2D;
    Vector3 moveDir;
    Animator anim;
    [SerializeField] bool isGround = false;
    [SerializeField] LayerMask ground;
    [SerializeField] Collider2D trigger;
    [SerializeField] Player objPlayer;
    [SerializeField] Collider2D playercheck;

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] float jumpForce = 5f;

    [Header("몬스터 스펙")]
    [SerializeField] float MaxHp = 1.0f;
    private float CurHp;
    [SerializeField] float MsSpeed = 1.0f;
    float MsDamage = 1.0f;
    [SerializeField] float Msturntime = 1.0f;
    private float Maxtime;
    [SerializeField] int Movetime = 5;
    private float MaxMovetime = 0.0f;//플레이어를 쫒아가는시간 최종적으로 0 쫒기시작하면 Movetime이 들어옴

    Transform trsPlayer;

    private void Awake()
    {
        CurHp = MaxHp;
        Maxtime = Msturntime;
        //MaxMovetime = Movetime;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        //playercheck = transform.Find("Player").GetComponent<Collider2D>();
    }

    void Update()
    {
        CheckGround();

        Msmoving();

        checkGravity();

        ChasePlayercheck();

        MsAttack();

        checkChaseTime();
    }

    private void checkChaseTime()
    {
        if (MaxMovetime != 0.0f)//플레이어를 추적하라 명령이 들어온 상태
        {
            MaxMovetime -= Time.deltaTime;
            if(MaxMovetime < 0.0f)//쫒기를 포기한 상태
            {
                MaxMovetime = 0.0f;
            }
        }
    }

    private void Msmoving()
    {
        if(isGround == false)
        {
            return;
        }

        if (MaxMovetime == 0.0f)//플레이어를 추적하는 상태가 아닐때, 왔다갔다 하는 상태
        {
            rigid.velocity = new Vector2(MsSpeed, rigid.velocity.y);

            if (Maxtime > 0)
            {
                Maxtime -= Time.deltaTime;
            }
            else if (Maxtime <= 0)
            {
                Timeturn();
                Maxtime = Msturntime;
            }
        }
        else//플레이어를 추적하도록 함
        {
            Vector3 playerPos = trsPlayer.position;
            Vector3 MonsterPos = transform.position;
            //플레이어가 왼쪽을 보고 있다면 왼쪽, 오른쪽에있다면 오른쪽
            float right = playerPos.x - MonsterPos.x;
            if (right > 0)
            {
                rigid.velocity = new Vector2(-MsSpeed, rigid.velocity.y);
                //Debug.Log("오른쪽");
            }
            else if (right < 0)
            {
                rigid.velocity = new Vector2(MsSpeed, rigid.velocity.y);
                //Debug.Log("왼쪽");
            }
        }
        //Debug.Log(MaxMovetime);
    }

    private void FixedUpdate()
    {
        if(isGround == false)
        {
            return;
        }
        if (trigger.IsTouchingLayers(ground) == false)
        {
            turn();
        }
        
    }

    private void CheckGround()
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

    private void turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        MsSpeed *= -1;
        //Invoke("Timeturn", (float)Msturntime);
    }

    private void Timeturn()
    {
        if (transform.localScale.x == -1)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
            MsSpeed *= 1;
        }
        else if (transform.localScale.x == 1)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
            MsSpeed *= -1;
        }
    }

    private void MsAttack()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }

    public void MsHit(float _damage)//슬라임이 공격에 맞았을때
    {
        CurHp -= _damage;
        if (CurHp <= 0)
        {
            Destroy(gameObject);
            
        }
    }

    //private void Msanimation()
    //{
    //    anim.SetInteger("Msmove", (int)MsSpeed);
    //}

    public void chaseplayer(Transform _trsPlayer)
    {
        trsPlayer = _trsPlayer;
        if (MaxMovetime != 0.0f) return;

        MaxMovetime = Movetime;

        //Vector3 scale = transform.localScale;
        //transform.localScale = scale;
        //while (MaxMovetime >=0)
        //{
        //    if(MaxMovetime >=0)
        //    {
        //        Vector3 playerPos = objPlayer == null ? new Vector3(0, 0, 0) : objPlayer.transform.position;
        //        if (scale.x == 1)
        //        {
        //            float angle = Quaternion.FromToRotation(Vector3.right, playerPos - transform.position).eulerAngles.x;
        //        }
        //        else if(scale.x == -1)
        //        {
        //            float angle = Quaternion.FromToRotation(Vector3.left, playerPos - transform.position).eulerAngles.x;
        //        }
        //        MaxMovetime -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
        //Invoke("Chasemove", (float)MaxMovetime);
        //MaxMovetime = Movetime;
    }

    private void ChasePlayercheck()//플레이어의 위치를 상시 체크
    {
        //playercheck
    }
}
