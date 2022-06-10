using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    // Start is called before the first frame update
    //public LayerMask layermask;
    public string tag= "GlassFragments";
    void Start()
    {
        //Debug.Log((int)Mathf.Sqrt((int)layermask) / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == tag)
        {
            var rb= other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.isKinematic = false;
                
                rb.AddExplosionForce(5000000, transform.position+transform.forward*10, 60);
                
                Debug.Log("exploto");
            }
        }
        
    }
    public void OnCollisionEnter(Collision collision)
    {
    }
}
