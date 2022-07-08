using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoostenProductions;
public class GravityModifier : OverridableMonoBehaviour
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
    public override void FixedUpdateMe()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravity);
    }
}
