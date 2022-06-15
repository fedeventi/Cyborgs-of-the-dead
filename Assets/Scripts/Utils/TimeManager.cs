using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor=0.05f;
    public float slowdownLenght=2;
    public GameObject shockWave;
    float fixedDeltaTime;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale < 1)
        {
            Time.timeScale += (1 / slowdownLenght) * Time.unscaledDeltaTime;
        }
        else if(Time.timeScale > 1)
        {
            Time.timeScale= 1;
            Time.fixedDeltaTime = fixedDeltaTime;
        }
        
        

    }

    public void SlowMo()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale *slowdownFactor;

    }
    public void CreateShockwave(Vector3 position,Quaternion rotation)
    {
        
        Instantiate(shockWave, position, rotation);
        SlowMo();
    }

}
