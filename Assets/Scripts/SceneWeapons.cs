using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWeapons : MonoBehaviour
{
    PlayerModel player;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerModel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //PICKEO DE ARMAS
        if (this.gameObject.tag == "PistolScene" && other.gameObject.tag == "Player")
        {
            audioSource.Play();

            player.hasPickUpPistol = true;
            Destroy(gameObject, 0.2f);
        }
        if (this.gameObject.tag == "ShotgunScene" && other.gameObject.tag == "Player")
        {
            audioSource.Play();

            player.hasPickUpShotgun = true;
            Destroy(gameObject, 0.2f);
        }
    }
}
