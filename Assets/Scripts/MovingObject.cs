using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    static public MovingObject instance;

    public string CurrentMapName ="kings"; // 맵의 변수 값을 저장
    public string CurrentSceneName="kings";

    private SaveNLoad theSaveNLoad;

    public string characterName;

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
    private bool notCoroutine = false;

    public Queue<string> queue;
    //플레이어의 애니메이터 컴포넌트를 관리하기 위한 변수
    private Animator animator;

    //이동불가 지역 설정을 위한 충돌 컴포넌트
    private BoxCollider2D boxCollider;
    //어떤 레이어와 충돌했는지 판단해주는 변수
    public LayerMask layerMask;



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); // 씬 로드 시 캐릭터 파괴 금지
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        theSaveNLoad = FindObjectOfType <SaveNLoad>();
    }
    public void Move(string _dir, int _frequency = 5)
    {

        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }

    }

    //다중처리 함수(다른 함수와 같이 실행됨)
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        //코루틴 안에서 입력이 이루어지면 계속 이루어짐
        while (queue.Count != 0)
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;

            }
            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);
            switch (_dir)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIHGT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = 1f;
                    break;
            }

            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    applyRunSpeed = runSpeed;
            //    applyRunFlag = true;
            //}
            //else
            //{
            //    applyRunSpeed = 0;
            //    applyRunFlag = false;
            //}
            //vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            //키 중복 시 애니메이션이 어색하지 않도록 하기 위함
            //if (vector.x != 0)
            //    vector.y = 0;

            //값을 전달 받고 애니메이터를 실행하기 위함
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            while (true)
            {
                bool checkCollsionFlag = CheckCollsion();
                if (checkCollsionFlag)
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }
            animator.SetBool("Walking", true);
            boxCollider.offset = new Vector2(vector.x * 0.7f * speed, vector.y * 0.7f * speed);
            while (currentWalkCount < walkCount)
            {
                //다른 방법으로는 transform.position = vector..., RigidBody 등등
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;
                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero;
                yield return new WaitForSeconds(0.01f);
            }
            if (_frequency != 5)
                currentWalkCount = 0;
            animator.SetBool("Walking", false);
            //대기 시간 설정
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }
    protected bool CheckCollsion()
    {
        //레이저를 쏴서 두 지점을 기준으로 방해물이 없으면 null, 있으면 방해물로 리턴함
        RaycastHit2D hit;
        //캐릭터의 현재 위치 값
        Vector2 start = transform.position;
        //캐릭터다 이동하고자 하는 위치 값(방향벡터 * 48픽셀을 의미함)
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        //플레이어 자신과의 boxCollider 충돌을 막기 위함(비활성화 시킴)
        boxCollider.enabled = false;
        //tilemapCollider.enabled = false;
        //레이저를 쐈을 때 end 지점까지 무사히 도달하는지 판별
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;
        //tilemapCollider.enabled = true;
        //레이어 마스크에 해당하는 벽이 있으면 이후의 내용을 실행하지 않기로 함(여기는 while문 안에 있음을 잊지말 것)
        if (hit.transform != null)
            return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) {
            theSaveNLoad.CallSave();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            theSaveNLoad.CallLoad();
        }

        //if (canMove == true)
        //{
        //    //Input.GetAxisRaw("Horizontal") : 우 방향키가 눌리면 1, 좌 방향키가 눌리면 -1 리턴
        //    //Input.GetAxisRaw("Vertical") : 상 방향키가 눌리면 1, 하 방향키가 눌리면 -1 리턴
        //    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        //    {
        //        canMove = false;
        //        StartCoroutine(MoveCoroutine());
        //    }
        //}
    }
}
