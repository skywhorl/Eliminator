using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JangDollEnd : MonoBehaviour
{
    public Dialogue[] dialogue;
    public DialogueManager theDM;
    Inventory inven;
    public int ItemID;
    public DatabaseManager data;
    private int num=0;
    public int max;

    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        inven = FindObjectOfType<Inventory>();
        data = FindObjectOfType<DatabaseManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (theDM.talking == false)
            {
                if (data.JJang == true)
                {
                    theDM.ShowDialogue(dialogue[num]);
                    if (num < max)
                    {
                        num++;
                    }
                    else if( num == max)
                    {
                        SceneManager.LoadScene("Hidden");
                    }
                }
            }
        }
    }
}
