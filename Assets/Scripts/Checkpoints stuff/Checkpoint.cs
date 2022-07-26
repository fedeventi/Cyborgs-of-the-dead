using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objects = new List<GameObject>();
    void Start()
    {
        Save();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Restore();
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
    // Update is called once per frame
}
public interface ICheckpoint
{
     void Save();
    void Restore();
}
