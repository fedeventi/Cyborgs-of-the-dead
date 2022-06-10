using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneZE : MonoBehaviour
{
    [Header("Transform")]
    public Transform transformZone;
    [Header("Enemigo")]
    public ExplosiveZombie myEnemy;
    [Header("Explosion zone")]
    public ExplosionZoneZE explosionZone;

    private void Update()
    {
        //Sigue la posicion actual del enemigo.
        if (myEnemy.life > 0)
        {
            transform.position = transformZone.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (explosionZone.hasCollision && other.gameObject.tag == "Player")
        {
            explosionZone.hasCollision = false;
            other.gameObject.GetComponent<PlayerModel>().life -= 50;
            other.gameObject.GetComponent<PlayerModel>().toxicity += 50;
        }
    }
}
