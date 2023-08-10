using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Daño hacia el jugador")]
    public int damage;

    //bool para saber si ya ha colisionado
    [Header("Bool")]
    public bool hasHit = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("golpe");
            other.gameObject.GetComponent<PlayerModel>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerModel>().toxicity += 2;
            hasHit = true;
            StartCoroutine(BoolHit());
        }
    }

    IEnumerator BoolHit()
    {
        yield return new WaitForSeconds(0.1f);
        hasHit = false;
        yield break;
    }
}
