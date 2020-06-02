using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private DatabaseManager theDatabase;
    private OrderManager theOrder;// 데이터 베이스의 아이템 발견
    //private AudioManager theAudio;
    private OkOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private InventorySlot[] slots; // 인벤토리 슬롯들

    public List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트
    private List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    public Text Description_Text; // 부연 설명
    public string[] tabDescription; // 아이템 부연 설명

    public Transform tf; // slot 부모 객체

    public GameObject go; // 인벤토리 활성화 비활성화
    //public GameObject[] selectedTabImages; // 카테고리 안(소모품 등)에 있는 탭들
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // 선택 시 활성화 비활성화

    public int selectedItem; // 선택된 아이템
    private int selectedTab; // 선택된 탭(카테고리)

    private bool activated; // 인벤토리 활성화시 true
    private bool tabActivated; //탭 활성화시 true
    private bool itemActivated; // 아이템 활성화시 true
    private bool stopKeyInput; // 키입력 제한(소비할 때 절의가 나올텐데, 그 때 키 입력 방지)
    private bool preventExec; // 중복실행 제한

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theDatabase = FindObjectOfType<DatabaseManager>();
        //theOrder = FindObjectOfType<OrderManager>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theOrder = FindObjectOfType<OrderManager>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }

    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }

    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }

    public void GetAnItem(int _itemID, int _count = 1)
    {
        //return 쓰는 이유 : 아이템을 찾았으면 더 할 일이 없어서
        //들어온 아이템만큼 데이터베이스에서 검색
        for (int i = 0; i < theDatabase.itemList.Count; i++) // 데이터베이스 아이템 검색
        {
            if (_itemID == theDatabase.itemList[i].itemID) // 데이터 베이스의 아이템 발견
            {
                for (int j = 0; j < inventoryItemList.Count; j++) // 아이템 중복 검색
                {
                    //소지품에 이미 가지고 있는지
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        if (inventoryItemList[j].itemType == Item.ItemType.Use)
                        {
                            //기존에 있던 아이템의 개수가 하나 더 늘어나게 함
                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;
                    }
                }
                //데이터베이스에 아무것도 얻은게 없을 때
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다.");
    } // 아이템 습득 시

    public void RemoveSlot()
    {
        //초반에 띄어놓은 빈 슬롯들 없애기
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            //탭이 생기기도 전에 아이템이 생기면 안됨
            slots[i].gameObject.SetActive(false);
        }
    } // 인벤토리 슬롯 초기화

    public void ShowTab()
    {
        SelectedTab();
        RemoveSlot();
    } // 탭 활성화
    public void SelectedTab()
    {
        StopAllCoroutines();

        //선택된 부분이 반짝이게 하는 설정
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        //처음에 다 붉은 색으로 띄면 안되니까 0으로 초기화
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    } // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파 값을 0으로 조정
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    } // 선택된 탭의 반짝임 효과

    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;
        // 탭에 따른 아이템 분류, 그것을 아이템 인벤토리 탭 리스트에 추가
        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;

        } 

        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            // 인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }
        // 아이템 활성화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 엔벤토리 슬롯에 출력)
        SelectedItem();
    } 
    public void SelectedItem()
    {
        StopAllCoroutines();
        // 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파 값을 0으로 조정 
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    } 
    IEnumerator SelectedItemEffectCoroutine()
    {
        // 선택된 아이템의 반짝임 효과
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    } 

    void Update()
    {

        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;

                if (activated)
                {
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if (activated)
            {
                // 탭 활성화시 키입력 처리
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                } 

                else if (itemActivated)
                {
                    // 아이템 활성화시 키입력 처리
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            // 아래 버튼을 눌렀을 경우 +2씩 증가하는 걸 고려해야 함
                            if (selectedItem < inventoryTabList.Count - 2)
                                selectedItem += 2;
                            else
                                selectedItem %= 2;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                                selectedItem -= 2;
                            else
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem++;
                            else
                                selectedItem = 0;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = inventoryTabList.Count - 1;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0) // 소모품
                            {
                                stopKeyInput = true;
                                tabActivated = true;
                                // 물약을 마실거냐? 같은 선택지 호출
                                StartCoroutine(OOCCoroutine());
                            }
                            else if (selectedTab == 1)
                            {
                                // 장비 장착
                            }
                            else //비프음 출력
                            {
                                //theAudio.Play(beep_sound);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        //theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                } 

                if (Input.GetKeyUp(KeyCode.Z)) // 중복 실행 방지
                    preventExec = false;
            }
        }
    }

    IEnumerator OOCCoroutine()
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice("사용", "취소");
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            // 아이템 사용 후 탭 줄이기(2개 이상이면 1 줄이기)
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryItemList[selectedItem].itemID)
                {
                    theDatabase.UseItem(inventoryItemList[i].itemID);
                    if (inventoryItemList[i].itemCount > 1)
                    {
                        inventoryItemList[i].itemCount--;
                    }
                    else
                    {
                        inventoryItemList.RemoveAt(i); // i번째의 인벤토리 제외
                    }
                    //theAudio.Play(beep_sound); 아이템 먹는 소리 출력
                    ShowItem();
                    break;
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
