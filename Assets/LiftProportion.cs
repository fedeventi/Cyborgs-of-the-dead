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
        StartCoroutine(AddForce());

    }
    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().AddExplosionForce(300000, transform.parent.position, 5000, 1000);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Lift();
    }
}
