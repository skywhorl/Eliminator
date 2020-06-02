using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroys : MonoBehaviour
{
    public PlayerManager player;
    public DialogueManager can;
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        can = FindObjectOfType<DialogueManager>();
        Destroy(player.gameObject);
        Destroy(can.gameObject);
    }

}
