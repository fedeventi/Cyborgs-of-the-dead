using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T>
{
    BaseEnemy baseEnemy;
    EnemyView enemyView;
    float timerMax;
    float _speed;
    bool _lame;
    public PatrolState(BaseEnemy enemy, EnemyView view)
    {
        baseEnemy = enemy;
        enemyView = view;
    }
    public PatrolState(BaseEnemy enemy, EnemyView view, bool lame)
    {
        baseEnemy = enemy;
        enemyView = view;
        _lame = lame;
    }
    public override void Execute()
    {
        if(!baseEnemy.isDamage)
        {
            Patrol();
        }
        if (baseEnemy.LookingPlayer()) baseEnemy.Transition("Chase");
    }

    public void Patrol()
    {
        
        
        if (baseEnemy.currentWaypoint < baseEnemy.waypointsEnemy.Length && !baseEnemy.isInIdle)
        {
            

            enemyView.WalkingAnimation();
            Vector3 target = baseEnemy.waypointsEnemy[baseEnemy.currentWaypoint].position;
            target.y = baseEnemy.transform.position.y;
            if (target == Vector3.zero)
            {
                baseEnemy.currentWaypoint++;
            }
            var distanceWP = Vector3.Distance(baseEnemy.waypointsEnemy[baseEnemy.currentWaypoint].position, baseEnemy.transform.position);
            //cuando esta cerca del waypoint, se pone  en idle.
            if (distanceWP <= 50f)
            {
                //baseEnemy.StartCoroutine(baseEnemy.WaitOnWaypoint());
                baseEnemy.currentWaypoint++;
            }
            if (baseEnemy.currentWaypoint >= baseEnemy.waypointsEnemy.Length)
            {
                baseEnemy.currentWaypoint = 0;
            }
            
            Vector3 dir = target - baseEnemy.transform.position;
            if (baseEnemy.targetDetection.MyClosestObstacle())
            {


                dir = (baseEnemy.targetDetection.MyClosestPointToTarget(target) - baseEnemy.transform.position);

            }
            else
            {
                dir = target - baseEnemy.transform.position;
                
            }
            
            
            //rota mirando al target
            baseEnemy.transform.forward = Vector3.Slerp(baseEnemy.transform.forward, dir, Time.deltaTime * 5).normalized;
            baseEnemy.transform.position += baseEnemy.transform.forward * _speed*Time.deltaTime;
        }
    }
    IEnumerator changeSpeed()
    {
        while (true)
        {
            _speed = baseEnemy.speed*3f;
            yield return new WaitForSeconds(.5f);

            _speed = baseEnemy.speed * 1.5f;
            yield return new WaitForSeconds(.3f);
            
        }
    }
    public override void Awake()
    {
        baseEnemy.isInPatrol = true;
        if (_lame)
            baseEnemy.StartCoroutine(changeSpeed());
        else
            _speed = baseEnemy.speed * 3f;
    }
    public override void Sleep()
    {
        baseEnemy.isInPatrol = false;
        if (_lame)
            baseEnemy.StopCoroutine(changeSpeed());
        else
            _speed = baseEnemy.speed * 3f;
    }

}
