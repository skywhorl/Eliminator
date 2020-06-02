using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    public Dialogue Problem; //문제 
    public Dialogue answerD; //정답시
    public Dialogue noanswerD;  //오답시
    private string bools ="false"; //저장용
    public string names; //저장용 이름 모든 이름은 달라야 값이 다름
    [HideInInspector]
    public bool Yes; //정답을 맞췄으면 더이상 실행 x
    public string answer; //문제의 답
    public DialogueManager theDM;
    public InputManager inputs;
    public Canvas canvas;
    public int ItemID;
    public Inventory inventory;
    public bool additem=false; //아이템 줄 문제인지
    public DatabaseManager Database;

    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        inputs = FindObjectOfType<InputManager>();
        inventory = FindObjectOfType<Inventory>();
        canvas = FindObjectOfType<Canvas>();
        GetBool();
    }
    public void GetBool() //첫 실행시는 false, 실행중에 정답시엔 계속 true로 고정
    {
        bools = PlayerPrefs.GetString(names,"false");
        Yes = System.Convert.ToBoolean(bools);   
    }
    public void SetBool() //true로 바뀌면 저장 씬이동해도 변하지 않음
    {
        PlayerPrefs.SetString(names, Yes.ToString());
    }

    public void ResultItem() //정답시 아이템을 줄 거면 이코드 사용
    {
        Inventory.instance.GetAnItem(ItemID,1);
        Database.KEY++; //이건 클리어용, 열쇠 4개를 모아 탈출하기
    }
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (theDM.talking == false) //대화창 중복 실행 방지
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Yes == false) //정답을 아직 못맞췄을시 실행
                {
                    theDM.ShowDialogue(Problem); //대화창 출력
                    canvas.AppearInput(); //텍스트 입력창 보이게하기
                    inputs = FindObjectOfType<InputManager>(); //이쯤 찾아야 찾을 수 있음
                    Database = FindObjectOfType<DatabaseManager>();
                }
            }
        }
    }
           
}
