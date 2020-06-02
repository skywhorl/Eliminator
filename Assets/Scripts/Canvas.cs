using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public static Canvas instance;
    public InputManager inputs;
    public DialogueManager Dm;

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

    public void AppearInput()
    {
        inputs.gameObject.SetActive(true); //보이게 하기
        Dm.prom = true; //문제 출력중
    }
    
}
