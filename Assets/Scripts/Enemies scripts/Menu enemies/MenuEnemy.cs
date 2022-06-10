using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenuEnemy : MonoBehaviour
{

    //Componentes
    public NavMeshAgent navMesh;
    EnemyView enemyView;

    [Header("Patrol waypoints")]
    public Transform[] waypointsEnemy;
    public int currentWaypoint = 0;

    //Bools 
    bool waitOnWP = false;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyView = GetComponent<EnemyView>();
    }

    private void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (currentWaypoint < waypointsEnemy.Length && !waitOnWP)
        {
            navMesh.speed = 1;
            navMesh.stoppingDistance = 1;

            enemyView.WalkingAnimation();
            Vector3 target = waypointsEnemy[currentWaypoint].position;
            target.y = transform.position.y;
            navMesh.SetDestination(target);
            if (target == Vector3.zero)
            {
                currentWaypoint++;
                Vector3 wpPos = waypointsEnemy[currentWaypoint].position;
                navMesh.SetDestination(wpPos);
            }

            //rota mirando al target
            var rot = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3).normalized;
        }

        var distanceWP = Vector3.Distance(waypointsEnemy[currentWaypoint].position, transform.position);
        //cuando esta cerca del waypoint, se pone  en idle.
        if (distanceWP <= 1.5f)
        {
            StartCoroutine(WaitOnWaypoint());
            currentWaypoint++;
        }
        if (currentWaypoint >= waypointsEnemy.Length)
        {
            currentWaypoint = 0;
        }
    }


    public IEnumerator WaitOnWaypoint()
    {
        waitOnWP = true;
        enemyView.IdleAnimation();
        navMesh.speed = 0;

        yield return new WaitForSeconds(1f);

        waitOnWP = false;
        navMesh.speed = 1;

        yield break;
    }
}
