using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JoostenProductions;

public class Truck : OverridableMonoBehaviour
{
    PlayerModel playerModel;
    float timePressed;
    public float timeToMove = 3;
    bool pressed;
    public bool finished;
    public float speed = 10;
    Vector3 startPosition;
    Animator _animator;
    Action<float, bool> _view;
    public float movementThreshold = 600;
    AudioSource audioSource;
    public List<AudioClip> clips = new List<AudioClip>();
    public Vector3 interactionPosition;
    void Start()
    {
        _animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }
    void Interaction(bool pressing)
    {
        pressed = pressing;
        if (pressed)
        {
            Debug.Log("presionado");
            if (audioSource != null)
                audioSource.PlayOneShot(clips[0]);

        }

    }
    // Update is called once per frame
    public override void UpdateMe()
    {
        if (!finished)
        {
            if (pressed)
            {
                if (timePressed < timeToMove)
                {
                    timePressed += Time.deltaTime;
                    _view(timePressed / timeToMove, true);
                }
                else
                {
                    finished = true;
                    _view(1, false);
                }
            }
            else
            {
                timePressed = 0;
                if (_view != null)
                    _view(0, false);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPosition) < 600)
            {
                transform.position += transform.right * -1 * speed * Time.deltaTime;
                _animator.SetFloat("Movement", -1);
            }
            else
            {
                _animator.SetFloat("Movement", 0);
                audioSource.Stop();
            }

            if (playerModel != null)
                playerModel.interaction -= Interaction;
        }

    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position+interactionPosition, 10);
    }
    public void OnTriggerEnter(Collider other)
    {
        playerModel=other.GetComponent<PlayerModel>();
        if (playerModel != null)
        {
            if (!finished)
            {
                playerModel.interaction += Interaction;
                _view += playerModel.GetComponent<PlayerView>().SetInteractionTimer;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        playerModel = other.GetComponent<PlayerModel>();
        if (playerModel != null)
        {
            playerModel.GetComponent<PlayerView>().InteractionImage(transform.position+interactionPosition,pressed?false:true);
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (playerModel != null)
        {
            pressed = false;
            playerModel.interaction -= Interaction;
            _view -= playerModel.GetComponent<PlayerView>().SetInteractionTimer;
            playerModel.GetComponent<PlayerView>().InteractionImage(transform.position,false);
        }
    }

    public void MovingSound()
    {
        audioSource.PlayOneShot(clips[1]);
    }
}
