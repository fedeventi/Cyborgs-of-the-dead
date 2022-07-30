﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour , ICheckpoint
{
    // Start is called before the first frame update
    Collider _collider;
    public List<GameObject> glasses = new List<GameObject>();
    glassCheckpointData checkpointData;

    // Update is called once per frame
    void Start()
    {
        _collider = GetComponent<Collider>();
        
    }
    void Update()
    {
        
    }
    public void ReplaceGlass()
    {

        

        foreach (var item in glasses)
        {
            item.gameObject.SetActive(true);
        }
        _collider.gameObject.SetActive(false);
    }
   
    public void Save()
    {
        checkpointData=new glassCheckpointData(glasses);
    }

    public void Restore()
    {
        if (checkpointData != null)
            checkpointData.RestoreData(this);
        _collider.gameObject.SetActive(true);
    }
}
public class glassCheckpointData
{
    List<Vector3> _positions = new List<Vector3>();
    List<Quaternion> _rotation = new List<Quaternion>();
    List<Vector3> _localScale = new List<Vector3>();
    public glassCheckpointData(List<GameObject> glasses)
    {
        foreach (var item in glasses)
        {
            _positions.Add(item.transform.position);
            _rotation.Add(item.transform.rotation);
            _localScale.Add(item.transform.localScale);
        }
    }
    public void RestoreData(BreakGlass glass)
    {
        for (int i = 0; i < glass.glasses.Count; i++)
        {
            glass.glasses[i].transform.position = _positions[i];
            glass.glasses[i].transform.rotation = _rotation[i];
            glass.glasses[i].transform.localScale = _localScale[i];
            glass.glasses[i].SetActive(false);
        }
    }
}
