using System.Collections;
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

    private void Update()
    {
        //Actualiza el texto de la munición.
        if (ammoText != null)
        {
            ammoText.text = "";
        }

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
        //CheckEnemy();
        base.Shoot();
        var shootString = combo?"shoot":"secondShoot";
        animator.SetTrigger(shootString);
        animator.SetBool("idle", false);
        animator.SetBool("walking", false);
        animator.SetBool("running", false);
    }
    public void CheckEnemy()
    {
        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 50))
        {
            var target = hit.transform.GetComponent<BaseEnemy>();
            if (target != null && !target.isDead)
            {
                Debug.Log("toma");
                target.TakeDamage(damage);
                var bloodEffect = Instantiate(target.bloodSpray, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffect, 0.5f);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            var target = collision.transform.GetComponent<BaseEnemy>();
            if (target != null && !target.isDead)
            {
                Debug.Log("toma collision");
                target.TakeDamage(damage);
                var bloodEffect = Instantiate(target.bloodSpray, transform.forward * 50, Quaternion.LookRotation(transform.forward * -1));
                Destroy(bloodEffect, 0.5f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            var target = other.transform.GetComponent<BaseEnemy>();
            if (target != null && !target.isDead)
            {
              
                target.TakeDamage(damage);
                var bloodEffect = Instantiate(target.bloodSpray, myCamera.transform.position+myCamera.transform.forward*50,Quaternion.LookRotation(transform.forward*-1));
                Destroy(bloodEffect, 0.5f);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(myCamera.transform.position, myCamera.transform.forward*50);
    }

}
