using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : State<T>
{
    EnemyView enemyView;
    BaseEnemy baseEnemy;
    public IdleState(BaseEnemy enemy, EnemyView view)
    {
        baseEnemy = enemy;
        enemyView = view;
    }

    public override void Awake()
    {
        baseEnemy.isInIdle = true;
        baseEnemy.StartCoroutine(Wait(2.5f));
        
    }
    public override void Execute()
    {
        enemyView.IdleAnimation();
        
    }
    public override void Sleep()
    {
        baseEnemy.isInIdle = false;
    }
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        var transition = "Patrol";
        if (baseEnemy.LookingPlayer())
            transition = "Chase";

        baseEnemy.Transition(transition);
    }
}
