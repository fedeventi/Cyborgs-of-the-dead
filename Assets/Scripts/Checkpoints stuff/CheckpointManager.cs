using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static CheckpointManager instance;
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public int currentCheckpoint = 0;
    public Text instructions;
    Guide _guide;
    void Awake()
    {
        if(instance == null)
            instance = this;
        
            
    }
    void Start()
    {
        _guide=FindObjectOfType<Guide>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Save()
    {
        checkpoints[currentCheckpoint].Save();
        StopAllCoroutines();
        StartCoroutine(WriteCR(checkpoints[currentCheckpoint].checkpointInstructions));
        _guide.SetDestination(checkpoints[currentCheckpoint].objetivePosition);
    }
    public void Restore()
    {
        checkpoints[currentCheckpoint].Restore();
        StopAllCoroutines();
        StartCoroutine(WriteCR(checkpoints[currentCheckpoint].checkpointInstructions));
        _guide.SetDestination(checkpoints[currentCheckpoint].objetivePosition);
    }
    public IEnumerator WriteCR(string text)
    {
        instructions.text = "";

        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            instructions.text += chars[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
