using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasVehicle : MonoBehaviour
{
    public float amount;
    float required = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(amount > required)
        {
            Debug.Log("Ganaste");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().index+1);
        }
    }
}
