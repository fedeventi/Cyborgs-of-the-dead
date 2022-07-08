using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T>
{
    BaseEnemy baseEnemy;
    EnemyView enemyView;
    float _timerToIdle;
    float _speed;
    bool _lame;
    Vector3 _destination;
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    bool _updateAlways;
    float randomRange = 500;
    int _currentWaypoint;
    float _distanceTreshold=30;
    Vector3 _lastMovedPosition;
    List<Vector3> _waypoints=new List<Vector3>();
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
    public override void Awake()
    {
        baseEnemy.isInPatrol = true;
        if (_lame)
            baseEnemy.StartCoroutine(changeSpeed());
        else
            _speed = baseEnemy.speed * 3f;

        _destination = baseEnemy.transform.position+UnityEngine.Random.insideUnitSphere * randomRange;
        _destination.y = 0;
        baseEnemy.StartCoroutine(UpdatePath());
        _currentWaypoint = 0;



    }
    void OnPathFound(Vector3[] waypoints, bool onPathSuccesful)
    {
        if (onPathSuccesful)
            _waypoints = new List<Vector3>(waypoints);
        else
            baseEnemy.Transition("Idle");
    }
    public IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(baseEnemy.transform.position,_destination , OnPathFound));
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = _destination;
        _lastMovedPosition = baseEnemy.transform.position;
        //while (_updateAlways)
        //{
        //    yield return new WaitForSeconds(minPathUpdateTime);
        //    //chequea si el objetivo se movio de lugar
        //    if ((baseEnemy.player.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
        //    {

        //        PathRequestManager.RequestPath(new PathRequest(baseEnemy.transform.position, _destination, OnPathFound));
        //        targetPosOld = _destination;
        //    }
        //}
    }
    public override void Execute()
    {
        baseEnemy.waypointsEnemy = _waypoints.ToArray();

        var distance = (baseEnemy.transform.position - _lastMovedPosition).magnitude;
        if (distance > _distanceTreshold)
        {
            _lastMovedPosition = baseEnemy.transform.position;
            _timerToIdle = 0;
        }
        else if (distance < _distanceTreshold && distance>0)
        {
            _timerToIdle += Time.deltaTime;
        }
        else
        {
            _timerToIdle = 0;
        }
        if (_timerToIdle > 1.3f)
        {
            Debug.Log("Me estoy trabando");
            _timerToIdle = 0;
            _destination = baseEnemy.transform.position + UnityEngine.Random.insideUnitSphere * randomRange;
            _destination.y = 0;
            baseEnemy.StartCoroutine(UpdatePath());
            _currentWaypoint = 0;
        }
        

        if(!baseEnemy.isDamage)
        {
            Patrol();
        }
        if (baseEnemy.LookingPlayer()) baseEnemy.Transition("Chase");
    }
    public void Patrol()
    {
        if (_waypoints.Count <= 0) return;
        if (_currentWaypoint >= _waypoints.Count) return;
       
        enemyView.WalkingAnimation();
        
        Vector3 target = _waypoints[_currentWaypoint];
        
        target.y=baseEnemy.transform.position.y;
        Vector3 dir = target - baseEnemy.transform.position;
       
        float distanceToWaypoint = Vector3.Distance(baseEnemy.transform.position, _waypoints[_currentWaypoint]);
        if(distanceToWaypoint< _distanceTreshold)
        {
            if(_currentWaypoint<_waypoints.Count-1)
                _currentWaypoint++;
            else
            {
                _currentWaypoint = 0;
                _destination = baseEnemy.transform.position + UnityEngine.Random.insideUnitSphere * randomRange;
                _destination.y = 0;
                
                if (Vector3.Distance(_destination, baseEnemy.transform.position) > 100)
                {
                    baseEnemy.StartCoroutine(UpdatePath());
                }
                
            }

        }
        baseEnemy.transform.forward = Vector3.Slerp(baseEnemy.transform.forward, dir, Time.deltaTime * 2).normalized;
        
        baseEnemy.rb.MovePosition(baseEnemy.transform.position + baseEnemy.transform.forward * _speed * Time.deltaTime);
    }
    //public void OldPatrol()
    //{
        
        
    //    if (baseEnemy.currentWaypoint < baseEnemy.waypointsEnemy.Length && !baseEnemy.isInIdle)
    //    {
            

    //        enemyView.WalkingAnimation();
    //        Vector3 target = baseEnemy.waypointsEnemy[baseEnemy.currentWaypoint].position;
    //        target.y = baseEnemy.transform.position.y;
    //        var distanceWP = Vector3.Distance(baseEnemy.waypointsEnemy[baseEnemy.currentWaypoint].position, baseEnemy.transform.position);
          

    //        var roulleteWheel = new RoulleteWheel<bool>();
    //        var probabilitiesToChangeWaypoint = new List<Tuple<int, bool>> { /*new Tuple<int, bool>(3,true),*/
    //                                                                         new Tuple<int, bool>(10,false) };

    //        _timerToIdle += Time.deltaTime;
    //        if (_timerToIdle > 6)
    //        {
    //            var roulleteWheel2 = new RoulleteWheel<bool>();
    //            var probabilitieToIdle = new List<Tuple<int, bool>> { new Tuple<int, bool>(3,true),
    //                                                                             new Tuple<int, bool>(6,false) };
    //            if (roulleteWheel.ProbabilityCalculator(probabilitieToIdle))
    //                baseEnemy.Transition("Idle");
    //            _timerToIdle = 0;
    //        }
    //        if (distanceWP <= 50f)
    //        {
               
                
    //            if (roulleteWheel.ProbabilityCalculator(probabilitiesToChangeWaypoint))
    //                baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length );
    //            else
    //                baseEnemy.currentWaypoint++;

    //        }
    //        else
    //        {

    //            //_timerForError += Time.deltaTime;
    //            //if (_timerForError > timerMax)
    //            //{
    //            //    _timerForError = 0;
    //            //    baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length);
    //            //}


    //        }
    //        if (baseEnemy.currentWaypoint >= baseEnemy.waypointsEnemy.Length)
    //        {
                
    //            if (roulleteWheel.ProbabilityCalculator(probabilitiesToChangeWaypoint))
    //                baseEnemy.currentWaypoint = UnityEngine.Random.Range(0, baseEnemy.waypointsEnemy.Length );
    //            else
    //                baseEnemy.currentWaypoint=0;
    //        }
            
    //        Vector3 dir = target - baseEnemy.transform.position;
    //        if (baseEnemy.targetDetection.MyClosestObstacle())
    //        {


    //            dir = (baseEnemy.targetDetection.MyClosestPointToTarget(target) - baseEnemy.transform.position);

    //        }
    //        else
    //        {
    //            dir = target - baseEnemy.transform.position;
                
    //        }
            
            
    //        //rota mirando al target
    //        baseEnemy.transform.forward = Vector3.Slerp(baseEnemy.transform.forward, dir, Time.deltaTime * 2).normalized;
    //        //baseEnemy.transform.position += baseEnemy.transform.forward * _speed * Time.deltaTime;
    //        baseEnemy.rb.MovePosition(baseEnemy.transform.position + baseEnemy.transform.forward * _speed * Time.deltaTime);
    //    }
    //}
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
   
    public override void Sleep()
    {
        baseEnemy.isInPatrol = false;
        if (_lame)
            baseEnemy.StopCoroutine(changeSpeed());
        else
            _speed = baseEnemy.speed * 3f;
    }

}
