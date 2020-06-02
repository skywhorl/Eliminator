using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 자식클레스같은 느낌
public class Dialogue 
{
    [TextArea(1,2)] //Public으로 밖에서 입력할때(1개당,2줄씩 보기편하게) 
    public string[] sentences; //대사
    public Sprite[] sprites; //캐릭터 스프라이트
    public Sprite[] dialogueWindows; //대화창 스프라이트   
}
