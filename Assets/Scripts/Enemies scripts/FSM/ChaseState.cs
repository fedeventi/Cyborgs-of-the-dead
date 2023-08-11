using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState<T> : State<T>
{
    BaseEnemy _baseEnemy;
    EnemyView enemyView;
    float _speed;
    float _rotationTime = 0.6f;
    NavMeshAgent _agent;
    public ChaseState(BaseEnemy enemy, EnemyView view)
    {
        _baseEnemy = enemy;
        enemyView = view;
        _agent = enemy.GetComponent<NavMeshAgent>();
    }
    public override void Awake()
    {
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
    public void SearchEnemy()
    {
        _agent.destination = _baseEnemy.player.transform.position;

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



        var direction = Vector3.RotateTowards(_baseEnemy.transform.forward, dir, _rotationTime * Time.deltaTime, 0);
        _baseEnemy.transform.rotation = Quaternion.LookRotation(direction);



        _baseEnemy.rb.velocity += _baseEnemy.transform.forward * _speed * Time.deltaTime;
        if (!_baseEnemy.LookingPlayer()) _baseEnemy.Transition("Search");
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
public class SearchState<T> : State<T>
{
    BaseEnemy _baseEnemy;
    EnemyView enemyView;
    float _speed;

    NavMeshAgent _agent;
    NavMeshPath _path = new NavMeshPath();
    int _currentWaypoint = 0;
    Vector3[] waypoints;
    public SearchState(BaseEnemy enemy, EnemyView view)
    {
        _baseEnemy = enemy;
        enemyView = view;
        _agent = enemy.GetComponent<NavMeshAgent>();
    }
    public override void Awake()
    {
        _agent.CalculatePath(_baseEnemy.player.transform.position, _path);
        _currentWaypoint = 0;
        Debug.Log("buscando");
    }

    public override void Execute()
    {

        GoToPlayer();
        // _baseEnemy.targetDetection.DidMyPositionChange(0.7f);
    }
    public void GoToPlayer()
    {
        _speed = _baseEnemy.speed * 5;
        // _speed = 500;
        enemyView.ChasingAnimation();

        waypoints = _path.corners;
        _baseEnemy._waypoints = waypoints;
        _baseEnemy._currentWaypoint = _currentWaypoint;
        if (_currentWaypoint < waypoints.Length)
            if (Vector3.Distance(_baseEnemy.transform.position, waypoints[_currentWaypoint]) >= 60)
            {

                Vector3 dir;
                dir = waypoints[_currentWaypoint] - _baseEnemy.transform.position;


                _baseEnemy.transform.rotation = Quaternion.LookRotation(dir);
                _baseEnemy.rb.velocity += _baseEnemy.transform.forward * _speed * Time.deltaTime;

            }
            else
            {

                if (_currentWaypoint < waypoints.Length)
                    _currentWaypoint++;

            }
        else
        {
            RecalculatePath();
        }
        if (playerHasMove)
            RecalculatePath();
        Debug.Log($"waypoint:{_currentWaypoint}:{waypoints.Length}");
        if (_baseEnemy.LookingPlayer()) _baseEnemy.Transition("Chase");
    }
    public void RecalculatePath()
    {
        _currentWaypoint = 0;
        _agent.CalculatePath(_baseEnemy.player.transform.position, _path);

    }
    bool playerHasMove => Vector3.Distance(_baseEnemy.player.transform.position, waypoints[waypoints.Length - 1]) > 100;



    public override void Sleep()
    {
        base.Sleep();
    }
}
