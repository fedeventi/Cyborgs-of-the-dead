using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWeapons : MonoBehaviour
{
    public WeaponType WeaponType;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //PICKEO DE ARMAS
        if (other.gameObject.tag == "Player")
        {
            audioSource.Play();
            var poket = other.GetComponentInChildren<WeaponHolder>();
            poket.weaponsCollected[(int)WeaponType] =poket.allWeapons[(int)WeaponType];
            Debug.Log("agarro arma");
            poket.actualWeapon = WeaponType;
            gameObject.SetActive(false);
        }
        
    }
}
