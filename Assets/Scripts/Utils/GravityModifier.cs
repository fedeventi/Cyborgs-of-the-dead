using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [Range(0,100)]
    public float gravity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravity);
    }
}
