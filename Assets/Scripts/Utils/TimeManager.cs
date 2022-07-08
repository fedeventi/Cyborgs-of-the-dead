using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoostenProductions;
public class TimeManager : MonoBehaviour
{
    public float slowdownFactor=0.05f;
    public float slowdownLenght=2;
    public GameObject shockWave;
    float fixedDeltaTime;
    bool _isSlowmo;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if(Time.timeScale < 1)
        {
            Time.timeScale += (1 / slowdownLenght) * Time.unscaledDeltaTime;
        }
        else if(Time.timeScale > 1)
        {
            if (_isSlowmo)
            {
                Time.timeScale= 1;
                Time.fixedDeltaTime = fixedDeltaTime;
                _isSlowmo = false;
            }
            
        }
        
        

    }

    public void SlowMo()
    {
        
        _isSlowmo = true;
        fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale *slowdownFactor;

    }
    public void CreateShockwave(Vector3 position,Quaternion rotation)
    {
        if (_isSlowmo) return;
        Instantiate(shockWave, position, rotation);
        SlowMo();
    }

}
