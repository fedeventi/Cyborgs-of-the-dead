using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> glasses = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReplaceGlass()
    {

        

        foreach (var item in glasses)
        {
            item.gameObject.SetActive(true);
        }
        
    }
}
