using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : State<T>
{
    EnemyView enemyView;
    BaseEnemy baseEnemy;
    float timer;
    public IdleState(BaseEnemy enemy, EnemyView view)
    {
        baseEnemy = enemy;
        enemyView = view;
    }

    public override void Awake()
    {
        baseEnemy.isInIdle = true;


    }
    public override void Execute()
    {
        // if (Vector3.Distance(baseEnemy.transform.position, baseEnemy.player.transform.position) < 5000)
        // {
        if (baseEnemy.gameObject.activeSelf)
            Wait(2.5f);
        // }
        enemyView.IdleAnimation();
    }
    public override void Sleep()
    {
        baseEnemy.isInIdle = false;
    }
    public void Wait(float seconds)
    {
        if (timer >= seconds)
        {
            var transition = "Chase";
            // var transition = "Search";
            // if (baseEnemy.LookingPlayer())
            //     transition = "Chase";

            baseEnemy.Transition(transition);
            timer = 0;
        }
        timer += Time.deltaTime;
    }

}
