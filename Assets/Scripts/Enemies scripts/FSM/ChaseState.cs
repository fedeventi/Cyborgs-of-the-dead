﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState<T> : State<T>
{
    BaseEnemy _baseEnemy;
    EnemyView enemyView;
    float _speed;
    float rotationTime = 0;
    public ChaseState(BaseEnemy enemy, EnemyView view)
    {
        _baseEnemy = enemy;
        enemyView = view;
    }
    public override void Execute()
    {

        CheckDistanceWithTarget();
        if (!_baseEnemy.isDamage)
        {
            FollowPlayer();
            _baseEnemy.targetDetection.DidMyPositionChange(0.7f);
        }

    }

    public void FollowPlayer()
    {
        _speed = _baseEnemy.speed * 5;
        enemyView.ChasingAnimation();
        var target = _baseEnemy.player.transform.position;
        target.y = _baseEnemy.transform.position.y;


        Vector3 dir;

        if (_baseEnemy.targetDetection.MyClosestObstacle())
        {


            dir = _baseEnemy.targetDetection.MyClosestPointToTarget(target) - _baseEnemy.transform.position;

        }
        else
        {
            dir = target - _baseEnemy.transform.position;


        }
        var rot = Quaternion.LookRotation(dir.normalized);
        _baseEnemy.transform.rotation = Quaternion.Slerp(_baseEnemy.transform.rotation, rot, Time.deltaTime);
        _baseEnemy.rb.velocity += _baseEnemy.transform.forward * _speed * Time.deltaTime;

    }
    public void CheckDistanceWithTarget()
    {
        var distance = Vector3.Distance(_baseEnemy.player.transform.position, _baseEnemy.transform.position);
        // if (distance > _baseEnemy.viewDistance) _baseEnemy.Transition("Patrol");

        if (_baseEnemy.InRangeToAttack())
        {
            _baseEnemy.Transition("Attack");
        }
    }

    public override void Sleep()
    {
        base.Sleep();

    }
}
