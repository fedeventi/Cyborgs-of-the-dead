using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>
{
    public virtual void Awake()
    {

    }
    public virtual void Execute()
    {

    }
    public virtual void Sleep()
    {

    }

    Dictionary<T, State<T>> states = new Dictionary<T, State<T>>(); //Diccionario que va a almacenar los estados

    public void AddTransition(T input, State<T> state) //Añade transiciones a traves del diccionario crado
    {
        if (!states.ContainsKey(input))
        {
            states.Add(input, state);
        }
    }

    public State<T> GetState(T input) //Agarra el estado actual
    {
        if (states.ContainsKey(input))
        {
            return states[input];
        }
        return null;
    }
}
