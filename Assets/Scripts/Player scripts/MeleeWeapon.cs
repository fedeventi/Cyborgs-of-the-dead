using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Componentes")]
    public Camera myCamera;

    [Header("DAMAGE")]
    public int damage;

    //Bool
    bool isAttacking = false;

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
            
        }
    }


    IEnumerator BoolAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag=="Enemy" && !isAttacking)
        //{
        //    StartCoroutine(BoolAttack());

        //    var enemy = collision.gameObject.GetComponent<BaseEnemy>();
        //    enemy.TakeDamage(damage);
        //    var bloodEffect = Instantiate(enemy.bloodSpray, enemy.transform.position, enemy.transform.rotation);
        //    Destroy(bloodEffect, 0.4f);
        //}
    }

}
