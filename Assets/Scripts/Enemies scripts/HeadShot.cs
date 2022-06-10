using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShot : MonoBehaviour
{
    [Header("Transform de la cabeza del enemigo")]
    public Transform headTransform;
    [Header("Enemigo")]
    public BaseEnemy myEnemy;

    private void Update()
    {
        //Sigue la posicion actual del enemigo.
        if(myEnemy.life>0)
        {
            transform.position = headTransform.position;
        }
    }
}
