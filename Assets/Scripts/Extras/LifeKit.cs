using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeKit : MonoBehaviour , ICheckpoint
{
    public ParticleSystem ps;
    AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerModel>().life<100)
            {
                audioSource.Play();
                ps.transform.position = transform.position+Vector3.up*10;
                
                ps.Play();

                other.gameObject.GetComponent<PlayerModel>().life = 100;
                //Destroy(ps, 2);
                gameObject.SetActive(false);

            }
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
