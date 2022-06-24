using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    PlayerModel playerModel;
    float timePressed;
    public float timeToMove=3;
    bool pressed;
    public bool finished;
    public float speed = 10;
    Vector3 startPosition;
    Animator _animator;
    public float movementThreshold=600;
    void Start()
    {
        _animator = GetComponent<Animator>();
        startPosition = transform.position;
    }
    void Interaction(bool pressing)
    {
        pressed = pressing;
        if (pressed)
        {
            Debug.Log("presionado");
            
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            if (pressed)
            {
                if (timePressed < timeToMove)
                {
                    timePressed += Time.deltaTime;

                }
                else
                {
                    finished = true;
                }
            }
            else
                timePressed = 0;
        }
        else
        {
            if (Vector3.Distance(transform.position, startPosition) < 600)
            {
                    transform.position+=transform.right*-1*speed*Time.deltaTime;
                _animator.SetFloat("Movement", -1);
            }
            else
            {
                _animator.SetFloat("Movement", 0);
            }


            playerModel.interaction -= Interaction;
        }
                    
    }
    void OnTriggerEnter(Collider other)
    {
        playerModel=other.GetComponent<PlayerModel>();
        if (!finished)
        {
            playerModel.interaction += Interaction;
        }
    }
    void OnTriggerExit(Collider other)
    {
        playerModel.interaction -= Interaction;
    }
}
