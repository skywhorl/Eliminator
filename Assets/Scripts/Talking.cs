using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour
{
    [SerializeField]
    public Dialogue[] dialogue; // 배열로 여러번 시도 가능

    public Dialogue[] invens;
    public Inventory inventory;
    //public Item ritem;
    //public Item additem;
    public int itemID;
    //public bool itemuse = false;
    public bool itemadd = false;
    public bool loop=false; // 처음부터 다시 계속 실행하게 할건지

    public string numname; //한정 출력시에 필요한요소 저장할 수 있는 값
   
    public int number = 0; //현재 x번째 dialogue
    public int maxnumber;  //  Dialogue 배열갯수와 같으면 반복 x, 배열갯수보다 작으면 최대치의 텍스트 반복 

    public DialogueManager theDM;
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        inventory = FindObjectOfType<Inventory>();
        Getnumber(numname);//저장된 것 가져오기
    }

    public void Getnumber(string name) //가져오기 
    {
        number = PlayerPrefs.GetInt(name);
    }
    public void SetNumber(int num) //저장하기
    {
        PlayerPrefs.SetInt(numname,num);
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dialogue.Length > number)
        {
            if (theDM.talking == false) //중복 방지
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    theDM.ShowDialogue(dialogue[number]);
                    if (maxnumber > number)
                    {
                        number++;
                        SetNumber(number);
                    }
                }
            }
        }
        else if (maxnumber == number)
        {
            if (theDM.talking == false) //중복 방지
            {
                //if (itemuse == true) //아이템 사용할꺼면으로 만들었는데 제거방법을 모르겠음
                //{
                //    itemuse = false;
                //    if (inventory.inventoryItemList.Contains(ritem)) //찾기
                //    {
                //        inventory.inventoryItemList.Remove(ritem);
                //        theDM.ShowDialogue(invens[0]); //사용
                //    }
                //    else
                //    {
                //        theDM.ShowDialogue(invens[1]); //찾았을 때 없으면
                //    }
                //}
                /*else*/
                if (itemadd == true) //아이템 획득
                {
                    if (numname == "JJANGDOLL")
                    {
                        DatabaseManager.instance.JJang = true;
                    }
                    itemadd = false;
                    Inventory.instance.GetAnItem(itemID, 1);
                    theDM.ShowDialogue(invens[0]);
                }
                if (loop == true) //무한반복용
                {
                    number = 0;
                }
            }
        }
    }

}

