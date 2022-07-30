using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gasoil : MonoBehaviour , ICheckpoint
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
    
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerModel>())
        {
             playerModel = other.GetComponent<PlayerModel>();
           
             
            if(playerModel)
                playerModel.Gas += amount;
            Debug.Log("consegui nafta");
            gameObject.SetActive(false);
            
                
            
        }
    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void Restore()
    {
        
        gameObject.SetActive(true);
        
    }
}
