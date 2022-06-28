using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    // Start is called before the first frame update
    //public LayerMask layermask;
    string tag= "GlassFragments";

    AudioSource audioSource;
    
    
    void Start()
    {
        //Debug.Log((int)Mathf.Sqrt((int)layermask) / 2);

        audioSource = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "GlassFragments")
        {
            var obj= other.GetComponent<BreakGlass>();
            if (obj == null) return;
            obj.ReplaceGlass();
            //sonido de los vidrios rompiendose 
            if(audioSource != null)
                audioSource.PlayOneShot(audioSource.clip);
            foreach (var item in obj.glasses)
            {
                var rb = item.gameObject.GetComponent<Rigidbody>();

                rb.AddExplosionForce(500000, transform.position + transform.forward * 10, 60);
                Debug.Log("empujo");

            }
            Destroy(obj.gameObject);
        }
        
    }
    

    public void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }

}
