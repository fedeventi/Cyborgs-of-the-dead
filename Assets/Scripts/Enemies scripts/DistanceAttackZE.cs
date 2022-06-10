using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttackZE : MonoBehaviour
{
    private void Update()
    {
        transform.position += transform.forward * 50f * Time.deltaTime;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerModel>())
        {
            other.gameObject.GetComponent<PlayerModel>().life -= 10;
        }
    }
}
