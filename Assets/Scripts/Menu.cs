using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public GameObject go;
    private bool Activated;

    public void Exit()
    {
        Application.Quit();
    }
    //유니티에서 게임으로 내보내면 게임종료가 활성화

    public void Continue()
    {
        Activated = false;
        go.SetActive(false);
    }
    //저장 버튼의 저장기능은 플레이어의 스크립트속 Callsave메소드 필요

    //public OrderManager orderManager;
    //구현 시키지 못한 메소드의 컴포넌트

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Activated = !Activated;
            if (Activated)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }
}
