using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ejecuta las funciones del model 

    //Componentes: 
    PlayerModel model;
    PlayerView view;
    Rigidbody body;
    private void Start()
    {
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        body=GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void FixedUpdate()
    {
        if(!model.isRunning)
        {
            model.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            view.MovementAnimation(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            model.isMoving = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            model.isRunning = true;
            model.Run(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            model.isRunning = false;
        }
        view.RunningAnimation(model.isRunning);

    }
    private void Update()
    {
        //Movimiento del jugador

        if(Input.GetAxisRaw("Horizontal")==0 || Input.GetAxisRaw("Vertical")==0)
        {
            model.isMoving = false;
        }

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
           // view.Step();
        }


       

        //Movimiento de la camara 
        model.RotationCamera();

        //Stunned 
        if(model.isStunned)
        {
            StartCoroutine(model.Stunned());
        }
    }
}
