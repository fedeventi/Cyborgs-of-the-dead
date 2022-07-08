using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoostenProductions;
public class ExplosionZoneZE : OverridableMonoBehaviour
{
    [Header("Transform")]
    public Transform transformZone;
    [Header("Enemigo")]
    public ExplosiveZombie myEnemy;

    [Header("Bool")]
    public bool hasCollision = false;

    public override void UpdateMe()
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
