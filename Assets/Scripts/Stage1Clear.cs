using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Clear : MonoBehaviour
{
    public Inventory inventory;
    public Dialogue[] dialogue;
    public DatabaseManager database;
    public DialogueManager theDM;
    public Item item;
    public string Sceanname;
    private int num=1;
    private int max = 2;

    private void Start()
    {
        database = FindObjectOfType<DatabaseManager>();
        theDM = FindObjectOfType<DialogueManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (theDM.talking == false)
        {
            if (database.KEY==4)
            {

                theDM.ShowDialogue(dialogue[1]);
                if (num < max)
                {
                    num++;
                }
                else
                {
                    SceneManager.LoadScene("Claer");
                    inventory.inventoryItemList.Clear(); //모든 요소 제거(아이템 제거법을 몰라서 작성)
                }
                
                
            }
            else
            {
                theDM.ShowDialogue(dialogue[0]);
                
            }   
        }
    }
}
