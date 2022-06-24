using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gasoil : MonoBehaviour
{
    // Start is called before the first frame update
    public int amount;
    PlayerModel playerModel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FillGas( bool pressed)
    {
        if (pressed)
        {
            playerModel.Gas += amount;
            playerModel.interaction -= FillGas;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerModel>())
        {
             playerModel = other.GetComponent<PlayerModel>();
             playerModel.interaction = FillGas;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            playerModel.interaction -= FillGas;
        }
    }
}
