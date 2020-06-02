﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName; // 이동할 맵의 이름
    public string transferSceneName; //이동할 씬의 이름

    private MovingObject thePlayer;
    private PlayerManager thepm;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
        thepm = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            thepm.CurrentSceneName = transferSceneName;
            thePlayer.CurrentMapName = transferMapName;
            thePlayer.CurrentSceneName = transferSceneName;
            SceneManager.LoadScene(transferSceneName);
        }
    }
}