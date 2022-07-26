using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMovement : MonoBehaviour
{
    
    Transform parent;
    public float speed = 1;
    public float xScale = 1;
    public float yScale = 1;
    Vector3 debugPosition;
    [Range(0f, 1f)]
    public float blend;
    
    void Start()
    {
        parent = transform.parent;
    }

    void Update()
    {


         debugPosition =transform.position + (parent.transform.right * Mathf.Sin(Time.timeSinceLevelLoad / 2 * speed) * xScale*blend)
                                    - (parent.transform.up * Mathf.Sin(Time.timeSinceLevelLoad * speed) * yScale*blend);



    }
    public Vector3 GetDir()
    {
        return debugPosition;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(debugPosition, 5);
    }
}
