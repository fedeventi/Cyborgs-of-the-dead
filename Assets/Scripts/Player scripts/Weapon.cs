using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public RaycastHit hit;
    [Header("Collision Mask")]
    public LayerMask collisionMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Shoot()
    {

    }
    public virtual void ShootNoAmmo()
    {

    }
}
