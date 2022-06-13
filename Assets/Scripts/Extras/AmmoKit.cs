using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoKit : MonoBehaviour
{
    PlayerModel player;

    public ParticleSystem ps;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerModel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && player.hasPickUpPistol || other.gameObject.tag=="Player" && player.hasPickUpShotgun)
        {
            audioSource.Play();

            ps.Play();

            if(player.hasPickUpPistol)
            {
                player.weaponHolder.weaponsCollected[0].GetComponent<GunPistol>().currentMaxAmmo += 14;
            }
            if(player.hasPickUpShotgun)
            {
                player.weaponHolder.weaponsCollected[1].GetComponent<GunPistol>().currentMaxAmmo += 6;
            }
            Destroy(gameObject, 0.2f);

            Destroy(ps, 2);
        }
    }
}
