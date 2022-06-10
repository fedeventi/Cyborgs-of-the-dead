using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    public delegate void myDelegate(); //Puedo llamar a la funcion como una variable
    myDelegate myAction;

    public ActionNode(myDelegate action) //Llamo a las acciones
    {
        myAction = action;
    }
    public void ExecuteAction() 
    {
        myAction();
    }
}
