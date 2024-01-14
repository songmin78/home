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

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;

    [Header("몬스터 스펙")]
    [SerializeField] float MaxHp = 1.0f;
    private float CurHp;
    [SerializeField] float MsSpeed = 1.0f;
    [SerializeField] float Msturntime = 1.0f;
    private float Maxtime;

    private void Awake()
    {
        CurHp = MaxHp;
        Maxtime = Msturntime;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        checkGround();

        Msmoving();

        checkGravity();

        MsAttack();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }

    private void Msmoving()
    {
        if(isGround == false)
        {
            return;
        }
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

    public void MsHit(float _damage)
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

    public void chaseplayer()
    {
        Vector3 playerPos = objPlayer == null ? new Vector3(0, -3, 0) : objPlayer.transform.position;
        float angle = Quaternion.FromToRotation(Vector3.up, playerPos - transform.position).eulerAngles.z;
    }
}
