using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GasVehicle : MonoBehaviour
{
    PlayerModel model;
    public float amount;
    public float required = 100;
    bool pressed;
    Action<float, bool> _view;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Interaction(bool pressing)
    {
        if (model.Gas < 0) return;
        pressed = pressing;
        if (pressed)
        {
            

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!model) return;
        if (amount < required)
        {
            if (pressed)
            {
                if (model.Gas > 0)
                {

                     model.Gas -= Time.deltaTime*5;
                     amount+=Time.deltaTime*5;
                    if (_view != null)
                        _view(amount / required, true);
                }

            }
            else
            {
                if (_view != null)
                    _view(amount, false);
            }
        }

        else
        {
            
            if (_view != null)
            {
                _view(amount, false);
                model.interaction -= Interaction;
            }
            Debug.Log("Ganaste");
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        model = other.GetComponent<PlayerModel>();
        if (model != null)
        {
           if(amount < required)
            {
                model.interaction += Interaction;
                _view += model.GetComponent<PlayerView>().SetInteractionTimer;
            }
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (model != null)
        {
            pressed = false;
            _view(amount, false);
            model.interaction -= Interaction;
            _view -= model.GetComponent<PlayerView>().SetInteractionTimer;
        }
    }
}
