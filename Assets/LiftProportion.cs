using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftProportion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Lift()
    {
        transform.GetComponentInParent<BaseEnemy>().ActiveRagdoll(true);
        GetComponent<Rigidbody>().AddExplosionForce(600000, transform.position-transform.up*-5, 5000,1000);


    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Lift();
    }
}
