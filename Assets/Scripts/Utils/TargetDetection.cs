using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection: MonoBehaviour
{
   
    int debugIndex;
    [SerializeField]
    LayerMask walls;
    [Range(5, 25)]
    public int amount=9;
    bool[] debugIndexBoolColliding = new bool[25];
    public bool[] DebugIndexBoolColliding { get => debugIndexBoolColliding; set => debugIndexBoolColliding = value; }
    public bool debugBool;
    [Range(-1, 100)]
    public float verticalOffset;
    [SerializeField]
    [Range(1, 1000)]
    float _obstacleAvoidance;
    [SerializeField]
    [Range(1, 1000)]
    float obstacleDetection;
    [SerializeField]

    public Transform MyClosestObstacle()
    {

        var obstacles = Physics.OverlapSphere(transform.position, obstacleDetection, walls);
        Transform _closest = null;
        if (obstacles.Length > 0)
        {
            foreach (var item in obstacles)
            {
                if (!_closest)
                    _closest = item.transform;
                else if (Vector3.Distance(item.transform.position, transform.position) < Vector3.Distance(_closest.position, transform.position))
                    _closest = item.transform;
            }
        }

        return _closest;
    }

    public Vector3 MyClosestPointToTarget(Vector3 myTarget)
    {
        Vector3[] possiblePoints = new Vector3[25];

        int positionIndex = 0;
        int targetIndex = 0;
        float distance = new float();
        float distanceAux = new float();


        for (int i = 0; i < amount; i++)
        {

            Vector3 targetDir = ((Quaternion.AngleAxis((180f / (amount - 1)) * i, transform.up) * transform.right * -1)).normalized;


            if (!Physics.Raycast(transform.position + new Vector3(0, verticalOffset, 0), (targetDir + transform.position) - transform.position, _obstacleAvoidance, walls))
            {

                
                    

                possiblePoints[positionIndex] = transform.position + targetDir.normalized * _obstacleAvoidance;
                distanceAux = Vector3.Distance(transform.position + targetDir.normalized * _obstacleAvoidance,
                                                                     new Vector3(myTarget.x,
                                                                                        transform.position.y,
                                                                               myTarget.z));

                debugIndexBoolColliding[i] = false;
                if (distance == 0)
                {
                    distance = distanceAux;
                    targetIndex = positionIndex;


                }
                else
                {
                    if (distanceAux < distance)
                    {
                        distance = distanceAux;

                        targetIndex = positionIndex;


                    }

                }
            }
            else
            {
                debugIndexBoolColliding[i] = true;
                
            }
            positionIndex++;
        }

        debugIndex = targetIndex;

        return possiblePoints[targetIndex];
    }
   
    private void OnDrawGizmos()
    {



        Gizmos.DrawWireSphere(transform.position, obstacleDetection);
       
        for (int i = 0; i < amount; i++)
        {
            Gizmos.color = Color.yellow;
            if (debugBool)
            {
                if (debugIndexBoolColliding[i]) Gizmos.color = Color.magenta;
                else
                    Gizmos.color = Color.yellow;


                Gizmos.DrawRay(transform.position + new Vector3(0, verticalOffset, 0),
                    ((Quaternion.AngleAxis((180f/(amount-1))*i,transform.up)*transform.right*-1+ transform.position) - transform.position) * obstacleDetection);


                if (debugIndex == i)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere((transform.position + new Vector3(0, verticalOffset, 0) +
                                  ((Quaternion.AngleAxis((180f / (amount - 1)) * i, transform.up) * 
                                  transform.right * -1 + transform.position) - transform.position)* 
                                  obstacleDetection), 5f);
                }

            }
        }
        
        if (debugBool)
        {
           

        }

        Gizmos.color = Color.white;






    }
}
