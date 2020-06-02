using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReMovePlayer : MonoBehaviour
{
    public static ReMovePlayer instance;
    public float speed;
    public float runSpeed;
    private float applyRunSpeed;
    private Vector3 vector;

    //픽셀단위가 아닌 타일 단위로 움직이게 하는 변수들
    public int walkCount;
    private int currentWalkCount;

    //코루틴이 빠르게 실행되는 것을 방지하기 위한 변수
    private bool canMove = true;
    private bool applyRunFlag = true;

    //플레이어의 애니메이터 컴포넌트를 관리하기 위한 변수
    private Animator animator;

    //이동불가 지역 설정을 위한 충돌 컴포넌트
    private BoxCollider2D boxCollider;
    //어떤 레이어와 충돌했는지 판단해주는 변수
    public LayerMask layerMask;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //다중처리 함수(다른 함수와 같이 실행됨)
    IEnumerator MoveCoroutine()
    {
        //코루틴 안에서 입력이 이루어지면 계속 이루어짐
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }


            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            //키 중복 시 애니메이션이 어색하지 않도록 하기 위함
            if (vector.x != 0)
                vector.y = 0;

            //값을 전달 받고 애니메이터를 실행하기 위함
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            //레이저를 쏴서 두 지점을 기준으로 방해물이 없으면 null, 있으면 방해물로 리턴함
            RaycastHit2D hit;
            //캐릭터의 현재 위치 값
            Vector2 start = transform.position;
            //캐릭터다 이동하고자 하는 위치 값(방향벡터 * 48픽셀을 의미함)
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            //플레이어 자신과의 boxCollider 충돌을 막기 위함(비활성화 시킴)
            boxCollider.enabled = false;
            //레이저를 쐈을 때 end 지점까지 무사히 도달하는지 판별
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            //레이어 마스크에 해당하는 벽이 있으면 이후의 내용을 실행하지 않기로 함(여기는 while문 안에 있음을 잊지말 것)
            if (hit.transform != null)
                break;

            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                //다른 방법으로는 transform.position = vector..., RigidBody 등등
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                if (applyRunFlag == true)
                    currentWalkCount++;

                currentWalkCount++;
                //대기 시간 설정
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            //Input.GetAxisRaw("Horizontal") : 우 방향키가 눌리면 1, 좌 방향키가 눌리면 -1 리턴
            //Input.GetAxisRaw("Vertical") : 상 방향키가 눌리면 1, 하 방향키가 눌리면 -1 리턴
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

}

