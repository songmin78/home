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
    private float verticalVelocity;//�������� �޴� ��
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] float jumpForce = 5f;

    [Header("���� ����")]
    [SerializeField] float MaxHp = 1.0f;
    private float CurHp;
    [SerializeField] float MsSpeed = 1.0f;
    float MsDamage = 1.0f;
    [SerializeField] float Msturntime = 1.0f;
    private float Maxtime;
    [SerializeField] int Movetime = 5;
    private float MaxMovetime = 0.0f;//�÷��̾ �i�ư��½ð� ���������� 0 �i������ϸ� Movetime�� ����

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
        if (MaxMovetime != 0.0f)//�÷��̾ �����϶� ����� ���� ����
        {
            MaxMovetime -= Time.deltaTime;
            if(MaxMovetime < 0.0f)//�i�⸦ ������ ����
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

        if (MaxMovetime == 0.0f)//�÷��̾ �����ϴ� ���°� �ƴҶ�, �Դٰ��� �ϴ� ����
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
        else//�÷��̾ �����ϵ��� ��
        {
            Vector3 playerPos = trsPlayer.position;
            Vector3 MonsterPos = transform.position;
            //�÷��̾ ������ ���� �ִٸ� ����, �����ʿ��ִٸ� ������
            float right = playerPos.x - MonsterPos.x;
            if (right > 0)
            {
                rigid.velocity = new Vector2(-MsSpeed, rigid.velocity.y);
                //Debug.Log("������");
            }
            else if (right < 0)
            {
                rigid.velocity = new Vector2(MsSpeed, rigid.velocity.y);
                //Debug.Log("����");
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
        if (isGround == false)//���߿� �� ������
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (verticalVelocity < -10.0f)//���࿡ �������� �ӵ��� -10���� �۾����� -10���� ����
            {
                verticalVelocity = -10f;
            }
        }
        else//���� �پ� ������
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

    public void MsHit(float _damage)//�������� ���ݿ� �¾�����
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

    private void ChasePlayercheck()//�÷��̾��� ��ġ�� ��� üũ
    {
        //playercheck
    }
}
