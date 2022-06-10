using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackState<T> : State<T>
{
    BaseEnemy baseEnemy;
    EnemyView enemyView;
    Action _action;
    float effectCadence;
    float effectCadenceMax=3;
    
    public AttackState(BaseEnemy enemy, EnemyView view)
    {
        baseEnemy = enemy;
        enemyView = view;
    }
    public AttackState(BaseEnemy enemy, EnemyView view, params Action[] actions)
    {
        baseEnemy = enemy;
        enemyView = view;
        foreach (Action action in actions)
            _action += action;
    }
    public override void Awake()
    {
        
    }
    public override void Execute()
    {
        if (!baseEnemy.InRangeToAttack()) baseEnemy.Transition("Chase");
        if (!baseEnemy.isDamage)
        {
            //ataca
            
           // baseEnemy.stoppingDistanceChase = 150;
            enemyView.AttackAnimation();
            if (_action != null)
                if (effectCadence >= effectCadenceMax)
                {
                    _action.Invoke();
                    effectCadence = 0;
                }
                else
                    effectCadence += Time.deltaTime;
        }

    }

   
}
