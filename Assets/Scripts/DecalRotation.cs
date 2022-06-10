using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalRotation : MonoBehaviour
{
    public Transform positionDecal;

    private void Update()
    {
        if(positionDecal!=null)
        {
            transform.position = positionDecal.position;
        }

        transform.Rotate(0, 0, 1);

        if(positionDecal==null)
        {
            Destroy(gameObject);
        }
    }
}
