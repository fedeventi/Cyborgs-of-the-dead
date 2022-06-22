using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    float movementTolerance=50;
    public Transform destination;
    // Start is called before the first frame update
    public LineRenderer _line;
    bool _updateAlways=true;
    float minPathUpdateTime=0f;
    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, destination.position, OnPathFound));

        float sqrMoveThreshold = movementTolerance * movementTolerance;
        Vector3 oldPosition = transform.position;

        while (_updateAlways)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            //chequea si el objetivo se movio de lugar
            if ((transform.position - oldPosition).sqrMagnitude > sqrMoveThreshold)
            {

                PathRequestManager.RequestPath(new PathRequest(transform.position, destination.position, OnPathFound));
                oldPosition = transform.position;
            }
        }
    }
    void OnPathFound(Vector3[] positions, bool succesfullPath)
    {
        if (!succesfullPath) return;
        List<Vector3> allPositions = new List<Vector3>();
        allPositions.Add(transform.position);
        allPositions.AddRange(positions);
        _line.positionCount= allPositions.Count;
        _line.SetPositions(allPositions.ToArray());
        
        
    }
}
