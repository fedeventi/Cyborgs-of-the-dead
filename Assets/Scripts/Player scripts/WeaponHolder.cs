using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using JoostenProductions;
public class WeaponHolder : MonoBehaviour
{
    public Animator animator;
    public Weapon[] weaponsCollected = new Weapon[3];
    public Weapon[] allWeapons;
    public WeaponType actualWeapon = 0;
    public Image weaponImg;
    public AnimatorOverrideController[] myClips=new AnimatorOverrideController[3];
    
    // 
    public Sprite[] weaponsImagesUI;

    //
    GameManager gameManager;
    PlayerView view;
    PlayerModel model;

    //
    //bool hasPickUpPistol = false;
    //bool hasPickUpShotgun = false;
   
    private void Start()
    {
        myClips[0] = new AnimatorOverrideController(animator.runtimeAnimatorController);
        gameManager = FindObjectOfType<GameManager>();
        view = FindObjectOfType<PlayerView>();
        model = FindObjectOfType<PlayerModel>();
    }

    public  void Update()
    {


        if (model.IsDead)
        {
            foreach (var item in weaponsCollected)
            {
                item.gameObject.SetActive(false);
            }
            return;
        }
        if (!model.isShooting && !model.isReloading)
        {
            ChangingWeapon();
            if(weaponsCollected[(int)actualWeapon]!=null)
                ActivateOrDeactivateGameObject();
        }


        

       ChangeAnimator();
    }

    //función para activar o desactivar el gameobject de las armas
    void ActivateOrDeactivateGameObject()
    {
        
        if(weaponsCollected.Length>(int)actualWeapon)
            weaponsCollected[(int)actualWeapon].gameObject.SetActive(true);
        weaponImg.sprite = weaponsImagesUI[(int)actualWeapon];
        animator.runtimeAnimatorController = myClips[(int)actualWeapon];

        for (int i = 0; i < weaponsCollected.Length; i++)
        {
            if (i != (int)actualWeapon)
            {
                if(weaponsCollected[i]!=null)
                    weaponsCollected[i].gameObject.SetActive(false);
                
            }
        }
    }
    //funcion para cambiar las armas con la ruedita 
    void ChangingWeapon()
    {
        if (weaponsCollected.Length < 1) return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if ((int)actualWeapon < weaponsCollected.Length - 1)
            {
                actualWeapon += 1;
                if (weaponsCollected[(int)actualWeapon] == null)
                    actualWeapon += 1;
            }
            else
                actualWeapon = 0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (actualWeapon > 0)
            {
                actualWeapon -= 1;
                if (weaponsCollected[(int)actualWeapon] == null)
                    actualWeapon -= 1;

            }
            else
                actualWeapon = (WeaponType)weaponsCollected.Length -1;
        }
        
    }
    void ChangeAnimator()
    {

        view.animator.SetBool(actualWeapon.ToString(), true);
        foreach (WeaponType enumValue in Enum.GetValues(typeof(WeaponType)))
        {
            if(enumValue!=actualWeapon)
            view.animator.SetBool(enumValue.ToString(), false);
        }

    }
}
    public enum WeaponType
    {
        Hammer,
        Pistol,
        Shotgun
    }
