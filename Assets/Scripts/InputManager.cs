using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public DialogueManager theDM;
    public TestCode test;
    public InputField text;


    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        this.gameObject.SetActive(false);
    }
    public void disappearInput()
    {
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            test = FindObjectOfType<TestCode>();
            if (Input.GetKeyDown(KeyCode.Return))
            {      
                theDM.prom = false;
                theDM.StopAllCoroutines();
                theDM.ExitDialogue();
                disappearInput();
                if (test.answer == text.text)
                {
                    theDM.ShowDialogue(test.answerD);
                    if(test.additem == true)
                    {
                        test.ResultItem();
                    }
                    test.Yes=true;
                    text.text = "";
                    test.SetBool();
                }
                else
                {
                    theDM.ShowDialogue(test.noanswerD);
                }
                
            }
        }
    }
}
