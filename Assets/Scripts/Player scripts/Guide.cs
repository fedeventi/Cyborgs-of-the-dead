using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    float movementTolerance=1;
    public Transform destination;
    // Start is called before the first frame update
    public LineRenderer _line;
    bool _updateAlways=true;
    float minPathUpdateTime=0f;
    float mask=0;
    Material _material;
    bool _show;
    void Start()
    {
        _line.alignment = LineAlignment.TransformZ;
        _material =_line.material;
        _material.SetFloat("_Mask", mask);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(UpdatePath());
            _show =true;
        }
        if(_show)
            mask += Time.deltaTime*3;
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            StopAllCoroutines();
            _line.positionCount = 0;
            _show = false;
            mask =0;
        }
        _material.SetFloat("_Mask", mask);
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
        if (!_show)
        {
            
            return;
        }
        List<Vector3> allPositions = new List<Vector3>();
        allPositions.Add(transform.position);
        allPositions.AddRange(positions);
        for (int i = 0; i < allPositions.Count; i++)
        {
            var aux = allPositions[i];
            aux.y=transform.position.y;
            allPositions[i] = aux;
        }
        _line.positionCount= allPositions.Count;
        _line.SetPositions(allPositions.ToArray());
        
        
    }
}
