using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : MonoBehaviour
{
    public int a = 0;//잠시 만든 변수
    [SerializeField]
    public Dialogue dialogue;

    public DialogueManager theDM;
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        a = 0;
    }

    // Update is called once per frame

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (a == 0&& collision.tag=="Player")
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            a++;
            theDM.ShowDialogue(dialogue);//대화창 출력 시작
            //}
        }
        
    }
    
  
}
