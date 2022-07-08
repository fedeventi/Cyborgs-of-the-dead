using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeKit : MonoBehaviour
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

                ps.Play();

                other.gameObject.GetComponent<PlayerModel>().life = 100;
                //Destroy(ps, 2);
                Destroy(gameObject, 0.5f);

            }
        }
    }
}
