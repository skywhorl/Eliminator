﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //데이터베이스에 있는 아이템을 참조
    public int itemID;
    public int _count;
    //public string pickUpSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //AudioManager.instance.Play(pickUpSound);
            //인벤토리 추가
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }
    }
}
