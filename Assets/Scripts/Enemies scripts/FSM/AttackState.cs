using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackState<T> : State<T>
{
    BaseEnemy baseEnemy;
    EnemyView enemyView;
    Action _action;
    float _attackCadence;
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
        _attackCadence = 0;
        enemyView.AttackAnimation();
    }
    public override void Execute()
    {

        
        if (!baseEnemy.isDamage)
        {
            
            AimToTarget();
            Attack();
            ExecuteSecondaryEffect();
        }

    }
    public void Attack()
    {
        
        _attackCadence += Time.deltaTime;
        if (_attackCadence > 1.5f)
        {
            if (baseEnemy.InRangeToAttack())
                    enemyView.AttackAnimation();    
            else
                baseEnemy.Transition("Chase");

            _attackCadence = 0;
        }

    }
    public void AimToTarget()
    {
        var target = baseEnemy.player.transform.position;
        target.y = baseEnemy.transform.position.y;
        Vector3 dir = target - baseEnemy.transform.position;
        var rot = Quaternion.LookRotation(dir);
        baseEnemy.transform.rotation = Quaternion.Slerp(baseEnemy.transform.rotation, rot, Time.deltaTime * 2).normalized;
    }
    public void ExecuteSecondaryEffect()
    {
        if (_action != null)
            if (effectCadence >= effectCadenceMax)
            {
                _action.Invoke();
                effectCadence = 0;
            }
            else
                effectCadence += Time.deltaTime;
    }
    public override void Sleep()
    {
        
        
    }

}
