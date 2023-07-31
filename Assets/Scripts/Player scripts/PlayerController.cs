using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoostenProductions;
public class PlayerController : OverridableMonoBehaviour
{
    //ejecuta las funciones del model 

    //Componentes: 
    PlayerModel model;
    PlayerView view;
    Rigidbody body;
    Guide guide;
    bool canControlPlayer = true;
    public override void Start()
    {
        base.Start();
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        body = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        guide = GetComponentInChildren<Guide>();
    }
    public override void FixedUpdateMe()

    {

        if (!canControlPlayer) return;
        if (!model.isRunning)
        {
            model.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            view.MovementAnimation(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            model.isMoving = true;

        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0)
        {
            if (model.exhausted)
            {
                model.isRunning = false;
                return;
            }
            model.isRunning = true;
            model.Run(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            model.isRunning = false;
        }
        view.RunningAnimation(model.isRunning);



        //if (Input.GetKeyDown(KeyCode.E))
        //{

        //}
        //else if (Input.GetKeyUp(KeyCode.E))
        //{
        //    model.Interact(false);
        //}

    }

    public override void UpdateMe()
    {
        //Movimiento del jugador
        if (guide)
            canControlPlayer = !guide.Show;

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            model.isMoving = false;
            model.isRunning = false;
        }

        model.Interact(Input.GetKey(KeyCode.E) && !model.IsDead);






        //Stunned 
        if (model.isStunned)
        {
            StartCoroutine(model.Stunned());
        }
    }
    public void LateUpdate()
    {
        if (canControlPlayer)
            model.RotationCamera();
        else
        {
            view.MovementAnimation(0, 0);
            model.LookTowards(guide.location);
        }
    }
}
