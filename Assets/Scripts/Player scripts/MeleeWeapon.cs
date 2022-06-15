﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : Weapon
{
    [Header("Componentes")]
    public Camera myCamera;

    [Header("DAMAGE")]
    public int damage=30;

    //Bool
    public bool isAttacking = false;
    bool combo;
    [Header("UI")]
    public Text ammoText;
    [Header("PARTICULA ")]
    public GameObject particle;
    public TimeManager timeManager;
    private void Update()
    {
        //Actualiza el texto de la munición.
        if (ammoText != null)
        {
            ammoText.text = "";
        }

        if(timeManager ==null) timeManager =FindObjectOfType<TimeManager>();

        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                Shoot();
                combo=!combo;
            }
        }
    }


    
    
    public override void Shoot()
    {
        
        base.Shoot();
        var shootString = combo?"shoot":"secondShoot";
        animator.SetTrigger(shootString);
        animator.SetBool("idle", false);
        animator.SetBool("walking", false);
        animator.SetBool("running", false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            var target = other.transform.GetComponent<BaseEnemy>();
            bool headshot = other.transform.gameObject.tag == "headshot";
            var _damage = headshot ? damage * 3 : damage;
            
            if (headshot)
            {
                target = other.transform.GetComponentInParent<BaseEnemy>();
            }
            if (target != null )
            {
                if(!target.isDead)
                    target.TakeDamage(_damage,headshot);
                var bloodEffect = Instantiate(target.bloodSpray, myCamera.transform.position+myCamera.transform.forward*50,Quaternion.LookRotation(transform.forward*-1));
                //Destroy(bloodEffect, 0.5f);
            }
        }
        if (other.gameObject.layer == 8)
        {
            var obj = other.GetComponent<BreakGlass>();
            var _myParticle=Instantiate(particle, myCamera.transform.position + myCamera.transform.forward * 50, transform.rotation);
            _myParticle.transform.localScale = Vector3.one*10;
            if (obj == null) return;
            obj.ReplaceGlass();
            Destroy(obj.gameObject);
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(myCamera.transform.position, myCamera.transform.forward*50);
    }
    
}
