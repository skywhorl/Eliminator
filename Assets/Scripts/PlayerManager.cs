using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Movectrl 
{
    public static PlayerManager instance;

    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    public bool canMove = true;

    public bool notMove = false;

    public string CurrentMapName; // 맵의 변수 값을 저장
    public string CurrentSceneName;
    private SaveNLoad theSaveNLoad;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {   
                DontDestroyOnLoad(this.gameObject);
                instance = this;  
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton 


    private void Start()
    {
        animator = GetComponent<Animator>();
        //tilemapCollider = GetComponent<TilemapCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        queue = new Queue<string>();
        PlayerPrefs.DeleteAll();//첫 실행시 모든 저장된 값 초기화
        theSaveNLoad = FindObjectOfType<SaveNLoad>();
    }
    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0&& !notMove)
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

            if (vector.x != 0)
                vector.y = 0;


            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);

            bool checkCollsionFlag = base.CheckCollsion();
            if (checkCollsionFlag)
                break;

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed, vector.y * 0.7f * speed);
            while (currentWalkCount < walkCount)
            {

                transform.Translate(vector.x * (speed + applyRunSpeed), vector.y * (speed + applyRunSpeed), 0);
                if (applyRunFlag)
                    currentWalkCount++;
                currentWalkCount++;
                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero;
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
        if (Input.GetKeyDown(KeyCode.F5))
        {
            theSaveNLoad.CallSave();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            theSaveNLoad.CallLoad();
        }

        if (canMove&& !notMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

    }
}

