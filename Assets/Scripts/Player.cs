using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Transactions;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;
    Animator anim;

    [SerializeField] bool isGround = false;//false ���߿� ���ִ� ����, true ���� �پ��ִ� ����

    private bool isJump = false;
    private float verticalVelocity;//�������� �޴� ��
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;
    private Camera mainCam;
    Transform trsMonster;
    private float rightleftcheck = 0;//������ ���� Ȯ�� 1�� ������ 2�� ����

    [Header("�÷��̾�")]
    [SerializeField] GameObject objPlayer;

    [Header("������")]
    [SerializeField] private GameObject objAttack;

    [Header("����")]
    [SerializeField] private bool Akchecking = false;//false = ������ Ű�� �Է��ؾ� ����
    private float timer = 0.0f;
    [SerializeField] bool Isattack = false;
    //[SerializeField, Range(0.0f, 3.0f)] private float timerAttack = 0.5f;//�����ϴ� ���� 
    //[SerializeField] private float Attackdamage = 0.0f;

    [Header("�÷��̾��")]
    [SerializeField] float PlayerHP = 10.0f;//�÷��̾� HP
    [SerializeField] float moveSpeed = 5f;//�ÿ��̾� �̼�
    float hitdamage = 0.0f;//�÷��̾� ���ݵ�����
    private float CurHp;
    private bool Playerpush = false;//�÷��̾ ���� �и����ִ��� �ƴ��� Ȯ��
    [SerializeField] float pushtime = 1;//�÷��̾ �и��� �ð�
    private float Maxpushtime;

    BoxCollider2D atboxcollider;


    //[Header("������")]
    //private bool wallStep = false;
    //private bool doWallStep = false;
    //private bool doWallStepTimer = false;
    //private float wallStepTimer = 0.0f;
    //[SerializeField] private float wallStepTime = 0.3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            trsMonster= collision.transform;
        }
    }

    void Awake()
    {
        Maxpushtime = pushtime;
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
        pushtimed();

        jumping();
        Attackcheck();

        checkGravity();

        doAnimation();

        testFunction();
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
        if(Playerpush == true)
        {
            return;
        }
 
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
        if (moveDir.x > 0 && transform.localScale.x == -1)//���������� �̵���
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
        else if (moveDir.x < 0 && transform.localScale.x == 1)//�������� �̵���
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

        if (Playerpush == false && Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isJump = true;
        }

    }

    private void checkGravity()
    {
        //if (dash == true) return;

        //if (doWallStep == true)//�������� �ؾ��Ҷ�
        //{
        //    Vector2 dir = rigid.velocity;
        //    dir.x *= -1;
        //    rigid.velocity = dir;//Ƣ�� ����
        //    verticalVelocity = jumpForce;//���� ���� ������ ������ ����
        //    doWallStep = false;
        //    doWallStepTimer = true;
        //}

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
            Isattack = false;
            //Invoke("testFunction", 1.5f);

        }
        //if(Akchecking == false && Input.GetKeyDown(KeyCode.L))
        //{
        //    ShootAtteck();
        //}
        //else if ( Akchecking == true)
        //{
            //timer += Time.deltaTime;//Ÿ�̸�����
            //if(timerAttack <= timer)//Ÿ�̸Ӱ� ������ �����ϸ�
            //{
            //    ShootAtteck();//����
            //    timer = 0.0f;//Ÿ�̸Ӱ� �ʱ�ȭ
            //}
        //}
    }

    public void PlayerHit(float _damage, Vector3 _curPos)//�÷��̾ �´� ������
    {
        CurHp -= _damage;
        if (CurHp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            
            Playerpush = true;
            anim.SetTrigger("Test");

            //Vector3 PlayerPos = transform.position;//�÷��̾��� ��ġ
            //Vector3 MsPos = _curPos;//������ ��ġ

            //float right = PlayerPos.x - MsPos.x;//�÷��̾ ���Ϳ��� �¾����� ��ġȮ��
            //if (right > 0 )
            //{

            //    //PlayerPos.x += 0.25f;
            //    //rigid.velocity = PlayerPos;
            //    //anim.SetTrigger("Test");
            //    //rightleftcheck = 1;

            //    //if (pushtime < 0)//pushtime�� 0���� ������ ���ĳ��°��� �׸��Ѵ�
            //    //{
            //    //    Playerpush = false;
            //    //}
            //    //pushtime -= Time.deltaTime;
            //    //Playerpush = false;

            //    //while(pushtime > 0)
            //    //{
            //    //    moveDir.x = PlayerPos.x + 1;
            //    //    rigid.velocity = moveDir;
            //    //    pushtime -= Time.deltaTime;
            //    //}
            //    ////Playerpush = false;

            //    //anim.SetTrigger("Test");
            //    //Debug.Log(pushtime);
            //    //Debug.Log(Playerpush);

            //}
            //else if(right < 0)
            //{
            //    //PlayerPos.x -= 0.25f;
            //    //rigid.velocity = PlayerPos;
            //    //anim.SetTrigger("Test");
            //    //rightleftcheck = 2;
            //    //anim.SetTrigger("Test");
            //    //Debug.Log("����");
            //    //Playerpush = false;
            //}
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            anim.SetTrigger("Test");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {

        }
    }

    private void AttackInBox()
    {
        atboxcollider.enabled = true;
    }
    private void AttackoutBox()
    {
        atboxcollider.enabled = false;
    }
    
    private void pushtimed()
    {
        if (Playerpush == true)
        {
            //Vector3 PlayerPos = transform.position;//�÷��̾��� ��ġ
            //moveDir.x = rigid.velocity.x;//�÷��̾ �̵��� ��ġ

            if(rightleftcheck == 1)
            {
                //moveDir.x = PlayerPos.x + 1;
                Maxpushtime -= Time.deltaTime;
                if (Maxpushtime < 0)//pushtime�� 0���� ������ ���ĳ��°��� �׸��Ѵ�
                {
                    Playerpush = false;
                }
            }
            else if(rightleftcheck == 2)
            {
                //moveDir.x = PlayerPos.x - 1;
                Maxpushtime -= Time.deltaTime;
                if (Maxpushtime < 0)//pushtime�� 0���� ������ ���ĳ��°��� �׸��Ѵ�
                {
                    Playerpush = false;
                }
            }
            
        }
        if(Playerpush == false)
        {
            rightleftcheck = 0;
            Maxpushtime = pushtime;
        }

    }

    private void pushtimetrue()
    {
        if(Playerpush == true)
        {
            Vector3 PlayerPos = transform.position;//�÷��̾��� ��ġ
            Vector3 MsPos = transform.position;//������ ��ġ

            float right = PlayerPos.x - MsPos.x;//�÷��̾ ���Ϳ��� �¾����� ��ġȮ��
            if (right > 0)
            {
                PlayerPos.x += 0.25f;
                rigid.velocity = PlayerPos;
                rightleftcheck = 1;
            }
            else if (right < 0)
            {
                PlayerPos.x -= 0.25f;
                rigid.velocity = PlayerPos;
                rightleftcheck = 2;
            }
        }
    }

    private void pushtimefalse()
    {
        Playerpush = false;
    }

}
