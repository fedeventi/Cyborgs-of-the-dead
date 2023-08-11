using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoKit : MonoBehaviour , ICheckpoint
{
    PlayerModel player;

    public ParticleSystem ps;
    AudioSource audioSource;
    int[] ammoByWeapon = new int[4] {0,14,6,3 };
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
                ps.transform.position = transform.position;
                ps.Play();
            }

            if (player.weaponHolder.weaponsCollected.Count > 1)
            {
                for (int i = 1; i < player.weaponHolder.weaponsCollected.Count; i++)
                {
                    if(player.weaponHolder.weaponsCollected[i] != null)
                        player.weaponHolder.weaponsCollected[i].GetComponent<Weapon>().currentMaxAmmo += ammoByWeapon[i];

                }
                
            }
           
            gameObject.SetActive(false);
            
        }
    }

    public void Save()
    {

    }

    public void Restore()
    {
        gameObject.SetActive(true);
    }
}
