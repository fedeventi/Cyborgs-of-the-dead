using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInfo : MonoBehaviour
{
    public Image myImage;
    public int myZone;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            myImage.enabled = true;
        }

    }

    private void Update()
    {
        if(myZone==0)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                myImage.enabled = false;
                Destroy(this.gameObject);
            }
                
                
        }

        if(myZone==1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                myImage.enabled = false;
                Destroy(this.gameObject);
            }
        }

        if (myZone == 2)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                myImage.enabled = false;
                Destroy(this.gameObject);
            }
        }

        if (myZone == 3)
        {
            if (Input.GetMouseButton(1))
            {
                myImage.enabled = false;
                Destroy(this.gameObject);
            }
        }
    }
}
