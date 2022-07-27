using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInfo : MonoBehaviour
{
    public Image[] images;
    public int myZone;

    private void OnTriggerEnter(Collider other)
    {
        

    }

    private void Update()
    {

        
            foreach (var item in images)
            {
                item.enabled = false;
            }

            if(myZone<images.Length)
                images[myZone].enabled = true;
            


        if (myZone==0)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                myZone = 1;
               

            }
                
                
        }

        if(myZone==1)
        {
            if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
            {
                myZone = 2;
                
               
            }
        }

        if (myZone == 2)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                myZone = 3;
               
            }
        }

        if (myZone == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                myZone = 4;
                
            }
        }
    }
}
