using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T>
{
    BaseEnemy baseEnemy;
    EnemyView enemyView;
    float _timerForError;
    float timerMax=15;
    float _timerToIdle;
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
            var distanceWP = Vector3.Distance(baseEnemy.waypointsEnemy[baseEnemy.currentWaypoint].position, baseEnemy.transform.position);
          

            var roulleteWheel = new RoulleteWheel<bool>();
            var probabilitiesToChangeWaypoint = new List<Tuple<int, bool>> { /*new Tuple<int, bool>(3,true),*/
                                                                             new Tuple<int, bool>(10,false) };

            _timerToIdle += Time.deltaTime;
            if (_timerToIdle > 6)
            {
                var roulleteWheel2 = new RoulleteWheel<bool>();
                var probabilitieToIdle = new List<Tuple<int, bool>> { new Tuple<int, bool>(3,true),
                                                                                 new Tuple<int, bool>(6,false) };
                if (roulleteWheel.ProbabilityCalculator(probabilitieToIdle))
                    baseEnemy.Transition("Idle");
                _timerToIdle = 0;
            }
            if (distanceWP <= 50f)
            {
               
                
                if (roulleteWheel.ProbabilityCalculator(probabilitiesToChangeWaypoint))
                    baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length );
                else
                    baseEnemy.currentWaypoint++;

            }
            else
            {

                _timerForError += Time.deltaTime;
                if (_timerForError > timerMax)
                {
                    _timerForError = 0;
                    baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length);
                }


            }
            if (baseEnemy.currentWaypoint >= baseEnemy.waypointsEnemy.Length)
            {
                
                if (roulleteWheel.ProbabilityCalculator(probabilitiesToChangeWaypoint))
                    baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length );
                else
                    baseEnemy.currentWaypoint=0;
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
            baseEnemy.transform.forward = Vector3.Slerp(baseEnemy.transform.forward, dir, Time.deltaTime * 2).normalized;
            //baseEnemy.transform.position += baseEnemy.transform.forward * _speed * Time.deltaTime;
            baseEnemy.rb.MovePosition(baseEnemy.transform.position + baseEnemy.transform.forward * _speed * Time.deltaTime);
        }
    }
    IEnumerator changeSpeed()
    {
        while (true)
        {
            _speed = baseEnemy.speed * 1.5f;
            yield return new WaitForSeconds(.5f);

            _speed = baseEnemy.speed * 3f;
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
