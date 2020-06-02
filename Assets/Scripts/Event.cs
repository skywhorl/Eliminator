using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!flag && Input.GetKey(KeyCode.Space)&&thePlayer.animator.GetFloat("DirY")==1f)
        {
            flag = true;
            //StartCoroutine(EventCoroutine());
        } 
    }

    //IEnumerator EventCoroutine()
    //{
    //    theOrder.PreLoadCharacter();

    //    theOrder.not
    //}
    // Update is called once per frame
    void Update()
    {
        
    }
}
