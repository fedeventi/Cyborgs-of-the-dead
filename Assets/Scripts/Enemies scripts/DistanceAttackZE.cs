using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttackZE : MonoBehaviour
{
    public GameObject collision;
    Rigidbody rb;
    public Vector3 destinyPosition;
    float _speed = 3f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       

    }
    void Update()
    {
        destinyPosition.y=transform.position.y;
        var distance=Vector3.Distance(destinyPosition,transform.position);
        transform.position+= ((destinyPosition-transform.position)+transform.up).normalized*_speed*distance*Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(collision,transform.position,transform.rotation);
        if(other.gameObject.GetComponent<PlayerModel>())
        {
            other.gameObject.GetComponent<PlayerModel>().life -= 15;
            other.gameObject.GetComponent<PlayerModel>().toxicity += 10;
            Destroy(gameObject);
        }
        if (other.gameObject.layer == 8)
        {
        
            Destroy(gameObject);
        }
        
    }
}
