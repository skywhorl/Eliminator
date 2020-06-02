using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private MovingObject thePlayer;
    private CameraManager theCamera;

    public void LoadStart() {
        StartCoroutine(LoadWaitCoroutine());
    }

    IEnumerator LoadWaitCoroutine() {
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<MovingObject>();
        theCamera = FindObjectOfType<CameraManager>();

        theCamera.target = GameObject.Find("Player");
    }
}

