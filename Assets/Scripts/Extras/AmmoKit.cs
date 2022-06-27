using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoKit : MonoBehaviour
{
    PlayerModel player;

    public ParticleSystem ps;
    AudioSource audioSource;
    int[] ammoByWeapon = new int[3] {0,14,6 };
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
                for (int i = 1; i < player.weaponHolder.weaponsCollected.Count; i++)
                {
                    player.weaponHolder.weaponsCollected[i].GetComponent<Weapon>().currentMaxAmmo += ammoByWeapon[i];

                }
                
            }
           
            Destroy(gameObject, 0.2f);
            if (ps != null)
            {
                Destroy(ps, 2);
            }
        }
    }
}
