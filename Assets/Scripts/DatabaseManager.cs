using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    //[HideInInspector]
    public int KEY = 0;
    public bool JJang = false;
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
    //적은 양은 저장할 때에는 스크립트로 하는 것이 편하다



    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;//이벤트 형식(참/거짓으로)

    //Item 데이터 추가
    public List<Item> itemList = new List<Item>();

    public void UseItem(int _itemID)
    {
        switch (_itemID)
        {
            case 10001:
                Debug.Log("HP가 50 회복 되었습니다.");
                break;
            case 10002:
                Debug.Log("MP가 50 회복 되었습니다.");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //아이템 데이터 생성
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 회복시켜주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 회복시켜주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 회복시켜주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농축 파란 포션", "마나를 80 회복시켜주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(11001, "랜덤 상자", "랜덤으로 포션이 나온다. 낮은 확률로 꽝", Item.ItemType.Use));
        itemList.Add(new Item(101, "1번방 열쇠", "열쇠다. 어디에 쓸까?", Item.ItemType.Quest));
        itemList.Add(new Item(102, "2번방 열쇠", "열쇠다. 어디에 쓸까?", Item.ItemType.Quest));
        itemList.Add(new Item(103, "3번방 열쇠", "열쇠다. 어디에 쓸까?", Item.ItemType.Quest));
        itemList.Add(new Item(104, "4번방 열쇠", "열쇠다. 어디에 쓸까?", Item.ItemType.Quest));
        itemList.Add(new Item(105, "짱돌", "뭔가를 깨트릴 수 있을 것 같다.", Item.ItemType.Quest));
    }
}
