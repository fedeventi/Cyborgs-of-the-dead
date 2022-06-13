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
        if (other.gameObject.tag == "Player" )
        {
            audioSource.Play();

            if(ps != null)
            {
                ps.Play();
            }

            if (player.weaponHolder.weaponsCollected.Count > 1)
            {
                player.weaponHolder.weaponsCollected[(int)WeaponType.Pistol].GetComponent<GunPistol>().currentMaxAmmo += 14;
                player.weaponHolder.weaponsCollected[(int)WeaponType.Pistol].GetComponent<GunPistol>().currentMaxAmmo += 6;
            }
           
            Destroy(gameObject, 0.2f);
            if (ps != null)
            {
                Destroy(ps, 2);
            }
        }
    }
}
