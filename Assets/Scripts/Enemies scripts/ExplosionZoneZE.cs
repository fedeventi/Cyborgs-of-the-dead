using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionZoneZE : MonoBehaviour
{
    [Header("Transform")]
    public Transform transformZone;
    [Header("Enemigo")]
    public ExplosiveZombie myEnemy;

    [Header("Bool")]
    public bool hasCollision = false;

    public  void Update()
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

    
}
