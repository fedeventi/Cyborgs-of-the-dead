using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShot : MonoBehaviour
{
    [Header("Enemigo")]
    public BaseEnemy myEnemy;

    private void Update()
    {
        
     
    }
    void TakeDamage(float damage)
    {
        myEnemy.TakeDamage(damage * 1.5f);
    }
}
