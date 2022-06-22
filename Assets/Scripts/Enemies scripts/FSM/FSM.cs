using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    State<T> currentState;
    public FSM(State<T> initState) //Esto sirve para saber cual va a ser el estado inicial. Se lo llama en el controller del enemy
    {
        SetInit(initState);
    }
    public void SetInit(State<T> init)
    {
        currentState = init;
        currentState.Awake();
    }
    public void OnUpdate()
    {
        currentState.Execute();
    }
    public void Transition(T input)
    {
        //Obtenes el estado al cual transicionar
        State<T> newState = currentState.GetState(input);
        //SI no existe cortas la funcion
        if (newState == null)
        {
            return;
        }
        //Reproducis el componente de salida del estado actual
        currentState.Sleep();
        //Cambias el estado actual por el nuevo estado
        currentState = newState;
        //Reproducis el componente de entrada del nuevo estado
        currentState.Awake();
    }
    public string GetState()
    {
        return currentState.ToString();
    }
}
