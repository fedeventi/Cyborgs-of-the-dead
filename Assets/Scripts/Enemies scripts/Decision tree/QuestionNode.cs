using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{
    public delegate bool myDelegate(); //Puedo llamar a la funcion como una variable
    myDelegate question;

    INode trueNode;
    INode falseNode;

    public QuestionNode(myDelegate q, INode trueN, INode falseN) //Llamo a las preguntas
    {
        question = q;
        trueNode = trueN;
        falseNode = falseN;
    }

    public void ExecuteAction()
    {
        if (question())
        {
            trueNode.ExecuteAction();
        }
        else
        {
            falseNode.ExecuteAction();
        }
    }
}
