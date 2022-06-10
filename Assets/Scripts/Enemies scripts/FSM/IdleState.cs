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
    }
    public override void Execute()
    {
        enemyView.IdleAnimation();
        baseEnemy.speed = 0;
    }
    public override void Sleep()
    {
        baseEnemy.isInIdle = false;
    }
}
