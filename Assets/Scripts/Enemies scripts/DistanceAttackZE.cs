using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttackZE : MonoBehaviour
{
    public GameObject collision;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward*1000+transform.up*100,ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerModel>())
        {
            other.gameObject.GetComponent<PlayerModel>().life -= 10;
            Instantiate(collision,transform.position,transform.rotation);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == 8)
        {
            Instantiate(collision, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
