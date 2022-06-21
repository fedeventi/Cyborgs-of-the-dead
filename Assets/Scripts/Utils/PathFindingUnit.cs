﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindingUnit : MonoBehaviour {

	const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .5f;

	public Transform target;
	public float speed = 20;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;

	MyPath path;
	public bool _updateAlways;

	public List<Vector3> positions;

	void Start() 
	{

		StartCoroutine(UpdatePath());
	}

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			path = new MyPath(waypoints, transform.position, turnDst, stoppingDst);

			positions = new List<Vector3>(path.lookPoints);
            
        }
	}
	public List<Vector3> GetListOfPositions()
    {
		
		return positions;
    }
	IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;

		do {
			yield return new WaitForSeconds (minPathUpdateTime);
			
			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));
				targetPosOld = target.position;
			}
		}while (_updateAlways);
	}

	IEnumerator FollowPath() {

		bool followingPath = true;
		int pathIndex = 0;
		transform.LookAt (path.lookPoints [0]);

		float speedPercent = 1;

		while (followingPath) {
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			while (path.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {

				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					if (speedPercent < 0.01f) {
						followingPath = false;
					}
				}
				var direction = new Vector3(path.lookPoints[pathIndex].x, transform.position.y,path.lookPoints[pathIndex].z);
				
				Vector3 targetRotation = (direction - transform.position);
				transform.forward = Vector3.Lerp (transform.forward, targetRotation, Time.deltaTime*0.2f * turnSpeed);
				transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			}

			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos ();
		}
		Gizmos.DrawRay(transform.position, transform.forward * 100);
		if(positions!=null)
            foreach (var item in positions)
            {
				Gizmos.DrawSphere(item, 30);
            }
	}
}
