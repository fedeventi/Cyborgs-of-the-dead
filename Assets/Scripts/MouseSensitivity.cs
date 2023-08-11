using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    public Slider sensitivitySlider;

    private void Awake()
    {
        sensitivitySlider = GetComponent<Slider>();
    }

    private void Start()
    {

        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 5);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeSensitivity(float s)
    {
        s = sensitivitySlider.value;
        Save();
    }


    void Load()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
    }
}
