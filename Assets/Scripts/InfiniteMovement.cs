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
    float _time;
    void Start()
    {
        parent = transform.parent;
        _time = 0;
    }

    void Update()
    {

        _time += Time.deltaTime;
         debugPosition =transform.position + (parent.transform.right * Mathf.Sin(_time / 2 * speed) * xScale*blend)
                                    - (parent.transform.up * Mathf.Sin(_time * speed) * yScale*blend);



    }
    public void Reset()
    {
        _time = 0;
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
