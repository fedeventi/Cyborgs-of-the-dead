using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoKit : MonoBehaviour , ICheckpoint
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
                ps.transform.position = transform.position;
                ps.Play();
            }

            if (player.weaponHolder.weaponsCollected.Length > 1)
            {
                for (int i = 1; i < player.weaponHolder.weaponsCollected.Length; i++)
                {
                    player.weaponHolder.weaponsCollected[i].GetComponent<Weapon>().currentMaxAmmo += ammoByWeapon[i];

                }
                
            }
           
            gameObject.SetActive(false);
            
        }
    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void Restore()
    {
        gameObject.SetActive(true);
    }
}
