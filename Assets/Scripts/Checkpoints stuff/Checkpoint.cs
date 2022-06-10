using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameMaster gameMaster;
    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameMaster.lastCPos = transform.position;
        }
    }
}
