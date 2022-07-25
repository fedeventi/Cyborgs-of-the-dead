using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoostenProductions;
public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public RaycastHit hit;
    [Header("Collision Mask")]
    public LayerMask collisionMask;
    [Header("Ammo")]
    public int maxClip;
    public int maxAmmo;
    public int currentAmmo;
    public int currentMaxAmmo;
   

    // Update is called once per frame
    //public override void UpdateMe()
    //{
        
    //}
    public virtual void Shoot()
    {

    }
    public virtual void ShootNoAmmo()
    {

    }
}
