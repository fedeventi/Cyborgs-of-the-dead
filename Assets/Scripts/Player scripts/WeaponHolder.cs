using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject[] weaponsCollected;
    public int actualWeapon = 0;

    // 
    public Image[] weaponsImagesUI;

    //
    GameManager gameManager;
    PlayerView view;
    PlayerModel model;

    //
    bool hasPickUpPistol = false;
    bool hasPickUpShotgun = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        view = FindObjectOfType<PlayerView>();
        model = FindObjectOfType<PlayerModel>();
    }

    private void Update()
    {
        //pickeo de armas
        if(model.hasPickUpPistol && !hasPickUpPistol)
        {
            hasPickUpPistol = true;
            weapons[0] = weaponsCollected[0];
            weapons[0].SetActive(true);
            weaponsImagesUI[0].enabled=true;
            actualWeapon = 0;
            model.body.SetActive(true);
        }
        if (model.hasPickUpShotgun && !hasPickUpShotgun)
        {
            hasPickUpShotgun = true;
            weapons[1] = weaponsCollected[1];
            weapons[1].SetActive(true);
            weaponsImagesUI[1].enabled = true;
            actualWeapon = 1;
            model.body.SetActive(true);
        }

        if (model.hasPickUpPistol&&model.hasPickUpShotgun)
        {
            if (!weapons[actualWeapon].GetComponent<GunPistol>().model.isShooting && !model.isReloading)
            {
                ChangingWeapon();
                ActivateOrDeactivateGameObject();
            }

            
        }

        //está con la pistola
        if (actualWeapon == 0)
        {
            view.animator.SetBool("isPistol", true);
            view.animator.SetBool("isShotgun", false);
        }
        //está con la escopeta
        if (actualWeapon == 1)
        {
            view.animator.SetBool("isPistol", false);
            view.animator.SetBool("isShotgun", true);
        }
    }

    //función para activar o desactivar el gameobject de las armas
    void ActivateOrDeactivateGameObject()
    {
        weapons[actualWeapon].SetActive(true);
        weaponsImagesUI[actualWeapon].enabled = true;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i != actualWeapon)
            {
                weapons[i].SetActive(false);
                weaponsImagesUI[i].enabled = false;
            }
        }
    }
    //funcion para cambiar las armas con la ruedita 
    void ChangingWeapon()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(actualWeapon<weapons.Length-1)
                actualWeapon += 1;
            else
                actualWeapon = 0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (actualWeapon > 0)
                actualWeapon -= 1;
            else
                actualWeapon = weapons.Length-1;
        }
        
    }

}
