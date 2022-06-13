using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearDamageShotgun : MonoBehaviour
{
    //le agrega daño a la escopeta cuando esta cerca del enemigo.

    WeaponHolder weaponHolder;

    private void Start()
    {
        weaponHolder = FindObjectOfType<WeaponHolder>();
    }

    private void OnTriggerStay(Collider other)
    {
        //layer del enemigo
        if (other.gameObject.layer == 9 && !weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().onZoneMoreDamageShotgun)

        {
            weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().onZoneMoreDamageShotgun = true;
            weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().damage = 65;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //layer del enemigo
        if (other.gameObject.layer == 9)
        {
            weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().onZoneMoreDamageShotgun = false;
            weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().damage = weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<GunPistol>().startDamage;
        }

    }
}
