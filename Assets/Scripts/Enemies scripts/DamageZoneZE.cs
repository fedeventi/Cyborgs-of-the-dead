using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneZE : MonoBehaviour
{
    
    
    public int damage;
   


    private void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
         
            other.gameObject.GetComponent<PlayerModel>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerModel>().toxicity+=50;
            GetComponent<SphereCollider>().enabled = false;
            
        }
    }
}
