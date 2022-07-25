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
    public override void Start()
    {
        //base.Start();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravity);
    }
}
