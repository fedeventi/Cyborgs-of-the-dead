using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objects = new List<GameObject>();
    Collider collider;
    public string checkpointInstructions;
    public Transform checkpointObjetive;
    public Vector3 objetiveOffset;
    public Vector3 objetivePosition=>checkpointObjetive.position+objetiveOffset;
    [Header("Debug")]
    public int size=40;
    public Mesh mesh;
    void Start()
    {
        
        collider=GetComponent<Collider>();
    }
    void Update()
    {
       
    }

    public void Save()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            var chk = objects[i].GetComponent<ICheckpoint>();
            if(chk != null)
                chk.Save();
        }
    }
    public void Restore()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            var chk = objects[i].GetComponent<ICheckpoint>();
            if (chk != null)
                chk.Restore();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            CheckpointManager.instance.currentCheckpoint=GetCheckpointIndex();
            CheckpointManager.instance.Save();
            collider.enabled = false;
        }
    }
    int GetCheckpointIndex()
    {
        for (int i = 0; i < CheckpointManager.instance.checkpoints.Count; i++)
        {
            if (CheckpointManager.instance.checkpoints[i] == this)
                return i;
        }
        return 0;
    }
    public void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        if(mesh != null)
            Gizmos.DrawMesh(mesh, objetivePosition,checkpointObjetive.rotation, Vector3.one* size);
    }
    // Update is called once per frame
}
public interface ICheckpoint
{
     void Save();
    void Restore();
}
