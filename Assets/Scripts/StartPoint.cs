using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // 플레이어가 시작할 위치
    private MovingObject thePlayer;
    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<MovingObject>();

        if(startPoint == thePlayer.CurrentMapName)
        {
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            thePlayer.transform.position = this.transform.position;
        }
    }
}
