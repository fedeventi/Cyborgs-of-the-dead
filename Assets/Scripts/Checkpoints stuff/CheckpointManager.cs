using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static CheckpointManager instance;
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public int currentCheckpoint = 0;
    void Awake()
    {
        if(instance == null)
            instance = this;
        
            
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Save()
    {
        checkpoints[currentCheckpoint].Save();
    }
    public void Restore()
    {
        checkpoints[currentCheckpoint].Restore();
    }
}
