﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;
    void Start()
    {
        float time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 4) if (Input.GetKeyDown(KeyCode.E)) SceneManager.LoadScene(0);
    }
}
