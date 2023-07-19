using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 50f;
    }

    // Update is called once per frame
    void Update()
    {

        timer--;

        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
