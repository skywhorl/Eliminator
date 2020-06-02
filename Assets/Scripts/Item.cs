using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID; //아이템의 고유 ID값, 중복 불가능(이미지 이름 잘 설정해 놓을 것)
    public string itemName; //아이템의 이름, 중복 가능
    public string itemDescription; //아이템 설명
    public int itemCount; //소지 개수
    public Sprite itemIcon; //아이템의 아이콘
    public ItemType itemType;


    //ItemType에 4가지 값만 갖게 함
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        //  "/"의미는 "?"를 의미함, as Sprite는 스프라이트로 강제 캐스팅
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }
}
